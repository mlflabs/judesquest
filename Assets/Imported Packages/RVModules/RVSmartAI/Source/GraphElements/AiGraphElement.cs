// Created by Ronis Vision. All rights reserved
// 05.01.2020.

using System;
using System.Collections;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

#pragma warning disable 252,253

namespace RVModules.RVSmartAI.GraphElements
{
    /// <summary>
    /// Base class for all elements in AiGraph
    /// </summary>
    public abstract class AiGraphElement : MonoBehaviour, IAiGraphElement
    {
        #region Fields

        [SmartAiExposeField]
        public new bool enabled = true;

        private IContext context;

        [SerializeField]
        protected string description;

        [SerializeField]
        [HideInInspector]
        protected string graphElementName;

        [SerializeField]
        [HideInInspector]
        private AiGraph aiGraph;

        [SerializeField]
        private IAiGraphElement parentGraphElement;

        #endregion

        #region Properties

        public AiGraph AiGraph
        {
            get => aiGraph;
            set => aiGraph = value;
        }

        public IContext Context
        {
            get => context;
            set
            {
                context = value;
                // update context for all children
                foreach (var allGraphElement in GetChildGraphElements())
                {
                    // avoid stack overflow
                    if (allGraphElement == this) continue;
                    allGraphElement.Context = value;
                }

                OnContextUpdated();
            }
        }

        /// <summary>
        /// Shorcut for casting context to desired type
        /// </summary>
        public T ContextAs<T>() where T : class => Context as T;

        public virtual string Name
        {
            get => graphElementName;
            set
            {
                graphElementName = value;
                UpdateGameObjectName();
            }
        }

        public virtual string Description
        {
            get => description;
            set => description = value;
        }

        public void UpdateGameObjectName()
        {
            if (aiGraph == null) return;
#if UNITY_EDITOR
            if (PrefabUtility.IsPartOfAnyPrefab(this)) return;
#endif
            
            var n = "";
//            var parent = GetParentGraphElement();
//            if (parent as Object != null)
//            {
//                n = parent.Name;
//                if (string.IsNullOrEmpty(parent.Name)) n = parent.GetType().Name;
//                n += "_";
//            }

            n += graphElementName;
            n += $" ({GetType().Name})";

            gameObject.name = n;
        }

        public virtual void RemoveNulls()
        {
        }

        public void UpdateReferences()
        {
            var assignedChildren = GetChildGraphElements();

            for (int i = 0; i < transform.childCount; i++)
            {
                var child = transform.GetChild(i);
                IAiGraphElement childGraphElement = child.GetComponent<IAiGraphElement>();
                if (!assignedChildren.Contains(childGraphElement)) AssignSubSelement(childGraphElement);
            }
        }

        public bool Enabled
        {
            get => enabled;
            set => enabled = value;
        }

        public virtual IList ChildGraphElements => null;

        #endregion

        #region Public methods

        /// <summary>
        /// Returns children graph elements including this
        /// </summary>
        /// <returns></returns>
        public virtual IAiGraphElement[] GetChildGraphElements() => new IAiGraphElement[] {this};

        public IAiGraphElement GetParentGraphElement() => parentGraphElement ?? (parentGraphElement = transform.parent.GetComponent<IAiGraphElement>());
        public void Remove(bool _destroyGameObject) => GetParentGraphElement()?.RemoveSubElement(this, _destroyGameObject);

        /// <summary>
        /// Returns all children graph elements recursively (children off children etc) including this
        /// </summary>
        /// <returns></returns>
        public virtual IAiGraphElement[] GetAllGraphElements() => new IAiGraphElement[] {this};

        /// <summary>
        /// Returns Types of graph elements that can be assigned as children
        /// </summary>
        /// <returns></returns>
        public virtual Type[] GetAssignableSubElementTypes() => new Type[0];

        /// <summary>
        /// Needs to be called when overriden
        /// </summary>
        /// <param name="_aiGraphElement"></param>
        public virtual void AssignSubSelement(IAiGraphElement _aiGraphElement)
        {
            // dont ever try to assign context at edit time!
            if (!Application.isPlaying) return;
            foreach (var graphElement in _aiGraphElement.GetAllGraphElements())
                graphElement.Context = Context;

            AiGraphElement aiGraphElement = _aiGraphElement as AiGraphElement;
            if (aiGraphElement != null)
                aiGraphElement.parentGraphElement = this;
        }

        public void RemoveSubElement(IAiGraphElement _aiGraphElement, bool _destroyGo)
        {
            if (_aiGraphElement == null) return;
            if (ChildGraphElements == null) return;
            if (!ChildGraphElements.Contains(_aiGraphElement)) return;
            ChildGraphElements.Remove(_aiGraphElement);
            OnSubGraphElementRemoved(_aiGraphElement);
            if (_aiGraphElement.gameObject != null && _destroyGo) Destroy(_aiGraphElement.gameObject);
        }

        public virtual void RemoveAllSubElements(bool _destroyGos)
        {
            if (ChildGraphElements == null) return;
            // needs to have copy so we wont modify collection we foreach on
            var children = ChildGraphElements.Cast<object>().ToArray();
            foreach (var childGraphElement in children) RemoveSubElement((IAiGraphElement) childGraphElement, _destroyGos);
        }

        /// <summary>
        /// Returns true if destroy was possible
        /// </summary>
        /// <returns></returns>
        public bool Destroy()
        {
#if UNITY_EDITOR
            if (CanBeRemoved())
            {
                DestroyMethod(gameObject);
                return true;
            }

            return false;
#else
            Destroy(gameObject);
            return true;
#endif
        }

        public override string ToString() => GetType().Name;

        #endregion

        #region Not public methods

        protected virtual void OnSubGraphElementRemoved(IAiGraphElement _aiGraphElement)
        {
        }

        protected virtual void OnContextUpdated()
        {
        }

        #endregion

#if UNITY_EDITOR
        public bool CanBeRemoved() =>
            AiGraph != null && AiGraph.isRuntimeDebugGraph || !IsParentPartOfPrefab() ||
            PrefabUtility.IsAddedGameObjectOverride(gameObject);

        internal bool IsPartOfPrefab() => PrefabUtility.IsPartOfAnyPrefab(transform.gameObject);

        internal bool IsParentPartOfPrefab() => transform.parent != null && PrefabUtility.IsPartOfAnyPrefab(transform.parent.gameObject);

        private void DestroyMethod(Object _object)
        {
            if (Application.isPlaying)
                Destroy(_object);
            else
                Undo.DestroyObjectImmediate(_object);
        }
#endif
    }
}
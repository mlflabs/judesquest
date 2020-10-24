// Created by Ronis Vision. All rights reserved
// 04.07.2020.

using System;
using System.Collections;
using RVModules.RVSmartAI.GraphElements;
using RVModules.RVSmartAI.GraphElements.Utilities;
using UnityEngine;
using XNode;

namespace RVModules.RVSmartAI.Nodes
{
    /// <summary>
    /// Base type for all SmartAi nodes used in AiGraph
    /// </summary>
    public abstract class SmartAiNode : MonoNode, IAiGraphElement
    {
        #region Fields

        [SerializeField]
        private string description;

        [SerializeField]
        private AiGraph aiGraph;

        #endregion

        #region Properties

        public abstract IContext Context { get; set; }

        public void RemoveNulls()
        {
        }

        public void UpdateReferences()
        {
        }

        public void UpdateGameObjectName() => gameObject.name = $"<<<NODE>>> {ToString().ToUpper()} <<<NODE>>>";

        public bool Enabled => true;
        public abstract void AssignSubSelement(IAiGraphElement _aiGraphElement);
        public abstract void RemoveSubElement(IAiGraphElement _aiGraphElement, bool _destroyGameObject);
        public abstract void RemoveAllSubElements(bool _destroyGameObjects);
        public abstract IList ChildGraphElements { get; }

        public abstract int OutputsCount { get; }

        public string Description
        {
            get => description;
            set => description = value;
        }

        public AiGraph AiGraph
        {
            get => aiGraph;
            set => aiGraph = value;
        }

        #endregion

        #region Public methods

        public override object GetValue(NodePort port) => null;

        public bool Destroy()
        {
            if (Application.isPlaying)
                Destroy(gameObject);
            else
                DestroyImmediate(gameObject);
            return true;
        }

        public abstract IAiGraphElement[] GetAllGraphElements();
        public abstract IAiGraphElement[] GetChildGraphElements();

        public IAiGraphElement GetParentGraphElement() => null;

        public void Remove(bool _destroyGameObject)
        {
            if (!_destroyGameObject) return;
            Destroy(gameObject);
        }

        public abstract Type[] GetAssignableSubElementTypes();

        public override string ToString()
        {
            if (string.IsNullOrEmpty(Name)) return GetType().Name;
            return Name;
        }

        public abstract SmartAiNode GetConnectedNode(AiUtility _connectedAiUtility);

        #endregion

        #region Not public methods

        protected abstract override void Init();

        #endregion
    }
}
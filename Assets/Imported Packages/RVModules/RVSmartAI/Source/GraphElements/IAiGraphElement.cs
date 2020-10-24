// Created by Ronis Vision. All rights reserved
// 23.08.2019.

using System;
using System.Collections;
using UnityEngine;

namespace RVModules.RVSmartAI.GraphElements
{
    public interface IAiGraphElement
    {
        #region Properties

        string Name { get; set; }
        string Description { get; set; }
        AiGraph AiGraph { get; set; }
        IContext Context { get; set; }
        GameObject gameObject { get; }

        /// <summary>
        /// Remove all nulls from own children, NOT recursive, so children of children wont get nulls removed
        /// </summary>
        void RemoveNulls();

        /// <summary>
        /// Find and add all child graph elements that are not referenced by this graph element
        /// </summary>
        void UpdateReferences();

        /// <summary>
        /// Updates game object accordingly to graph element name
        /// </summary>
        void UpdateGameObjectName();

        bool Enabled { get; }

        #endregion

        #region Public methods

        void AssignSubSelement(IAiGraphElement _aiGraphElement);

        void RemoveSubElement(IAiGraphElement _aiGraphElement, bool _destroyGameObject);

        void RemoveAllSubElements(bool _destroyGameObjects);

        // only child graph elements, without self
        IList ChildGraphElements { get; }

        bool Destroy();

        /// <summary>
        /// all child recursive + self
        /// </summary>
        IAiGraphElement[] GetAllGraphElements();

        /// <summary>
        /// all child + self
        /// </summary>
        IAiGraphElement[] GetChildGraphElements();

        /// <summary>
        /// Can return null 
        /// </summary>
        IAiGraphElement GetParentGraphElement();

        /// <summary>
        /// Removes this graph element from it's parent
        /// </summary>
        void Remove(bool _destroyGameObject);

        /// <summary>
        /// Returns array of graph elements types that can be added to this graph element as child 
        /// </summary>
        Type[] GetAssignableSubElementTypes();

        #endregion
    }
}
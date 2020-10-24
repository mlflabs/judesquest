// Created by Ronis Vision. All rights reserved
// 13.07.2020.

using System;
using System.Collections.Generic;
using System.Linq;
using RVModules.RVSmartAI.GraphElements;
using RVModules.RVSmartAI.GraphElements.Stages;
using RVModules.RVSmartAI.GraphElements.Utilities;
using RVModules.RVSmartAI.Nodes;
using UnityEditor;
using UnityEngine;
using XNode;

namespace RVModules.RVSmartAI
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable] public class AiGraph : MonoNodeGraph
    {
        #region Fields

        public string description;

        public bool isRuntimeDebugGraph;
        public string carrier;

        [NonSerialized]
        public SmartAiNode lastNode;

        [NonSerialized]
        public AiUtility lastAiUtility;

        [NonSerialized]
        public AiTask lastTask;

        [SerializeField]
        private AiGraph parentGraph;

        [SerializeField]
        private int graphStepsPerUpdate = 9999;

        [SerializeField]
        private int updateFrequency;

        [NonSerialized]
        private Stage currentStage;

        [NonSerialized]
        private SmartAiNode currentNode;

        [SerializeField]
        private SmartAiNode rootNode;

        private IContext context;

        // instanced subGraphs
        private List<AiGraph> subGraphs = new List<AiGraph>();

        private bool dontHideInHierarchy;

        #endregion

        #region Properties

        /// <summary>
        /// Root node
        /// </summary>
        public SmartAiNode RootNode
        {
            get => rootNode;
            set => rootNode = value;
        }

        /// <summary>
        /// If this graph instance was created as nested graph(reference in other graph) it will have it's parent graph reference
        /// </summary>
        public AiGraph ParentGraph => parentGraph;

        /// <summary>
        /// 
        /// </summary>
        public int GraphStepsPerUpdate
        {
            get => graphStepsPerUpdate;
            set => graphStepsPerUpdate = value;
        }

        /// <summary>
        /// Update frequency in hz set by Ai. You can change it by setting it in Ai class
        /// </summary>
        public int UpdateFrequency
        {
            get => updateFrequency;
            internal set => updateFrequency = value;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Updates context for all graph elements
        /// </summary>
        public void UpdateContext(IContext _newContext)
        {
            context = _newContext;
            foreach (var node in nodes)
            {
                var sn = node as SmartAiNode;
                if (sn == null) continue;
                sn.Context = _newContext;
            }
        }

        public void UpdateAiGraphForAllElements()
        {
            foreach (var monoNode in nodes)
            {
                var sn = monoNode as SmartAiNode;
                if (sn == null) continue;
                sn.AiGraph = this;
                sn.graph = this;
                foreach (var allGraphElement in sn.GetAllGraphElements())
                    allGraphElement.AiGraph = this;
            }
        }

        /// <summary>
        /// Remove all nulls from graph elements and update references, does not include sub graphs!
        /// </summary>
        public void RemoveNullsAndUpdateReferences()
        {
            foreach (var graphElement in GetAllGraphElements())
            {
                graphElement.RemoveNulls();
                graphElement.UpdateReferences();
                //Debug.Log(graphElement, graphElement as Object);
            }
        }

        /// <summary>
        /// Returns all AiGraphElement from this AiGraph
        /// </summary>
        /// <returns></returns>
        public IAiGraphElement[] GetAllGraphElements(bool includeSubgraphs = false)
        {
            var list = new List<IAiGraphElement>();
            foreach (var node in nodes)
            {
                var aiNode = node as SmartAiNode;
                if (aiNode == null) continue;
                list.AddRange(aiNode.GetAllGraphElements());
            }

            if (includeSubgraphs)
                foreach (var subGraph in subGraphs)
                    list.AddRange(subGraph.GetAllGraphElements(true));

            return list.ToArray();
        }

        /// <summary>
        /// 
        /// </summary>
        public void UpdateGraph(float _deltaTime)
        {
            //bool hasTask = false;
            // this loop controls how many utilities without task we can 'skip' in one go to not slow down ai decision making process
            // for debugging purposes its best to set it at 1 so it wont skip any utilities
            for (var i = 0; i < GraphStepsPerUpdate; i++)
            {
                // just all null checks and shit to find first stage
                if (nodes.Length == 0) return;
                if (currentNode == null)
                {
                    currentNode = RootNode;
#if UNITY_EDITOR
                    debugValues.Clear();
                    winners.Clear();
                    winNodes.Clear();
                    lastNode = null;
                    foreach (var subGraph in subGraphs)
                    {
                        subGraph.debugValues.Clear();
                        subGraph.winners.Clear();
                        subGraph.winNodes.Clear();
                        subGraph.lastNode = null;
                    }
#endif
                }

                if (currentNode == null) return;
                lastNode = currentNode;
                AiUtility utility = null;
                var sn = currentNode as StageNode;

                // we're in stage node?
                if (sn != null)
                {
                    currentStage = sn.stage;

                    // we have first stage, evaluate it! 
                    utility = currentStage.Select(_deltaTime);
                    if (utility == null)
                    {
                        lastAiUtility = null;
                        currentNode = RootNode;
//#if UNITY_EDITOR
//                        debugValues.Clear();
//                        winners.Clear();
//                        winNodes.Clear();
//                        foreach (var subGraph in subGraphs)
//                        {
//                            subGraph.debugValues.Clear();
//                            subGraph.winners.Clear();
//                            subGraph.winNodes.Clear();
//                        }
//#endif
                        break;
                    }

                    lastAiUtility = utility;
#if UNITY_EDITOR
                    winners.Add(lastAiUtility);
                    winNodes.Add(currentNode);
#endif

                    foreach (var task in utility.tasks)
                    {
                        if (!task.Enabled) continue;

                        task.Exec(_deltaTime);
                        lastTask = task;
                        //hasTask = true;
                    }
                }
                // we're not in stage node, we still have to add it to win nodes so itll get highlighted in debug
                else
                {
                    var gn = currentNode as GraphNode;
                    if (gn != null)
                        if (gn.graphReference != null)
                        {
                            gn.graphReference.UpdateGraph(_deltaTime);
                            lastTask = gn.graphReference.lastTask;
                            lastAiUtility = gn.graphReference.lastAiUtility;
#if UNITY_EDITOR

#endif
                        }
#if UNITY_EDITOR
                    winNodes.Add(currentNode);
#endif
                }

                // go to next connected node, can be null
                currentNode = currentNode.GetConnectedNode(utility);

                // only one graph walkthrough/reset permitted per update
                if (currentNode == null) break;
            }
        }

        public void UpdateGameObjectNames()
        {
            foreach (var allGraphElement in GetAllGraphElements()) allGraphElement.UpdateGameObjectName();
        }

        #endregion

        #region Not public methods

        internal static AiGraph CreateAndInitializeGraph(bool _dontHideInHierarchy, int _instantiatedGraphGraphStepsPerUpdate, string _carrierName,
            AiGraph _prefab, AiGraph _parentGraph = null)
        {
            var olName = _prefab.name;
            var instantiatedGraph = Instantiate(_prefab);
            instantiatedGraph.dontHideInHierarchy = _dontHideInHierarchy;
            instantiatedGraph.name = olName;
            instantiatedGraph.isRuntimeDebugGraph = true;
            instantiatedGraph.carrier = _carrierName;
            instantiatedGraph.GraphStepsPerUpdate = _instantiatedGraphGraphStepsPerUpdate;
            instantiatedGraph.parentGraph = _parentGraph;
            if (!_dontHideInHierarchy) instantiatedGraph.gameObject.hideFlags = HideFlags.HideInHierarchy;
            instantiatedGraph.RemoveNullsAndUpdateReferences();

            // instance all subgraphs
            foreach (var monoNode in instantiatedGraph.nodes)
            {
                var graphNode = monoNode as GraphNode;
                if (graphNode == null) continue;
                if (graphNode.graphReference == null) continue;
                var graphInstance = CreateAndInitializeGraph(_dontHideInHierarchy, _instantiatedGraphGraphStepsPerUpdate, _carrierName,
                    graphNode.graphReference, instantiatedGraph);
                graphNode.graphReference = graphInstance;
                instantiatedGraph.subGraphs.Add(graphInstance);
            }

            return instantiatedGraph;
        }

        // called after adding this component, only once 
        private void Reset() => name = gameObject.name;

        #endregion

#if UNITY_EDITOR
        // this is needed only for debugging

        /// <summary>
        /// Editor-time only!
        /// </summary>
        public List<AiUtility> winners = new List<AiUtility>();

        /// <summary>
        /// Editor-time only!
        /// </summary>
        public List<MonoNode> winNodes = new List<MonoNode>();
#endif

#if UNITY_EDITOR
        /// <summary>
        /// Editor-time only!
        /// </summary>
        public Dictionary<object, float> debugValues = new Dictionary<object, float>();

        internal void AddDebugValue(object _value, float _score)
        {
            if(ParentGraph != null) ParentGraph.AddDebugValue(_value, _score);
            if (debugValues.ContainsKey(_value))
                debugValues[_value] += _score;
            else
                debugValues.Add(_value, _score);
        }
#endif

#if UNITY_EDITOR

        /// <summary>
        /// Editor-time method only!
        /// </summary>
        public object CreateNewElement(Type _type, IAiGraphElement _graphElementParent)
        {
            var newGo = new GameObject();
            Undo.RegisterCreatedObjectUndo(newGo, "create new graph element");
            if (_graphElementParent == null)
                newGo.transform.SetParent(transform);
            else
                newGo.transform.SetParent(_graphElementParent.gameObject.transform);


            var c = newGo.AddComponent(_type);
            var graphElement = c as IAiGraphElement;
            graphElement?.UpdateGameObjectName();
            return c;
        }

        public void AssignRootNode() => RootNode = nodes.FirstOrDefault(n => n as SmartAiNode != null) as SmartAiNode;

        /// <summary>
        /// Adds node. Editor-time method only!
        /// </summary>
        public override INode AddNode(Type type)
        {
            MonoNode.graphHotfix = this;
            var node = CreateNewElement(type, null) as MonoNode;
            node.OnEnable();
            node.graph = this;
            var nodesList = new List<MonoNode>(nodes);
            nodesList.Add(node);
            nodes = nodesList.ToArray();
            Undo.RegisterCreatedObjectUndo(node.gameObject, "create node");
            var stageNode = node as SmartAiNode;
            // update context for when adding nodes at runtime
            if (stageNode != null && Application.isPlaying) stageNode.Context = context;

            if (stageNode != null && RootNode == null) RootNode = stageNode;
            return node;
        }

        /// <summary>
        /// Removes node. Editor-time method only!
        /// </summary>
        public override void RemoveNode(INode node)
        {
            Undo.DestroyObjectImmediate((node as Component).gameObject);
            base.RemoveNode(node);
        }

        /// <summary>
        /// Returns null of no graph element is selected
        /// Editor-time method only!
        /// </summary>
        /// <returns></returns>
        public IAiGraphElement GetSelectedGraphElement()
        {
            var stageNode = Selection.activeObject as StageNode;
            if (stageNode == null)
            {
                var smartNode = Selection.activeObject as SmartAiNode;
                if (smartNode == null) return null;

                return smartNode;
            }

            IAiGraphElement selectedGraphElement = stageNode.selectedElement;
            if (selectedGraphElement == null) selectedGraphElement = stageNode;

            return selectedGraphElement;
        }
#endif
    }
}
// Created by Ronis Vision. All rights reserved
// 01.04.2019.

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using RVModules.RVSmartAI.GraphElements;
using RVModules.RVSmartAI.GraphElements.Stages;
using RVModules.RVSmartAI.GraphElements.Utilities;
using RVModules.RVSmartAI.Nodes;
using RVModules.RVUtilities;
using RVModules.RVUtilitiesEditor;
using UnityEditor;
using UnityEditor.Experimental.SceneManagement;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using XNode;
using XNodeEditor;
using Object = UnityEngine.Object;

namespace RVModules.RVSmartAI.Editor
{
    [XNodeEditor.CustomEditor(typeof(AiGraph))]
    public class AiGraphEditor : NodeGraphEditor
    {
        [InitializeOnLoadMethod]
        public static void GetTitleContent()
        {
            titleContent = new GUIContent("SmartAIGraph", Resources.Load<Texture>("rvLogoSmall"));
        }

        private static GUIContent titleContent;

        private static bool CantEditGraph(AiGraph aiGraph)
        {
            if (aiGraph == null) return EditorApplication.isCompiling;

            return
                EditorApplication.isCompiling ||
                (PrefabStageUtility.GetPrefabStage(aiGraph.gameObject) != null && EditorApplication.isPlayingOrWillChangePlaymode) ||
                (!aiGraph.isRuntimeDebugGraph && EditorApplication.isPlayingOrWillChangePlaymode) ||
                (PrefabUtility.IsPartOfPrefabAsset(aiGraph.gameObject));
        }

        public override void OnGUI()
        {
            AiGraph aiGraph = target as AiGraph;
            Undo.RecordObject(aiGraph, "ai graph");

            // hide graph when compiling to avoid loss of data (to analyse - why this happens?) 
            // it still happens :<
            if (CantEditGraph(aiGraph))
            {
                AssignGraphForCurrentNodeEditorWindow();
                return;
            }

            titleContent.text = aiGraph.name;
            NodeEditorWindow.current.titleContent = titleContent;

            if (PrefabStageUtility.GetCurrentPrefabStage()?.prefabContentsRoot != null)
            {
                if (PrefabStageUtility.GetCurrentPrefabStage().prefabContentsRoot.GetComponent<AiGraph>() != null)
                {
                    if (aiGraph != PrefabStageUtility.GetCurrentPrefabStage().prefabContentsRoot.GetComponent<AiGraph>())
                        AssignGraphForCurrentNodeEditorWindow(PrefabStageUtility.GetCurrentPrefabStage().prefabContentsRoot.GetComponent<AiGraph>());
                }
            }

            if (aiGraph.RootNode == null && aiGraph.nodes.Length > 0) aiGraph.AssignRootNode();

            if (aiGraph.ParentGraph != null)
            {
                EditorHelpers.ChangeGuiColorsTemporarily(() =>
                {
                    if (GUILayout.Button($"<- Back to parent - {aiGraph.ParentGraph.name}"))
                    {
                        NodeEditorWindow.OpenWithGraph(aiGraph.ParentGraph);
                    }
                }, new GuiColorChange(GuiColorType.Background, Utilities.RonisVisionOrange));
            }

            var mousePos = GUIUtility.GUIToScreenPoint(Event.current.mousePosition);
            if (Application.isPlaying || NodeEditorWindow.current.position.Contains(mousePos))
            {
                NodeEditorWindow.current.Repaint();
                if (aiGraph.isRuntimeDebugGraph)
                {
                    titleContent.text += $" debug ({aiGraph.carrier})";
                    EditorGUILayout.LabelField($"Debug graph mode for {aiGraph.carrier}", GUIHelpers.GuiStyle(2));
                }
            }

            if (!aiGraph.isRuntimeDebugGraph && PrefabUtility.IsAnyPrefabInstanceRoot(aiGraph.gameObject) && PrefabStageUtility.GetCurrentPrefabStage() == null)
            {
                if (loadedGraphPrefab != aiGraph.gameObject)
                {
                    var c = GUI.contentColor;
                    GUI.contentColor = Color.red;
                    EditorGUILayout.LabelField("WARNING: EDITING PREFAB INSTANCE OF GRAPH", GUIHelpers.GuiStyle(2));
                    GUI.contentColor = c;
                }
            }

            if (!aiGraph.isRuntimeDebugGraph)
            {
                EditorGUILayout.LabelField("Assign graph element from prefab", GUIHelpers.GuiStyle(2));
                var obj = EditorGUILayout.ObjectField(null, typeof(GameObject), true, GUILayout.Width(200));
                if (obj != null) ValidateAndAddToGraphNewElement(aiGraph, obj as GameObject, aiGraph.GetSelectedGraphElement());
            }

            //            if (PrefabUtility.IsAnyPrefabInstanceRoot(aiGraph.gameObject) && !aiGraph.isRuntimeDebugGraph && !Application.isPlaying && PrefabUtility.GetObjectOverrides(aiGraph.gameObject).Count > 0)
            //                if (GUILayout.Button("Apply prefab overrides", GUILayout.Width(200)))
            //                {
            //                }    
        }

        public static IAiGraphElement ValidateAndAddToGraphNewElement(AiGraph _aiGraph, GameObject _sourceGo, IAiGraphElement _targetElement,
            bool _instantiateSourceGo = true)
        {
            GameObject sourceGo = null;

            if (_instantiateSourceGo)
            {
                // if user passed prefab, we have to instantiate it first 
                if (PrefabUtility.IsPartOfPrefabAsset(_sourceGo))
                {
                    sourceGo = PrefabUtility.InstantiatePrefab(_sourceGo) as GameObject;
                } // this was causing situation where if you worked on graph variant, ANY node was treated as prefab and went into this block
                // spawning base of variant graph and puking error.
                // now it works as it should, and you can copy/duplicate variant graph nodes that are not prefabs or are prefabs 
                //else if (PrefabUtility.IsPartOfPrefabInstance(_sourceGo))
                else if (PrefabUtility.IsAnyPrefabInstanceRoot(_sourceGo))
                {
                    sourceGo = PrefabUtility.InstantiatePrefab(PrefabUtility.GetCorrespondingObjectFromSource(_sourceGo)) as GameObject;
                }
                else
                    sourceGo = Object.Instantiate(_sourceGo);
            }
            else
                sourceGo = _sourceGo;

            if (sourceGo == null) sourceGo = Object.Instantiate(_sourceGo);

            if (sourceGo == null)
            {
                Debug.LogError("Something went wrong! Can't instantiate from source go", sourceGo);
                return null;
            }

            var newGraphElement = sourceGo.GetComponent<IAiGraphElement>();
            if (newGraphElement == null)
            {
                Debug.Log("This is not valid graph element prefab");
                if (_instantiateSourceGo) Object.DestroyImmediate(sourceGo);
                return null;
            }

            var isOfExpectedType = false;
            Type[] expectedTypes = null;

            if (_targetElement != null)
                expectedTypes = _targetElement.GetAssignableSubElementTypes();
            else
                expectedTypes = new[] { typeof(StageNode) };

            if (newGraphElement is StageNode)
                expectedTypes = new[] { typeof(StageNode) };

            foreach (var expectedType in expectedTypes)
            {
                isOfExpectedType = expectedType.IsAssignableFrom(newGraphElement.GetType()) || newGraphElement.GetType() == expectedType;
                if (isOfExpectedType) break;
            }

            // if we cant find him, it's not proper object, need to inform user and exit
            if (!isOfExpectedType)
            {
                string expectedTypesString = "";
                foreach (var expectedType in expectedTypes) expectedTypesString += expectedType.Name + "; ";

                Debug.Log("No expected graph element found in passed object. This graph elements allows for the following graph elements assignment: " +
                          expectedTypesString);

                if (_instantiateSourceGo) Object.DestroyImmediate(sourceGo);
                return null;
            }

            try
            {
                if (!(newGraphElement is StageNode))
                    newGraphElement.gameObject.transform.SetParent(_targetElement.gameObject.transform);
                else
                    newGraphElement.gameObject.transform.SetParent(_aiGraph.transform);
            }
            catch (Exception e)
            {
                Debug.LogError("Error reparenting source go! " + e, newGraphElement.gameObject);
                if (_instantiateSourceGo) Object.DestroyImmediate(sourceGo);
                return null;
            }

            if (_targetElement != null)
            {
                if (_targetElement.GetChildGraphElements().Contains(newGraphElement))
                {
                    Debug.Log("This graph element is already added!");
                    if (_instantiateSourceGo) Object.DestroyImmediate(sourceGo);
                    return newGraphElement;
                }

                Undo.RecordObject(_targetElement as Object, "inspector");
                _targetElement.AssignSubSelement(newGraphElement);
            }

            _aiGraph.OnBeforeSerialize();
            _aiGraph.UpdateAiGraphForAllElements();
            _aiGraph.UpdateGameObjectNames();
            return newGraphElement;
        }

        public override NodeEditorPreferences.Settings GetDefaultPreferences()
        {
            return new NodeEditorPreferences.Settings()
            {
                gridBgColor = new Color(.2f, .2f, .2f),
                gridLineColor = Color.gray,
                typeColors = new Dictionary<string, Color>()
                {
                    {typeof(AiUtility).PrettyName(), RVUtilities.Utilities.RonisVisionOrange},
                }
            };
        }

        [InitializeOnLoadMethod]
        public static void LoadPrefabOnAssetSelectionAndAssignSavingCallbacks()
        {
            EditorApplication.quitting += SaveAndUnloadGraphPrefabFromMemory;
            NodeEditorWindow.onWindowDisable += _window => SaveAndUnloadGraphPrefabFromMemory();
            NodeEditorWindow.onGraphOpen += (_graph) => { AssignGraphForCurrentNodeEditorWindow(_graph as AiGraph); };
            NodeEditorWindow.onGuiWithNullGraph += async () =>
            {
                if (CantEditGraph(null) || Application.isPlaying) return;

                if (NodeEditorWindow.current.lastOpenedSmartAiGraphId != 0)
                {
                    await Task.Delay(1);
                    LoadGraphPrefabContents(AssetDatabase.GetAssetPath(NodeEditorWindow.current.lastOpenedSmartAiGraphId), false);
                }
            };

            Selection.selectionChanged += () =>
            {
                if (CantEditGraph(null) || Application.isPlaying || PrefabStageUtility.GetCurrentPrefabStage() != null) return;

                var go = Selection.activeGameObject;
                if (go == null) return;
                LoadGraphPrefabContents(go);
            };
        }

        public static void LoadGraphPrefabContents(GameObject go)
        {
            if (NodeEditorWindow.current == null) return;

            var prefabPath = AssetDatabase.GetAssetPath(go);

            if (string.IsNullOrEmpty(prefabPath) || !File.Exists(prefabPath) || !prefabPath.EndsWith(".prefab")) return;
            if (go.GetComponent<AiGraph>() == null) return;

            //NodeEditorWindow.current.lastOpenedSmartAiGraphId = go.GetInstanceID();
            LoadGraphPrefabContents(prefabPath);
        }

        public static GameObject loadedGraphPrefab;
        //public static string NodeEditorWindow.current.prefabPath;

        private static void LoadGraphPrefabContents(string _path, bool autoFocus = true)
        {
            if (NodeEditorWindow.current == null) return;

            // save and unload earlier loaded graph prefab
            SaveAndUnloadGraphPrefabFromMemory();

            var asset = AssetDatabase.LoadAssetAtPath<GameObject>(_path);

            if (asset == null) return;

            // load new graph prefab to memory
            var graphPrefab = PrefabUtility.LoadPrefabContents(_path);
            if (graphPrefab == null) return;

            loadedGraphPrefab = graphPrefab;
            var graph = loadedGraphPrefab.GetComponent<AiGraph>();
            if (graph == null) return;

            // this is the only exception to assigning current.graph directly, it must be here
            NodeEditorWindow.current.graph = graph;
            NodeEditorWindow.current.Repaint();

            if (autoFocus) FocusOnRoot(graph);
            titleContent.text = graph.name;
            NodeEditorWindow.current.titleContent = titleContent;

            NodeEditorWindow.current.lastOpenedSmartAiGraphId = asset.GetInstanceID();
        }

        public static void AssignGraphForCurrentNodeEditorWindow(AiGraph _aiGraph = null)
        {
            if (_aiGraph == null) return;

            if (NodeEditorWindow.current == null) NodeEditorWindow.Init();

            SaveAndUnloadGraphPrefabFromMemory();
            NodeEditorWindow.current.graph = _aiGraph;
            _aiGraph.RemoveNullsAndUpdateReferences();

            //if (NodeEditorWindow.current.graph == null)
            //   titleContent.text = "RVSmartAIGraph";
            if (NodeEditorWindow.current.graph != null) FocusOnRoot(_aiGraph);
        }

        private static void FocusOnRoot(AiGraph _aiGraph)
        {
            if (_aiGraph == null) return;
            if (!SmartAiSettings.AutoFocusOnRoot) return;

            if (_aiGraph.RootNode == null)
                NodeEditorWindow.current.panOffset = Vector2.zero;
            else
                NodeEditorWindow.current.panOffset = -_aiGraph.RootNode.position - new Vector2(150, 100);
        }

        private static void SaveAndUnloadGraphPrefabFromMemory()
        {
            if (loadedGraphPrefab == null) return;

            //WARNING this is risky!
            if (PrefabStageUtility.GetCurrentPrefabStage() != null)
            {
                loadedGraphPrefab = null;
                return;
            }

            var graph = NodeEditorWindow.current.graph as AiGraph;
            if (graph == null)
            {
                PrefabUtility.UnloadPrefabContents(loadedGraphPrefab);
                return;
            }

            graph.RemoveNullsAndUpdateReferences();
            graph.UpdateGameObjectNames();

            if (File.Exists(AssetDatabase.GetAssetPath(NodeEditorWindow.current.lastOpenedSmartAiGraphId)))
                PrefabUtility.SaveAsPrefabAsset(loadedGraphPrefab, AssetDatabase.GetAssetPath(NodeEditorWindow.current.lastOpenedSmartAiGraphId));
            PrefabUtility.UnloadPrefabContents(loadedGraphPrefab);
        }

        public static void UnpackSelectedGraphElementPrefab(AiGraph _ai, IAiGraphElement _graphElement)
        {
            PrefabUtility.UnpackPrefabInstance(_graphElement.gameObject, PrefabUnpackMode.OutermostRoot, InteractionMode.UserAction);
        }

        public static bool CheckIfIsPartOfPrefab(AiGraph _aiGraph, bool onlyCheck = false)
        {
            // dont allow scene-only graphs
            if (!_aiGraph.isRuntimeDebugGraph && !UnityEditor.PrefabUtility.IsPartOfPrefabAsset(_aiGraph.gameObject) &&
                UnityEditor.Experimental.SceneManagement.PrefabStageUtility.GetPrefabStage(_aiGraph.gameObject) == null)
            {
                if (onlyCheck) return false;

                Debug.Log("Using graphs as non-prefabs is not allowed! click this log for object reference" + _aiGraph.gameObject, _aiGraph.gameObject);
                return false;
            }

            return true;
        }

        // draw debug 'highlighted' lines
        /// <summary>
        /// 
        /// </summary>
        public override Color GetPortColor(NodePort port)
        {
            var graph = target as AiGraph;
            if (graph == null) return base.GetPortColor(port);
            if (!graph.isRuntimeDebugGraph) return base.GetPortColor(port);
            // 
            //var lastNode = graph.lastNode;
            var node = port.node as SmartAiNode;
            //if (lastNode != currNode) return base.GetPortColor(port);

            if (node == null) return base.GetPortColor(port);
            //if (n.stage.lastWinUtility == null) return base.GetPortColor(port);

            // input port
            if (port.IsInput)
            {
                if (!port.IsConnected) return base.GetPortColor(port);

                var stageNodeInput = port.Connection?.node as SmartAiNode;
                if (stageNodeInput == null) return base.GetPortColor(port);

                if (!graph.winNodes.Contains(stageNodeInput)) return base.GetPortColor(port);

                if (graph.winNodes.Contains(node)) return Color.cyan;

                var connectedGraphNode = stageNodeInput as GraphNode;
                if (connectedGraphNode != null && graph.winNodes.Contains(connectedGraphNode)) return Color.cyan;

                for (int i = 0; i < stageNodeInput.Outputs.Count(); i++)
                {
                    // output ports
                    var p = stageNodeInput.Outputs.ElementAt(i);
                    if (i >= stageNodeInput.OutputsCount)
                        continue;

                    StageNode sn = stageNodeInput as StageNode;
                    if (sn != null)
                    {
                        var ut = sn.stage.utilities[i];

                        if (p == port.Connection && graph.winners.Contains(ut)) return Color.cyan;
                    }
                }

                return base.GetPortColor(port);
            }

            GraphNode graphNode = node as GraphNode;
            if (graph.winNodes.Contains(graphNode))
            {
                return Color.cyan;
            }

            for (int i = 0; i < node.Outputs.Count(); i++)
            {
                // output ports
                var p = node.Outputs.ElementAt(i);
                if (p != port) continue;
                if (i >= node.OutputsCount) continue;

                //Debug.Log(i);

                var stageNode = node as StageNode;
                if (stageNode != null)
                {
                    var utility = stageNode.stage.utilities[i];
                    if (utility == null) continue;

                    foreach (var graphWinner in graph.winners)
                    {
                        if (utility.guid == graphWinner.guid)
                            return Color.cyan;
                    }
                }
            }

            // find port, and related stage to see if its the one that was lastly used 
            //            for (int i = 0; i < currNode.Ports.Count(); i++)
            //            {
            //                var p = currNode.Ports.ElementAt(i);
            //                if (p != port) continue;
            //                if (i >= currNode.stage.utilitys.Count)
            //                    continue;
            //                var utility = currNode.stage.utilitys[i];
            //                if (utility == null) continue;
            //                if (utility == graph.lastUtility)
            //                    return Color.cyan;
            //            }

            return base.GetPortColor(port);
        }
    }
}
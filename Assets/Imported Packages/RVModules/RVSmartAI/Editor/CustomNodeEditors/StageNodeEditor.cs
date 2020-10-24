// Created by Ronis Vision. All rights reserved
// 25.03.2019.

using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using RVModules.RVSmartAI.Editor.SelectWindows;
using RVModules.RVSmartAI.GraphElements;
using RVModules.RVSmartAI.Nodes;
using RVModules.RVUtilities.Extensions;
using UnityEditor;
using UnityEngine;
using XNode;
using XNodeEditor;
using Object = UnityEngine.Object;

namespace RVModules.RVSmartAI.Editor.CustomNodeEditors
{
    [XNodeEditor.CustomEditor(typeof(StageNode))]
    public class StageNodeEditor : NodeEditor
    {
        private GUIStyle buttonStyle;

        //private SerializedObject serializedstage;
        public override int GetWidth()
        {
            return 300;
        }

        public override Color GetTint()
        {
            var s = (target as StageNode);
            var graph = (s.graph) as AiGraph;
            if (graph == null) return base.GetTint();
            if (graph.lastNode == null) return base.GetTint();
            if (graph.lastNode == target)
                return Color.cyan;
            return base.GetTint();
        }

        public override void OnHeaderGUI()
        {
            var s = (target as StageNode);
            if (s.stage == null)
            {
                GUILayout.Label(s.GetType().Name, NodeEditorResources.styles.nodeHeader, GUILayout.Height(30));
                return;
            }

            var c = GUI.contentColor;
            if (PrefabUtility.IsAnyPrefabInstanceRoot(s.gameObject))
                GUI.contentColor = GUIHelpers.GuiStyle(3).normal.textColor;

            var nejm = "";

            if (string.IsNullOrEmpty(s.Name))
                nejm = s.stage.GetType().Name;
            else
                nejm = s.Name;

            if (s.IsRoot) nejm += " (ROOT)";

            GUILayout.Label(nejm, NodeEditorResources.styles.nodeHeader, GUILayout.Height(30));

            GUI.contentColor = c;
        }

        public override void AddContextMenuItems(GenericMenu menu)
        {
            StageNode sn = target as StageNode;

            AiGraph graph = sn.graph as AiGraph;
            if (graph == null) return;

            // Custom sctions if only one node is selected
            if (Selection.objects.Length == 1 && Selection.activeObject is INode)
            {
                INode node = Selection.activeObject as INode;
                NodeEditorWindow.AddCustomContextMenuItems(menu, node);

                menu.AddSeparator("");

                menu.AddItem(new GUIContent("Copy selected graph element"), false, CopySelectedGraphElement);
                menu.AddItem(new GUIContent("Duplicate selected graph element"), false, DuplicateSelectedGraphElement);
                if (graphElementClipboard != null) menu.AddItem(new GUIContent("Paste copied graph element"), false, PasteGraphElement);
                menu.AddItem(new GUIContent("Select corresponding game object"), false, SelectGoOfGraphElement);
                menu.AddItem(new GUIContent("Make selected graph element prefab"), false, MakeSelectedGraphElementPrefab);

                if (graph.GetSelectedGraphElement() == null) return;

                var selectedGo = graph.GetSelectedGraphElement().gameObject;

                if (PrefabUtility.IsAnyPrefabInstanceRoot(selectedGo))
                    menu.AddItem(new GUIContent("Unpack selected graph element prefab"), false,
                        () => PrefabUtility.UnpackPrefabInstance(
                            selectedGo,
                            PrefabUnpackMode.OutermostRoot,
                            InteractionMode.UserAction));

                if (PrefabUtility.IsPartOfAnyPrefab(graph.GetSelectedGraphElement().gameObject))
                {
                    menu.AddItem(new GUIContent("Show selected graph element prefab"), false,
                        () => Selection.activeObject =
                            AssetDatabase.LoadAssetAtPath<Object>(PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(selectedGo)));
                }
            }
        }

        private static IAiGraphElement graphElementClipboard;

        private void DuplicateSelectedGraphElement()
        {
            if (GetSelectedGraphElement(out var aiGraph, out var selectedGraphElement)) return;
            AiGraphEditor.ValidateAndAddToGraphNewElement(aiGraph, selectedGraphElement.gameObject,
                selectedGraphElement.gameObject.transform.parent.GetComponent<IAiGraphElement>());
        }

        private void CopySelectedGraphElement()
        {
            if (GetSelectedGraphElement(out var aiGraph, out var selectedGraphElement)) return;

            graphElementClipboard = selectedGraphElement;
            Debug.Log($"{graphElementClipboard} copied into clipboard. It will be lost on serialization.", graphElementClipboard?.gameObject);
        }

        private void PasteGraphElement()
        {
            if (GetSelectedGraphElement(out var aiGraph, out var selectedGraphElement)) return;

            AiGraphEditor.ValidateAndAddToGraphNewElement(aiGraph, graphElementClipboard.gameObject, selectedGraphElement);
        }

        private void SelectGoOfGraphElement()
        {
            StageNode sn = target as StageNode;
            AiGraph aiGraph = sn.graph as AiGraph;

            var selectedGraphElement = aiGraph.GetSelectedGraphElement();
            UnityEditor.Selection.activeGameObject = selectedGraphElement.gameObject;
        }

        private void MakeSelectedGraphElementPrefab()
        {
            if (GetSelectedGraphElement(out var aiGraph, out var selectedGraphElement)) return;

            var path = "";
            if (UnityEditor.Experimental.SceneManagement.PrefabStageUtility.GetCurrentPrefabStage() == null)
                path = AssetDatabase.GetAssetPath(NodeEditorWindow.current.lastOpenedSmartAiGraphId);
            else
                path = UnityEditor.Experimental.SceneManagement.PrefabStageUtility.GetCurrentPrefabStage().prefabAssetPath;

            if (string.IsNullOrEmpty(path)) path = Application.dataPath + "\\";
            else
                path = new FileInfo(path).Directory.FullName + "\\";

            path = path + (aiGraph.name + "_" + selectedGraphElement.Name + ".prefab").GetWithoutIllegal();

            var goPrefab = UnityEditor.PrefabUtility.SaveAsPrefabAssetAndConnect(
                selectedGraphElement.gameObject, path, UnityEditor.InteractionMode.UserAction);
            UnityEditor.Selection.activeObject = goPrefab;
        }

        private bool GetSelectedGraphElement(out AiGraph aiGraph, out IAiGraphElement selectedGraphElement)
        {
            StageNode sn = target as StageNode;
            aiGraph = sn.graph as AiGraph;

            selectedGraphElement = aiGraph.GetSelectedGraphElement();
            if (selectedGraphElement == null)
            {
                Debug.Log("Select graph element first");
                return true;
            }

            return false;
        }

        public override void OnBodyGUI()
        {
            if (buttonStyle == null)
                buttonStyle = GUIStyles.GetGUIStyle(0);

            SerializedObject.Update();

            StageNode sn = target as StageNode;
            Undo.RecordObject(target, "inspector");

            AiGraph graph = sn.graph as AiGraph;

            if (sn.stage == null) return;
            var expanded = sn.expanded;

            Undo.RecordObject(sn.stage, "inspector");

            NodeEditorGUILayout.PortField(sn.GetInputPort("input"), GUILayout.Width(5));

            var buttonTexture = GUIHelpers.DownButton();
            if (sn.expanded) buttonTexture = GUIHelpers.UpButton();
            
            if (GUI.Button(new Rect(266, 35, 15, 15), buttonTexture, GUIHelpers.GuiStyle(1)))
            {
                sn.expanded = !expanded;
            }

            //if (!expanded) return; 

            for (int i = 0; i < sn.stage.utilities.Count; i++)
            {
                EditorGUILayout.BeginHorizontal(GUILayout.ExpandWidth(true), GUILayout.MinWidth(500));

                var utility = sn.stage.utilities[i];
                if (utility == null)
                {
                    sn.stage.utilities.RemoveAt(i);
                    break;
                }

                Undo.RecordObject(utility, "inspector");

                buttonStyle = GUIHelpers.GuiStyle(0);
                // for debugg vis:
                foreach (var graphWinner in graph.winners)
                {
                    if (graphWinner.guid == utility.guid)
                    {
                        buttonStyle = GUIHelpers.GuiDebugStyle(0);
                        break;
                    }
                }

                var c = GUI.backgroundColor;
                GUI.backgroundColor = new Color(0, 0, 0, 0);
                GUILayout.Box(GUIHelpers.utilityIcon(), GUILayout.Height(20), GUILayout.Width(20));
                GUI.backgroundColor = c;

                string utlBtnStg = utility.Name;
                // if this utility is prefab, make text for it blue
                if (!utility.AiGraph.isRuntimeDebugGraph && !Application.isPlaying)
                {
                    if (PrefabUtility.IsAnyPrefabInstanceRoot(utility.gameObject))
                        buttonStyle = GUIHelpers.GuiStyle(3);
                    //utlBtnStg += " (prefab)";
                }

                if (graph.isRuntimeDebugGraph)
                    utlBtnStg += $" ({Math.Round(utility.lastScore, 2)})";

                int width = 166;
                //if (graph.isRuntimeDebugGraph) width = 184;

                c = GUI.contentColor;
                if (!utility.Enabled) GUI.contentColor = Color.gray;

                if (GUILayout.Button(utlBtnStg, buttonStyle, GUILayout.Width(width)))
                {
                    Selection.objects = new Object[] {sn};
                    SetSelectedElementRetarded(utility);
                }

                GUI.contentColor = c;

                GUILayout.Space(5);
                buttonStyle = GUIHelpers.GuiStyle(0);

                // adding task button
                //if (!graph.isRuntimeDebugGraph)
                if (GUILayout.Button(GUIHelpers.AddTaskButton(), GUIHelpers.GuiStyle(1), GUILayout.MaxWidth(18),
                    GUILayout.MaxHeight(18)) /*&& !dontAllowAddTask*/)
                {
                    var e = ScriptableObject.CreateInstance<SelectTaskWindow>();
                    e.onSelectedItem = _aiTask =>
                    {
                        Undo.RecordObject(utility, "inspector");
                        // create it
                        var newTask = graph.CreateNewElement(_aiTask, utility) as AiTask;
                        // assign
                        utility.AssignSubSelement(newTask);
                    };

                    //e.onSelectedGameObject += _o => { graph.ValidateAndAddToGraphNewElement(_o, utility); };
                }

                // up down remove buttons for utilities
                GUIHelpers.UpDownRemove(
                    () =>
                    {
                        if (i < 1) return;
                        var nodePort = sn.Outputs.ElementAt(i).Connection;
                        var nodePortOther = sn.Outputs.ElementAt(i - 1).Connection;
                        sn.Outputs.ElementAt(i).ClearConnections();
                        sn.Outputs.ElementAt(i - 1).ClearConnections();
                        if (nodePortOther != null) sn.Outputs.ElementAt(i).Connect(nodePortOther);
                        if (nodePort != null) sn.Outputs.ElementAt(i - 1).Connect(nodePort);

                        sn.stage.utilities.Move(i, MoveDirection.Up);
                    },
                    () =>
                    {
                        if (i >= sn.stage.utilities.Count - 1) return;

                        var nodePort = sn.Outputs.ElementAt(i).Connection;
                        var nodePortOther = sn.Outputs.ElementAt(i + 1).Connection;
                        sn.Outputs.ElementAt(i).ClearConnections();
                        sn.Outputs.ElementAt(i + 1).ClearConnections();
                        if (nodePortOther != null) sn.Outputs.ElementAt(i).Connect(nodePortOther);
                        if (nodePort != null) sn.Outputs.ElementAt(i + 1).Connect(nodePort);

                        sn.stage.utilities.Move(i, MoveDirection.Down);
                    },
                    () =>
                    {
                        Undo.RegisterCompleteObjectUndo(sn.stage, "inspector");
                        for (int j = i; j < sn.stage.utilities.Count; j++)
                        {
                            sn.Outputs.ElementAt(j).ClearConnections();

                            if (j >= sn.stage.utilities.Count - 1) break;

                            var nodePort = sn.Outputs.ElementAt(j + 1).Connection;
                            if (nodePort == null) continue;
                            sn.Outputs.ElementAt(j).Connect(nodePort);
                        }

                        if (utility.Destroy())
                            sn.stage.utilities.Remove(utility);
                    }, !utility.CanBeRemoved());

                NodeEditorGUILayout.PortField(sn.Outputs.ElementAt(i), GUILayout.Width(-5));

                EditorGUILayout.EndHorizontal();

                if (expanded)
                    for (int taskId = 0; taskId < utility.tasks.Count; taskId++)
                    {
                        var task = utility.tasks[taskId];
                        if (task == null) continue;

                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.Space();
                        EditorGUILayout.Space();
                        EditorGUILayout.Space();

                        c = GUI.backgroundColor;

                        GUI.backgroundColor = new Color(0, 0, 0, 0);
                        GUILayout.Box(GUIHelpers.ActionIcon(), GUILayout.Height(20), GUILayout.Width(20));
                        GUI.backgroundColor = c;

                        c = GUI.contentColor;

                        if (!task.Enabled || !utility.Enabled) GUI.contentColor = Color.gray;

                        if (PrefabUtility.IsAnyPrefabInstanceRoot(task.gameObject))
                            buttonStyle = GUIHelpers.GuiStyle(3);

                        var taskText = task.Name;
                        if (graph.isRuntimeDebugGraph && task.taskScorers.Count > 0) taskText += $" ({Math.Round(task.lastScore, 2)})";
                        
                        if (GUILayout.Button(taskText, buttonStyle))
                        {
                            Selection.objects = new Object[] {sn};
                            SetSelectedElementRetarded(task);
                        }

                        GUI.contentColor = c;
                        buttonStyle = GUIStyles.GetGUIStyle(0);

                        GUILayout.FlexibleSpace();

                        var allowDestroy = task.CanBeRemoved();

                        GUIHelpers.UpDownRemove(() => { utility.tasks.Move(taskId, MoveDirection.Up); },
                            () => { utility.tasks.Move(taskId, MoveDirection.Down); },
                            () =>
                            {
                                Undo.RegisterCompleteObjectUndo(utility, "inspector");
                                utility.tasks.Remove(task);
                                task.Destroy();
                            }, !allowDestroy);

                        EditorGUILayout.EndHorizontal();
                    }

                if (expanded) EditorGUILayout.Separator();
                //EditorGUILayout.Separator();
            }

            PrefabUtility.RecordPrefabInstancePropertyModifications(target);
            PrefabUtility.RecordPrefabInstancePropertyModifications(sn.stage);
            SerializedObject.ApplyModifiedProperties();
        }

        private async void SetSelectedElementRetarded(AiGraphElement _element)
        {
            await Task.Delay(5);
            (target as StageNode).selectedElement = _element;
        }

        [InitializeOnLoadMethod]
        private static void OnNodeSelected()
        {
            Selection.selectionChanged += () =>
            {
                var s = Selection.objects.FirstOrDefault();
                if (s == null) return;
                StageNode sn = s as StageNode;
                if (sn == null) return;
                sn.selectedElement = null;
            };
        }
    }
}
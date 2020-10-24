// Created by Ronis Vision. All rights reserved
// 14.04.2019.

using System;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using XNodeEditor;

namespace RVModules.RVSmartAI.Editor.CustomInspectors
{
    [CanEditMultipleObjects] [UnityEditor.CustomEditor(typeof(Ai))]
    public class AIInspector : UnityEditor.Editor
    {
        private SerializedProperty graphProp;
        private SerializedProperty contextProviderProp;
        private SerializedProperty stepsPerUpdateProp;
        private SerializedProperty updateFrequencyProp;
        private SerializedProperty dontHideInHierarchyProp;
        private SerializedProperty secondaryGraphs;
        private SerializedProperty instantiatedSecondaryGraphs;
        private Ai ai;


        private void OnEnable()
        {
            ai = target as Ai;
            graphProp = serializedObject.FindProperty("aiGraph");
            contextProviderProp = serializedObject.FindProperty("contextProvider");
            stepsPerUpdateProp = serializedObject.FindProperty("graphStepsPerUpdate");
            dontHideInHierarchyProp = serializedObject.FindProperty("dontHideInHierarchy");
            updateFrequencyProp = serializedObject.FindProperty("updateFrequency");
            secondaryGraphs = serializedObject.FindProperty("secondaryGraphs");
            instantiatedSecondaryGraphs = serializedObject.FindProperty("instantiatedSecondaryGraphs");
        }

        private bool fold;

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            //EditorGUILayout.LabelField("RV Smart AI", GUIHelpers.GuiStyle(0));

            if (!Application.isPlaying)
            {
                EditorGUI.BeginChangeCheck();
                EditorGUILayout.PropertyField(graphProp);
                if (EditorGUI.EndChangeCheck())
                {
                    if (graphProp.objectReferenceValue != null)
                    {
                        if (AiGraphEditor.CheckIfIsPartOfPrefab((graphProp.objectReferenceValue as AiGraph), true) == false)
                        {
                            Debug.LogError("Using non-prefab graphs is not allowed. Assign graph prefab here", target);
                            graphProp.objectReferenceValue = null;
                        }
                    }
                }

                EditorGUILayout.PropertyField(secondaryGraphs, true);

                // to avoid problems/confusion with prefab edito mode we dont have button in gui for opening graph
                //if (GUILayout.Button("Open graph")) NodeEditorWindow.OpenWithGraph(ai.aiGraph);

                EditorGUILayout.PropertyField(contextProviderProp);

                //fold = EditorGUILayout.Foldout(fold, "Advanced");
                //if (fold)
                {
                    EditorGUILayout.HelpBox("How many times per second update this AI's graph", MessageType.Info);
                    EditorGUILayout.IntSlider(updateFrequencyProp, 1, 8);
//                    EditorGUILayout.HelpBox(
//                        "How many steps will graph do in one update. One step is going from one stage node to another. " +
//                        "Think of this as AI speed of thinking vs performance (more steps -> faster AI). For debugging purposes you will " +
//                        "almost always want to have it at 1, so you can easily observe AI decision process step by step.",
//                        MessageType.Info);
//                    EditorGUILayout.IntSlider(stepsPerUpdateProp, 1, 20);
//                    EditorGUILayout.HelpBox(
//                        "By default runtime instances of graphs are hidden. Sometimes it may be usefull to not hide them, for example if " +
//                        "you want to save whole graph with runtime changes as prefab.",
//                        MessageType.Info);
                    EditorGUILayout.PropertyField(dontHideInHierarchyProp);
                }
            }
            else
            {
                if (ai.AiGraph == null) return;

                if (GUILayout.Button("Debug graph " + ai.AiGraph.name))
                    NodeEditorWindow.OpenWithGraph(ai.AiGraph);

                EditorGUILayout.LabelField("Debug info");
                EditorGUILayout.BeginHorizontal("box");
                EditorGUILayout.LabelField("Current node: " + ai.CurrentNode);
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal("box");
                EditorGUILayout.LabelField("Last utility: " + ai.LastUtility);
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal("box");
                EditorGUILayout.LabelField("Last task: " + ai.LastTask);
                EditorGUILayout.EndHorizontal();

                if (ai.SecondaryGraphs.Length > 0)
                {
                    EditorGUILayout.LabelField("Secondary graphs");
                    for (int i = 0; i < instantiatedSecondaryGraphs.arraySize; i++)
                    {
                        var sGraph = instantiatedSecondaryGraphs.GetArrayElementAtIndex(i);
                        EditorGUILayout.PropertyField(sGraph);
                        if (GUILayout.Button("Debug graph " + ai.SecondaryGraphs[i].name))
                        {
                            NodeEditorWindow.OpenWithGraph(ai.SecondaryGraphs[i]);
                        }
                    }
                }
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}
// Created by Ronis Vision. All rights reserved
// 25.03.2019.

using System;
using System.Collections.Generic;
using System.Linq;
using RVModules.RVSmartAI.Content.Code;
using RVModules.RVSmartAI.Editor.SelectWindows;
using RVModules.RVSmartAI.GraphElements;
using RVModules.RVSmartAI.GraphElements.Utilities;
using RVModules.RVSmartAI.Nodes;
using RVModules.RVUtilities.Extensions;
using RVModules.RVUtilities.Reflection;
using UnityEditor;
using UnityEngine;
using XNodeEditor;
using Object = UnityEngine.Object;

namespace RVModules.RVSmartAI.Editor.CustomInspectors
{
    [UnityEditor.CustomEditor(typeof(StageNode), true)]
    public class StageNodeInspector : UnityEditor.Editor
    {
        protected SerializedProperty nameProp;
        protected SerializedProperty descProp;

        private void OnEnable()
        {
            try
            {
                nameProp = serializedObject.FindProperty("_name");
                descProp = serializedObject.FindProperty("description");
            }
            catch
            {
            }
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            Undo.RecordObject(target, "inspector");

            StageNode sn = target as StageNode;
            Undo.RecordObject(sn.stage, "inspector");

            if (sn.selectedElement != null)
            {
                DrawSpecificElementInspector(sn.selectedElement);
                PrefabUtility.RecordPrefabInstancePropertyModifications(target);
                PrefabUtility.RecordPrefabInstancePropertyModifications(sn.stage);
                serializedObject.ApplyModifiedProperties();
                return;
            }

            EditorGUI.BeginChangeCheck();
            GUIHelpers.GUIDrawNameAndDescription(target, sn.GetType().Name, nameProp, descProp, out string desc);
            if (EditorGUI.EndChangeCheck()) sn.UpdateGameObjectName();
            sn.Description = desc;

            PrefabUtility.RecordPrefabInstancePropertyModifications(target);
            PrefabUtility.RecordPrefabInstancePropertyModifications(sn.stage);
            serializedObject.ApplyModifiedProperties();
        }

        private void DrawSpecificElementInspector(AiGraphElement _element)
        {
            Undo.RecordObject(_element, "inspector");
            AiUtility utility = _element as AiUtility;
            if (utility != null) AiUtilityInspector(utility);

            AiTask task = _element as AiTask;
            if (task != null) AiTaskInspector(task);
        }

        private void AiUtilityInspector(AiUtility _utility)
        {
            GUIHelpers.GUIDrawNameAndDescription(_utility, _utility.ToString(), _utility.Name, _utility.Description, out string name, out string desc);
            _utility.Name = name;
            _utility.Description = desc;

            GUIHelpers.GUIDrawFields(_utility);

            if (_utility is FixedScoreAiUtility)
            {
                serializedObject.ApplyModifiedProperties();
                return;
            }
            
            var scorers = _utility.scorers;
            var graph = _utility.AiGraph;
            var parentGraphElement = _utility as AiGraphElement;

            DrawScorers(graph, parentGraphElement, scorers);

            serializedObject.ApplyModifiedProperties();
        }

        private static void DrawScorers(AiGraph graph, AiGraphElement parentGraphElement, List<AiScorer> scorers)
        {
            EditorGUILayout.Separator();
            EditorGUILayout.Separator();

            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.LabelField("Scorers:");

            //if (!utility.AiGraph.isRuntimeDebugGraph)
            if (GUILayout.Button("+"))
            {
                var w = CreateInstance<SelectScorerWindow>();
                w.onSelectedItem = _type =>
                {
                    var ge = (IAiGraphElement) graph.CreateNewElement(_type, parentGraphElement);
                    Undo.RecordObject(parentGraphElement, "inspector");
                    parentGraphElement.AssignSubSelement(ge);
                };
            }

            EditorGUILayout.EndHorizontal();


            for (int i = 0; i < scorers.Count; i++)
            {
                var scorer = scorers[i];
                if (scorer == null) continue;
                EditorGUILayout.BeginVertical("box");
                EditorGUILayout.BeginHorizontal();

                string scorerName = ObjectNames.NicifyVariableName(scorer.GetType().Name);
                if (graph.isRuntimeDebugGraph) scorerName += $" ({scorer.lastScore})";

                EditorGUILayout.LabelField(scorerName);

                GUIHelpers.UpDownRemove(
                    () => { scorers.Move(i, MoveDirection.Up); },
                    () => { scorers.Move(i, MoveDirection.Down); },
                    () =>
                    {
                        scorers.Remove(scorer);
                        scorer.Destroy();
                    }, !scorer.CanBeRemoved());

                EditorGUILayout.EndHorizontal();
                GUIHelpers.GUIDrawFields(scorer);
                ForceUpdateButton(scorer);
                EditorGUILayout.EndVertical();
                EditorGUILayout.Separator();
                EditorGUILayout.Separator();
            }
        }

        private void AiTaskInspector(AiTask _task)
        {
            GUIHelpers.GUIDrawNameAndDescription(_task, _task.ToString(), _task.Name, _task.Description, out string name, out string desc);
            _task.Name = name;
            _task.Description = desc;

            // custom action data
            GUIHelpers.GUIDrawFields(_task);

            ForceUpdateButton(_task);

            EditorGUILayout.Separator();
            EditorGUILayout.Separator();

            if (_task is IAiTaskParams)
                AiTaskParamsInspector(_task);
            else if(_task is ITaskWithScorers)
            {
                DrawScorers(_task.AiGraph, _task, _task.taskScorers);
            }
        }
        
        private static void ForceUpdateButton(IAiGraphElement _graphElement)
        {
            if (_graphElement?.AiGraph == null) return;
            if (!_graphElement.AiGraph.isRuntimeDebugGraph) return;
            if (!(_graphElement is IForceUpdate forceUpdate)) return;

            if (GUILayout.Button("Update")) forceUpdate.ForceUpdate();
        }

        private void AiTaskParamsInspector(AiTask _task)
        {
            var scorers = (_task as IAiTaskParams).GetScorers();

            EditorGUILayout.Separator();
            EditorGUILayout.Separator();

            //scorers
            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.LabelField("Scorers:");

            //if (!task.AiGraph.isRuntimeDebugGraph)
            if (GUILayout.Button("+", GUILayout.Width(100)))
            {
                // todo fix, properly detect AiScorerParams type in class hierarchy instead of assuming its one up
                Type scorerType = typeof(AiScorerParams<>).MakeGenericType(_task.GetType().BaseType.GetGenericArguments()[0]);

                var okno = (SelectScorerWindow) CreateInstance(typeof(SelectScorerWindow));
                okno.types = ReflectionHelper.GetDerivedTypes(scorerType);
                var list = okno.types.ToList();
                list.AddRange(ReflectionHelper.GetDerivedTypes(typeof(AiScorerParams<object>)));
                okno.types = list.ToArray();

                okno.onSelectedItem = _type =>
                {
                    Undo.RecordObject(_task, "inspector");
                    scorers.Add(_task.AiGraph.CreateNewElement(_type, _task));
                    (_task as IAiTaskParams).SetScorers(scorers);
                };
            }

            EditorGUILayout.EndHorizontal();

            for (int i = 0; i < scorers.Count; i++)
            {
                var scorer = scorers[i];
                if (scorer == null) continue;
                EditorGUILayout.BeginVertical("box");
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(ObjectNames.NicifyVariableName(scorer.GetType().Name));
                GUIHelpers.UpDownRemove(
                    () =>
                    {
                        scorers.Move(i, MoveDirection.Up);
                        (_task as IAiTaskParams).SetScorers(scorers);
                    },
                    () =>
                    {
                        scorers.Move(i, MoveDirection.Down);
                        (_task as IAiTaskParams).SetScorers(scorers);
                    },
                    () =>
                    {
                        scorers.Remove(scorer);
                        (_task as IAiTaskParams).SetScorers(scorers);
                        (scorer as AiGraphElement).Destroy();
                    }, !(scorer as AiGraphElement).CanBeRemoved());

                EditorGUILayout.EndHorizontal();
                GUIHelpers.GUIDrawFields(scorer as AiGraphElement);
                ForceUpdateButton(scorer as AiGraphElement);
                EditorGUILayout.EndVertical();
                EditorGUILayout.Separator();
                EditorGUILayout.Separator();
            }
        }
    }
}
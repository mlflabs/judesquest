using System;
using UnityEditor;
using UnityEditor.Graphs;
using UnityEditor.ProjectWindowCallback;
using UnityEngine;
using XNodeEditor;
using Object = UnityEngine.Object;

namespace RVModules.RVSmartAI.Editor.CustomInspectors
{
    [UnityEditor.CustomEditor(typeof(AiGraph))]
    public class AiGraphInspector : UnityEditor.Editor
    {
        //private SerializedProperty graphNameProp;
        private SerializedProperty graphDescriptionProp;

        private void OnEnable()
        {
            //graphNameProp = serializedObject.FindProperty("name");
            graphDescriptionProp = serializedObject.FindProperty("description");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            Undo.RecordObject(target, "inspector");

            AiGraph graph = target as AiGraph;

            foreach (var graphNode in graph.nodes)
                Undo.RecordObject(graphNode, "inspector");

            // because prefab workflows, now opening graph instances should not be possible
//            if (GUILayout.Button("Open graph"))
//            {
//                graph.UpdateAiGraphForAllElements();
//                NodeEditorWindow.OpenWithGraph(graph);
//                //NodeEditorWindow.current.Focus();
//            }

            GUIHelpers.GUIDrawNameAndDescription(graph, graph.GetType().Name, null, graphDescriptionProp, out string desc);
            graph.description = desc;

            PrefabUtility.RecordPrefabInstancePropertyModifications(target);
            foreach (var graphNode in graph.nodes)
                PrefabUtility.RecordPrefabInstancePropertyModifications(graphNode);

            serializedObject.ApplyModifiedProperties();
        }

        [MenuItem("RVSmartAI/Open SmartAi graph window")]
        private static void OpenSmartAiGraphWindow()
        {
            if (Selection.activeGameObject != null)
            {
                var graph = Selection.activeGameObject.GetComponent<XNode.INodeGraph>();
                if (graph == null)
                {
                    Debug.LogError("Select SmartAi graph first");
                    return;
                }

                NodeEditorWindow.OpenWithGraph(graph);
                AiGraphEditor.LoadGraphPrefabContents(Selection.activeGameObject);
                return;
            }
            Debug.LogError("Select SmartAi graph first");
        }

        [MenuItem("RVSmartAI/Create new AiGraph")]
        private static void CreateNewAiGraph()
        {
            ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0, CreateInstance<CreateGraphPrefab>(), "New AiGraph", null,
                null /*SmartAiSettings.prefabPath*/);
        }

        private class CreateGraphPrefab : EndNameEditAction
        {
            public override void Action(int instanceId, string pathName, string resourceFile)
            {
                GameObject go = new GameObject("AiGraph");
                var graph = go.AddComponent<AiGraph>();

                var path = pathName + ".prefab";
                Object asset = null;

                try
                {
                    PrefabUtility.SaveAsPrefabAssetAndConnect(go, path, InteractionMode.UserAction);
                    asset = AssetDatabase.LoadAssetAtPath<Object>(pathName + ".prefab");
                }
                catch (Exception e)
                {
                    DestroyImmediate(go);
                    Debug.LogError(e);
                    return;
                }

                ProjectWindowUtil.ShowCreatedAsset(asset);
                DestroyImmediate(go);
                OpenSmartAiGraphWindow();
            }
        }
    }
}
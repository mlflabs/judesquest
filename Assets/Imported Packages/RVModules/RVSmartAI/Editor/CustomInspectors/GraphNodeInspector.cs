// Created by Ronis Vision. All rights reserved
// 04.07.2020.

using RVModules.RVSmartAI.Nodes;
using UnityEditor;

namespace RVModules.RVSmartAI.Editor.CustomInspectors
{
    [UnityEditor.CustomEditor(typeof(GraphNode), true)]
    public class GraphNodeInspector : UnityEditor.Editor
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

            GraphNode gn = target as GraphNode;
            
            EditorGUI.BeginChangeCheck();
            GUIHelpers.GUIDrawNameAndDescription(target, gn.GetType().Name, nameProp, descProp, out string desc);
            if (EditorGUI.EndChangeCheck()) gn.UpdateGameObjectName();

            gn.Description = desc;

            PrefabUtility.RecordPrefabInstancePropertyModifications(target);
            serializedObject.ApplyModifiedProperties();
        }
    }
}
// Created by Ronis Vision. All rights reserved
// 02.04.2019.

using System.Linq;
using UnityEditor;
using UnityEngine;
using XNode;
using XNodeEditor;

namespace RVModules.RVSmartAI.Editor
{
    public static class RuntimeDebugGraph
    {
        [InitializeOnLoadMethod()]
        public static void AddOpenGraphCallbackToSelectionChange()
        {
            Selection.selectionChanged += () =>
            {
                var selectedGraph = Selection.activeObject as AiGraph;
                if (selectedGraph == null) return;

                if (AssetDatabase.Contains(selectedGraph) || !Application.isPlaying)
                    return;

                // if you want to use one window 
                //NodeEditorWindow w = GetWindow(typeof(NodeEditorWindow), false, "SmartAIGraph", true) as NodeEditorWindow;
                // 
                NodeEditorWindow w = GetDebugWindow();
                if (w == null) w = EditorWindow.CreateInstance<NodeEditorWindow>();

                w.Show();
                w.Focus();
                AiGraphEditor.AssignGraphForCurrentNodeEditorWindow(selectedGraph);
            };
        }

        private static NodeEditorWindow GetDebugWindow()
        {
            return Resources.FindObjectsOfTypeAll<NodeEditorWindow>().FirstOrDefault(e =>
            {
                var g = e.graph as AiGraph;
                if (g == null) return false;
                return g.isRuntimeDebugGraph;
            });
        }

        [InitializeOnLoadMethod()]
        public static void CloseRuntimeGraphs()
        {
            EditorApplication.playModeStateChanged += CloseRuntimeGraphs;
        }

        // closes debug window efter exiting from play mode
        private static void CloseRuntimeGraphs(PlayModeStateChange state)
        {
            if (state != PlayModeStateChange.ExitingPlayMode) return;
            if (GetDebugWindow() == null) return;
            AiGraphEditor.AssignGraphForCurrentNodeEditorWindow(null);
            //GetDebugWindow().graph = null;
        }
    }
}
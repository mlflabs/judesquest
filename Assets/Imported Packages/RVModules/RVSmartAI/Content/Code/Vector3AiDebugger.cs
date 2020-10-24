// Created by Ronis Vision. All rights reserved
// 23.08.2019.

using RVModules.RVUtilities;
using UnityEngine;

namespace RVModules.RVSmartAI.Content.Code
{
    /// <summary>
    /// Renders score results numbers in scene view for Vector3 tasks/scorers. Label is rendered at Vector3 world space position
    /// and is it's float value(sum for this Vector3 from all results)
    /// </summary>
    [RequireComponent(typeof(Ai))] [DefaultExecutionOrder(200)]
    public class Vector3AiDebugger : MonoBehaviour
    {
#if UNITY_EDITOR
        [HideInInspector]
        public AiGraph graph;

        public Color textColor = Color.white;
        public Color backgroundColor = Color.black;

        private void OnDrawGizmos()
        {
            if (!Application.isPlaying || !enabled || graph == null) return;
            foreach (var graphDebugValue in graph.debugValues)
            {
                if (!(graphDebugValue.Key is Vector3)) continue;
                DebugUtilities.DrawString(graphDebugValue.Value.ToString("n2"), (Vector3) graphDebugValue.Key, textColor, backgroundColor);
            }
        }

        private void Awake()
        {
            graph = GetComponent<Ai>().AiGraph;
        }
#endif
    }
}
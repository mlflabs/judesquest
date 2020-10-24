// Created by Ronis Vision. All rights reserved
// 18.10.2019.

using UnityEditor;
using UnityEngine;

namespace RVModules.RVUtilities.FilesManagement
{
    [CustomEditor(typeof(CopyComponentsHierarchy))]
    public class CopyComponentsHierarchyEditor : Editor
    {
        private CopyComponentsHierarchy tool;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            tool = target as CopyComponentsHierarchy;

            if (GUILayout.Button("Copy components"))
            {
                tool.Work();
            }
        }
    }
}
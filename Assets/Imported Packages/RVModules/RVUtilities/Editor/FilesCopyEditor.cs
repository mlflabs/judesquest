// Created by Ronis Vision. All rights reserved
// 09.10.2019.

using UnityEditor;
using UnityEngine;

namespace RVModules.RVUtilities.FilesManagement
{
    [CustomEditor(typeof(FoldersCopyConfig))]
    public class FilesCopyEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("Copy"))
            {
                FoldersCopyConfig foldersCopyConfig = target as FoldersCopyConfig;
                CopyFoldersTool.CopyModulesSourceFilesToTargets(foldersCopyConfig);
            }
        }
    }
}
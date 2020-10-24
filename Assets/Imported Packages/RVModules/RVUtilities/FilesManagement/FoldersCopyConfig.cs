// Created by Ronis Vision. All rights reserved
// 22.08.2019.

using UnityEngine;

namespace RVModules.RVUtilities.FilesManagement
{
    [CreateAssetMenu] public class FoldersCopyConfig : ScriptableObject
    {
        public Object[] files;
        public string[] targetPaths;
        public string matchPattern = "*.*";
        public string[] excludes = {".meta"};
        public string[] folderExcludes = {"test", "tests"};
        public bool putAssembliesIntoOwnFolders = false;
    }
}
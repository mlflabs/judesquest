// Created by Ronis Vision. All rights reserved
// 27.03.2019.

using UnityEngine;

namespace RVModules.RVSmartAI.Editor
{
    public class GUIStyles : ScriptableObject
    {
        public GUIStyle[] styles;
        public GUIStyle[] debugStyles;

        public static GUIStyle[] cachedStyles;
        public static GUIStyle[] cachedDebugStyles;
        
        public static GUIStyle GetGUIStyle(int _id) 
        {
            if (cachedStyles == null)
                cachedStyles = Resources.Load<GUIStyles>("GUIStyles").styles;
            return cachedStyles[_id];
        }
        
        public static GUIStyle GetGUIDebugStyle(int _id)
        {
            if (cachedDebugStyles == null)
                cachedDebugStyles = Resources.Load<GUIStyles>("GUIDebugStyles").styles;
            return cachedDebugStyles[_id];
        } 
    }
}
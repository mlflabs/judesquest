// Created by Ronis Vision. All rights reserved
// 27.03.2019.

using System;
using System.Collections.Generic;
using System.Linq;
using RVModules.RVUtilities.Reflection;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace RVModules.RVSmartAI.Editor.SelectWindows
{
    /// <summary>
    /// Ta klasa tak musi byc, bo nie mozna wziac typu dynamicnznie z typ w OnGUI
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class SelectWindowBase<T> : EditorWindow where T : class
    {
        public Type[] types;

        protected virtual string Title { get; }

        public Action<Type> onSelectedItem;

        protected virtual void OnEnable()
        {
            // allow for only one opened window
            var windows = Resources.FindObjectsOfTypeAll(GetType());
            foreach (var w in windows)
            {
                if (w != this) ((EditorWindow) w).Close();
            }

            ShowUtility();

            titleContent = new GUIContent
            {
                text = Title
            };
            types = ReflectionHelper.GetDerivedTypes(typeof(T));
        }

        private string searchText = "";

        private void OnGUI()
        {
            Repaint();
            searchText = EditorGUILayout.TextField(searchText);
            var t = types.OrderBy(_type => _type.Name);
            foreach (var type in t)
            {
                var nameToDisplay = NameToDisplay(type);
                if (!nameToDisplay.ToUpper().Contains(searchText.ToUpper())) continue;
                if (!GUILayout.Button(nameToDisplay, GUIHelpers.GuiStyle(4))) continue;
                onSelectedItem?.Invoke(type);
                Close();
            }
        }

        protected virtual string NameToDisplay(Type type) => type.Name;
    }
}
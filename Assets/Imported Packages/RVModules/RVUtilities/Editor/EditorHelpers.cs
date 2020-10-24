// Created by Ronis Vision. All rights reserved
// 04.07.2020.

using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using Object = UnityEngine.Object;

namespace RVModules.RVUtilitiesEditor
{
    public static class EditorHelpers
    {
        #region Public methods

        /// <summary>
        /// Zero is smallest (will still make space)
        /// </summary>
        public static void VerticalSpace(int size) => GUILayoutUtility.GetRect(0, size);

        public static void DrawProperties(Dictionary<string, SerializedProperty> _dictionary)
        {
            foreach (var serializedProperty in _dictionary)
                EditorGUILayout.PropertyField(serializedProperty.Value, true);
        }

        public static void AddProperties(Dictionary<string, SerializedProperty> _dictionary, SerializedObject _serializedObject, params string[] _propNames)
        {
            foreach (var propName in _propNames) AddProperty(_dictionary, propName, _serializedObject);
        }

        public static void AddProperty(Dictionary<string, SerializedProperty> _dictionary, string _propName, SerializedObject _serializedObject) =>
            _dictionary.Add(_propName, _serializedObject.FindProperty(_propName));

        public static ReorderableList InitReorderableList(SerializedProperty _serializedProperty, SerializedObject _serializedObject)
        {
            var reorderableList = new ReorderableList(_serializedObject, _serializedProperty, true, false, true, true);
            reorderableList.drawElementCallback += (_rect, _index, _active, _focused) =>
            {
                EditorGUI.PropertyField(_rect, _serializedProperty.GetArrayElementAtIndex(_index), new GUIContent($"Waypoint {_index + 1}"), true);
            };
            reorderableList.onSelectCallback += _list => { };
            return reorderableList;
        }


        public static void ChangeGuiColorsTemporarily(Action _action, params GuiColorChange[] colorChanges)
        {
            foreach (var guiColorChange in colorChanges)
                switch (guiColorChange.type)
                {
                    case GuiColorType.Color:
                        guiColorChange.orgColor = GUI.color;
                        GUI.color = guiColorChange.color;
                        break;
                    case GuiColorType.Content:
                        guiColorChange.orgColor = GUI.contentColor;
                        GUI.contentColor = guiColorChange.color;
                        break;
                    case GuiColorType.Background:
                        guiColorChange.orgColor = GUI.backgroundColor;
                        GUI.backgroundColor = guiColorChange.color;
                        break;
                    case GuiColorType.LabelNormalText:
                        guiColorChange.orgColor = EditorStyles.label.normal.textColor;
                        EditorStyles.label.normal.textColor = guiColorChange.color;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

            //EditorStyles.label.normal.textColor = Color.white;
            try
            {
                _action.Invoke();
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }

            foreach (var guiColorChange in colorChanges)
                switch (guiColorChange.type)
                {
                    case GuiColorType.Color:
                        GUI.color = guiColorChange.orgColor;
                        break;
                    case GuiColorType.Content:
                        GUI.contentColor = guiColorChange.orgColor;
                        break;
                    case GuiColorType.Background:
                        GUI.backgroundColor = guiColorChange.orgColor;
                        break;
                    case GuiColorType.LabelNormalText:
                        EditorStyles.label.normal.textColor = guiColorChange.orgColor;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
        }

        public static void WrapInBox(Action _action, int indentationLevel = 1, int internalSpaces = 1, int externalSpacesAfter = 0)
        {
            for (var i = 0; i < indentationLevel; i++) EditorGUI.indentLevel++;
            EditorGUILayout.BeginVertical("box");
            for (var i = 0; i < internalSpaces; i++) VerticalSpace(0);
            _action();
            for (var i = 0; i < internalSpaces; i++) VerticalSpace(0);
            EditorGUILayout.EndVertical();
            for (var i = 0; i < externalSpacesAfter; i++) VerticalSpace(0);
            for (var i = 0; i < indentationLevel; i++) EditorGUI.indentLevel--;
        }

        /// <summary>
        /// Instantiates prefab with connection to it, no matter if it's prefab instance, or prefab asset, or not prefab at all
        /// </summary>
        public static GameObject InstantiatePrefab(GameObject prefab)
        {
            GameObject charGo = null;
            if (PrefabUtility.IsAnyPrefabInstanceRoot(prefab))
                charGo = PrefabUtility.InstantiatePrefab(PrefabUtility.GetCorrespondingObjectFromSource(prefab)) as GameObject;
            else if (PrefabUtility.IsPartOfPrefabAsset(prefab))
                charGo = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
            else
                charGo = Object.Instantiate(prefab);
            
            return charGo;
        }

        public static void ChangeSerializedPropertyValue(Object _object, string property, Action<SerializedProperty> _action)
        {
            var so = new SerializedObject(_object);
            so.Update();
            var prop = so.FindProperty(property);
            if (prop == null) return;
            _action(prop);
            so.ApplyModifiedProperties();
        }

        #endregion
    }

    public enum GuiColorType
    {
        Color,
        Content,
        Background,
        LabelNormalText,
    }

    public class GuiColorChange
    {
        #region Fields

        public GuiColorType type;
        public Color color;
        public Color orgColor;

        #endregion

        public GuiColorChange(GuiColorType _type, Color _color)
        {
            type = _type;
            color = _color;
        }
    }
}
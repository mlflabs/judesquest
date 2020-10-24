// Created by Ronis Vision. All rights reserved
// 26.03.2019.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using RVModules.RVSmartAI.Nodes;
using RVModules.RVUtilities.Reflection;
using UnityEditor;
using UnityEngine;
using XNode;
using Object = UnityEngine.Object;

namespace RVModules.RVSmartAI.Editor
{
    public static class GUIHelpers
    {
        private static int buttonSize = 18;

        public static void UpDownRemove(Action _up, Action _down, Action _remove, bool _nonActive = false)
        {
            var c = GUI.color;

            // up
            if (GUILayout.Button(UpButton(), GuiStyle(1), GUILayout.MaxWidth(buttonSize), GUILayout.MaxHeight(buttonSize)))
                _up();
            // down
            if (GUILayout.Button(DownButton(), GuiStyle(1), GUILayout.MaxWidth(buttonSize), GUILayout.MaxHeight(buttonSize)))
                _down();

            if (_nonActive) GUI.color = Color.gray;

            // remove
            if (GUILayout.Button(RemoveButton(), GuiStyle(1), GUILayout.MaxWidth(buttonSize), GUILayout.MaxHeight(buttonSize)) && !_nonActive)
                _remove();

            if (_nonActive)
                GUI.color = c;
        }

        public static Texture utilityIcon()
        {
            return Resources.Load<Texture>("gui/utilityIcon");
        }

        public static Texture ActionIcon()
        {
            return Resources.Load<Texture>("gui/actionIcon");
        }

        public static Texture RemoveButton()
        {
            return Resources.Load<Texture>("gui/removeButton");
        }

        public static Texture UpButton()
        {
            return Resources.Load<Texture>("gui/upButton");
        }

        public static Texture DownButton()
        {
            return Resources.Load<Texture>("gui/DownButton");
        }

        public static Texture AddTaskButton()
        {
            return Resources.Load<Texture>("gui/AddTaskButton");
        }

        public static GUIStyle GuiStyle(int _id)
        {
            return GUIStyles.GetGUIStyle(_id);
        }

        public static GUIStyle GuiDebugStyle(int _id)
        {
            return GUIStyles.GetGUIDebugStyle(_id);
        }

        public static void GUIDrawNameAndDescription(Object target, string title, SerializedProperty nameProp, SerializedProperty descProp, out string desc)
        {
            Undo.RecordObject(target, "inspector");

            EditorGUILayout.LabelField(ObjectNames.NicifyVariableName(title), new GUIStyle() {fontSize = 15, fontStyle = FontStyle.Bold});
            EditorGUILayout.Separator();

            if (nameProp != null) EditorGUILayout.PropertyField(nameProp);

            EditorGUILayout.LabelField("Description");
            desc = EditorGUILayout.TextArea(descProp.stringValue, GUILayout.MinHeight(50));

            EditorGUILayout.Separator();
            EditorGUILayout.Separator();
        }

        public static void GUIDrawNameAndDescription(Object target, string title, string nameProp, string descProp, out string _name, out string desc)
        {
            Undo.RecordObject(target, "inspector");

            EditorGUILayout.LabelField(ObjectNames.NicifyVariableName(title), new GUIStyle() {fontSize = 15, fontStyle = FontStyle.Bold});
            EditorGUILayout.Separator();

            _name = EditorGUILayout.TextField("Name", nameProp);

            EditorGUILayout.LabelField("Description");
            desc = EditorGUILayout.TextArea(descProp, GUILayout.MinHeight(50));

            EditorGUILayout.Separator();
            EditorGUILayout.Separator();
        }

        /// <summary>
        /// Automatic undo and dirty handling - Unity proper way if serialisation allows for it
        /// </summary>
        /// <param name="_object"></param>
        /// <param name="_serializedObject"></param>
        public static void GUIDrawFields(Object _object)
        {
            if (_object == null) return;
            SerializedObject serializedObject = new SerializedObject(_object);

            serializedObject.Update();
            SmartAiExposeField exposeFieldattribute = null;
            foreach (var fieldInfo in _object.GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Default))
            {
                bool draw = false;
                foreach (var fieldInfoAttribute in fieldInfo.CustomAttributes)
                {
                    if (fieldInfoAttribute.AttributeType == typeof(SmartAiExposeField))
                    {
                        exposeFieldattribute = fieldInfo.GetCustomAttribute(typeof(SmartAiExposeField)) as SmartAiExposeField;
                        draw = true;
                        break;
                    }
                }

                if (!draw) continue;
                var p = serializedObject.FindProperty(fieldInfo.Name);

                if (!string.IsNullOrEmpty(exposeFieldattribute.description))
                {
                    EditorGUILayout.BeginVertical("box");
                    EditorGUILayout.LabelField(exposeFieldattribute.description);
                    EditorGUILayout.PropertyField(p, true);
                    EditorGUILayout.EndVertical();
                }
                else
                    EditorGUILayout.PropertyField(p, true);
            }

            serializedObject.ApplyModifiedProperties();
        }

        /// <summary>
        /// obsolete
        /// manual overload for abstract classes, when Unity cannot serialize stuff we have to do all the gui work manually 
        /// </summary>
        /// <param name="_object"></param>
//        public static void GUIDrawFields(Object _object)
//        {
//            foreach (var fieldInfo in ReflectionHelper.GetFields(_object.GetType()))
//            {
//                bool draw = false;
//                var exposeFieldattribute = fieldInfo.GetCustomAttribute(typeof(SmartAiExposeField)) as SmartAiExposeField;
//                if (exposeFieldattribute == null) continue;
//
//                draw = true;
//
//                if (!draw) continue;
//
//                if (fieldInfo.FieldType != typeof(float) && fieldInfo.FieldType != typeof(int) && fieldInfo.FieldType != typeof(bool) &&
//                    fieldInfo.FieldType != typeof(AnimationCurve))
//                    continue;
//
//                if (_object == null) continue;
//
//                // handle undo
//                Undo.RecordObject(_object, "inspector");
//
//                // own, more complex configuration gui
//                EditorGUILayout.BeginVertical("box");
//                if (!string.IsNullOrEmpty(exposeFieldattribute.description))
//                    EditorGUILayout.LabelField(exposeFieldattribute.description);
//                EditorGUILayout.BeginHorizontal();
//
//                if (fieldInfo.FieldType == typeof(float))
//                {
//                    fieldInfo.SetValue(_object,
//                        EditorGUILayout.FloatField(ObjectNames.NicifyVariableName(fieldInfo.Name), (float) fieldInfo.GetValue(_object)));
//                }
//                else if (fieldInfo.FieldType == typeof(int))
//                {
//                    fieldInfo.SetValue(_object, EditorGUILayout.IntField(ObjectNames.NicifyVariableName(fieldInfo.Name), (int) fieldInfo.GetValue(_object)));
//                }
//                else if (fieldInfo.FieldType == typeof(bool))
//                {
//                    fieldInfo.SetValue(_object, EditorGUILayout.Toggle(ObjectNames.NicifyVariableName(fieldInfo.Name), (bool) fieldInfo.GetValue(_object)));
//                }
//                else if (fieldInfo.FieldType == typeof(AnimationCurve))
//                {
//                    fieldInfo.SetValue(_object,
//                        EditorGUILayout.CurveField(ObjectNames.NicifyVariableName(fieldInfo.Name), (AnimationCurve) fieldInfo.GetValue(_object),
//                            GUILayout.Height(50)));
//                }
//
//                EditorGUILayout.EndHorizontal();
//                EditorGUILayout.EndVertical();
//                //EditorGUILayout.Separator();
//            }
//        }
    }
}
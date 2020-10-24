// Created by Ronis Vision. All rights reserved
// 26.03.2019.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace RVModules.RVUtilities.Reflection
{
    /// <summary>
    /// 
    /// </summary>
    public static class ReflectionHelper
    {
        /// <summary> Get FieldInfo of a field, including those that are private and/or inherited </summary>
        public static FieldInfo GetFieldInfo(Type type, string fieldName)
        {
            // If we can't find field in the first run, it's probably a private field in a base class.
            FieldInfo field = type.GetField(fieldName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            // Search base classes for private fields only. Public fields are found above
            if (field == null)
                field = type.GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance);
            return field;
        }

        public static Type GetTypeByName(string _name, Type[] _allTypes = null) 
        {
            if (_allTypes == null)
            {
                Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
                foreach (var assembly in assemblies)
                {
                    Type[] types = assembly.GetTypes();
                    if (TypeByName(types, out var type)) return type;
                }
            }
            else
            {
                if (TypeByName(_allTypes, out var type)) return type;
            }

            return null;

            bool TypeByName(Type[] types, out Type type)
            {
                type = null;
                foreach (var type1 in types)
                {
                    if (type1.Name == _name)
                    {
                        type = type1;
                        return true;
                    }
                }

                return false;
            }
        }
        
        public static object CreateObjectFromName(string _className)
        {
            Type t = GetTypeByName(_className);
            return Activator.CreateInstance(t);
        }

        /// <summary> Get all classes deriving from baseType via reflection </summary>
        public static Type[] GetDerivedTypes(Type baseType)
        {
            List<Type> types = new List<Type>();
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (Assembly assembly in assemblies)
            foreach (var type in assembly.GetTypes().Where(t => !t.IsAbstract && baseType.IsAssignableFrom(t) && t != baseType).ToArray())
                types.Add(type);
            return types.ToArray();
        }

        public static Type[] GetDerivedGenericTypes(Type baseType)
        {
            List<Type> types = new List<Type>();
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (Assembly assembly in assemblies)
                types.AddRange(assembly.GetTypes().Where(t => t != baseType && IsAssignableToGenericType(t, baseType)));
            return types.ToArray();
        }

        public static bool IsAssignableToGenericType(Type givenType, Type genericType)
        {
            var interfaceTypes = givenType.GetInterfaces();

            foreach (var it in interfaceTypes)
            {
                if (it.IsGenericType && it.GetGenericTypeDefinition() == genericType)
                    return true;
            }

            if (givenType.IsGenericType && givenType.GetGenericTypeDefinition() == genericType)
                return true;

            Type baseType = givenType.BaseType;
            if (baseType == null) return false;

            return IsAssignableToGenericType(baseType, genericType);
        }

        public static string ExtractString(string s, string tag)
        {
            // You should check for errors in real-world code, omitted for brevity
            var startTag = "<" + tag + ">";
            int startIndex = s.IndexOf(startTag) + startTag.Length;
            int endIndex = s.IndexOf("</" + tag + ">", startIndex);
            return s.Substring(startIndex, endIndex - startIndex);
        }

        #region other
        public class ReflectionSearchIgnoreAttribute : Attribute
        {
            public ReflectionSearchIgnoreAttribute()
            {
            }
        }

        /// <summary>
        /// Gets all non-abstract types extending the given base type
        /// </summary>
        public static Type[] getSubTypes(Type baseType)
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                .Where((Assembly assembly) => assembly.FullName.Contains("Assembly"))
                .SelectMany((Assembly assembly) => assembly.GetTypes()
                    .Where((Type T) =>
                        (T.IsClass && !T.IsAbstract)
                        && T.IsSubclassOf(baseType)
                        && !T.GetCustomAttributes(typeof(ReflectionSearchIgnoreAttribute), false).Any())
                ).ToArray();
        }

        /// <summary>
        /// Gets all non-abstract types extending the given base type and with the given attribute
        /// </summary>
        public static Type[] getSubTypes(Type baseType, Type hasAttribute)
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                .Where((Assembly assembly) => assembly.FullName.Contains("Assembly"))
                .SelectMany((Assembly assembly) => assembly.GetTypes()
                    .Where((Type T) =>
                        (T.IsClass && !T.IsAbstract)
                        && T.IsSubclassOf(baseType)
                        && T.GetCustomAttributes(hasAttribute, false).Any()
                        && !T.GetCustomAttributes(typeof(ReflectionSearchIgnoreAttribute), false).Any())
                ).ToArray();
        }

        /// <summary>
        /// Returns all fields that should be serialized in the given type
        /// </summary>
        public static FieldInfo[] getSerializedFields(Type type)
        {
            return type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .Where((FieldInfo field) =>
                    (field.IsPublic && !field.GetCustomAttributes(typeof(NonSerializedAttribute), true).Any())
                    || field.GetCustomAttributes(typeof(SerializeField), true).Any()
                    && !field.GetCustomAttributes(typeof(ReflectionSearchIgnoreAttribute), false).Any())
                .ToArray();
        }

        /// <summary>
        /// Returns all fields that should be serialized in the given type, minus the fields declared in or above the given base type
        /// </summary>
        public static FieldInfo[] getSerializedFields(Type type, Type hiddenBaseType)
        {
            return type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .Where((FieldInfo field) =>
                    (hiddenBaseType == null || !field.DeclaringType.IsAssignableFrom(hiddenBaseType))
                    && ((field.IsPublic && !field.GetCustomAttributes(typeof(NonSerializedAttribute), true).Any())
                        || field.GetCustomAttributes(typeof(SerializeField), true).Any()
                        && !field.GetCustomAttributes(typeof(ReflectionSearchIgnoreAttribute), false).Any()))
                .ToArray();
        }

        /// <summary>
        /// Gets all fields in the classType of the specified fieldType
        /// </summary>
        public static FieldInfo[] getFieldsOfType(Type classType, Type fieldType)
        {
            return classType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .Where((FieldInfo field) =>
                    field.FieldType == fieldType || field.FieldType.IsSubclassOf(fieldType)
                    && !field.GetCustomAttributes(typeof(ReflectionSearchIgnoreAttribute), false).Any())
                .ToArray();
        }
        #endregion

        public static Type[] GetTypesFromAllAssemblies()
        {
            List<Type> types = new List<Type>();
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in assemblies)
            {
                types.AddRange(assembly.GetTypes());
            }

            return types.ToArray();
        }
    }
}
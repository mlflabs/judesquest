// Created by Ronis Vision. All rights reserved
// 15.03.2019.

using System.Threading;
using UnityEngine;

namespace RVModules.RVUtilities
{
    /// <summary>
    /// MonoBehaviour based singleton implementation
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
    {
        protected static T instance;
        private static bool created = false;

        public abstract string Name { get; }

        /// <summary>
        /// Make sure to check for null after adressing Instance as it can return null, if singleton carrier game object was destroyed
        /// typically its on exiting from play mode in editor. Best is to use it with null propagation to avoid boilerplate: Singleton.Instance?.DoSomething()
        /// </summary>
        public static T Instance
        {
            get
            {
                if (instance != null) return instance;
                if (created) return null;

                instance = FindObjectOfType<T>();
                GameObject obj = null;
                if (instance == null)
                {
                    obj = new GameObject();
                    instance = obj.AddComponent<T>();
                }
                else
                {
                    obj = instance.gameObject;
                }

                DontDestroyOnLoad(obj);
                instance.SingletonInitialization();
                obj.name = instance.Name;
                created = true;
                return instance;
            }
        }

        public static void DestroySingleton()
        {
            var instanc = Instance;    
            var go = Instance.gameObject;
            created = false;
            DestroyImmediate(instanc);
            DestroyImmediate(go);
        }

        protected virtual void SingletonInitialization()
        {
        }
    }
}
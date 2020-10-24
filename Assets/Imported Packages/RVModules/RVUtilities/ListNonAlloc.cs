// Created by Ronis Vision. All rights reserved
// 17.08.2019.

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RVModules.RVUtilities
{
    /// <summary>
    /// Simple array wrapper, with some very basic convenience functions(add, clear) to avoid GC that normal C# list causes
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable] public class ListNonAlloc<T>
    {
        [SerializeField]
        private T[] arr = null;

        public T[] Array => arr;

        [HideInInspector]
        public bool allowResize = true;

        public T this[int _id]
        {
            get { return arr[_id]; }
            set { arr[_id] = value; }
        }

        public ListNonAlloc()
        {
            arr = new T[0];
        }

        public ListNonAlloc(int _size = 1, bool _allowResize = true)
        {
            arr = new T[_size];
            allowResize = _allowResize;
        }

        public static implicit operator T[] (ListNonAlloc<T> _list) => _list.arr;

        public void AddRange(ICollection<T> _items)
        {
            foreach (var item in _items)
            {
                if (item == null) continue;
                Add(item);
            }
        }

        public void Add(T item)
        {
            for (int i = 0; i < arr.Length; i++)
            {
                if (arr[i] == null)
                {
                    arr[i] = item;
                    return;
                }
            }

            if (!allowResize) return;

            // copy
            var newLength = arr.Length + 1;
            T[] tempArr = new T[newLength];
            for (int i = 0; i < newLength; i++)
                if (i < arr.Length)
                    tempArr[i] = arr[i];
                else
                    tempArr[i] = default(T);
            arr = new T[newLength];
            for (int i = 0; i < newLength; i++)
                arr[i] = tempArr[i];

            arr[arr.Length - 1] = item;
        }

        public void Clear()
        {
            for (int i = 0; i < arr.Length; i++)
                arr[i] = default;
        }
    }
}
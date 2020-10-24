// Created by Ronis Vision. All rights reserved
// 14.10.2019.

using System;
using System.Collections.Concurrent;

namespace RVModules.RVUtilities
{
    /// <summary>
    /// Thread safe object pool
    /// </summary>
    public class ObjectPool<T> where T : IPoolable
    {
        private ConcurrentQueue<T> objects;
        private Func<T> objectGenerator;

        public ObjectPool(Func<T> _objectGenerator)
        {
            objects = new ConcurrentQueue<T>();
            objectGenerator = _objectGenerator ?? throw new ArgumentNullException(nameof(_objectGenerator));
        }

        public T GetObject()
        {
            if (objects.TryDequeue(out var item))
            {
                item.OnSpawn?.Invoke();
                return item;
            }

            item = objectGenerator();
            item.OnDespawn += () => PutObject(item);
            item.OnSpawn?.Invoke();
            return item;
        }

        public void PutObject(T _item)
        {
            //_item.OnDespawn?.Invoke();
            objects.Enqueue(_item);
        }

        public void Clear() => objects = new ConcurrentQueue<T>();
    }

    public interface IPoolable
    {
        Action OnSpawn { get; set; }
        Action OnDespawn { get; set; }
    }
}
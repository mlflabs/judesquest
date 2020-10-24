// Created by Ronis Vision. All rights reserved
// 20.10.2019.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using RVModules.RVUtilities;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Object = UnityEngine.Object;

namespace SREngine
{
    /// <summary>
    /// todo make thread safe
    /// </summary>
    public class PoolManager : MonoBehaviour
    {
        public RvLogger logger;

        public bool createPoolsOnAwake = true;

        [SerializeField]
        private List<PoolConfig> poolsConfig = new List<PoolConfig>();

        private Dictionary<string, ObjectPool<IPoolable>> pools = new Dictionary<string, ObjectPool<IPoolable>>();

        public ObjectPool<IPoolable>[] GetAllPoolsAsArray => pools.Values.ToArray();
        public Dictionary<string, ObjectPool<IPoolable>> GetAllPool => pools;

        public bool TryGetPool(string _name, out ObjectPool<IPoolable> _pool) => pools.TryGetValue(_name, out _pool);

//        public bool TryGetPoolByPrefab(GameObject _prefab, out UnityObjectPool _pool)
//        {
//            foreach (var poolsValue in pools)
//            {
//                if (poolsValue.Value.Prefab.name != _prefab.name) continue;
//                _pool = poolsValue.Value;
//                return true;
//            }
//
//            _pool = null;
//            return false;
//        }

        public bool TryGetObject<T>(string _name, out T _object) where T : IPoolable
        {
            if (pools.TryGetValue(_name, out ObjectPool<IPoolable> pool))
            {
                _object = (T)pool.GetObject();
                return true;
            }

            logger.LogWarning($"There is no pool named {_name}");
            _object = default(T);
            return false;
        }

        /// <summary>
        /// Returns true is succesfully returned object to pool 
        /// </summary>
        public bool ReturnObject(string poolName, IPoolable poolable)
        {
            if (poolable == null) throw new ArgumentNullException();

            if (pools.TryGetValue(poolName, out ObjectPool<IPoolable> _pool))
            {
                _pool.PutObject(poolable);
                return true;
            }

            logger.LogWarning($"There is no pool for object {poolable}", poolable as Object);
            return false;
        }

        public void CreatePools()
        {
            if (PoolsCreated)
            {
                logger.LogWarning("Pools has already been created!");
                return;
            }

            var sw = Stopwatch.StartNew();
            foreach (var poolConfig in poolsConfig)
            {
                if (poolConfig.prefab == null)
                {
                    Debug.LogError($"pool {poolConfig.optionalName} prefab cannot be null!");
                    continue;
                }

                IPoolable ipoolable = poolConfig.prefab.GetComponent<IPoolable>();

                if (ipoolable == null)
                {
                    Debug.LogError($"prefab {poolConfig.prefab} doesn't have IPoolable component on it!");
                    continue;
                }

                var newPool = new ObjectPool<IPoolable>(
                    () => ((GameObject) Instantiate(poolConfig.prefab, poolConfig.optionalParent)).GetComponent<IPoolable>());
                //new UnityObjectPool(poolConfig.prefab, poolConfig.initialSize, poolConfig.allowExpand, poolConfig.optionalParent);
                string poolName = poolConfig.optionalName;
                if (string.IsNullOrEmpty(poolName)) poolName = poolConfig.prefab.name;
                if (pools.TryGetValue(poolName, out var p))
                {
                    Debug.LogError($"pool with name '{poolName}' already exist!");
                    continue;
                }

                pools.Add(poolName, newPool);
            }

            logger.LogInfo($"{name} pool manager initialization: {sw.ElapsedMilliseconds}ms");
            sw.Stop();
            PoolsCreated = true;
        }

        public bool PoolsCreated { get; private set; }

        private void Awake()
        {
            if (!createPoolsOnAwake) return;
            CreatePools();
        }
    }
}
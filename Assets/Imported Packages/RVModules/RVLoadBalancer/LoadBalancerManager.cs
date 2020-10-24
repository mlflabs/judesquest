// Created by Ronis Vision. All rights reserved
// 05.11.2019.

using System;
using System.Collections.Generic;
using System.Linq;
using RVModules.RVLoadBalancer;
using RVModules.RVLoadBalancer.Debug;
using UnityEngine;

namespace RVLoadBalancer
{
    /// <summary>
    /// API NOT thread-safe!
    /// </summary>
    public class LoadBalancerManager
    {
        #region Fields

        public string name;

        private Dictionary<object, Dictionary<Action<float>, LoadBalancerConfig>> objectToActionsMap =
            new Dictionary<object, Dictionary<Action<float>, LoadBalancerConfig>>();

        internal Dictionary<LoadBalancerConfig, LoadBalancer> loadBalancersDict = new Dictionary<LoadBalancerConfig, LoadBalancer>();

        private bool debugMode;

        #endregion

        #region Properties

        public int LoadBalancersCount => loadBalancersDict.Count;
        public Dictionary<LoadBalancerConfig, LoadBalancer> LoadBalancersDict => loadBalancersDict;

        #endregion

        public LoadBalancerManager(string _name)
        {
            name = _name;
        }

        #region Public methods

        public bool TryGetLoadBalancer(LoadBalancerConfig _lbc, out LoadBalancer _loadBalancer) => loadBalancersDict.TryGetValue(_lbc, out _loadBalancer);

        /// <summary>
        /// Attaches debugger component to provided gameObject
        /// </summary>
        /// <param name="_gameObjectToAttachDebugger"></param>
        public void EnableDebug(GameObject _gameObjectToAttachDebugger, bool _destroyWhenEmpty)
        {
            if (debugMode) return;

            var debugger = _gameObjectToAttachDebugger.AddComponent<LoadBalancerManagerDebug>();
            debugger.AssignLbm(this, _destroyWhenEmpty);
            debugMode = true;
        }

        public void Tick(float _deltaTime, float _unscaledDeltaTime)
        {
            for (var i = 0; i < loadBalancersDict.Count; i++)
            {
                var lb = loadBalancersDict.ElementAt(i).Value;
                if (lb == null) continue;
                var dt = _deltaTime;
                if (lb.UseUnscaledDeltaTime) dt = _unscaledDeltaTime;
                lb.Tick(dt);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Register(object _object, Action<float> _action, int _tickFrequencyHz, bool _calculateDeltaTime = false, bool _useUnscaledDt = false)
        {
            Register(_object, _action, new LoadBalancerConfig(LoadBalancerType.TimeIntervalPerSec, _tickFrequencyHz, _calculateDeltaTime, _useUnscaledDt));
        }

        public void Register(object _object, Action<float> _action, LoadBalancerConfig _loadBalancerConfig)
        {
            if (_object == null) return;
            // this is map for every object that maps that object actions to specific lb 

            // get object dictionary for this object or
            if (!objectToActionsMap.TryGetValue(_object, out var objectDictionary))
            {
                // create dictionary for our object if it hasn't yet
                objectDictionary = new Dictionary<Action<float>, LoadBalancerConfig>();
                objectToActionsMap.Add(_object, objectDictionary);
            }
            else
                // map new Action<float> to set frequency
            if (objectDictionary.ContainsKey(_action))
            {
                // if this action and this object was already registered, unregister it, that will allow changing call frequency just by calling
                // register again 
                Unregister(_object, _action);
                Register(_object, _action, _loadBalancerConfig);
                return;
            }

            objectDictionary.Add(_action, _loadBalancerConfig);

            LoadBalancer lb;
            // check if we have lb with such frequency
            if (!loadBalancersDict.TryGetValue(_loadBalancerConfig, out lb))
            {
                lb = _loadBalancerConfig.GetLoadBalancer();
                loadBalancersDict.Add(_loadBalancerConfig, lb);
            }

            // if we have, add our new Action<float> to it
            lb.AddObject(_action);
        }

        public bool Unregister(object _object)
        {
            if (_object == null) return false;

            Dictionary<Action<float>, LoadBalancerConfig> objectDictionary;

            if (!objectToActionsMap.TryGetValue(_object, out objectDictionary)) return false;

            // loop over all actions of passed object
            foreach (var keyValuePair in objectDictionary)
            {
                loadBalancersDict[keyValuePair.Value].RemoveObject(keyValuePair.Key);
                RemoveLoadBalancer(keyValuePair);
            }

            objectToActionsMap.Remove(_object);
            return true;
        }

        /// <summary>
        /// Removes only passed _action added by _object
        /// </summary>
        public bool Unregister(object _object, Action<float> _action)
        {
            Dictionary<Action<float>, LoadBalancerConfig> objectDictionary;

            if (!objectToActionsMap.TryGetValue(_object, out objectDictionary)) return false;

            var found = false;
            // loop over all actions of passed object
            foreach (var keyValuePair in objectDictionary)
            {
                if (keyValuePair.Key != _action) continue;

                loadBalancersDict[keyValuePair.Value].RemoveObject(keyValuePair.Key);
                RemoveLoadBalancer(keyValuePair);

                objectToActionsMap[_object].Remove(_action);
                if (objectToActionsMap[_object].Count == 0) objectToActionsMap.Remove(_object);
                found = true;
                break;
            }

            return found;
        }

        private void RemoveLoadBalancer(KeyValuePair<Action<float>, LoadBalancerConfig> keyValuePair)
        {
            if (keyValuePair.Value.dontRemoveWhenEmpty) return;

            if (loadBalancersDict[keyValuePair.Value].Actions.Count == 0)
                loadBalancersDict.Remove(keyValuePair.Value);
        }

        // dont destroy empty lb is not implemented yet
        public void RemoveEmptyLoadBalancer(LoadBalancerConfig _loadBalancerConfig)
        {
            if (!loadBalancersDict.TryGetValue(_loadBalancerConfig, out LoadBalancer _loadBalancer)) return;
            if (_loadBalancer.ActionsCount > 0) return;

            loadBalancersDict.Remove(_loadBalancerConfig);
        }

        #endregion

        // old system
        //private Dictionary<object, Dictionary<Action<float>, int>> objectToActionsMap = new Dictionary<object, Dictionary<Action<float>, int>>();
        //internal Dictionary<int, TimeIntervalLoadBalancer> loadBalancersDict = new Dictionary<int, TimeIntervalLoadBalancer>();
//        /// <summary>
//        /// Register new Action
//        /// Object is owner of action
//        /// _action is method to call at _frequency (n time per second)
//        /// </summary>
//        public void Register(object _object, Action<float> _action, int _frequency)
//        {
//            // this is map for every object that maps that object actions to specific lb frequency
//            Dictionary<Action<float>, int> objectDictionary;
//
//            // get object dictionary for this object or
//            if (!objectToActionsMap.TryGetValue(_object, out objectDictionary))
//            {
//                // create dictionary for our object if it hasn't yet
//                objectDictionary = new Dictionary<Action<float>, int>();
//                objectToActionsMap.Add(_object, objectDictionary);
//            }
//            else
//                // map new Action<float> to set frequency
//            if (objectDictionary.ContainsKey(_action))
//            {
//                // if this action and this object was already registered, unregister it, that will allow changing call frequency just by calling
//                // register again 
//                Unregister(_object, _action);
//                Register(_object, _action, _frequency);
//                return;
//            }
//
//            objectDictionary.Add(_action, _frequency);
//
//            TimeIntervalLoadBalancer lb;
//            // check if we have lb with such frequency
//            if (!loadBalancersDict.TryGetValue(_frequency, out lb))
//            {
//                lb = new TimeIntervalLoadBalancer(_frequency, 1);
//                loadBalancersDict.Add(_frequency, lb);
//            }
//
//            // if we have, add our new Action<float> to it
//            lb.AddObject(_action);
//        }
//
//        /// <summary>
//        /// Removes all actions registered by _object
//        /// Returns false if object was not found
//        /// </summary>
//        public bool Unregister(object _object)
//        {
//            Dictionary<Action<float>, int> objectDictionary;
//
//            if (!objectToActionsMap.TryGetValue(_object, out objectDictionary)) return false;
//
//            // loop over all actions of passed object
//            foreach (var keyValuePair in objectDictionary)
//            {
//                loadBalancersDict[keyValuePair.Value].RemoveObject(keyValuePair.Key);
//                if (loadBalancersDict[keyValuePair.Value].ObjsToInvoke.Count == 0)
//                    loadBalancersDict.Remove(keyValuePair.Value);
//            }
//
//            objectToActionsMap.Remove(_object);
//            return true;
//        }
//
//        /// <summary>
//        /// Removes only passed _action added by _object
//        /// </summary>
//        public bool Unregister(object _object, Action<float> _action)
//        {
//            Dictionary<Action<float>, int> objectDictionary;
//
//            if (!objectToActionsMap.TryGetValue(_object, out objectDictionary)) return false;
//
//            var found = false;
//            // loop over all actions of passed object
//            foreach (var keyValuePair in objectDictionary)
//            {
//                if (keyValuePair.Key != _action) continue;
//
//                loadBalancersDict[keyValuePair.Value].RemoveObject(keyValuePair.Key);
//                if (loadBalancersDict[keyValuePair.Value].ObjsToInvoke.Count == 0)
//                    loadBalancersDict.Remove(keyValuePair.Value);
//
//                objectToActionsMap[_object].Remove(_action);
//                if (objectToActionsMap[_object].Count == 0) objectToActionsMap.Remove(_object);
//                found = true;
//                break;
//            }
//
//            return found;
//        }
    }
}
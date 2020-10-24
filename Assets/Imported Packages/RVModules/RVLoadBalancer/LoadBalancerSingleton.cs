// Created by Ronis Vision. All rights reserved
// 05.11.2019.

using System;
using RVModules.RVLoadBalancer;
using RVModules.RVUtilities;
using UnityEngine;
using Object = UnityEngine.Object;

namespace RVLoadBalancer
{
    /// <summary>
    /// Allows easy setting of frequency on per method basis.
    /// This allows for not only for calling on every object with different frequency, but it also means, all actions with same frequency will be
    /// load balanced(not updating all collection at once, but only some part of it per frame, to spread work nicely)
    /// This api is super fast, based on dictionaries, so many Register/Unregister calls are not problem.
    /// Override update method to update load balancers from own threads.
    /// Usage:
    /// Call register on awake/start/onEnable as you prefer
    /// LoadBalancerManager.Instance.Register(this, OncePerSecUpdate, 1);
    /// Call unregister when object is destroyed or disabled - OnDestroy/OnDisable
    /// LoadBalancerManager.Instance.Unregister(this);
    /// You can also turn off single methods, and enable them again later
    /// LoadBalancerManager.Instance.Unregister(this, OncePerSecUpdate);
    /// 
    /// </summary>
    public class LoadBalancerSingleton : MonoSingleton<LoadBalancerSingleton>
    {
        #region Fields

        private LoadBalancerManager loadBalancerManager;
        private bool debugMode;

        #endregion

        #region Properties

        public override string Name => "LoadBalancerManager";

        #endregion

        #region Public methods

        public void EnableDebug()
        {
            if (debugMode) return;
            loadBalancerManager.EnableDebug(gameObject, false);
            debugMode = true;
        }

        /// <summary>
        /// Register new Action
        /// Object is owner of action
        /// _action is method to call at _frequency (n time per second)
        /// </summary>
        public void Register(object _object, Action<float> _action, int _frequency, bool _calculateDeltaTime = false, bool _useUnscaledDt = false) =>
            loadBalancerManager.Register(_object, _action, _frequency, _calculateDeltaTime, _useUnscaledDt);

        public void Register(object _object, Action<float> _action, LoadBalancerConfig _lbc) => loadBalancerManager.Register(_object, _action, _lbc);

        /// <summary>
        /// Removes all actions registered by _object
        /// </summary>
        public void Unregister(object _object) => loadBalancerManager.Unregister(_object);

        /// <summary>
        /// Removes only passed _action added by _object
        /// </summary>
        public void Unregister(object _object, Action<float> _action)
        {
            if (!loadBalancerManager.Unregister(_object, _action)) Debug.LogError($"Failed to unregister {_object}", _object as Object);
        }

        #endregion

        #region Not public methods

        protected override void SingletonInitialization() => loadBalancerManager = new LoadBalancerManager("LoadBalancerSingleton");

        private void Update() => loadBalancerManager.Tick(Time.deltaTime, Time.unscaledDeltaTime);

        #endregion
    }
}
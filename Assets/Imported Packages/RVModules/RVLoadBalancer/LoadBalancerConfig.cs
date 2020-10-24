// Created by Ronis Vision. All rights reserved
// 16.12.2019.

using System;
using RVLoadBalancer;

namespace RVModules.RVLoadBalancer
{
    [Serializable] public struct LoadBalancerConfig
    {
        public string name;
        public LoadBalancerType loadBalancerType;
        public int value;
        public bool calculateDeltaTime;
        public bool useUnscaledDeltaTime;

        internal bool dontRemoveWhenEmpty;

        internal LoadBalancer GetLoadBalancer()
        {
            LoadBalancer lb = null;

            switch (loadBalancerType)
            {
                case LoadBalancerType.EveryXFrames:
                    lb = new EveryxFramesLoadBalancer(value, calculateDeltaTime, useUnscaledDeltaTime);
                    break;
                case LoadBalancerType.TimeIntervalPerSec:
                    lb = new TimeIntervalLoadBalancer(value, 1, calculateDeltaTime, useUnscaledDeltaTime);
                    break;
                case LoadBalancerType.PercentagePerFrame:
                    lb = new PercentageLoadBalancer(value, calculateDeltaTime, useUnscaledDeltaTime);
                    break;
                case LoadBalancerType.FixedNumberPerFrame:
                    lb = new FixedNumberLoadBalancer(value, calculateDeltaTime, useUnscaledDeltaTime);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return lb;
        }

        public LoadBalancerConfig(LoadBalancerType _loadBalancerType, int _value, bool _calculateDeltaTime = false, bool _useUnscaledDeltaTime = false)
        {
            loadBalancerType = _loadBalancerType;
            value = _value;
            calculateDeltaTime = _calculateDeltaTime;
            name = "";
            dontRemoveWhenEmpty = false;
            useUnscaledDeltaTime = _useUnscaledDeltaTime;
        }

        public LoadBalancerConfig(string _name, LoadBalancerType _loadBalancerType, int _value, bool _calculateDeltaTime = false,
            bool _useUnscaledDeltaTime = false)
        {
            name = _name;
            loadBalancerType = _loadBalancerType;
            value = _value;
            calculateDeltaTime = _calculateDeltaTime;
            dontRemoveWhenEmpty = false;
            useUnscaledDeltaTime = _useUnscaledDeltaTime;
        }

        public override string ToString()
        {
            var s = $"{name} -{loadBalancerType} -({value})";
            if (calculateDeltaTime) s += " -dt";
            if (useUnscaledDeltaTime) s += " -unscaled";
            return s;
        }
    }

    public enum LoadBalancerType
    {
        /// <summary>
        /// Updates all elements every x frames
        /// </summary>
        EveryXFrames,

        /// <summary>
        /// Updates x percent of elements every tick
        /// </summary>
        PercentagePerFrame,

        /// <summary>
        /// Updates all elements x times per second
        /// </summary>
        TimeIntervalPerSec,

        /// <summary>
        /// Update x elements every tick
        /// </summary>
        FixedNumberPerFrame
    }
}
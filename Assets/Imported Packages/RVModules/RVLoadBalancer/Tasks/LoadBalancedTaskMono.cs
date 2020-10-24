// Created by Ronis Vision. All rights reserved
// 24.06.2020.

using System;
using RVLoadBalancer;
using RVModules.RVLoadBalancer;
using RVModules.RVLoadBalancer.Tasks;
using UnityEngine;

namespace RVModules.RVSmartAI
{
    public abstract class LoadBalancedTaskMono : LoadBalancedTaskMonoBase, I<DummyMonoBehaviour>, I<LoadBalancedTaskWrapper>
    {
        private void Awake()
        {
        }

        protected virtual void OnDestroy()
        {
            Task.FinishTask();
        }
    }
}
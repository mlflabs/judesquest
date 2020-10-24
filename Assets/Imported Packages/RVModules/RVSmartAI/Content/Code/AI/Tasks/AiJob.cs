// Created by Ronis Vision. All rights reserved
// 06.07.2020.

using System;
using RVModules.RVLoadBalancer;
using RVModules.RVLoadBalancer.Tasks;
using UnityEngine;
using RVModules.RVSmartAI;

namespace RVModules.RVSmartAI.Content.Code.AI.Tasks
{
    /// <summary>
    /// Base class for ai jobs
    /// </summary>
    public abstract class AiJob : AiJobBase, I<AiAgentBaseTask>, I<LoadBalancedTask>, ILoadBalancedTask
    {
        #region Fields

        private Action onTaskStart;
        private Action onTaskFinish;
                                                                
        [SmartAiExposeField]
        [SerializeField]
        protected TaskHandler jobHandler;

        [SmartAiExposeField]
        [SerializeField]
        protected LoadBalancerConfig loadBalancingConfig;

        [SmartAiExposeField]
        [SerializeField]
        [HideInInspector]
        protected int jobPriority;

        [SmartAiExposeField]
        [SerializeField]
        [HideInInspector]
        protected string jobLayer;

        #endregion

        #region Not public methods

        protected sealed override void Execute(float _deltaTime)
        {
            layer = jobLayer;
            priority = jobPriority;
            jobHandler.ScheduleTask(this, loadBalancingConfig);
        }

        protected virtual string GetAiJobName() => GetType().Name;

        protected override void OnContextUpdated()
        {
            base.OnContextUpdated();
            jobHandler = (Context as IJobHandlerProvider)?.AiJobHandler;
            if (jobHandler == null) return;
            // call fake constructor to create task
            LoadBalancedTask(OnJobUpdateInternal, OnJobStart, OnJobFinish, jobPriority, GetAiJobName());
        }

        protected void FinishJob() => FinishTask();

        private void OnJobUpdateInternal(float _deltaTime)
        {
            if (Context as UnityEngine.Object == null || this == null)
            {
                FinishJob();
                return;
            }

            OnJobUpdate(_deltaTime);
        }

        protected abstract void OnJobStart();
        protected abstract void OnJobUpdate(float _deltaTime);
        protected abstract void OnJobFinish();

        #endregion
    }
}
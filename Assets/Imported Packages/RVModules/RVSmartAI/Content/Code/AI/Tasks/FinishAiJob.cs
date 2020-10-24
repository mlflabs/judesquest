// Created by Ronis Vision. All rights reserved
// 10.07.2020.

using UnityEngine;

namespace RVModules.RVSmartAI.Content.Code.AI.Tasks
{
    public class FinishAiJob : AiAgentBaseTask
    {
        #region Fields

        [SmartAiExposeField]
        [SerializeField]
        protected string jobName;

        [SmartAiExposeField]
        [SerializeField]
        protected TaskHandler jobHandler;

        #endregion

        #region Not public methods

        protected override void OnContextUpdated()
        {
            base.OnContextUpdated();
            jobHandler = (Context as IJobHandlerProvider)?.AiJobHandler;
        }

        protected override void Execute(float _deltaTime) => jobHandler.FinishTask(jobName);

        #endregion
    }
}
// Created by Ronis Vision. All rights reserved
// 10.07.2020.

using UnityEngine;

namespace RVModules.RVSmartAI.Content.Code.AI.Scorers
{
    public class IsAiJobRunning : AiAgentBaseScorer
    {
        #region Fields

        [SmartAiExposeField]
        [SerializeField]
        protected string jobName;

        [SmartAiExposeField]
        [SerializeField]
        protected float scoreNotRunning;

        [SmartAiExposeField]
        [SerializeField]
        protected TaskHandler jobHandler;

        #endregion

        #region Public methods

        public override float Score(float _deltaTime) => jobHandler.IsTaskRunning(jobName) ? score : scoreNotRunning;

        #endregion

        #region Not public methods

        protected override void OnContextUpdated()
        {
            base.OnContextUpdated();
            jobHandler = (Context as IJobHandlerProvider)?.AiJobHandler;
        }

        #endregion
    }
}
// Created by Ronis Vision. All rights reserved
// 06.07.2020.

using UnityEngine;

namespace RVModules.RVSmartAI.Content.Code.AI.Tasks
{
    /// <summary>
    /// Just example AiJob
    /// </summary>
    public class TestAiJob : AiJob
    {
        private int counter = 0;

        protected override void OnJobStart()
        {
            Debug.Log("Starting test job!");

            // reset all your variables when starting job, as job object is reaused for every next run of job!
            counter = 0;
        }

        protected override void OnJobUpdate(float _deltaTime)
        {
            Debug.Log($"Running test job, counter: {counter}");

            counter++;
            // remember to call FinishJob() at some point!
            if (counter > 5) FinishJob();
        }

        protected override void OnJobFinish()
        {
            Debug.Log("Test job finished!");
        }
    }
}
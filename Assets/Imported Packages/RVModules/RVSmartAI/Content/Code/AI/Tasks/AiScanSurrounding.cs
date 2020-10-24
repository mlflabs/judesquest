// Created by Ronis Vision. All rights reserved
// 23.08.2019.

using System;
using Object = UnityEngine.Object;

namespace RVModules.RVSmartAI.Content.Code.AI.Tasks
{
    /// <summary>
    /// Fills AiAgentGenericContext.nearbyObjects using IEnvironmentScanner in defined scanRange
    /// </summary>
    public class AiScanSurrounding : AiAgentBaseTask  
    {
        #region Fields

        [SmartAiExposeField("Radius, in meters")]
        public int scanRange = 5;

        /// <summary>
        /// How much time must pass before this AiTask can be executed
        /// </summary>
        [SmartAiExposeField("Time interval between AiTask execution, in seconds")]
        public float callInterval = 2f;

        public DateTime lastCallTime;

        protected bool called = false;

        #endregion

        #region Public methods

        protected override void Execute(float _deltaTime) 
        {
            // we cant scan if we dont have any environmentScanner
            if (environmentScanner == null) return;

            called = false;
            
            // exit early if we called it too soon
            if ((DateTime.Now - lastCallTime).TotalSeconds < callInterval) return;
            lastCallTime = DateTime.Now;

            called = true;
            
            //
            NearbyObjects.Clear();
            var entities = environmentScanner.ScanEnvironment(movement.Position, scanRange);

            for (var index = 0; index < entities.Length; index++)
            {
                var entity = entities[index];
                if (entity == null) continue;
                
                AddToNearbyObjects(entity);
            }
        }

        protected virtual void AddToNearbyObjects(Object _object)
        {
            NearbyObjects.Add(_object);
        }

        #endregion
    }
}
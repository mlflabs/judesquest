// Created by Ronis Vision. All rights reserved
// 23.08.2019.

using UnityEngine;

namespace RVModules.RVSmartAI.Content.Code.AI.Tasks.TasksParams
{
    public class MoveToBestPosNearMe : AiAgentBaseTaskParams<Vector3>
    {
        #region Fields

        [SmartAiExposeField]
        public float distToTarget = 3;

        #endregion

        #region Public methods

        protected override void Execute(float _deltaTime)
        {
            // get walkable positions close to our follow target
            var positions = movementScanner.FindWalkablePositions(movement.Position, distToTarget);

            // if we cant find any walkable position just exit, to avoid exception in GetBest method
            if (positions.Count == 0) return;

            // set our agent destination to this pos
            movement.Destination = GetBest(positions.ToArray());
        }

        #endregion
    }
}
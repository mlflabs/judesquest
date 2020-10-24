// Created by Ronis Vision. All rights reserved
// 23.08.2019.

using RVModules.RVUtilities.Extensions;

using UnityEngine;

namespace RVModules.RVSmartAI.Content.Code.AI.Tasks
{
    public class MoveToPointNearestMoveTarget : AiAgentBaseTask
    {
        #region Fields

        [SmartAiExposeField]
        public float distToTarget = 3;

        #endregion

        #region Public methods

        protected override void Execute(float _deltaTime)
        {
            // get walkable positions close to our follow target
            var positions = movementScanner.FindWalkablePositions(MoveTarget.position, distToTarget);

            // exit if there are no walkables close to our follow target
            if (positions.Count == 0) return;

            var closestPos = Vector3.zero;
            var closest = float.MaxValue;

            // find position closest to our follow target
            foreach (var position in positions)
            {
                var d = position.ManhattanDistance2d(MoveTarget.position);

                if (!(d < closest)) continue;
                closestPos = position;
                closest = d;
            }

            // set our agent destination to this pos
            movement.Destination = closestPos;
        }

        #endregion
    }
}
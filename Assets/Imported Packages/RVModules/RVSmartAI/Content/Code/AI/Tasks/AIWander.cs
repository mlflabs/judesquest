// Created by Ronis Vision. All rights reserved
// 23.08.2019.

using UnityEngine;

namespace RVModules.RVSmartAI.Content.Code.AI.Tasks
{
    /// <summary>
    /// Finds walkable position within set range and goes there. Range is measured in straight line from agent position(like radius)
    /// Requires context providing IMovement and IMovementScanner
    /// </summary>
    public class AIWander : AiAgentBaseTask
    {
        #region Fields

        [SmartAiExposeField("Radius in which agent will go next, measuring from his position")]
        public int range = 5;

        #endregion

        #region Public methods

        protected override void Execute(float _deltaTime)
        {
            var walkablePositions = movementScanner.FindWalkablePositions(movement.Position, range);
            if (walkablePositions.Count == 0) return;

            var dest = walkablePositions[Random.Range(0, walkablePositions.Count)];
            movement.Destination = dest;
        }

        #endregion
    }
}
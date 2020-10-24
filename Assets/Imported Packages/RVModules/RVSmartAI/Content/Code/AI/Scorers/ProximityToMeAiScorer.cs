// Created by Ronis Vision. All rights reserved
// 23.08.2019.

using RVModules.RVUtilities.Extensions;
using UnityEngine;

namespace RVModules.RVSmartAI.Content.Code.AI.Scorers
{
    /// <summary>
    /// Check distance in 2d using X and Z axis of vectors, and use this distance to return score based on configuration
    /// </summary>
    public abstract class ProximityToMeAiScorer : AiAgentBaseScorerCurve
    {
        #region Fields

        [SmartAiExposeField("Distance at time of 1 on curve")]
        public float distance = 10;

        #endregion

        #region Properties

        public abstract Vector3 PositionToMeasure { get; }

        #endregion

        #region Public methods

        public override float Score(float _deltaTime) => GetScoreFromCurve(movement.Position.ManhattanDistance2d(PositionToMeasure) / distance);

        #endregion
    }
}
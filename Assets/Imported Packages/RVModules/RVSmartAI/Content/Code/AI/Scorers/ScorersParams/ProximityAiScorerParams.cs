// Created by Ronis Vision. All rights reserved
// 23.08.2019.

using RVModules.RVUtilities.Extensions;
using UnityEngine;

namespace RVModules.RVSmartAI.Content.Code.AI.Scorers.ScorersParams
{
    /// <summary>
    /// Check distance in 2d using X and Z axis of vectors, and use this distance to return score based on configuration
    /// </summary>
    public abstract class ProximityAiScorerParams : AiAgentBaseScorerCurveParams<Vector3>
    {
        #region Fields

        [SmartAiExposeField("Distance at time of 1 on curve")]
        public float distance = 10;

        #endregion

        #region Properties

        public abstract Vector3 PositionToMeasure { get; }

        #endregion

        #region Public methods

        protected override float Score(Vector3 _parameter) => GetScoreFromCurve(_parameter.ManhattanDistance2d(PositionToMeasure) / distance);

        #endregion
    }
}
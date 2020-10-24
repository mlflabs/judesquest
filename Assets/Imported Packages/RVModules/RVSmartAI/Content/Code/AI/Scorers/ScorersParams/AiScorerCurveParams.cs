// Created by Ronis Vision. All rights reserved
// 23.08.2019.

using RVModules.RVSmartAI.GraphElements;
using UnityEngine;

namespace RVModules.RVSmartAI.Content.Code.AI.Scorers.ScorersParams
{
    public abstract class AiScorerCurveParams<T> : AiScorerParams<T>
    {
        #region Fields

        [SmartAiExposeField]
        public AnimationCurve curve = AnimationCurve.Linear(0, 0, 1, 1);

        #endregion

        #region Public methods
        
        /// <summary>
        /// Input value should be normalized (0-1), returned value is multiplied by score.
        /// If you want normalized result just divide returned value by score. 
        /// </summary>
        public float GetScoreFromCurve(float _score) => curve.Evaluate(_score) * score;

        #endregion
    }
}
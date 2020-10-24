// Created by Ronis Vision. All rights reserved
// 23.08.2019.

using RVModules.RVSmartAI.GraphElements;
using UnityEngine;

namespace RVModules.RVSmartAI.Content.Code.AI.Scorers
{
    public class RandomScoreAiScorer : AiScorer
    {
        #region Fields

        [SmartAiExposeField]
        public float range;

        #endregion

        #region Public methods

        public override float Score(float _deltaTime)
        {
            return Random.Range(0, range);
        }

        #endregion
    }
}
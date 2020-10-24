// Created by Ronis Vision. All rights reserved
// 23.08.2019.

using RVModules.RVSmartAI.GraphElements;
using UnityEngine;

namespace RVModules.RVSmartAI.Content.Code.AI.Scorers
{
    public class RandomScoreVector3 : AiScorerParams<Vector3>
    {
        #region Fields

        [SmartAiExposeField]
        public float range;

        #endregion

        #region Public methods

        protected override float Score(Vector3 _parameter) => Random.Range(0, range);

        #endregion

    }
}
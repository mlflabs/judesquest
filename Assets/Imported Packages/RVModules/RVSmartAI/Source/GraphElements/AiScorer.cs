// Created by Ronis Vision. All rights reserved
// 05.01.2020.

using System;
using RVModules.RVSmartAI.Content.Code;

namespace RVModules.RVSmartAI.GraphElements
{
    [Serializable] public abstract class AiScorer : AiGraphElement
    {
        #region Fields

        [SmartAiExposeField]
        public ScorerType scorerType;

        [SmartAiExposeField]
        public float score = 1;

        /// <summary>
        /// for debugging only
        /// </summary>
        public float lastScore;

        #endregion

        #region Public methods

        public abstract float Score(float _deltaTime);

        #endregion
    }
}
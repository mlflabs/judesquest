// Created by Ronis Vision. All rights reserved
// 06.01.2020.

using System;
using RVModules.RVSmartAI.Content.Code;

namespace RVModules.RVSmartAI.GraphElements
{
    [Serializable] public abstract class AiScorerParams<T> : AiGraphElement, IAiScorer
    {
        #region Fields

        [SmartAiExposeField]
        public ScorerType scorerType;

        [SmartAiExposeField]
        public float score = 1;

        #endregion

        #region Public methods

        public float Score_(object _parameter) => Score((T) _parameter);
        public ScorerType ScorerType => scorerType;

        #endregion

        #region Not public methods

        protected abstract float Score(T _parameter);

        #endregion
    }
}
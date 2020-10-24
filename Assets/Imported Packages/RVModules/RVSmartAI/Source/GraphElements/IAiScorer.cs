// Created by Ronis Vision. All rights reserved
// 05.01.2020.

using RVModules.RVSmartAI.Content.Code;

namespace RVModules.RVSmartAI.GraphElements
{
    public interface IAiScorer : IAiGraphElement
    {
        #region Public methods

        float Score_(object _parameter);
        ScorerType ScorerType { get; }

        #endregion
    }
}
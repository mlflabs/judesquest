// Created by Ronis Vision. All rights reserved
// 05.01.2020.

using System;
using RVModules.RVSmartAI.GraphElements.Utilities;

namespace RVModules.RVSmartAI.GraphElements.Stages
{
    [Serializable] public class FirstUtilityWinsStage : Stage
    {
        #region Public methods

        public override AiUtility Select(float _deltaTime)
        {
            foreach (var utility in utilities)
            {
                if (!utility.Enabled) continue;
                if (utility.Score(_deltaTime) > 0)
                    return utility;
            }

            return null;
        }

        #endregion
    }
}
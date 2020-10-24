// Created by Ronis Vision. All rights reserved
// 05.01.2020.

using System.Collections.Generic;

namespace RVModules.RVSmartAI.GraphElements
{
    public interface IAiTaskParams
    {
        #region Public methods

        List<object> GetScorers();
        void SetScorers(List<object> _scorers);

        #endregion
    }
}
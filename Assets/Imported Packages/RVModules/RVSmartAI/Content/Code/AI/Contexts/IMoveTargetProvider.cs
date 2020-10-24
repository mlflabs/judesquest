// Created by Ronis Vision. All rights reserved
// 23.08.2019.

using UnityEngine;

namespace RVModules.RVSmartAI.Content.Code.AI.Contexts
{
    public interface IMoveTargetProvider
    {
        #region Properties

        Transform FollowTarget { get; set; }

        #endregion
    }
}
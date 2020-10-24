// Created by Ronis Vision. All rights reserved
// 23.08.2019.

using UnityEngine;

namespace RVModules.RVSmartAI.Content.Code.Scanners
{
    /// <summary>
    /// Used for scanners, so any component inheriting from IScannable can be fetched by scanners
    /// </summary>
    public interface IScannable
    {
        #region Properties

        Object GetObject { get; }

        #endregion
    }
}
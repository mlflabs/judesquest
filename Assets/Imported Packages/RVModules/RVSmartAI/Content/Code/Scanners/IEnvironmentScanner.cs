// Created by Ronis Vision. All rights reserved
// 23.08.2019.

using UnityEngine;

namespace RVModules.RVSmartAI.Content.Code.Scanners
{
    public interface IEnvironmentScanner
    {
        #region Public methods

        /// <summary>
        /// 
        /// </summary>
        Object[] ScanEnvironment(Vector3 _position, float _range);

        #endregion
    }
}
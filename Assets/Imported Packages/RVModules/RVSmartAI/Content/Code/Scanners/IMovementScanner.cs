// Created by Ronis Vision. All rights reserved
// 23.08.2019.

using System.Collections.Generic;
using UnityEngine;

namespace RVModules.RVSmartAI.Content.Code.Scanners
{
    public interface IMovementScanner
    {
        #region Public methods

        List<Vector3> FindWalkablePositions(Vector3 _position, float _range);

        #endregion
    }
}
// Created by Ronis Vision. All rights reserved
// 23.08.2019.

using RVModules.RVSmartAI.Content.Code.Scanners;
using UnityEngine;

namespace RVModules.RVSmartAI.Content.Code
{
    public class Obstacle : MonoBehaviour, IScannable
    {
        #region Properties

        public Object GetObject => this;

        #endregion
    }
}
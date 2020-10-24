// Created by Ronis Vision. All rights reserved
// 23.08.2019.

using RVModules.RVSmartAI.Content.Code.Scanners;
using UnityEngine;

namespace RVModules.RVSmartAI.Content.Code
{
    /// <summary>
    /// Simple generic ai character representation designed to be as flexible as possible
    /// This is for example purpose only, but you can use it as base class for your NPCs or create your own
    /// </summary>
    public class AiAgent : MonoBehaviour, IScannable
    {
        #region Properties

        // IScannable implementation
        public Object GetObject => this;

        #endregion
    }
}
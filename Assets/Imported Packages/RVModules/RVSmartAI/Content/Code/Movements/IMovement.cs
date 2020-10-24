// Created by Ronis Vision. All rights reserved
// 23.08.2019.

using UnityEngine;

namespace RVModules.RVSmartAI.Content.Code.Movements
{
    public interface IMovement
    {
        #region Properties

        /// <summary>
        /// 
        /// </summary>
        bool AtDestination { get; }

        /// <summary>
        /// Sets current destination and automatically starts moving towards it
        /// Should be set to Vector3.zero after arriving
        /// </summary>
        Vector3 Destination { get; set; }
        
        Vector3 Velocity { get; }

        /// <summary>
        /// Current position
        /// </summary>
        Vector3 Position { get; }
        
        Quaternion Rotation { get; }

        #endregion
    }
}
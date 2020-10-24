// Created by Ronis Vision. All rights reserved
// 13.11.2016.

using UnityEngine;

namespace RVModules.RVUtilities.PrototypingAndTesting
{
    public class Destroyer : MonoBehaviour
    {
        #region Fields

        public int frames;
        private int framesCount;

        #endregion

        #region Not public methods

        private void Update()
        {
            if (framesCount >= frames)
                Destroy(gameObject);
            framesCount++;
        }

        #endregion
    }
}
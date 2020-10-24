// Created by Ronis Vision. All rights reserved
// 13.11.2016.

using UnityEngine;

namespace RVModules.RVUtilities.PrototypingAndTesting
{
    public class ScreenToWorldRaycaster
    {
        #region Fields

        public LayerMask mask;

        #endregion

        #region Not public methods

        protected bool RaycastCheck(out RaycastHit hit)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, 1000, mask))
                return true;
            return false;
        }

        #endregion
    }
}
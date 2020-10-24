// Created by Ronis Vision. All rights reserved
// 23.08.2019.

using RVModules.RVUtilities;

using UnityEngine;

namespace RVModules.RVSmartAI.Content.Code.AI.Contexts
{
    public interface INearbyObjectsProvider
    {
        #region Properties

        ListNonAlloc<Object> NearbyObjects { get; }

        #endregion
    }
}
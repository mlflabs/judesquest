// Created by Ronis Vision. All rights reserved
// 11.09.2016.

namespace RVModules.RVUtilities
{
    public interface IObserver
    {
        #region Public methods

        void OnNotify(object _event);
        // destroyed observer must inform main observerHandler that it's no more
        //void OnDestroy();

        #endregion
    }
}
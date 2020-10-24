// Created by Ronis Vision. All rights reserved
// 18.09.2016.

using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public interface INeedInterface<T> where T : class
{
    #region Public methods

    void PassInterface(T _interface);

    #endregion
}
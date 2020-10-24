// Created by Ronis Vision. All rights reserved
// 23.09.2016.

using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public interface IHaveInterface<T> where T : class
{
    #region Public methods

    T GetInterfaceInstance();

    #endregion
}
// Created by Ronis Vision. All rights reserved
// 28.09.2019.

using System;
using UnityEngine;

namespace RVModules.RVUtilities
{
    /// <summary>
    /// Helper class for casting data
    /// </summary>
    public static class Cast
    {
        /// <summary> 
        /// Use if you dont want to use if null pattern after casting
        /// </summary>
        /// <returns></returns>
        public static T Try<T>(object _dataToConvert, Action<T> onSuccess = null) where T : class
        {
            T convertedData = _dataToConvert as T;
            if (convertedData != null) onSuccess?.Invoke(convertedData);
            return convertedData;
        }
    }
}
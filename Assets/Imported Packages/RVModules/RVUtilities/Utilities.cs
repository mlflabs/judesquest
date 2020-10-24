// Created by Ronis Vision. All rights reserved
// 22.03.2019.

using System;
using System.Diagnostics;
using UnityEngine;

namespace RVModules.RVUtilities
{
    public static class Utilities
    {
        /// <summary>
        /// Returns time it took for _action, in miliseconds
        /// </summary>
        /// <param name="_action"></param>
        /// <returns></returns>
        public static int MeasureMs(Action _action)
        {
            Stopwatch sw = Stopwatch.StartNew();
            _action.Invoke();
            return (int) sw.ElapsedMilliseconds;
        }

        /// <summary>
        /// 
        /// </summary>
        public static void LogMs(Action _action, string _msgBeforeMs = "")
        {
            UnityEngine.Debug.Log(_msgBeforeMs + MeasureMs(_action) + "ms");
        }


        public static string GetAfter(this string input, string _after)
        {
            var index = input.IndexOf(_after);
            var output = "";
            if (index > 0)
                output = input.Substring(0, index);
            return output;
        }

        public static Color RonisVisionOrange = new Color(0.984f, 0.690f, 0.250f);
        // 251, 176, 64
    }
}
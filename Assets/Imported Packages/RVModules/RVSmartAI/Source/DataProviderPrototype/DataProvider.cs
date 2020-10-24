// Created by Ronis Vision. All rights reserved
// 11.07.2020.

using System;
using UnityEngine;

namespace RVModules.RVSmartAI.DataProviderPrototype
{
    [Serializable] public abstract class DataProvider<T> : ScriptableObject
    {
        /// <summary>
        /// Helper method to use instead of casting to your context type 
        /// </summary>
        protected ExpectedContext Context<ExpectedContext>(IContext _context) => _context is ExpectedContext context ? context : default;

        public abstract T GetData(IContext _context);
    }
}
// Created by Ronis Vision. All rights reserved
// 11.07.2020.

using UnityEngine;

namespace RVModules.RVSmartAI.DataProviderPrototype
{
    [CreateAssetMenu]
    public class FloatProviderReflection : FloatProvider
    {
        [SmartAiExposeField]
        public string property;

        public override float GetData(IContext _context)
        {
            return (float) Helpers.BuildPropertyGetter(_context, property)(_context);
        }
    }
}
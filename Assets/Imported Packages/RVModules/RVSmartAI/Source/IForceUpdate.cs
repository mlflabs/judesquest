// Created by Ronis Vision. All rights reserved
// 04.01.2020.

namespace RVModules.RVSmartAI
{
    /// <summary>
    /// Used for reflection-based graph elements, where user needs to provide property name at runtime
    /// </summary>
    public interface IForceUpdate
    {
        void ForceUpdate();
    }
}
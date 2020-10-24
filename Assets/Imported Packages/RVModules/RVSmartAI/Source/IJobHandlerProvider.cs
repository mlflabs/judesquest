// Created by Ronis Vision. All rights reserved
// 23.06.2020.

namespace RVModules.RVSmartAI
{
    public interface IJobHandlerProvider
    {
        TaskHandler AiJobHandler { get; }
    }
}
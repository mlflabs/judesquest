// Created by Ronis Vision. All rights reserved
// 26.03.2019.

using System.Collections.Generic;

namespace RVModules.RVUtilities.Extensions
{
    public static class ListExtensions
    {
        public static void Move<T>(this IList<T> list, int iIndexToMove, MoveDirection direction)
        {
            if (direction == MoveDirection.Up)
            {
                if (iIndexToMove <= 0) return;

                var old = list[iIndexToMove - 1];
                list[iIndexToMove - 1] = list[iIndexToMove];
                list[iIndexToMove] = old;
            }
            else
            {
                if (iIndexToMove >= list.Count - 1) return;
                var old = list[iIndexToMove + 1];
                list[iIndexToMove + 1] = list[iIndexToMove];
                list[iIndexToMove] = old;
            }
        }
    }

    public enum MoveDirection
    {
        Up,
        Down
    }
}
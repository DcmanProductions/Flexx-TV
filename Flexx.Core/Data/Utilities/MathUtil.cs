using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Flexx.Core.Data.Utilities
{
    public static class MathUtil
    {
        public static IEnumerable<T[]> GetChunk<T>(T[] fullArray, int size)
        {
            for (int i = 0; i < fullArray.Length; i += size)
            {
                T[] range;
                try
                {
                    range = fullArray.ToList().GetRange(i, Math.Min(size, fullArray.Length - i)).ToArray();
                }
                catch (ArgumentOutOfRangeException)
                {
                    continue;
                }
                yield return range;
            }
        }
    }
}

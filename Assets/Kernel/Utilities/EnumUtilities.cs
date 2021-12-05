using System;

namespace Kernel.Utilities
{
    public class EnumUtilities
    {
        public static T RandomEnumValue<T>()
        {
            var values = Enum.GetValues(typeof(T));
            var random = UnityEngine.Random.Range(0, values.Length);
            return (T) values.GetValue(random);
        }
    }
}
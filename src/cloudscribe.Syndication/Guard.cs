using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cloudscribe.Syndication.Models
{
    public static class Guard
    {
        public static void ArgumentNotNull(object value, string name)
        {
            if (value == null)
            {
                throw new ArgumentNullException(name);
            }
        }

        public static void ArgumentNotNullOrEmptyString(string value, string name)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException(name);
            }
        }

       
        public static void ArgumentNotGreaterThan(int value, string name, int maximum)
        {
            if (value > maximum)
            {
                throw new ArgumentOutOfRangeException(name);
            }
        }

        public static void ArgumentNotLessThan(long value, string name, long minimum)
        {
            if (value < minimum)
            {
                throw new ArgumentOutOfRangeException(name);
            }
        }
    }
}

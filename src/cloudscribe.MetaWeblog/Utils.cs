
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace cloudscribe.MetaWeblog
{
    public static class Utils
    {
        public static string GetDateTimeStringForFileName()
        {
            return GetDateTimeStringForFileName(false);
        }

        public static string GetDateTimeStringForFileName(bool includeMiliseconds)
        {
            DateTime d = DateTime.Now;
            string dateString = d.Year.ToInvariantString();

            string monthString = d.Month.ToInvariantString();
            if (monthString.Length == 1)
            {
                monthString = "0" + monthString;
            }
            string dayString = d.Day.ToInvariantString();
            if (dayString.Length == 1)
            {
                dayString = "0" + dayString;
            }
            string hourString = d.Hour.ToInvariantString();
            if (hourString.Length == 1)
            {
                hourString = "0" + hourString;
            }

            string minuteString = d.Minute.ToInvariantString();
            if (minuteString.Length == 1)
            {
                minuteString = "0" + minuteString;
            }

            string secondString = d.Second.ToInvariantString();
            if (secondString.Length == 1)
            {
                secondString = "0" + secondString;
            }

            dateString
                = dateString
                + monthString
                + dayString
                + hourString
                + minuteString + secondString;

            if (includeMiliseconds)
            {
                return dateString + d.Millisecond.ToInvariantString();
            }

            return dateString;
        }


        public static string ToInvariantString(this int i)
        {
            return i.ToString(CultureInfo.InvariantCulture);

        }

        public static string ConvertDatetoISO8601(DateTime date)
        {
            var temp = string.Format(
                "{0}{1}{2}T{3}:{4}:{5}",
                date.Year,
                date.Month.ToString().PadLeft(2, '0'),
                date.Day.ToString().PadLeft(2, '0'),
                date.Hour.ToString().PadLeft(2, '0'),
                date.Minute.ToString().PadLeft(2, '0'),
                date.Second.ToString().PadLeft(2, '0'));
            return temp;
        }

        //iso8601 often come in slightly different flavours rather than the standard "s" that string.format supports.
        //http://stackoverflow.com/a/17752389
        //static readonly string[] formats = { 
        //    // Basic formats
        //    "yyyyMMddTHHmmsszzz",
        //    "yyyyMMddTHHmmsszz",
        //    "yyyyMMddTHHmmssZ",
        //    // Extended formats
        //    "yyyy-MM-ddTHH:mm:sszzz",
        //    "yyyy-MM-ddTHH:mm:sszz",
        //    "yyyy-MM-ddTHH:mm:ssZ",
        //    "yyyyMMddTHH:mm:ss:zzz",
        //    "yyyyMMddTHH:mm:ss:zz",
        //    "yyyyMMddTHH:mm:ss:Z",
        //    "yyyyMMddTHH:mm:ss",
        //    // All of the above with reduced accuracy
        //    "yyyyMMddTHHmmzzz",
        //    "yyyyMMddTHHmmzz",
        //    "yyyyMMddTHHmmZ",
        //    "yyyy-MM-ddTHH:mmzzz",
        //    "yyyy-MM-ddTHH:mmzz",
        //    "yyyy-MM-ddTHH:mmZ",
        //    // Accuracy reduced to hours
        //    "yyyyMMddTHHzzz",
        //    "yyyyMMddTHHzz",
        //    "yyyyMMddTHHZ",
        //    "yyyy-MM-ddTHHzzz",
        //    "yyyy-MM-ddTHHzz",
        //    "yyyy-MM-ddTHHZ"
        //};

    }
}

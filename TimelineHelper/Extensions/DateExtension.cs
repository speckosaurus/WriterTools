using System;

namespace TimelineAssistant.Extensions
{
    public static class DateExtension
    {
        public static DateTime SanitiseDate(this string input)
        {
            DateTime dateTimeInput;
            DateTime.TryParse(input, out dateTimeInput);
            if (dateTimeInput != new DateTime())
            {
                return dateTimeInput;
            }

            int yearInput;
            int.TryParse(input, out yearInput);
            if (yearInput > 0)
            {
                return new DateTime(yearInput, 1, 1);
            }

            return DateTime.Today;
        }

        public static bool DisplayYearOnly(this string input)
        {
            DateTime dateTimeInput;
            DateTime.TryParse(input, out dateTimeInput);
            if (dateTimeInput != new DateTime())
            {
                return false;
            }

            int yearInput;
            int.TryParse(input, out yearInput);
            if (yearInput > 0)
            {
                return true;
            }

            return false;
        }

        public static string FormatDate(this DateTime date, bool displayYearOnly)
        {
            if (displayYearOnly)
            {
                return date.ToString("yyyy");
            }

            return date.ToString("dd/MM/yyyy");
        }
    }
}

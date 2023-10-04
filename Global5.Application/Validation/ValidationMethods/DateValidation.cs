using System;

namespace Global5.Application.Validation
{
    public class DateValidation
    {

        public static bool IsDate(string input)
        {
            bool noIsDate = false;

            if (!string.IsNullOrWhiteSpace(input))
            {
                var day = input.ToString().Substring(0, 2);
                var month = input.ToString().Substring(3, 2);
                var year = input.ToString().Substring(6, 4);

                if (!string.IsNullOrWhiteSpace(day) && !string.IsNullOrWhiteSpace(month) && !string.IsNullOrWhiteSpace(year))
                {
                    if (Convert.ToInt16(day) > 0 && Convert.ToInt16(day) <= 31)
                    {
                        if (Convert.ToInt16(month) > 0 && Convert.ToInt16(month) <= 12)
                        {
                            if (Convert.ToInt16(year) > 0 && Convert.ToInt16(year) <= Convert.ToInt32(DateTime.Now.ToString("yyyy")))
                            {
                                noIsDate = true;
                            }
                        }
                    }
                }
            }
            return noIsDate;
        }
    }
}

using System;

namespace DatingApp.Api
{
    public static class DateExtensions
    {
        public static int GetAge(this DateTime date, DateTime currentDate) 
        {
            var age = currentDate.Year - date.Year;
            if (date.AddYears(age) > currentDate) age--;

            return age;
        }
    }
}
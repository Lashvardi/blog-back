using System;

namespace Blog.Extensions
{
    public static class DateTimeExtensions
    {
        public static string ElapsedTimeSinceCreation(this DateTime creationTime)
        {
            var timeSince = DateTime.Now.Subtract(creationTime);

            if (timeSince.TotalDays > 365)
                return $"{(int)(timeSince.TotalDays / 365)} წლი{((int)(timeSince.TotalDays / 365) == 1 ? "" : "ს")} წინ";
            if (timeSince.TotalDays > 30)
                return $"{(int)(timeSince.TotalDays / 30)} თვი{((int)(timeSince.TotalDays / 30) == 1 ? "" : "ს")} წინ";
            if (timeSince.TotalDays > 7)
                return $"{(int)(timeSince.TotalDays / 7)} კვირი{((int)(timeSince.TotalDays / 7) == 1 ? "" : "ს")} წინ";
            if (timeSince.TotalDays >= 1)
                return $"{(int)timeSince.TotalDays} დღი{((int)timeSince.TotalDays == 1 ? "" : "ს")} წინ";
            if (timeSince.TotalHours >= 1)
                return $"{(int)timeSince.TotalHours} საათი{((int)timeSince.TotalHours == 1 ? "" : "ს")} წინ";
            if (timeSince.TotalMinutes >= 1)
                return $"{(int)timeSince.TotalMinutes} წუთი{((int)timeSince.TotalMinutes == 1 ? "" : "ს")} წინ";

            return "ახლახანს";
        }
    }
}
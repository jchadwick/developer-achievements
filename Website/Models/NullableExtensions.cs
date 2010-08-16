namespace ChadwickSoftware.DeveloperAchievements.Website.Models
{
    public static class NullableExtensions
    {

        public static string ToString<T>(this T? source) 
            where T : struct
        {
            if (source == null)
                return string.Empty;

            return source.Value.ToString();
        }

        public static string ToString<T>(this T? source, string format) 
            where T : struct
        {
            if (source == null)
                return string.Empty;

            string formatString = string.Format("{{0:{0}}}", format);
            return string.Format(formatString, source.Value);
        }

    }
}
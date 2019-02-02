namespace Server.Extensions
{
    public static class StringToIntExtensions
    {
        public static int ConvertToInt(this string value)
        {
            var success = int.TryParse(value, out var intValue);
            if (success)
            {
                return intValue;
            }

            else return 1;

        }
    }
}

namespace CleanApp.Models
{
    public static class OrderNumberGenerator
    {
        private static int _number = 1;

        public static int GetNumber()
        {
            return _number++;
        }
    }
}

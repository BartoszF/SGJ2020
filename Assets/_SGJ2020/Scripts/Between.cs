namespace _SGJ2020.Scripts
{
    public static class Between
    {
        
        public static bool IsBetween(this float value, float min, float max)
        {
            return value >= min && value < max;
        }
    }
}
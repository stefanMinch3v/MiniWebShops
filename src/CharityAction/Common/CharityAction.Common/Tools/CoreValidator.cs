namespace CharityAction.Common.Tools
{
    using System;

    public static class CoreValidator
    {
        public static void ThrowIfNull(object obj, string name)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(name);
            }
        }

        public static void ThrowIfNullOrEmpty(string data, string name)
        {
            if (string.IsNullOrEmpty(data) || string.IsNullOrWhiteSpace(data))
            {
                throw new ArgumentNullException(name);
            }
        }

        public static void ThrowIfInvalidIntegerProvided(int data, string name)
        {
            if (data < 1 || data >= int.MaxValue)
            {
                throw new ArgumentException(name);
            }
        }
    }
}

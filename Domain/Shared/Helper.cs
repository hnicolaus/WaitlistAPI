using System;

namespace Domain.Helpers
{
    public static class Helper
    {
        public static string GenerateRandomString(int length = 5)
        {
            return Guid.NewGuid().ToString().Substring(0, length).ToUpper();
        }
    }
}

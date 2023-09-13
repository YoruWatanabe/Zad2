using System;
using System.Linq;

namespace University.Extensions
{
    public static class StringExtensions
    {
        public static bool IsValidPESEL(this string input)
        {
            int[] weights = { 1, 3, 7, 9, 1, 3, 7, 9, 1, 3 };
            if (input.Length != 11)
                return false;

            int controlSum = input.Take(10).Select((c, i) => (c - '0') * weights[i]).Sum();
            int controlNum = (10 - (controlSum % 10)) % 10;
            int lastDigit = input[10] - '0';

            return controlNum == lastDigit;
        }
    }
}
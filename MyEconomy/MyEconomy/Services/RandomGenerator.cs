using System;
using System.Collections.Generic;

namespace MyEconomy.Services
{
    public static class RandomGenerator
    {
        static readonly Random _random = new Random();
        static readonly List<string> adjectives = new List<string>() { "old", "new", "used", "glowing", "nice" };
        static readonly List<string> nouns = new List<string>() { "TV", "car", "Surface", "soap", "guitar" };

        public static DateTime RandomDateTime()
        {
            int year = DateTime.UtcNow.Year;
            int month = _random.Next(1, 12);
            int day = _random.Next(1, 28);

            return new DateTime(year, month, day);
        }

        public static string RandomTitle()
        {
            return String.Format("{0} {1}", RandomAdjective(), RandomNoun());
        }

        public static string RandomAdjective()
        {
            string adjective = adjectives[_random.Next(0, adjectives.Count - 1)];
            return adjective;
        }

        public static string RandomNoun()
        {
            string noun = nouns[_random.Next(0, nouns.Count - 1)];
            return noun;
        }
    }
}

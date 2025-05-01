using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace AdventDay2
{
    class Program
    {
        static async Task Main(string[] args)
        {
            string sessionCookie = "53616c7465645f5f5c67531ec45c5e2d4cf3a02c719e5d7433c948d6258bc24f6018e33f0c8f7e8a5bf6fb4c3d4578e658abbb7fca3e8e6be0cf0571a30335ae";
            string input = await FetchInputAsync(sessionCookie);

            int safeCount = 0;

            foreach (var line in input.Split('\n', StringSplitOptions.RemoveEmptyEntries))
            {
                var parts = line.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
                var levels = new List<int>();

                foreach (var part in parts)
                {
                    if (int.TryParse(part, out int number))
                    {
                        levels.Add(number);
                    }
                }

                bool isIncreasing = true;
                bool isDecreasing = true;
                bool validSteps = true;

                for (int i = 1; i < levels.Count; i++)
                {
                    int diff = levels[i] - levels[i - 1];

                    if (diff > 0)
                    {
                        isDecreasing = false;
                    }
                    else if (diff < 0)
                    {
                        isIncreasing = false;
                    }
                    else
                    {
                        validSteps = false; // same number twice
                        break;
                    }

                    if (Math.Abs(diff) < 1 || Math.Abs(diff) > 3)
                    {
                        validSteps = false;
                        break;
                    }
                }

                if ((isIncreasing || isDecreasing) && validSteps)
                {
                    safeCount++;
                }
            }

            Console.WriteLine(safeCount);
        }

            static async Task<string> FetchInputAsync(string sessionCookie)
            {
                using var client = new HttpClient();
                client.DefaultRequestHeaders.Add("Cookie", $"session={sessionCookie}");

                var response = await client.GetAsync("https://adventofcode.com/2024/day/2/input");
                response.EnsureSuccessStatusCode();

                return await response.Content.ReadAsStringAsync();
            }
    }
}


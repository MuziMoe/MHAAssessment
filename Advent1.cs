using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Advent1
{
    class Program
    {
        static async Task Main()
        {
            string sessionCookie = "53616c7465645f5f5c67531ec45c5e2d4cf3a02c719e5d7433c948d6258bc24f6018e33f0c8f7e8a5bf6fb4c3d4578e658abbb7fca3e8e6be0cf0571a30335ae"; // Replace this securely
            string input = await FetchInputAsync(sessionCookie);

            var leftList = new List<int>();
            var rightList = new List<int>();

            foreach (var line in input.Split('\n', StringSplitOptions.RemoveEmptyEntries))
            {
                var parts = line.Split("   ", StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length == 2 &&
                    int.TryParse(parts[0], out int left) &&
                    int.TryParse(parts[1], out int right))
                {
                    leftList.Add(left);
                    rightList.Add(right);
                }
            }

            leftList.Sort();
            rightList.Sort();

            int intDifference = 0;

            for (int i = 0; i < leftList.Count; i++)
            {
                int intDiff = 0;

                intDiff = Math.Abs(leftList[i] - rightList[i]);

                intDifference += intDiff;

            }

            // Print for verification
            Console.WriteLine(intDifference);

        }

        static async Task<string> FetchInputAsync(string sessionCookie)
        {
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Cookie", $"session={sessionCookie}");

            var response = await client.GetAsync("https://adventofcode.com/2024/day/1/input");
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }

    }
}

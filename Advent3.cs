using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Advent3
{
    class Program
    {
        static async System.Threading.Tasks.Task Main(string[] args)
        {
            string sessionCookie = "53616c7465645f5f5c67531ec45c5e2d4cf3a02c719e5d7433c948d6258bc24f6018e33f0c8f7e8a5bf6fb4c3d4578e658abbb7fca3e8e6be0cf0571a30335ae";
            string input = await FetchInputAsync(sessionCookie);

            var regex = new Regex(@"mul\((\d{1,10}),(\d{1,10})\)");
            int total = 0;
            var correctSequence = regex.Matches(input);

            for (int i = 0; i < correctSequence.Count; i++)
            {
                string strNumbers = correctSequence[i].ToString();
                string[] splitNumbers = strNumbers.Replace("mul(", "").Replace(")", "").Split(',');

                int firstNum = int.Parse(splitNumbers[0]);
                int secondNum = int.Parse(splitNumbers[1]);

                //Console.WriteLine(firstNum);
                //Console.WriteLine(secondNum + "second");

                total += firstNum * secondNum;
            }

            Console.WriteLine(total);



        }

        static async Task<string> FetchInputAsync(string sessionCookie)
        {
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Cookie", $"session={sessionCookie}");

            var response = await client.GetAsync("https://adventofcode.com/2024/day/3/input");
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }
    }
}

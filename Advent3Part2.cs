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
            bool isEnabled = true;

            int i = 0;

            while (i < input.Length)
            {
                if (i + 4 < input.Length && input.Substring(i, 5) == "do()")
                {
                    isEnabled = true;
                    i += 5;
                }
                else if (i + 7 < input.Length && input.Substring(i, 8) == "don't()")
                {
                    isEnabled = false;
                    i += 8;
                }
                else
                {

                    //var match = regex.Match(input.Substring(i));

                    string remaining = input.Substring(i);
                    Match match = regex.Match(remaining);
                    if (isEnabled && match.Success)
                    {

                        int x = int.Parse(match.Groups[1].Value);
                        int y = int.Parse(match.Groups[2].Value);
                        total += x * y;

                        i += match.Length;
                    }
                    else
                    {
                        i++;
                    }

                }
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
  

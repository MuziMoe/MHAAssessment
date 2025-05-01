using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Advent4
{
    class Program
    {
        static async Task Main(string[] args)
        {
            string sessionCookie = "53616c7465645f5f5c67531ec45c5e2d4cf3a02c719e5d7433c948d6258bc24f6018e33f0c8f7e8a5bf6fb4c3d4578e658abbb7fca3e8e6be0cf0571a30335ae";
            string input = await FetchInputAsync(sessionCookie);

            var grid = ConvertToGrid(input);
            var word = "XMAS";
            int wordLength = word.Length;
            int rowCount = grid.GetLength(0);
            int colCount = grid.GetLength(1);

            int matches = 0;

            for (int i = 0; i < rowCount; i++)
            {
                for (int j = 0; j < colCount; j++)
                {
                    // Check horizontally (to the right)
                    if (j <= colCount - wordLength && CheckWord(grid, i, j, 0, 1, word))
                        matches++;

                    // Check horizontally (to the left)
                    if (j <= colCount - wordLength && CheckWord(grid, i, j, 0, -1, word))
                        matches++;

                    // Check vertically (downwards)
                    if (i <= rowCount - wordLength && CheckWord(grid, i, j, 1, 0, word))
                        matches++;

                    // Check vertically (upwards)
                    if (i <= rowCount - wordLength && CheckWord(grid, i, j, -1, 0, word))
                        matches++;

                    // Check diagonally (down-right)
                    if (i <= rowCount - wordLength && j <= colCount - wordLength && CheckWord(grid, i, j, 1, 1, word))
                        matches++;

                    // Check diagonally (up-right)
                    if (i >= wordLength - 1 && j <= colCount - wordLength && CheckWord(grid, i, j, -1, 1, word))
                        matches++;

                    // Check diagonally (down-left)
                    if (i <= rowCount - wordLength && j <= colCount - wordLength && CheckWord(grid, i, j, -1, -1, word))
                        matches++;

                    // Check diagonally (up-left)
                    if (i >= wordLength - 1 && j <= colCount - wordLength && CheckWord(grid, i, j, 1, -1, word))
                        matches++;
                }
            }

            Console.WriteLine(matches);
        }

        static async Task<string> FetchInputAsync(string sessionCookie)
        {
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Cookie", $"session={sessionCookie}");

            var response = await client.GetAsync("https://adventofcode.com/2024/day/4/input");
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }

        static char[,] ConvertToGrid(string input)
        {
            var rows = input.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            int rowCount = rows.Length;
            int colCount = rows[0].Length;
            var grid = new char[rowCount, colCount];

            for (int i = 0; i < rowCount; i++)
                for (int j = 0; j < colCount; j++)
                    grid[i, j] = rows[i][j];

            return grid;
        }

        
        static bool CheckWord(char[,] grid, int startRow, int startCol, int rowDir, int colDir, string word)
        {
            for (int k = 0; k < word.Length; k++)
            {
                int row = startRow + k * rowDir;
                int col = startCol + k * colDir;

                // Check if we're out of bounds
                if (row < 0 || col < 0 || row >= grid.GetLength(0) || col >= grid.GetLength(1))
                    return false;

                if (grid[row, col] != word[k])
                    return false;
            }
            return true;
        }

    }
}

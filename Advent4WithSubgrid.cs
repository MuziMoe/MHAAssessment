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
            int count = 0;

            int matches = 0;

            // Slide 4x4 subgrid across the main grid
            for (int i = 0; i <= rowCount - 4; i+=4)
            {
                for (int j = 0; j <= colCount - 4; j+=4)
                {
                    
                    var subgrid = ExtractSubgrid(grid, i, j, 4, 4);

                    for (int l = 0; l < 4; l++)
                    {
                        for (int m = 0; m < 4; m++)
                        {
                            // Check horizontally (to the right)
                            if (m <= colCount - wordLength && CheckWord(grid, i, j, 0, 1, word))
                                matches++;

                            // Check horizontally (to the left)
                            if (m <= colCount - wordLength && CheckWord(subgrid, i, j, 0, -1, word))
                                matches++;

                            // Check vertically (downwards)
                            if (l <= rowCount - wordLength && CheckWord(subgrid, i, j, 1, 0, word))
                                matches++;

                            // Check vertically (upwards)
                            if (l <= rowCount - wordLength && CheckWord(subgrid, i, j, -1, 0, word))
                                matches++;

                            // Check diagonally (down-right)
                            if (l <= rowCount - wordLength && j <= colCount - wordLength && CheckWord(subgrid, i, j, 1, 1, word))
                                matches++;

                            // Check diagonally (up-right)
                            if (l >= wordLength - 1 && j <= colCount - wordLength && CheckWord(subgrid, i, j, -1, 1, word))
                                matches++;

                            // Check diagonally (down-left)
                            if (l <= rowCount - wordLength && j <= colCount - wordLength && CheckWord(subgrid, i, j, -1, -1, word))
                                matches++;

                            // Check diagonally (up-left)
                            if (l >= wordLength - 1 && j <= colCount - wordLength && CheckWord(subgrid, i, j, 1, -1, word))
                                matches++;
                        }
                    }
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

        static char[,] ExtractSubgrid(char[,] fullGrid, int startRow, int startCol, int subHeight, int subWidth)
        {
            var subgrid = new char[subHeight, subWidth];
            for (int i = 0; i < subHeight; i++)
                for (int j = 0; j < subWidth; j++)
                    subgrid[i, j] = fullGrid[startRow + i, startCol + j];

            return subgrid;
        }

        static bool CheckWord(char[,] grid, int startRow, int startCol, int rowDir, int colDir, string word)
        {
            for (int k = 0; k < word.Length; k++)
            {
                int row = startRow + k * rowDir;
                int col = startCol + k * colDir;

                if (row < 0 || col < 0 || row >= grid.GetLength(0) || col >= grid.GetLength(1))
                    return false;

                if (grid[row, col] != word[k])
                    return false;
            }
            return true;
        }
    }
}

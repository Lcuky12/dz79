using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.InteropServices;

namespace ConsoleApp84
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.CursorVisible = false;
            Random random = new Random();

            bool isPlaying = true;
            bool isAlive = true;
            int heroX = 0;
            int heroY = 0;
            int allCoins = 0;
            int collectCoins = 0;
            int heroDX = 0;
            int heroDY = 1;
            int enemyX = 0;
            int enemyY = 0;
            int enemyDX = 0;
            int enemyDY = -1;

            char[,] map = ReadMap("map1", ref heroX, ref heroY,ref allCoins, ref enemyX, ref enemyY);
 
            DrawMap(map);


            while (isPlaying)
            {
                Console.CursorVisible = false;
                Console.SetCursorPosition(0, 30);
                Console.WriteLine($"Собрано монет {collectCoins}/{allCoins}");

                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo key = Console.ReadKey(true);
                    ChangeDirection(key, ref heroDX, ref heroDY);
                }

                if (map[heroX + heroDX, heroY + heroDY] != '#')
                {
                    CollectCoins(map, heroX, heroY, ref collectCoins);
                    Move(map, '&',ref heroX, ref heroY, heroDX, heroDY);
                }

                if (map[enemyX + enemyDX, enemyY + enemyDY] != '#')
                {
                    Move(map, '@', ref enemyX, ref enemyY, enemyDX, enemyDY);
                }
                else
                {
                    ChangeDirection(random, ref enemyDX, ref enemyDY);
                }

                if(enemyX == heroX && enemyY == heroY)
                {
                    isPlaying= false;
                }

                System.Threading.Thread.Sleep(150);

                if(collectCoins == allCoins || !isAlive)
                {
                    isPlaying= false;
                }
            } 
            
            Console.SetCursorPosition(0,35);
        
            if(collectCoins == allCoins)
            {
                Console.WriteLine("Вы победили!");
            }
            else if (isAlive)
            {
                Console.WriteLine("Вы проиграли");
            }
        }

        static void Move(char [,] map, char symbol, ref int X, ref int Y, int DX, int DY)
        {
            Console.SetCursorPosition(Y,X);
            Console.Write(map[X,Y]);

            X += DX;
            Y += DY;

            Console.SetCursorPosition(Y,X);
            Console.Write(symbol);
        }
        static void CollectCoins(char [,] map, int heroX, int heroY, ref int collectCoins)
        {
            if (map[heroX, heroY] == '.')
            {
                collectCoins++;
                map[heroX, heroY] = ' ';
            }
        }
        static void ChangeDirection(ConsoleKeyInfo key, ref int DX, ref int DY)
        {
            switch (key.Key)
            {
                case ConsoleKey.UpArrow:
                    DX = -1;
                    DY = 0;
                    break;
                case ConsoleKey.DownArrow:
                    DX = 1;
                    DY = 0;
                    break;
                case ConsoleKey.LeftArrow:
                    DX = 0;
                    DY = -1;
                    break;
                case ConsoleKey.RightArrow:
                    DX = 0;
                    DY = 1;
                    break;
            }

        }

        static void ChangeDirection(Random random, ref int DX, ref int DY)
        {
            int enemyDir = random.Next(1, 5);

            switch (enemyDir)
            {
                case 1:
                    DX = -1;
                    DY = 0;
                    break;
                case 2:
                    DX = 1;
                    DY = 0;
                    break;
                case 3:
                    DX = 0;
                    DY = -1;
                    break;
                case 4:
                    DX = 0;
                    DY = 1;
                    break;
            }
        }

        static void DrawMap(char[,] map)
        {
            for(int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    Console.Write(map[i, j]);
                }

                Console.WriteLine();
            }   
        }

        static char[,] ReadMap(string mapName, ref int heroX, ref int heroY, ref int allCoins, ref int enemyX, ref int enemyY)
        {
            string[] newFile = File.ReadAllLines($"Maps/{mapName}.txt");
            char[,] map = new char[newFile.Length, newFile[0].Length];

            for (int i = 0; i < map.GetLength(0); i++) 
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    map[i, j] = newFile[i][j];

                    if (map[i, j] == '&')
                    {
                        heroX = i;
                        heroY = j;
                        map[i, j] = '.';
                    }

                    else if (map[i, j] == '@')
                    {
                        enemyX= i;
                        enemyY = j;
                        map[i, j] = '.';
                    }

                    else if (map[i, j] == ' ')
                    {
                        map[i, j] = '.';
                        allCoins++;
                    }
                }
            }

            return map;
        }
    }
}

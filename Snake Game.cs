using System;
using System.Collections.Generic;
using System.Threading;

namespace SnakeGame
{
    class Program
    {
        static void Main(string[] args)
        {
            // Set up console window dimensions and initialize game variables
            Console.WindowHeight = 16;
            Console.WindowWidth = 32;
            int screenWidth = Console.WindowWidth;
            int screenHeight = Console.WindowHeight;
            Random random = new Random();
            int score = 5;
            int gameOver = 0;

            // Initialize snake head
            SnakePixel head = new SnakePixel();
            head.X = screenWidth / 2;
            head.Y = screenHeight / 2;
            head.Color = ConsoleColor.Red;

            string movement = "RIGHT";
            List<int> bodyX = new List<int>();
            List<int> bodyY = new List<int>();
            int berryX = random.Next(0, screenWidth);
            int berryY = random.Next(0, screenHeight);

            // Main game loop
            while (true)
            {
                Console.Clear();

                // Check for collision with walls
                if (head.X == screenWidth - 1 || head.X == 0 || head.Y == screenHeight - 1 || head.Y == 0)
                {
                    gameOver = 1;
                }

                // Draw game elements
                DrawWalls(screenWidth, screenHeight);
                if (berryX == head.X && berryY == head.Y)
                {
                    score++;
                    berryX = random.Next(1, screenWidth - 2);
                    berryY = random.Next(1, screenHeight - 2);
                }
                DrawSnake(head, bodyX, bodyY);
                if (gameOver == 1)
                {
                    break;
                }
                Console.SetCursorPosition(head.X, head.Y);
                Console.ForegroundColor = head.Color;
                Console.Write("■");
                Console.SetCursorPosition(berryX, berryY);
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("■");

                // Move the snake
                MoveSnake(ref head, ref movement, bodyX, bodyY, score);

                // Adjust game speed
                Thread.Sleep(100);
            }

            // Display game over message
            Console.SetCursorPosition(screenWidth / 5, screenHeight / 2);
            Console.WriteLine("Game over, Score: " + score);
        }

        // Draw walls
        static void DrawWalls(int width, int height)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            for (int i = 0; i < width; i++)
            {
                Console.SetCursorPosition(i, 0);
                Console.Write("■");
                Console.SetCursorPosition(i, height - 1);
                Console.Write("■");
            }
            for (int i = 0; i < height; i++)
            {
                Console.SetCursorPosition(0, i);
                Console.Write("■");
                Console.SetCursorPosition(width - 1, i);
                Console.Write("■");
            }
        }

        // Draw snake body
        static void DrawSnake(SnakePixel head, List<int> bodyX, List<int> bodyY)
        {
            Console.ForegroundColor = head.Color;
            Console.SetCursorPosition(head.X, head.Y);
            Console.Write("■");

            foreach (var segment in bodyX)
            {
                Console.SetCursorPosition(segment, bodyY[bodyX.IndexOf(segment)]);
                Console.Write("■");
            }
        }

        // Move the snake
        static void MoveSnake(ref SnakePixel head, ref string direction, List<int> bodyX, List<int> bodyY, int score)
        {
            bodyX.Add(head.X);
            bodyY.Add(head.Y);

            // Update snake head position based on direction
            switch (direction)
            {
                case "UP":
                    head.Y--;
                    break;
                case "DOWN":
                    head.Y++;
                    break;
                case "LEFT":
                    head.X--;
                    break;
                case "RIGHT":
                    head.X++;
                    break;
            }

            // Remove tail segment if snake size exceeds score
            if (bodyX.Count > score)
            {
                bodyX.RemoveAt(0);
                bodyY.RemoveAt(0);
            }
        }

        // Pixel class to represent individual elements of snake and berry
        class SnakePixel
        {
            public int X { get; set; }
            public int Y { get; set; }
            public ConsoleColor Color { get; set; }
        }
    }
}

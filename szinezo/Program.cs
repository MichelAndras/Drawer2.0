﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace MenuForDrawing
{
    internal class Program
    {
        static bool hasPreviousDrawing = false;
        static List<(int x, int y, char character, ConsoleColor color)> trail = new List<(int, int, char, ConsoleColor)>();

        static void Main(string[] args)
        {
            Console.CursorVisible = false;
            int selectedButton = 1;
            ConsoleKeyInfo input;

            DrawBorder();
            DrawButtons(selectedButton);

            do
            {
                input = Console.ReadKey(true);

                switch (input.Key)
                {
                    case ConsoleKey.UpArrow:
                        if (selectedButton > 1)
                            selectedButton--;
                        break;

                    case ConsoleKey.DownArrow:
                        if (selectedButton < 5)
                            selectedButton++;
                        break;

                    case ConsoleKey.Enter:
                        Console.SetCursorPosition(2, Console.WindowHeight - 1);
                        Console.WriteLine($"Gomb {selectedButton} kiválasztva!");

                        switch (selectedButton)
                        {
                            case 1:
                                Console.Clear();
                                DrawBorder();
                                DrawInConsole(false);
                                break;
                            case 2:
                                Console.Clear();
                                DrawBorder();
                                openFolder();
                                break;
                            case 3:
                                if (hasPreviousDrawing)
                                {

                                    Console.Clear();
                                    DrawBorder();
                                    DrawInConsole(true);
                                }
                                else
                                {
                                    Console.Clear();
                                    DrawBorder();
                                    openFolder();
                                }
                                break;
                            case 4:
                                Console.Clear();
                                DrawBorder();
                                Console.SetCursorPosition(0, Console.WindowHeight - 2);
                                Console.WriteLine("Add meg a fájl nevét, amiként menteni szeretnéd a rajzot: ");
                                string savePath = Console.ReadLine();
                                SaveDrawingToFile(savePath);
                                Console.Clear();
                                DrawBorder();
                                break;
                            case 5:
                                Environment.Exit(0);
                                break;
                        }
                        break;

                    case ConsoleKey.Escape:
                        Console.Clear();
                        break;
                }

                DrawButtons(selectedButton);

            } while (input.Key != ConsoleKey.Escape);
        }

        static void openFolder()
        {
            Console.SetCursorPosition(0, Console.WindowHeight - 2);
            string folderPath = "C: \\Users\\michel.andras.zoltan\\source\\repos\\MichelAndras\\NewRepo\\Drawer\\Drawer\\bin\\Debug\\paints";

            try
            {
                if (Directory.Exists(folderPath))
                {
                    string[] files = Directory.GetFiles(folderPath);
                    Console.Clear();
                    Console.WriteLine("Elérhető fájlok:");

                    for (int i = 0; i < files.Length; i++)
                    {
                        Console.WriteLine($"{i + 1}. {Path.GetFileName(files[i])}");
                    }

                    Console.WriteLine("Válaszd ki a fájlt a szám megadásával (vagy nyomd meg az Esc billentyűt a visszatéréshez): ");
                    ConsoleKeyInfo keyInfo = Console.ReadKey();

                    if (keyInfo.Key == ConsoleKey.Escape)
                    {
                        Console.Clear();
                        return;
                    }

                    if (int.TryParse(keyInfo.KeyChar.ToString(), out int fileIndex) && fileIndex > 0 && fileIndex <= files.Length)
                    {
                        string selectedFile = files[fileIndex - 1];
                        LoadDrawingFromFile(selectedFile);
                    }
                    else
                    {
                        Console.WriteLine("Érvénytelen választás.");
                    }
                }
                else
                {
                    Console.WriteLine("A megadott mappa nem található.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Hiba történt a mappa megnyitása során:");
                Console.WriteLine(ex.Message);
            }
        }
        static void LoadDrawingFromFile(string folderPath)
        {
            try
            {
                string[] fileContent = File.ReadAllLines(folderPath);

                Console.Clear();
                trail.Clear();

                foreach (var line in fileContent)
                {
                    var parts = line.Split(',');
                    if (parts.Length == 4)
                    {
                        int x = int.Parse(parts[0]);
                        int y = int.Parse(parts[1]);
                        char character = char.Parse(parts[2]);
                        ConsoleColor color = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), parts[3]);

                        trail.Add((x, y, character, color));
                        DrawCharacter(x, y, character, color);
                    }
                }

                hasPreviousDrawing = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Hiba történt a fájl beolvasása során:");
                Console.WriteLine(ex.Message);
            }
        }


        static void SaveDrawingToFile(string folderPath)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(folderPath))
                {
                    foreach (var item in trail)
                    {
                        writer.WriteLine($"{item.x},{item.y},{item.character},{item.color}");
                    }
                }
                Console.WriteLine("A rajz sikeresen elmentve.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Hiba történt a mentés során:");
                Console.WriteLine(ex.Message);
            }
        }
        static void DrawBorder()
        {
            Console.Write('╔');
            for (int i = 1; i <= Console.WindowWidth - 2; i++)
            {
                Console.Write('═');
            }
            Console.Write('╗');
            for (int i = 1; i <= Console.WindowHeight - 2; i++)
            {
                Console.WriteLine('║');
            }
            Console.Write('╚');
            for (int i = 1; i <= Console.WindowWidth - 2; i++)
            {
                Console.Write('═');
            }
            Console.Write('╝');
            for (int i = 1; i <= Console.WindowHeight - 2; i++)
            {
                Console.SetCursorPosition(Console.WindowWidth - 1, i);
                Console.Write('║');
            }
        }

        static void DrawButtons(int selectedButton)
        {
            string[] buttons = { "Új Felület Nyitása", "Elözö Munka Betöltése", "Rajzolás Folytatása", "Fájl Mentése Másként", "Kilépés" };

            int buttonWidth = 30;
            int buttonHeight = 4;
            int verticalSpacing = 1;

            int startX = (Console.WindowWidth - buttonWidth) / 2;
            int startY = (Console.WindowHeight - (buttons.Length * (buttonHeight + verticalSpacing))) / 2;

            for (int i = 0; i < buttons.Length; i++)
            {
                int buttonPosY = startY + i * (buttonHeight + verticalSpacing);

                Console.SetCursorPosition(startX, buttonPosY);
                Console.Write('╔');
                for (int j = 1; j <= buttonWidth - 2; j++)
                {
                    Console.Write('═');
                }
                Console.Write('╗');

                Console.SetCursorPosition(startX, buttonPosY + 1);
                Console.Write('║');

                if (i + 1 == selectedButton)
                {
                    Console.BackgroundColor = ConsoleColor.Gray;
                    Console.ForegroundColor = ConsoleColor.Black;
                }
                else
                {
                    Console.ResetColor();
                }

                int textPadding = (buttonWidth - buttons[i].Length - 2) / 2;
                Console.Write(new string(' ', textPadding) + buttons[i] + new string(' ', buttonWidth - buttons[i].Length - 2 - textPadding));
                Console.ResetColor();
                Console.Write('║');

                Console.SetCursorPosition(startX, buttonPosY + 2);
                Console.Write('╚');
                for (int j = 1; j <= buttonWidth - 2; j++)
                {
                    Console.Write('═');
                }
                Console.Write('╝');
            }
        }

        static int x = 10, y = 10;
        static int prevX = 10, prevY = 10;
        static ConsoleColor currentColor = ConsoleColor.White;
        static char currentChar = '█';

        static Dictionary<ConsoleKey, char> charMapping = new Dictionary<ConsoleKey, char>
        {
            { ConsoleKey.F1, '█' },
            { ConsoleKey.F2, '▓' },
            { ConsoleKey.F3, '▒' },
            { ConsoleKey.F4, '░' }
        };

        static Dictionary<ConsoleKey, ConsoleColor> colorMapping = new Dictionary<ConsoleKey, ConsoleColor>
        {
            { ConsoleKey.D1, ConsoleColor.Red },
            { ConsoleKey.D2, ConsoleColor.Green },
            { ConsoleKey.D3, ConsoleColor.Blue },
            { ConsoleKey.D4, ConsoleColor.Yellow },
            { ConsoleKey.D5, ConsoleColor.Cyan },
            { ConsoleKey.D6, ConsoleColor.Magenta },
            { ConsoleKey.D7, ConsoleColor.White },
            { ConsoleKey.D8, ConsoleColor.Gray }
        };

        static void DrawInConsole(bool resumePreviousDrawing)
        {
            Console.CursorVisible = false;

            if (resumePreviousDrawing)
            {
                foreach (var item in trail)
                {
                    DrawCharacter(item.x, item.y, item.character, item.color);
                }
            }

            DrawCursor();

            while (true)
            {
                ConsoleKeyInfo input = Console.ReadKey(true);

                bool positionChanged = false;

                switch (input.Key)
                {
                    case ConsoleKey.UpArrow:
                        if (y > 0) { prevY = y; prevX = x; y--; positionChanged = true; }
                        break;
                    case ConsoleKey.DownArrow:
                        if (y < Console.WindowHeight - 2) { prevY = y; prevX = x; y++; positionChanged = true; }
                        break;
                    case ConsoleKey.LeftArrow:
                        if (x > 0) { prevY = y; prevX = x; x--; positionChanged = true; }
                        break;
                    case ConsoleKey.RightArrow:
                        if (x < Console.WindowWidth - 1) { prevY = y; prevX = x; x++; positionChanged = true; }
                        break;
                    case ConsoleKey.Spacebar:
                        trail.Add((x, y, currentChar, currentColor));
                        hasPreviousDrawing = true;
                        DrawCharacter(x, y, currentChar, currentColor);
                        break;
                    case ConsoleKey.Escape:
                        return;
                }

                if (colorMapping.ContainsKey(input.Key))
                {
                    currentColor = colorMapping[input.Key];
                }

                if (charMapping.ContainsKey(input.Key))
                {
                    currentChar = charMapping[input.Key];
                }

                if (positionChanged)
                {
                    ClearCursorArea(prevX, prevY);
                    DrawCursor();
                }
            }
        }

        static void DrawCharacter(int posX, int posY, char character, ConsoleColor color)
        {
            Console.SetCursorPosition(posX, posY);
            Console.ForegroundColor = color;
            Console.Write(character);
        }

        static void DrawCursor()
        {
            Console.SetCursorPosition(x, y);
            Console.ForegroundColor = currentColor;
            Console.Write('_');

            Console.SetCursorPosition(0, Console.WindowHeight - 1);
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write($"Kurzor pozíció: ({x}, {y}) | Prev. Kurzor pozíció: ({prevX}, {prevY}) | Szín: {currentColor} | Karakter: {currentChar}");
        }

        static void ClearCursorArea(int posX, int posY)
        {
            var existingTrail = trail.Find(t => t.x == posX && t.y == posY);
            if (existingTrail != default)
            {
                Console.SetCursorPosition(posX, posY);
                Console.ForegroundColor = existingTrail.color;
                Console.Write(existingTrail.character);
            }
            else
            {
                Console.SetCursorPosition(posX, posY);
                Console.Write(' ');
            }
        }
    }
}
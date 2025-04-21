using System;
using System.IO;

class TheGame
{
    const int SIZE = 19;

    static void Main()
    {
        try
        {
            string[] lines = File.ReadAllLines("input.txt");

            if (lines.Length == 0)
            {
                Console.WriteLine("Помилка: Файл порожнiй.");
                return;
            }

            int index = 0;
            if (!int.TryParse(lines[index++], out int testCases))
            {
                Console.WriteLine("Помилка: Перша строка повинна мiстити кiлькiсть тестiв.");
                return;
            }

            for (int t = 0; t < testCases; t++)
            {
                if (index + SIZE > lines.Length)
                {
                    Console.WriteLine($"Помилка: Недостатньо рядкiв для тесту #{t + 1}.");
                    return;
                }

                int[,] board = new int[SIZE, SIZE];

                for (int i = 0; i < SIZE; i++)
                {
                    string[] row = lines[index++].Trim().Split();

                    if (row.Length != SIZE)
                    {
                        Console.WriteLine($"Помилка: У рядку #{i + 1} повинно бути {SIZE} чисел.");
                        return;
                    }

                    for (int j = 0; j < SIZE; j++)
                    {
                        if (!int.TryParse(row[j], out int value) || value < 0 || value > 2)
                        {
                            Console.WriteLine($"Помилка: Неправильне значення '{row[j]}' у рядку {i + 1}, стовпцi {j + 1}.");
                            return;
                        }
                        board[i, j] = value;
                    }
                }

                Console.WriteLine($"\nПоле #{t + 1}:");

                for (int i = 0; i < SIZE; i++)
                {
                    for (int j = 0; j < SIZE; j++)
                    {
                        switch (board[i, j])
                        {
                            case 1:
                                Console.Write("1 ");
                                break;
                            case 2:
                                Console.Write("2 ");
                                break;
                            default:
                                Console.Write(". ");
                                break;
                        }
                    }
                    Console.WriteLine();
                }


                // Аналiз дошки
                bool found = false;

                for (int i = 0; i < SIZE && !found; i++)
                {
                    for (int j = 0; j < SIZE && !found; j++)
                    {
                        int color = board[i, j];
                        if (color == 0) continue;

                        if (Check(board, i, j, 0, 1, color) ||
                            Check(board, i, j, 1, 0, color) ||
                            Check(board, i, j, 1, 1, color) ||
                            Check(board, i, j, 1, -1, color))
                        {
                            PrintResult(color, i, j);
                            found = true;
                            break;
                        }
                    }
                }

                if (!found)
                    Console.WriteLine(0);
            }
        }
        catch (IOException ex)
        {
            Console.WriteLine($"Помилка читання файлу: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Невiдома помилка: {ex.Message}");
        }
    }

    static bool Check(int[,] board, int x, int y, int dx, int dy, int color)
    {
        for (int k = 1; k < 5; k++)
        {
            int nx = x + dx * k;
            int ny = y + dy * k;
            if (!InBounds(nx, ny) || board[nx, ny] != color)
                return false;
        }

        int prevX = x - dx, prevY = y - dy;
        int nextX = x + dx * 5, nextY = y + dy * 5;

        if (InBounds(prevX, prevY) && board[prevX, prevY] == color)
            return false;
        if (InBounds(nextX, nextY) && board[nextX, nextY] == color)
            return false;

        return true;
    }

    static bool InBounds(int x, int y) => x >= 0 && y >= 0 && x < SIZE && y < SIZE;

    static void PrintResult(int color, int x, int y)
    {
        Console.WriteLine(color);
        Console.WriteLine($"{x + 1} {y + 1}");
    }
}

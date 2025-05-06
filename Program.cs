using System;
using System.IO;

class TheGame
{
    const int SIZE = 19;
    const int WIN_LENGTH = 5;

    /// <summary>
    /// Значення, що можуть бути у клітинці дошки.
    /// </summary>
    enum CellState
    {
        Empty = 0,
        Black = 1,
        White = 2
    }

    /// <summary>
    /// Точка входу в програму. Зчитує тести з файлу, аналізує кожну дошку та виводить результат.
    /// </summary>
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
                        switch ((CellState)board[i, j])
                        {
                            case CellState.Black:
                                Console.Write("1 ");
                                break;
                            case CellState.White:
                                Console.Write("2 ");
                                break;
                            default:
                                Console.Write(". ");
                                break;
                        }
                    }
                    Console.WriteLine();
                }

                if (!TryFindWinner(board, out int winnerColor, out int x, out int y))
                {
                    Console.WriteLine(0);
                }
                else
                {
                    PrintResult(winnerColor, x, y);
                }
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

    /// <summary>
    /// Шукає переможця на дошці.
    /// </summary>
    static bool TryFindWinner(int[,] board, out int color, out int x, out int y)
    {
        for (int i = 0; i < SIZE; i++)
        {
            for (int j = 0; j < SIZE; j++)
            {
                color = board[i, j];
                if (color == (int)CellState.Empty) continue;

                if (Check(board, i, j, 0, 1, color) ||
                    Check(board, i, j, 1, 0, color) ||
                    Check(board, i, j, 1, 1, color) ||
                    Check(board, i, j, 1, -1, color))
                {
                    x = i;
                    y = j;
                    return true;
                }
            }
        }

        color = 0;
        x = 0;
        y = 0;
        return false;
    }

    /// <summary>
    /// Перевіряє, чи є виграшна послідовність довжиною WIN_LENGTH у заданому напрямку.
    /// </summary>
    /// <param name="board">Дошка гри</param>
    /// <param name="x">Початкова координата X</param>
    /// <param name="y">Початкова координата Y</param>
    /// <param name="dx">Зсув по X</param>
    /// <param name="dy">Зсув по Y</param>
    /// <param name="color">Колір каменя (1 або 2)</param>
    /// <returns>true, якщо знайдена валідна виграшна комбінація</returns>
    static bool Check(int[,] board, int x, int y, int dx, int dy, int color)
    {
        for (int k = 1; k < WIN_LENGTH; k++)
        {
            int nx = x + dx * k;
            int ny = y + dy * k;
            if (!InBounds(nx, ny) || board[nx, ny] != color)
                return false;
        }

        int prevX = x - dx, prevY = y - dy;
        int nextX = x + dx * WIN_LENGTH, nextY = y + dy * WIN_LENGTH;

        return !(InBounds(prevX, prevY) && board[prevX, prevY] == color) &&
                  !(InBounds(nextX, nextY) && board[nextX, nextY] == color);
    }

    /// <summary>
    /// Перевіряє, чи знаходяться координати в межах дошки.
    /// </summary>
    static bool InBounds(int x, int y) => x >= 0 && y >= 0 && x < SIZE && y < SIZE;

    /// <summary>
    /// Виводить результат гри — колір переможця та координати його першої фішки.
    /// </summary>
    static void PrintResult(int color, int x, int y)
    {
        Console.WriteLine(color);
        Console.WriteLine($"{x + 1} {y + 1}");
    }
}

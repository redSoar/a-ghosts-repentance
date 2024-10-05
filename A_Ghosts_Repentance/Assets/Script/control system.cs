using System;

class Program
{
    static void Main(string[] args)
    {
        // Initial position
        int x = 0;
        int y = 0;

        ConsoleKeyInfo keyInfo;
        Console.WriteLine("Use 'W', 'A', 'S', 'D' keys to move. Press 'Esc' to exit.");

        do
        {
            Console.Clear();
            Console.SetCursorPosition(x, y);
            Console.Write("O");  // This represents the object or character

            keyInfo = Console.ReadKey(true);

            switch (keyInfo.KeyChar)
            {
                case 'w':
                    if (y > 0) y--; //  up
                    break;
                case 's':
                    if (y < Console.WindowHeight - 1) y++; // down
                    break;
                case 'a':
                    if (x > 0) x--; // left
                    break;
                case 'd':
                    if (x < Console.WindowWidth - 1) x++; // right
                    break;
            }

        } while (keyInfo.Key != ConsoleKey.Escape);  // Exit the loop when 'Esc' is pressed
    }
}
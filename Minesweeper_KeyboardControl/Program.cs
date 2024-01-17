using System;

namespace Minesweeper_KeyboardControl
{
    internal class Program
    {
        static Navigator navigate = new Navigator();
        static System.Diagnostics.Stopwatch watch;

        static int BoardSize = 100; //10x10
        static char[] Arr = new char[BoardSize];  //visible array
        static char[] Arr2 = new char[BoardSize]; //invisible array 
        static bool DeveloperMode = false;       //// DEVELOPER MODE, type "P" on keyboard
        static int InputIndex = -1; //default InputIndex 
        static int ActiveInputIndex = 55; //current highlighted spot
        static int GameStatus = -1; // (-1)-game not started, 0-normal game, 1-win, 2-loss
        static int Bombs = 10;
        static int SpotsReaveled = 0; //amount of spots clicked by player default: if == 88 WIN

        static int Flags = Bombs;

        static void Main()
        {
            bool exit = false;
            while (!exit)
            {
                if (GameStatus == -1) //setting up the game after win/loss
                    arrSetUp();

                while (GameStatus < 1)
                {
                    Board();
                    PlayerInput();
                    CheckIfWin();
                }

                if (GameStatus == 2)                       // loss
                {
                    for (int i = 0; i < BoardSize; i++)
                    {
                        if (Arr2[i] == 'X') { Arr[i] = Arr2[i]; }
                    }

                    char opt = '.';
                    while (opt != 'y' && opt != 'n')
                    {
                        Board();
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("You've lost the game.");
                        Console.ResetColor();
                        Console.WriteLine("Try again? Y/N");


                        opt = Console.ReadKey().KeyChar;
                        switch (opt)
                        {
                            case 'y':
                                GameStatus = -1;
                                break;

                            case 'n':
                                exit = true;
                                break;

                            default:
                                break;
                        }
                    }

                }
                else if (GameStatus == 1)                  // win
                {
                    watch.Stop();


                    for (int i = 0; i < BoardSize; i++)
                    {
                        if (Arr2[i] == 'X') { Arr[i] = 'F'; }
                    }

                    char opt = '.';
                    while (opt != 'n' && opt != 'y')
                    {
                        Board();
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write("Congratulations!");
                        Console.Write("\tTime: ");
                        Console.WriteLine(watch.Elapsed.ToString("mm':'ss'.'ff"));
                        Console.ResetColor();
                        Console.WriteLine("Try again? Y/N");


                        opt = Console.ReadKey().KeyChar;
                        switch (opt)
                        {
                            case 'y':
                                GameStatus = -1;
                                break;

                            case 'n':
                                exit = true;
                                break;

                            default:
                                break;
                        }
                    }
                }
            }
        }

        static void arrSetUp()
        {
            Flags = 12;
            for (int i = 0; i < BoardSize; i++)
            {
                Arr[i] = '~';
                Arr2[i] = '_';
            }
            Flags = Bombs;
            SpotsReaveled = 0;
            ActiveInputIndex = 55;
        }

        static void bombsSetUp(int startingpoint)
        { //acces through PlayerInput()

            int b = 0;
            Random random = new Random();

            do
            {
                int i = random.Next(0, BoardSize);
                if (Arr2[i] != 'X' && i != startingpoint)
                {
                    Arr2[i] = 'X';
                    b++;
                }


            } while (b < Bombs);

            GameStatus++; //set to 0 - normal game

        }

        static void numbersSetUp()
        {

            for (int i = 0; i < BoardSize; i++)
            {
                if (Arr2[i] == '_')
                {
                    int amountOfBombs = 0;
                    if (navigate.UpLeft(i) != -1 && Arr2[navigate.UpLeft(i)] == 'X') { amountOfBombs++; }
                    if (navigate.Up(i) != -1 && Arr2[navigate.Up(i)] == 'X') { amountOfBombs++; }
                    if (navigate.UpRight(i) != -1 && Arr2[navigate.UpRight(i)] == 'X') { amountOfBombs++; }
                    if (navigate.Right(i) != -1 && Arr2[navigate.Right(i)] == 'X') { amountOfBombs++; }
                    if (navigate.DownRight(i) != -1 && Arr2[navigate.DownRight(i)] == 'X') { amountOfBombs++; }
                    if (navigate.Down(i) != -1 && Arr2[navigate.Down(i)] == 'X') { amountOfBombs++; }
                    if (navigate.DownLeft(i) != -1 && Arr2[navigate.DownLeft(i)] == 'X') { amountOfBombs++; }
                    if (navigate.Left(i) != -1 && Arr2[navigate.Left(i)] == 'X') { amountOfBombs++; }

                    if (amountOfBombs > 0)
                    {
                        Arr2[i] = (char)(amountOfBombs + '0'); //converting int to char with numbers 1-8
                    }
                }
            }

        }

        static void Board() // always visible Board
        {
            Console.Clear();
            Console.WriteLine("\nFlags: " + Flags + "\n");
            //Console.WriteLine("\t A B C D E F G H I J"); //COORDINATES
            Console.WriteLine("\t _ _ _ _ _ _ _ _ _ _");
            for (int i = 1; i <= 10; i++)
            {
                if (i <= 11)
                {
                    //Console.Write(" " + i + "\t"); //COORDINATES
                    Console.Write(" \t");
                }
                else
                {
                    //Console.Write(i + " "); //COORDINATES
                    Console.Write(" ");
                }
                Console.Write("|");
                for (int j = 0; j < 10; j++)
                {
                    int index = (i - 1) * 10 + j;
                    if (Arr[index] == 'X') // adding colours to display
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("X");
                        Console.ResetColor();
                        Console.Write("|");
                    }
                    else if (Arr[index] == 'F')
                    {
                        if (index == ActiveInputIndex && GameStatus == 0)
                            Console.ForegroundColor = ConsoleColor.Magenta;
                        else
                            Console.ForegroundColor = ConsoleColor.Green;

                        Console.Write(Arr[index]);
                        Console.ResetColor();
                        Console.Write("|");
                    }
                    else if (Arr[index] == '~')
                    {
                        if (index == ActiveInputIndex && GameStatus <= 0)
                            Console.ForegroundColor = ConsoleColor.Magenta;
                        else
                            Console.ForegroundColor = ConsoleColor.Blue;

                        Console.Write(Arr[index]);
                        Console.ResetColor();
                        Console.Write("|");
                    }
                    else
                    {
                        if (index == ActiveInputIndex && GameStatus == 0)
                        {
                            Console.ForegroundColor = ConsoleColor.Magenta;
                            Console.Write(Arr[index]);
                            Console.ResetColor();
                            Console.Write("|");
                        }
                        else
                            Console.Write(Arr[index] + "|");
                    }
                }
                Console.WriteLine();
            }

            if (DeveloperMode == true) //Board visible only in developer mode
            {
                Console.WriteLine("\n\n\n\t A B C D E F G H I J");
                Console.WriteLine("\t _ _ _ _ _ _ _ _ _ _");
                for (int i = 1; i < 11; i++)
                {
                    Console.WriteLine(i + "\t|{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}|", Arr2[10 * i - 10], Arr2[10 * i - 9], Arr2[10 * i - 8], Arr2[10 * i - 7], Arr2[10 * i - 6], Arr2[10 * i - 5], Arr2[10 * i - 4], Arr2[10 * i - 3], Arr2[10 * i - 2], Arr2[10 * i - 1]);
                }
                Console.WriteLine("\t\t\t\t\t\t\t DelevoperMode: ON");
                Console.WriteLine("\t\t\t\t\t\t\t InputIndex: " + InputIndex);
                Console.WriteLine("\t\t\t\t\t\t\t ActiveInputIndex: " + ActiveInputIndex);
                Console.WriteLine("\t\t\t\t\t\t\t GameStatus: " + GameStatus);
                Console.WriteLine("\t\t\t\t\t\t\t SpotsReaveled: " + SpotsReaveled);
            }

        }

        static void PlayerInput()
        {
            Console.WriteLine("\nControls:   Enter   Space   Left Right Up Down");

            ConsoleKeyInfo playerInputKey = Console.ReadKey();

            if (playerInputKey.Key == ConsoleKey.LeftArrow)
            {
                if (navigate.Left(ActiveInputIndex) != -1) { ActiveInputIndex = navigate.Left(ActiveInputIndex); }
            }
            else if (playerInputKey.Key == ConsoleKey.RightArrow)
            {
                if (navigate.Right(ActiveInputIndex) != -1) { ActiveInputIndex = navigate.Right(ActiveInputIndex); }
            }
            else if (playerInputKey.Key == ConsoleKey.UpArrow)
            {
                if (navigate.Up(ActiveInputIndex) != -1) { ActiveInputIndex = navigate.Up(ActiveInputIndex); }
            }
            else if (playerInputKey.Key == ConsoleKey.DownArrow)
            {
                if (navigate.Down(ActiveInputIndex) != -1) { ActiveInputIndex = navigate.Down(ActiveInputIndex); }
            }
            else if (playerInputKey.Key == ConsoleKey.Enter)
            {
                InputIndex = ActiveInputIndex;

                if (GameStatus == -1)// setting up bombs and numbers around them after first input
                {
                    bombsSetUp(InputIndex);
                    numbersSetUp();
                    watch = System.Diagnostics.Stopwatch.StartNew();
                }

                InputSpot(InputIndex, false);

            }
            else if (playerInputKey.Key == ConsoleKey.Spacebar)
            {
                InputIndex = ActiveInputIndex;
                InputSpot(InputIndex, true);
            }
            else if (playerInputKey.Key == ConsoleKey.P)
            {
                DeveloperMode = DeveloperMode == false ? true : false;
            }

        }

        static void InputSpot(int input, bool flag)
        {
            if (flag == true && Arr[input] == '~')
            {
                Arr[input] = 'F';
                Flags--;
            }
            else if (Arr[input] == '~' && Arr2[input] == '_')
            {
                fillFlood(input);
            }
            else if (Arr[input] == '~' && Arr2[input] != 'X')
            {
                Arr[input] = Arr2[input];
            }
            else if (Arr[input] == '~' && Arr2[input] == 'X')
            {
                GameStatus = 2;
            }
            else if (Arr[input] == 'F')
            {
                Arr[input] = '~';
                Flags++;
            }
        }

        static void fillFlood(int i)
        {
            if (i < 0 || i >= BoardSize || Arr2[i] != '_')
            {
                return;
            }

            if (Arr[i] == '~') //aplying to only uncovered spots
            {
                Arr[i] = Arr2[i];

                fillFlood(navigate.UpLeft(i));
                fillFlood(navigate.Up(i));
                fillFlood(navigate.UpRight(i));
                fillFlood(navigate.Right(i));
                fillFlood(navigate.DownRight(i));
                fillFlood(navigate.Down(i));
                fillFlood(navigate.DownLeft(i));
                fillFlood(navigate.Left(i));



                if (navigate.UpLeft(i) != -1 && Arr2[navigate.UpLeft(i)] != '_')
                    Arr[navigate.UpLeft(i)] = Arr2[navigate.UpLeft(i)];

                if (navigate.Up(i) != -1 && Arr2[navigate.Up(i)] != '_')
                    Arr[navigate.Up(i)] = Arr2[navigate.Up(i)];

                if (navigate.UpRight(i) != -1 && Arr2[navigate.UpRight(i)] != '_')
                    Arr[navigate.UpRight(i)] = Arr2[navigate.UpRight(i)];

                if (navigate.Right(i) != -1 && Arr2[navigate.Right(i)] != '_')
                    Arr[navigate.Right(i)] = Arr2[navigate.Right(i)];

                if (navigate.DownRight(i) != -1 && Arr2[navigate.DownRight(i)] != '_')
                    Arr[navigate.DownRight(i)] = Arr2[navigate.DownRight(i)];

                if (navigate.Down(i) != -1 && Arr2[navigate.Down(i)] != '_')
                    Arr[navigate.Down(i)] = Arr2[navigate.Down(i)];

                if (navigate.DownLeft(i) != -1 && Arr2[navigate.DownLeft(i)] != '_')
                    Arr[navigate.DownLeft(i)] = Arr2[navigate.DownLeft(i)];

                if (navigate.Left(i) != -1 && Arr2[navigate.Left(i)] != '_')
                    Arr[navigate.Left(i)] = Arr2[navigate.Left(i)];

            }
        }

        static void CheckIfWin()
        {
            SpotsReaveled = 0;
            for (int i = 0; i < BoardSize; i++)
            {
                if (Arr[i] != '~' && Arr[i] != 'X' && Arr[i] != 'F') { SpotsReaveled++; }
            }

            if (SpotsReaveled == BoardSize - Bombs)
            {
                GameStatus = 1;
            }
        }
    }

    class Navigator
    {

        public int Up(int i)
        {

            if (i < 10)
                return -1;
            else
            {
                return i - 10;
            }
        }

        public int Down(int i)
        {

            if (i > 89)
                return -1;
            else
            {
                return i + 10;
            }
        }

        public int Left(int i)
        {

            if (i % 10 == 0)
                return -1;
            else
            {
                return i - 1;
            }
        }

        public int Right(int i)
        {

            if (i % 10 == 9)
                return -1;
            else
            {
                return i + 1;
            }
        }

        public int UpLeft(int i)
        {

            if (i < 10 || i % 10 == 0)
                return -1;
            else
            {
                return i - 11;
            }
        }

        public int UpRight(int i)
        {

            if (i < 10 || i % 10 == 9)
                return -1;
            else
            {
                return i - 9;
            }
        }

        public int DownLeft(int i)
        {

            if (i > 89 || i % 10 == 0)
                return -1;
            else
            {
                return i + 9;
            }
        }

        public int DownRight(int i)
        {

            if (i > 89 || i % 10 == 9)
                return -1;
            else
            {
                return i + 11;
            }
        }




    }
}



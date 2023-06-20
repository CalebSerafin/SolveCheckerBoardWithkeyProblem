

/// <summary>
/// Solves the problem depicted in this video <see href="https://www.youtube.com/shorts/1_tJbkG_ckE" />
/// <h1>Summery:</h1>
/// In a room there is a chessboard.
/// Each of the 64 squares has a coin on it.
/// The warden has randomised the state of each coin to be either heads or tails.
/// Under one of the squares is a key to escape.
/// But only your partner can take the key. And you cannot communicate the state of the board, nor where the key is.
/// You are allowed to flip one coin.
/// How do you communicate to your partner which square the key is under?
/// </summary>
/// <param name="boardSize">The size of the board.</param>
/// <param name="board">The board.</param>
/// <param name="coin">The coin.</param>
void SolveBoard(int boardSize, long board, long coin)
{
    board = Math.Abs(board);
    coin = Math.Abs(coin);
    if (coin == 0) {
        Console.WriteLine("There is not coin.");
        return;
    }
    if (Math.Log2(board) > boardSize) {
        Console.WriteLine("Board is too big.");
        return;
    }
    if (Math.Log2(coin) > boardSize) {
        Console.WriteLine("Coin is too big.");
        return;
    }

    void PrintBin(long n) {
        Console.Write(Convert.ToString(n, 2).PadLeft(boardSize, '0'));
        Console.WriteLine($"    ={n}");
    }
    Console.WriteLine("Board and Coin:");
    PrintBin(board);
    PrintBin(coin);
    Console.WriteLine("\n");

    Console.WriteLine("XOR indices of 1s in board:");
    long e = 0;
    for (int i = 0; i < 63; ++i)
    {
        long boardShifted = board >> i;
        if ((boardShifted & 0b1) == 1)
        {
            e ^= i;
            PrintBin(i);
        }
    }
    Console.WriteLine("^");
    Console.WriteLine(new string('-', boardSize));
    PrintBin(e);
    Console.WriteLine("\n");


    Console.WriteLine("Log2 of key to get key index:");
    PrintBin(coin);

    long keyIndex = (long)Math.Log2(coin);
    Console.WriteLine("log2");
    Console.WriteLine(new string('-', boardSize));
    PrintBin(keyIndex);
    Console.WriteLine("\n");

    Console.WriteLine("XOR keyIndex with e to get which coin to get flipIndex:");
    PrintBin(keyIndex);
    PrintBin(e);
    long flipIndex = keyIndex ^ e;
    Console.WriteLine("^");
    Console.WriteLine(new string('-', boardSize));
    PrintBin(flipIndex);
    Console.WriteLine("\n");

    Console.WriteLine("Shift 1 with flipIndex to get flipMask:");
    PrintBin(1);
    PrintBin(flipIndex);
    long flipMask = 1 << (int)flipIndex;
    Console.WriteLine("^");
    Console.WriteLine(new string('-', boardSize));
    PrintBin(flipMask);
    Console.WriteLine("\n");

    Console.WriteLine("XOR board with flipMask to create the new board:");
    long board2 = board ^ flipMask;
    PrintBin(board);
    PrintBin(flipMask);
    Console.WriteLine("^");
    Console.WriteLine(new string('-', boardSize));
    PrintBin(board2);
    Console.WriteLine("\n");

    Console.WriteLine("XOR indices of 1s in modified board to get the index of the flipped coin:");
    long coin2 = 0;
    for (int i = 0; i < 63; ++i)
    {
        long boardShifted = board2 >> i;
        if ((boardShifted & 0b1) == 1)
        {
            coin2 ^= i;
            PrintBin(i);
        }
    }
    Console.WriteLine("^");
    Console.WriteLine(new string('-', boardSize));
    PrintBin(coin2);
    Console.WriteLine("\n");

}

SolveBoard(8, 0b00101011, 0b00001000);


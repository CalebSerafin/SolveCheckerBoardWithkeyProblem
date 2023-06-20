

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
void SolveBoard(byte boardSize, ulong headsOnBoard, ulong keyOnBoard) {
    if (keyOnBoard == 0) {
        Console.WriteLine("There is not coin.");
        return;
    }
    if (ulong.Log2(headsOnBoard) > boardSize) {
        Console.WriteLine("Board is too big.");
        return;
    }
    if (ulong.Log2(keyOnBoard) > boardSize) {
        Console.WriteLine("Coin is too big.");
        return;
    }

    void PrintBin(ulong n) {
        Console.Write(Convert.ToString((long)n, 2).PadLeft(boardSize, '0'));
        Console.WriteLine($"    ={n}");
    }
    void PrintBinByte(byte n) {
        byte log2OfBoardSize = byte.Log2(boardSize);
        Console.Write(Convert.ToString(n, 2).PadLeft(log2OfBoardSize, '0').PadLeft(boardSize));
        Console.WriteLine($"    ={n}");
    }
    Console.WriteLine("Chess Board. 0 represents tails, 1 represents heads:");
    PrintBin(headsOnBoard);
    Console.WriteLine("Key. 1 represents where the key is:");
    PrintBin(keyOnBoard);
    Console.WriteLine("\n");

    Console.WriteLine("XOR the encoded positions of all the heads on the board:");
    IEnumerable<byte> EncodePositions(ulong board) {
        for (byte i = 0; i < 63; ++i) {
            // Check if bit is set
            ulong singleSetBitMask = 1UL << i;
            bool isBitSet = (board & singleSetBitMask) != 0;
            if (isBitSet)
                yield return i;
        }
    }
    byte EncodePosition(ulong board) {
        return (byte)ulong.Log2(board);
    }
    ulong DecodePosition(byte position) {
        return 1UL << position;
    }
    byte combinedHeadPositions = 0;
    foreach (byte position in EncodePositions(headsOnBoard)) {
        combinedHeadPositions ^= position;
        PrintBinByte(position);
    }
    Console.WriteLine("^");
    Console.WriteLine(new string('-', boardSize));
    PrintBin(combinedHeadPositions);
    Console.WriteLine("\n");


    Console.WriteLine("Encode the key as it's position:");
    PrintBin(keyOnBoard);
    byte keyIndex = EncodePosition(keyOnBoard);
    Console.WriteLine("log2");
    Console.WriteLine(new string('-', boardSize));
    PrintBinByte(keyIndex);
    Console.WriteLine("\n");

    Console.WriteLine("XOR the key's position with combined positions of the head coins to get index of position of the coin to flip:");
    PrintBinByte(keyIndex);
    PrintBinByte(combinedHeadPositions);
    byte flipIndex = (byte)(keyIndex ^ combinedHeadPositions);
    Console.WriteLine("^");
    Console.WriteLine(new string('-', boardSize));
    PrintBinByte(flipIndex);
    Console.WriteLine("\n");

    Console.WriteLine("Get the coin which needs to flip:");
    PrintBinByte(1);
    PrintBinByte(flipIndex);
    ulong flipMask = DecodePosition(flipIndex);
    Console.WriteLine("<<");
    Console.WriteLine(new string('-', boardSize));
    PrintBin(flipMask);
    Console.WriteLine("\n");

    Console.WriteLine("XOR the board with flipped coin to create the new board:");
    ulong board2 = headsOnBoard ^ flipMask;
    PrintBin(headsOnBoard);
    PrintBin(flipMask);
    Console.WriteLine("^");
    Console.WriteLine(new string('-', boardSize));
    PrintBin(board2);
    Console.WriteLine("\n");
    Console.WriteLine("\n");

    Console.WriteLine("From the reader's perspective:");
    Console.WriteLine("XOR positions of heads in modified board to get the combined position. This is also the position of the key:");
    byte key2Position = 0;
    foreach (byte position in EncodePositions(board2)) {
        key2Position ^= position;
        PrintBinByte(position);
    }
    Console.WriteLine("^");
    Console.WriteLine(new string('-', boardSize));
    PrintBinByte(key2Position);
    Console.WriteLine("\n");

    Console.WriteLine("What means that if the key is drawn on the board, it will look like:");
    PrintBinByte(1);
    PrintBinByte(key2Position);
    Console.WriteLine("<<");
    Console.WriteLine(new string('-', boardSize));
    ulong key2OnBaord = DecodePosition(key2Position);
    PrintBin(key2OnBaord);
    Console.WriteLine("\n");
}

SolveBoard(8, 0b00101011, 0b00001000);


# Solves the chessboard and coins puzzle

Solves the problem depicted in this video [Chessboard and coins puzzle](https://www.youtube.com/shorts/1_tJbkG_ckE)
## Problem Summary
> In a room there is a chessboard. <br/>
> Each of the 64 squares has a coin on it. <br/>
> The warden has randomised the state of each coin to be either heads or tails. <br/>
> Under one of the squares is a key to escape. <br/>
> But only your partner can take the key. And you cannot communicate the state of the board, nor where the key is. <br/>
> You are allowed to flip one coin. <br/>
> How do you communicate to your partner which square the key is under? <br/>

## Solution
We want to encode the position of the key within our modified board. <br/>
Let `board` be a bit array of size `boardSize`. <br/>
On a board of a given size `boardSize`. There are `boardSize` possibilities for the position of the key. <br/>
To represent a coin with tails, we will store a 0 in the board. Heads will store a 1 in the board. <br/>
Therefore, the `board` will be an array of 0 and 1. Eg: ...0010011 <br/>
<br/>
The possible position of the hidden key is Zero index based. Therefore, it is in the domain [0, `boardSize` -1] <br/>
To encode the key position in binary, we will need log2(`boardSize`) bits.<br/>
Let `encodedSize` = log2(`boardSize`). <br/>
Let `encodedField` be a bit array of size `encodedField` <br/>
<br/>
To represent the key being under bit 5 on the `board`, it will be encoded as `..101`. <br/>
This encoded position will fit within our `encodedField`. <br/>
Any possible combination of the `encodedField` will map to a unique square on the chessboard. <br/>
However, we cannot directly store `encodedField` inside the `board`, since we can only change one bit. <br/>
<br/>
Instead, we can encode the position of any head coins on the board in an `encodedField`. <br/>
However, that alone is only useful if we start out with an empty board. However, it is a useful starting point. <br/>
Lets say the initial `board` of head coins ha a coin at index 2, drawing it on the board looks like 00000100. <br/>
We can encode the single head's position as 010, because it is at index 2 from the right (remember zero-based).<br/>
If the key is on the board at index 5, drawing it on the board looks like 00100000.<br/>
The key's position can also be encoded to give us 101.<br/>
We can see that the encoded positions of the head coins and the key is different. In fact, the difference of each bits can be expressed using XOR to give us 111.<br/>
<br/>
So we can encode the position of a single-head coin on the board, and compare how different it is to where the key is on the board.<br/>
But what if we decode the difference, that we got from using XOR, to give us a position on the board?<br/>
Based on this position, we only need to flip one coin to record the difference as well as keep the initial board state.<br/>
Using the example, that means we will flip the coin at index 7. Giving us a board of 10000100. <br/>
<br/>
Now, all the reader needs to do, is re-encode the positions of both coins, giving them 111 and 010. <br/>
They do not know which coin we flipped nor which was there already, but that does not matter. <br/>
If they XOR the encoded positions of 111 and 010 they will get 101. Which when decoded tells us the key is at position 5, a drawing that looks like 00100000. <br/>
The reader has arrived at our answer. <br/>
 <br/>
So for edge cases, what happens if during the XOR step, the difference of the key's and head's positions cancel out and leave us with 000? <br/>
That means we will unflip the only head coin on the board and leave it empty. <br/>
But that is the answer because the only way for the difference between the key's position and the head-coins position to be the same as the head-coins position is if the key's position is zero. <br/>
And zero is what we have on the board. Note, there are two ways to represent any zero on the board, either with nothing, or a head coin at the zero index. <br/>
Multiple options are required because flipping coins is not optional, so we always need a valid move. <br/>
The concept of multiple options does not only apply to the key being stored at zero. <br/>
<br/>
Okay, let's finally address the elephant in the room, we don't only have one coin with heads, but instead could have all heads, no heads, or somewhere in between. <br/>
As shown, we encode the position of heads into a smaller domain. A single head is drawn on a big board as 00000100, which gets shrunk to 010 with no information lost. <br/>
If our example board is instead 10011101, we get the 8 positions encoded to 111, 100, 011, 010, and 000 respectively. <br/>
We don't want 8 different encodings of the initial board, because our previous solution only works with 1. <br/>
To fix this, we can then combine them all with XOR into a single encoding of 010. <br/>
Now, without any other changes, we can decode this position to give us a board of 00000100 <br/>
This is a board with only one head coin, which is a problem that we have already solved! <br/>
This process loses information, but it gives us a way to digest any complex initial board state, into a simple one. <br/>
<br/>
So now again, using XOR we can find the difference between the same key position as before, and the composite head coin encodings. <br/>
If the key's is encoded as 101, This tells us to flip a coin at position 111 or index 7. <br/>
We now flip this coin on the original board, not the simplified one. Giving us a new complex board as 00011101 <br/>
Using the same technique as before, the reader can encode all the head coin positions on the board as 100, 011, 010, and 000 respectively.
Combining them with XOR yields an encoded position of 101. Which tells us that the key is at index 5 or drawn on the board as 00100000 <br/>
<br/>

## Example Code
In this repo there is example C# cope in the Program.cs<br/>
It was critical to working out and testing candidate solutions.<br/>
It has a hardcoded example for demonstration purposes.<br/>
The program produces the following output.<br/>
```
Chess Board. 0 represents tails, 1 represents heads:
00101011    =43
Key. 1 represents where the key is:
00001000    =8


XOR the encoded positions of all the heads on the board:
     000    =0
     001    =1
     011    =3
     101    =5
^
--------
00000111    =7


Encode the key as it's position:
00001000    =8
log2
--------
     011    =3


XOR the key's position with combined positions of the head coins to get index of position of the coin to flip:
     011    =3
     111    =7
^
--------
     100    =4


Get the coin which needs to flip:
     001    =1
     100    =4
<<
--------
00010000    =16


XOR the board with flipped coin to create the new board:
00101011    =43
00010000    =16
^
--------
00111011    =59




From the reader's perspective:
XOR positions of heads in modified board to get the combined position. This is also the position of the key:
     000    =0
     001    =1
     011    =3
     100    =4
     101    =5
^
--------
     011    =3


What means that if the key is drawn on the board, it will look like:
     001    =1
     011    =3
<<
--------
00001000    =8
```

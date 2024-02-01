using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class ReversiBoard
{
    private readonly char[,] _board;

    private readonly int _width;
    private readonly int _height;

    public ReversiBoard(string inputString)
    {
        // split dimensions and actual board and then split both at " "
        int firstNewline = inputString.IndexOf("\n");
        string[] wd = inputString.Substring(0, firstNewline).Split(" ");
        // get substring starting after width and height, then remove all newlines, then also split at " "
        string[] boardString = Regex.Replace(inputString.Substring(firstNewline + 1, inputString.Length - (firstNewline + 1)), @"\t|\n|\r", "").Split(" ");
        
        // parse dimensions
        _width = int.Parse(wd[0]);
        _height = int.Parse(wd[1]);
        _board = new char[_width, _height];
        
        //Debug.Log("Created board with width " + _width + " and height " + _height);

        // parse board itself
        for (int y = 0; y < _height; y++)
            for (int x = 0; x < _width; x++)
                _board[x, y] = ParseTile(boardString[_width * y + x]);

        //Debug.Log(ConvertToStringBoard());
    }

    // i know this is kinda redundant but i still left it here bc maybe in the future it can be represented with an enum if that is wanted
    private char ParseTile(string t)
    {
        switch (t)
        {
            case ".":
                return '.';
            case "O":
                return 'O';
            case "X":
                return 'X';
            default:
                throw new System.ArgumentException("Tile in Reversi must be ., O or X. Was: >" + t + "<");
        }
    }

    /// <summary>
    /// Converts the board back to string <i>without</i> the width and height in front of it.
    /// </summary>
    private string ConvertToStringBoard(bool showIndex = false)
    {
        string s = "";
        for (int y = 0; y < _height; y++)
        {
            for (int x = 0; x < _width; x++)
                if (showIndex)
                    s += "[" + x + "," + y + "] = " + _board[x, y] + "  |  ";
                else
                    s += _board[x, y] + " ";
            
            s += "\n";
        }
        return s;
    }
    
    /// <summary>
    /// Returns the disc placement of the best step for the current board.
    /// </summary>
    public string NextStep()
    {
        int bestX = 0;
        int bestY = 0;
        int bestValue = -1;
        
        for (int y = 0; y < _height; y++)
            for (int x = 0; x < _width; x++)
                if (_board[x, y] == '.')
                {
                    int value = GetPlacementValue(x, y);

                    if (value > bestValue)
                    {
                        bestValue = value;
                        bestX = x;
                        bestY = y;
                    }
                }

        return IndexToMoveNotation(bestX, bestY);
    }

    private int GetPlacementValue(int x, int y)
    {
        int value = 0;

        value += IterateDirection(x, y, 0, 1); // down
        value += IterateDirection(x, y, 0, -1); // up
        value += IterateDirection(x, y, 1, 0); // right
        value += IterateDirection(x, y, -1, 0); // left
        
        value += IterateDirection(x, y, 1, 1); // down right
        value += IterateDirection(x, y, -1, -1); // up left
        value += IterateDirection(x, y, -1, 1); // down left
        value += IterateDirection(x, y, 1, -1); // up right
        
        return value;
    }

    /// <summary>
    /// Iterates a direction on the board from a given start position (x, y) in direction given by (moveX, moveY).
    /// </summary>
    /// <param name="x">Start position of x.</param>
    /// <param name="y">Start position of y.</param>
    /// <param name="moveX">Movement of x. Should only be -1, 0 or 1.</param>
    /// <param name="moveY">Movement of y. Should only be -1, 0 or 1.</param>
    /// <returns></returns>
    private int IterateDirection(int x, int y, int moveX, int moveY)
    {
        int value = 0;
        int enemyCount = 0;

        // i define this function to ensure that the for loop condition and the actual array access values are 100% the same. (so i cannot have any typos)
        int cI(int start, int index, int direction) => start + index * direction;
        
        // the break condition is simply a boundary check for the values i compute using i, x, y, and move.
        for (int i = 1;
             cI(x, i, moveX) >= 0 && 
             cI(x, i, moveX) < _width &&
             cI(y, i, moveY) >= 0 &&
             cI(y, i, moveY) < _height;
             i++)
        {
            int cx = cI(x, i, moveX);
            int cy = cI(y, i, moveY);
            char t = _board[cx, cy];
            if (t == '.')
            {
                break; // nothing in this direction
            }
            else if (t == 'O')
            {
                enemyCount++;
            }
            else if (t == 'X')
            {
                value += enemyCount; // we see an ally disc in this line all enemies between the two can be beaten
                break; // we cannot look further than this
            }
        }

        return value;
    }

    private string IndexToMoveNotation(int x, int y)
    {
        const string abc = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        return abc[x] + (y + 1).ToString();
    }
}

public class Solution
{

    public static string PlaceToken(string board)
    {
        ReversiBoard rb = new ReversiBoard(board);

        return rb.NextStep();
    }


    public static void Main()
    {
        // 1. Correct Answer: "E1"
        string board1 = @"5 1
X O O O . ";
        string result1 = Solution.PlaceToken(board1);
        Debug.Log("board 1: " + result1 + " and should be E1");
        
        
        // 2. Correct Answer: "B2" 
        string board2 = @"8 7
. . . . . . . . 
. . . . . . . . 
. . O . . . . . 
. . . O X . . . 
. . . X O O . . 
. . . . . X . . 
. . . . . . X . ";
        string result2 = Solution.PlaceToken(board2);
        Debug.Log("board 2: " + result2 + " and should be B2");
        
        
        // 3. Correct Answer: "D3", "C4", "F5", "E6" 
        string board3 = @"8 8
. . . . . . . . 
. . . . . . . . 
. . . . . . . . 
. . . O X . . . 
. . . X O . . . 
. . . . . . . . 
. . . . . . . . 
. . . . . . . . ";
        string result3 = Solution.PlaceToken(board3);
        Debug.Log("board 3: " + result3 + " and should be one of D3, C4, F5, E6");
        
        
        // 4. Correct Answer: "D6 
        string board4 = @"7 6
. . . . . . . 
. . . O . O . 
X O O X O X X 
. O X X X O X 
. X O O O . X 
. . . . . . . ";
        string result4 = Solution.PlaceToken(board4);
        Debug.Log("board 4: " + result4 + " and should be D6");
    }
}

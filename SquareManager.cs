using UnityEngine;

public class SquareManager : MonoBehaviour
{
    static Square[,] squares;
    static int maxX, maxY;
    static LevelGenerator lg;

    void Awake()
    {
        squares = null;
        maxX = 0;
        maxY = 0;
        lg = GetComponent<LevelGenerator>();
    }

    public static void OwnStart()
    {
        squares = lg.squares;
        maxX = squares.GetLength(0);
        maxY = squares.GetLength(1);
    }

    public static void OpenSquares(int x, int y)
    {
        // Открыть все блоки вокруг, если нажатый блок пустой
        if (squares[x, y].bombsAround == 0 && !squares[x, y].isBomb)
        {
            for (int X = -1; X <= 1; X++)
            {
                if (x + X < 0 || x + X >= maxX)
                    continue;

                for (int Y = -1; Y <= 1; Y++)
                {
                    if (y + Y < 0 || y + Y >= maxY || x + X == 0 && y + Y == 0)
                        continue;

                    squares[x + X, y + Y].Open();
                }
            }
        }
    }
}
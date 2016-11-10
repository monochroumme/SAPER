using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class SquareManager : MonoBehaviour
{
    Square[,] squares;
    int maxX, maxY;
    int bombsAmount;
    LevelGenerator lg;

    public static SquareManager instance;

    void Awake()
    {
        squares = null;
        maxX = 0;
        maxY = 0;
        lg = GetComponent<LevelGenerator>();
        instance = this;
    }

    public void OwnStart()
    {
        squares = lg.squares;
        maxX = squares.GetLength(0);
        maxY = squares.GetLength(1);
        bombsAmount = lg.bombsAmount;
        Bomber.instance.bombsAmount = bombsAmount;
    }

    public void OpenSquares(int x, int y)
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
                    if (y + Y < 0 || y + Y >= maxY) // || x + X == 0 && y + Y == 0)
                        continue;

                    squares[x + X, y + Y].Open();
                }
            }
        }
    }

    public void ShowBombSquares(bool win)
    {
        for (int i = 0; i < bombsAmount; i++)
        {
            lg.bombSquares[i].Show(win);
        }
    }
    public void ExplodeAllBombs(Square first)
    {
        List<Square> notExploded = new List<Square>();

        for (int i = 0; i < bombsAmount; i++)
        {
            if(first != lg.bombSquares[i]) // Если это не сама открытая бомба, то добавить его в массив неВзорвавшихся
            {
                notExploded.Add(lg.bombSquares[i]);
            }
        }

        while(notExploded.Count > 0) // Рандомно взрывать бомбы (кроме самой, которую открыли)
        {
            int random = Random.Range(0, notExploded.Count);
            Square randomSquare = notExploded[random];
            notExploded.Remove(randomSquare);
            randomSquare.Show(false);
            StartCoroutine("ExplodeBomb", randomSquare);
        }
    }

    IEnumerator ExplodeBomb(Square bomb)
    {
        yield return new WaitForSeconds(Random.Range(0.2f, 1f));
        bomb.Explode();
    }
}
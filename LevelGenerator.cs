using UnityEngine;
using System.Collections.Generic;

public class LevelGenerator : MonoBehaviour
{
    public int maxX, maxY;
    public float gapBetweenSquares;
    public GameObject squarePrefab;
    public bool createBombsOnCorners;
    public int bombsAmount;

    [HideInInspector]
    public Square[,] squares;
    [HideInInspector]
    public Square[] bombSquares;
    GameObject parentOfSquares;

    void Start()
    {
        GenerateLevel(maxX, maxY, bombsAmount);
        SquareManager.instance.OwnStart();
    }

    public void GenerateLevel(int X, int Y, int bombs)
    {
        maxX = X;
        maxY = Y;
        bombsAmount = bombs;
        squares = new Square[maxX, maxY];
        parentOfSquares = new GameObject("Squares");
        
        for (int x = 0; x < maxX; x++)
        {
            for (int y = 0; y < maxY; y++)
            {
                Vector3 position;

                if (x == 0 && y == 0)
                {
                    float firstSquarePosX = (gapBetweenSquares + squarePrefab.transform.localScale.x) / 2f * (maxX - 1f);
                    float firstSquarePosY = (gapBetweenSquares + squarePrefab.transform.localScale.y) / 2f * (maxY - 1f);
                    position = new Vector3(-firstSquarePosX, -firstSquarePosY);
                }
                else if (y == 0)
                {
                    float newPos = squares[x - 1, y].transform.position.x + squarePrefab.transform.localScale.x + gapBetweenSquares;
                    position = new Vector3(newPos, squares[x - 1, y].transform.position.y);
                }
                else
                {
                    float newPos = squares[x, y - 1].transform.position.y + squarePrefab.transform.localScale.y + gapBetweenSquares;
                    position = new Vector3(squares[x, y - 1].transform.position.x, newPos);
                }
                GameObject square = (GameObject)Instantiate(squarePrefab, position, Quaternion.identity);
                squares[x, y] = square.GetComponent<Square>();
                squares[x, y].x = x;
                squares[x, y].y = y;
                squares[x, y].transform.parent = parentOfSquares.transform;
            }
        }

        GenerateBombs(createBombsOnCorners);
        GetValues();
        SetValues();
    }

    void GenerateBombs(bool createOnCorners)
    {
        List<Square> availableSquares = new List<Square>();
        for (int x = 0; x < maxX; x++)
            for (int y = 0; y < maxY; y++)
                availableSquares.Add(squares[x, y]);

        // Если параметр СоздатьНаКраях отключен, то удалить их из массива с доступными ячейками
        if (!createOnCorners)
        {
            availableSquares.RemoveAt(availableSquares.Count - 1);
            availableSquares.RemoveAt(maxY * (maxX - 1));
            availableSquares.RemoveAt(maxY - 1);
            availableSquares.RemoveAt(0);
        }

        // Заспавнить бомбы
        bombSquares = new Square[bombsAmount];
        for (int i = 0; i < bombsAmount; i++)
        {
            int random = Random.Range(0, availableSquares.Count);
            Square randomSquare = availableSquares[random];
            availableSquares.Remove(randomSquare);
            squares[randomSquare.x, randomSquare.y].isBomb = true;

            bombSquares[i] = squares[randomSquare.x, randomSquare.y];
        }
    }

    void GetValues()
    {
        for (int x = 0; x < maxX; x++)
        {
            for (int y = 0; y < maxY; y++)
            {
                if (squares[x, y].isBomb)
                {
                    continue;
                }

                squares[x, y].bombsAround = GetAmountOfAdjacentMines(squares[x, y]);
            }
        }
    }

    void SetValues()
    {
        for(int x = 0; x < maxX; x++)
        {
            for (int y = 0; y < maxY; y++)
            {
                squares[x, y].SetValues();
            }
        }
    }

    int GetAmountOfAdjacentMines(Square square)
    {
        int mines = 0;
        for (int x = -1; x <= 1; x++)
        {
            if (square.x + x < 0 || square.x + x >= maxX)
                continue;

            for (int y = -1; y <= 1; y++)
            {
                if (square.y + y < 0 || square.y + y >= maxY || square.x + x == 0 && square.y + y == 0)
                    continue;

                if (squares[square.x + x, square.y + y].isBomb)
                    mines++;
            }
        }
        return mines;
    }
}
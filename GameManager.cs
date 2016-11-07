using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static bool isPlaying;
	static int squaresWOBombs;

	void Start()
	{
        isPlaying = true;
		LevelGenerator lg = GetComponent<LevelGenerator>();
		squaresWOBombs = lg.maxX * lg.maxY - lg.bombsAmount;
	}

	public static void OnOpen()
	{
		squaresWOBombs--;

		if(squaresWOBombs <= 0)
		{
			// Показать места с бомбами V и сделать небольшую штучку поздравительную, а потом включить WIN GUI.
			SquareManager.instance.ShowBombSquares(true);
            Win();
		}
	}

    static void Win()
    {
        // TODO WIN
        isPlaying = false;
    }

    public static void GameOver()
    {
        // TODO GAMEOVER
    }
}
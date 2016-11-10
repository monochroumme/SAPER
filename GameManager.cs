using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject firework;
    public GameObject winT;
    public GameObject goT;
    [HideInInspector]
    public bool isPlaying;
    [HideInInspector]
    public bool isStarted;
    int squaresWOBombs;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        GameObject.Find("Porter").GetComponent<Porter>().SetValues();
        isPlaying = true;
        LevelGenerator lg = GetComponent<LevelGenerator>();
        squaresWOBombs = lg.maxX * lg.maxY - lg.bombsAmount;
    }

    public void OnOpen()
    {
        squaresWOBombs--;

        if(squaresWOBombs <= 0)
        {
            // Показать места с бомбами V и сделать небольшую штучку поздравительную, а потом включить WIN GUI.
            SquareManager.instance.ShowBombSquares(true);
            Win();
        }
    }

    public void Win()
    {
        isPlaying = false;
        firework.SetActive(true);
        winT.SetActive(true);
    }

    public void GameOver()
    {
        isPlaying = false;
        goT.SetActive(true);
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GoBack()
    {
        DestroyPorter();
        SceneManager.LoadScene("Menu");
    }

    void DestroyPorter()
    {
        DestroyImmediate(GameObject.Find("Porter"));
    }
}
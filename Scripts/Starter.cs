using UnityEngine;
using UnityEngine.SceneManagement;

public class Starter : MonoBehaviour
{
    Porter porter;

    void Start()
    {
        porter = GetComponent<Porter>();
    }

    public void Easy()
    {
        porter.maxX = 9;
        porter.maxY = 9;
        porter.bombsAmount = 10;
        porter.createOnCorners = false;
        LoadLevel();
    }

    public void Medium()
    {
        porter.maxX = 12;
        porter.maxY = 12;
        porter.bombsAmount = 30;
        porter.createOnCorners = true;
        LoadLevel();
    }

    public void Hard()
    {
        porter.maxX = 14;
        porter.maxY = 18;
        porter.bombsAmount = 50;
        porter.createOnCorners = true;
        LoadLevel();
    }

    void LoadLevel()
    {
        SceneManager.LoadScene("Game");
    }
}
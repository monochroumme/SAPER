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
        porter.cameraOffset = new Vector3(0, 0.7f, -10f);
        porter.cameraSize = 5.9f;
        porter.createOnCorners = false;
        LoadLevel();
    }

    public void Medium()
    {
        porter.maxX = 12;
        porter.maxY = 12;
        porter.bombsAmount = 30;
        porter.cameraOffset = new Vector3(0, 0.7f, -10f);
        porter.cameraSize = 7.65f;
        porter.createOnCorners = true;
        LoadLevel();
    }

    public void Hard()
    {
        porter.maxX = 16;
        porter.maxY = 16;
        porter.bombsAmount = 69;
        porter.cameraOffset = new Vector3(0, 1.1f, -10f);
        porter.cameraSize = 10.35f;
        porter.createOnCorners = true;
        LoadLevel();
    }

    void LoadLevel()
    {
        SceneManager.LoadScene("Game");
    }
}
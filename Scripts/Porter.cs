using UnityEngine;
using System.Collections;

public class Porter : MonoBehaviour
{
    public int maxX, maxY;
    public int bombsAmount;
    public bool createOnCorners;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void SetValues()
    {
        LevelGenerator lg = GameObject.Find("GameManager").GetComponent<LevelGenerator>();
        lg.maxX = maxX;
        lg.maxY = maxY;
        lg.bombsAmount = bombsAmount;
        lg.createBombsOnCorners = createOnCorners;
        lg.StartGenerating();
    }
}

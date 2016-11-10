using UnityEngine;
using System.Collections;

public class Porter : MonoBehaviour
{
    public int maxX, maxY;
    public int bombsAmount;
    public Vector3 cameraOffset;
    public float cameraSize;
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
        Camera.main.orthographicSize = cameraSize;
        Camera.main.transform.position = cameraOffset;
    }
}

using UnityEngine;
using System.Collections;

public class AutoCamera : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {
        int maxX = GameObject.Find("Porter").GetComponent<Porter>().maxX;

        Camera.main.orthographicSize = maxX * Camera.main.aspect * 1.7f;
    }
}

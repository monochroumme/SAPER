using UnityEngine;
using UnityEngine.UI;

public class Bomber : MonoBehaviour
{
    public static Bomber instance;
    Text bombT;
    int bombs;
    public int bombsAmount
    {
        get
        {
            return bombs;
        }
        set
        {
            bombs = value;
            ChangeValue();
        }
    }


    void Awake()
    {
        instance = this;
        bombT = GetComponent<Text>();
        ChangeValue();
    }

    public void ChangeValue()
    {
        bombT.text = bombs.ToString();
    }
}
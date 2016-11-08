using UnityEngine;
using UnityEngine.UI;

public class Bomber : MonoBehaviour
{
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

    public static Bomber instance;

    void Start()
    {
        bombT = GetComponent<Text>();
        ChangeValue();
        instance = this;
    }

    public void ChangeValue()
    {
        bombT.text = bombs.ToString();
    }
}
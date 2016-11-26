using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Square : MonoBehaviour
{
    [HideInInspector]
    public int x;
    [HideInInspector]
    public int y;

    public bool isBomb;
    public int bombsAround;
    public Text bombsAroundText;
    public Image isBombI;
    public Image isFlaggedI;
    public ParticleSystem explosion;

    bool isOpened = false;
    bool isFlagged = false;
    bool isShowed = false;

    float holdTime = 0.3f;

    float lastTapTime;

    Ray ray;
    Ray mRay;
    RaycastHit hit;
    Color generalColor;

    void Start()
    {
        generalColor = GetComponent<Renderer>().material.color;
    }

    public void SetValues()
    {
        if(isBomb)
        {
            isBombI.gameObject.SetActive(true);
            return;
        }

        bombsAroundText.text = bombsAround > 0 ? bombsAround.ToString() : "";
        if (bombsAround == 1)
            bombsAroundText.color = Color.blue;
        else if (bombsAround == 2)
            bombsAroundText.color = Color.green;
        else if (bombsAround >= 3)
            bombsAroundText.color = Color.red;
    }

    void OnMouseDown()
    {
        lastTapTime = Time.time;
    }

    void OnMouseUp()
    {
        if(Time.time - lastTapTime < holdTime)
        {
            Open();
        }
        else
        {
            SetFlag();
        }
    }

    public void Open()
    {
        if (!isOpened && !isFlagged && !isShowed && GameManager.instance.isPlaying)
        {
            if (!GameManager.instance.isStarted)
                GameManager.instance.isStarted = true;
            transform.GetChild(0).gameObject.SetActive(true);
            isOpened = true;
            SquareManager.instance.OpenSquares(x, y);

            if (isBomb)
            {
                // Повернуть на 90 и на -90
                transform.localRotation = Quaternion.Euler(-transform.localRotation.x, 0, 0);
                isBombI.gameObject.SetActive(true); // Активировать спрайт
                GetComponent<Renderer>().material.color = Color.red;
                Explode();
                Invoke("ExplodeAllBombs", 0.5f);
                GameManager.instance.GameOver();
                return;
            }
            else
            {
                GameManager.instance.OnOpen(); // Сказать GameManager-y, что открыта ячейка без бомбы.
                GetComponent<Animator>().enabled = true;
                GetComponent<Animator>().Play("SquareOpen"); // Анимация открытия
                GetComponent<Renderer>().material.color = Color.white;
            }
        }
    }

    void ExplodeAllBombs()
    {
        SquareManager.instance.ExplodeAllBombs(this);
    }

    public void Show(bool win)
    {
        isShowed = true;
        transform.GetChild(0).gameObject.SetActive(true); // Activate canvas
        isFlaggedI.gameObject.SetActive(false); // Disable flag
        isBombI.gameObject.SetActive(true); // Activate bomb
        if(win)
            GetComponent<Renderer>().material.color = Color.yellow;
        else
            GetComponent<Renderer>().material.color = Color.red;
    }

    public void Explode()
    {
        if (GetComponent<Renderer>().material.color != Color.red)
            GetComponent<Renderer>().material.color = Color.red;
        // Вызвать анимацию взрыва
        isBombI.GetComponent<AudioSource>().Play(); // Звук
        explosion.gameObject.SetActive(true);
        explosion.Play();
    }

    void SetFlag()
    {
        if (isOpened || !GameManager.instance.isPlaying)
            return;

        // Повернуть на 90 и на -90
        transform.localRotation = Quaternion.Euler(-transform.localRotation.x, 0, 0);

        transform.GetChild(0).gameObject.SetActive(!transform.GetChild(0).gameObject.activeSelf);
        transform.GetChild(0).GetChild(0).gameObject.SetActive(!transform.GetChild(0).GetChild(0).gameObject.activeSelf);
        isBombI.gameObject.SetActive(false);
        isFlaggedI.gameObject.SetActive(!isFlaggedI.gameObject.activeSelf); // Активировать спрайт

        Color color = GetComponent<Renderer>().material.color;
        color = color == generalColor ? new Color(1, 0.5f, 0) : generalColor;
        GetComponent<Renderer>().material.color = color;
        isFlagged = !isFlagged;

        // Обновить UI (бомбы)
        if (isFlagged)
            Bomber.instance.bombsAmount--;
        else
            Bomber.instance.bombsAmount++;
    }
}
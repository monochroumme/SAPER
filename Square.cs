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

    Ray ray;
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

    void Update()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            if (Input.GetMouseButtonDown(1))
            {
                if(hit.collider.gameObject == gameObject)
                {
                    SetFlag();
                }
            }
        }
    }

    void OnMouseDown()
    {
        Open();
    }

    public void Open()
    {
        if (!isOpened && !isFlagged && !isShowed && GameManager.isPlaying)
        {
            if (!GameManager.isStarted)
                GameManager.isStarted = true;
            transform.GetChild(0).gameObject.SetActive(true);
            //currentColor = Color.white;
            isOpened = true;
            SquareManager.instance.OpenSquares(x, y);

            if (isBomb)
            {
                print("Game over!");
                isBombI.gameObject.SetActive(true); // Активировать спрайт
                StartCoroutine("AnimOnClick", Color.red);
                Explode();
                Invoke("ExplodeAllBombs", 0.5f);
                GameManager.GameOver();
                return;
            }
            else
            {
                GameManager.OnOpen(); // Сказать GameManager-y, что открыта ячейка без бомбы.
                StartCoroutine("AnimOnClick", Color.white);
            }
        }
    }

    IEnumerator AnimOnClick(Color toColor)
    {
        GetComponent<Renderer>().material.color = Color.Lerp(GetComponent<Renderer>().material.color, toColor, 0.33f);
        yield return new WaitForSeconds(0.05f);
        if(GetComponent<Renderer>().material.color != toColor)
            StartCoroutine("AnimOnClick", toColor);
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
            StartCoroutine("AnimOnClick", Color.yellow); //GetComponent<Renderer>().material.color = Color.yellow;
        else
            StartCoroutine("AnimOnClick", Color.red); //GetComponent<Renderer>().material.color = Color.red;
    }

    public void Explode()
    {
        if (GetComponent<Renderer>().material.color != Color.red)
            StartCoroutine("AnimOnClick", Color.red); //GetComponent<Renderer>().material.color = Color.red;
        // Вызвать анимацию взрыва
        isBombI.GetComponent<AudioSource>().Play(); // Звук
        explosion.gameObject.SetActive(true);
        explosion.Play();
    }

    void SetFlag()
    {
        if (isOpened || !GameManager.isPlaying)
            return;

        transform.GetChild(0).gameObject.SetActive(!transform.GetChild(0).gameObject.activeSelf);
        transform.GetChild(0).GetChild(0).gameObject.SetActive(!transform.GetChild(0).GetChild(0).gameObject.activeSelf);
        isBombI.gameObject.SetActive(false);
        isFlaggedI.gameObject.SetActive(!isFlaggedI.gameObject.activeSelf); // Активировать спрайт

        Color color = GetComponent<Renderer>().material.color;
        color = color == generalColor ? new Color(1, 0.5f, 0) : generalColor;
        StartCoroutine("AnimOnClick", color); //GetComponent<Renderer>().material.color = GetComponent<Renderer>().material.color == generalColor ? new Color(1, 0.5f, 0) : generalColor;
        isFlagged = !isFlagged;

        // Обновить UI (бомбы)
        if (isFlagged)
            Bomber.instance.bombsAmount--;
        else
            Bomber.instance.bombsAmount++;
    }
}
using UnityEngine;
using UnityEngine.UI;

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
        generalColor = transform.GetComponent<Renderer>().material.color;
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
            transform.GetChild(0).gameObject.SetActive(true);
            gameObject.GetComponent<Renderer>().material.color = Color.white;
            isOpened = true;
            SquareManager.instance.OpenSquares(x, y);

            if (isBomb)
            {
                print("Game over!");
                isBombI.gameObject.SetActive(true); // Активировать спрайт
                Explode();
                Invoke("ExplodeAllBombs", 0.2f);
                GameManager.GameOver();
                return;
            }
            else
            {
                // Сказать GameManager-y, что открыта ячейка без бомбы.
                GameManager.OnOpen();
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
        transform.GetChild(0).gameObject.SetActive(true);
        isFlaggedI.gameObject.SetActive(false);
        isBombI.gameObject.SetActive(true);
        if(win)
            GetComponent<Renderer>().material.color = Color.yellow;
        else
            GetComponent<Renderer>().material.color = Color.red;
    }

    public void Explode()
    {
        if(gameObject.GetComponent<Renderer>().material.color != Color.red)
        gameObject.GetComponent<Renderer>().material.color = Color.red;
        // Вызвать анимацию взрыва
        isBombI.GetComponent<AudioSource>().Play(); // Звук
        explosion.gameObject.SetActive(true);
        explosion.Play();
        Destroy(explosion.gameObject, 2f);
    }

    void SetFlag()
    {
        if (isOpened || !GameManager.isPlaying)
            return;

        transform.GetChild(0).gameObject.SetActive(!transform.GetChild(0).gameObject.activeSelf);
        transform.GetChild(0).GetChild(0).gameObject.SetActive(!transform.GetChild(0).GetChild(0).gameObject.activeSelf);
        isBombI.gameObject.SetActive(false);
        isFlaggedI.gameObject.SetActive(!isFlaggedI.gameObject.activeSelf); // Активировать спрайт

        gameObject.GetComponent<Renderer>().material.color = transform.GetComponent<Renderer>().material.color == generalColor ? new Color(1, 0.5f, 0) : generalColor;
        isFlagged = !isFlagged;
    }
}
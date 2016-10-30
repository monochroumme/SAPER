using UnityEngine;
using UnityEngine.UI;

public class Square : MonoBehaviour
{
    [HideInInspector]
    public int x;
    [HideInInspector]
    public int y;

    public bool isOpened = false;
    public bool isBomb;
    public bool isFlagged = false;
    public int bombsAround;
    public Text ba;
    public Image isBombI;
    public Image isFlaggedI;

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

        ba.text = bombsAround > 0 ? bombsAround.ToString() : "";
        if (bombsAround == 1)
            ba.color = Color.blue;
        else if (bombsAround == 2)
            ba.color = Color.green;
        else if (bombsAround >= 3)
            ba.color = Color.red;
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
        if (!isOpened && !isFlagged)
        {
            transform.GetChild(0).gameObject.SetActive(true);
            gameObject.GetComponent<Renderer>().material.color = Color.white;
            isOpened = true;
            SquareManager.OpenSquares(x, y);

            if (isBomb)
            {
                print("Game over!");
                gameObject.GetComponent<Renderer>().material.color = Color.red;
                isBombI.gameObject.SetActive(true);
                return;
                //TODO ------------------------------
            }
        }
    }

    void SetFlag()
    {
        if (isOpened)
            return;

        transform.GetChild(0).gameObject.SetActive(!transform.GetChild(0).gameObject.activeSelf);
        transform.GetChild(0).GetChild(0).gameObject.SetActive(!transform.GetChild(0).GetChild(0).gameObject.activeSelf);
        isBombI.gameObject.SetActive(false);
        //isFlaggedI.gameObject.SetActive(!isFlaggedI.gameObject.activeSelf);

        gameObject.GetComponent<Renderer>().material.color = transform.GetComponent<Renderer>().material.color == generalColor ? new Color(1, 0.5f, 0) : generalColor;
        isFlagged = !isFlagged;
    }
}
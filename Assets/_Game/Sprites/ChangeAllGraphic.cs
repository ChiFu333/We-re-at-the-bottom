using System;
using UnityEngine;

public class ChangeAllGraphic : MonoBehaviour
{
    public GameObject door;
    public Sprite goodD, badD;
    public PlatformerPlayer p;
    public GameObject bagGrid;
    public GameObject goodGrid;

    public static ChangeAllGraphic inst;

    public void Awake()
    {
        inst = this;
    }

    public void ChangeGraphic(bool b)
    {
        p.ChangeToGood(b);
        bagGrid.SetActive(!b);
        goodGrid.SetActive(b);
        door.GetComponent<BoxCollider2D>().enabled = b;
        door.GetComponent<SpriteRenderer>().sprite = b ? goodD : badD;
    }
}

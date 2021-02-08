using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Drag : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public HeroInventory heroInventory;
    public Item item;
    public string ownerItem;
    public int countItem;

    public Image image;
    public Sprite defaultSprite;
    public Text count;

    public Text descriptionCell;

    public void OnPointerEnter(PointerEventData eventData)
    {

    }

    public void OnPointerExit(PointerEventData eventData)
    {

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Right)
        {
            heroInventory.RemoveItem(this);
        }
        else if (eventData.button == PointerEventData.InputButton.Left)
        {
            heroInventory.UseItem(this);
        }
    }

   
}

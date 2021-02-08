using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroInventory : MonoBehaviour
{
    HeroStats heroStats;

    public List<Item> item = new List<Item>();
    public List<Drag> drag;
    public GameObject inventory;

    public GameObject cell;
    public Transform cellParent;

    void Start()
    {
        heroStats = GetComponent<HeroStats>();
        InventoryDisable();
    }

    void Update()
    {
        InventoryActive();
    }

    public void InventoryActive()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (inventory.activeSelf)
            {
                InventoryDisable();
            }
            else
            {
                InvenrotyEnabled();
            }
        }
    }

    public void InventoryDisable()
    {
        foreach (Drag drag in drag)
            Destroy(drag.gameObject);
        drag.Clear();

        inventory.SetActive(false);      
    }

    public void InvenrotyEnabled()
    {
        inventory.SetActive(true);

        foreach (Drag drag in drag)
            Destroy(drag.gameObject);
        drag.Clear();

        for (int i = 0; i < item.Count; i++)
        {
            GameObject newCell = Instantiate(cell);
            newCell.transform.SetParent(cellParent, false);
            drag.Add(newCell.GetComponent<Drag>());
        }

            for (int i = 0; i < item.Count; i++)
        {
            Item it = item[i];
            for(int j = 0; j < drag.Count; j++)
            {
                if(drag[j].ownerItem != "")
                {
                    if (item[i].isStackable)
                    {
                        if(drag[j].item.nameItem == it.nameItem)
                        {
                            drag[j].countItem++;
                            drag[j].count.text = drag[j].countItem.ToString();
                            break;
                        }
                    }
                    else
                    {
                        continue;
                    }
                }
                else
                {
                    drag[j].item = it;
                    drag[j].image.sprite = Resources.Load<Sprite>(it.pathSprite);
                    drag[j].ownerItem = "myItem";
                    drag[j].countItem++;
                    drag[j].count.text = "" + drag[j].countItem;
                    drag[j].heroInventory = this;
                    break;
                }
            }
        }
        for(int i = drag.Count - 1; i > 0; i--)
        {
            if(drag[i].ownerItem == "")
            {
                Destroy(drag[i].gameObject);
                drag.RemoveAt(i);
            }
        }
    }

    public void RemoveItem(Drag drag)
    {
        Item it = drag.item;
        GameObject newObj = Instantiate<GameObject>(Resources.Load<GameObject>(it.pathPrefab));
        newObj.transform.position = transform.position + transform.forward + transform.up;
        item.Remove(it);
        InvenrotyEnabled();
    }   
    public void UseItem(Drag drag)
    {
        heroStats.AddHealth(drag.item.addHealth);
        item.Remove(drag.item);
        InvenrotyEnabled();
    }
}

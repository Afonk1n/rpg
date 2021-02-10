using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroInventory : MonoBehaviour
{
    HeroStats heroStats;

    public List<Item> food = new List<Item>();
    public List<Item> weapon = new List<Item>();

    public int typeOutput;


    public List<Drag> drag;
    public GameObject inventory;

    public GameObject cell;
    public Transform cellParent;

    public Transform rHand;

    void Start()
    {
        InventoryDisable();
        typeOutput = 1;
        heroStats = GetComponent<HeroStats>();
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

        if (typeOutput == 1)
        {
            for (int i = 0; i < food.Count; i++)
            {
                GameObject newCell = Instantiate(cell);
                newCell.transform.SetParent(cellParent, false); 
                drag.Add(newCell.GetComponent<Drag>());
            }
            for (int i = 0; i < food.Count; i++)
            {
                Item it = food[i];
                for (int j = 0; j < drag.Count; j++)
                {
                    if (drag[j].ownerItem != "")
                    {
                        if (food[i].isStackable)
                        {
                            if (drag[j].item.nameItem == it.nameItem)
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
        }
        else if (typeOutput == 2)
        {
            for (int i = 0; i < weapon.Count; i++)
            {
                GameObject newCell = Instantiate(cell);
                newCell.transform.SetParent(cellParent, false);
                drag.Add(newCell.GetComponent<Drag>());
            }
            for (int i = 0; i < weapon.Count; i++)
            {
                Item it = weapon[i];
                for (int j = 0; j < drag.Count; j++)
                {
                    if (drag[j].ownerItem != "")
                    {
                        if (weapon[i].isStackable)
                        {
                            if (drag[j].item.nameItem == it.nameItem)
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
        }


        
        for(int i = drag.Count - 1; i >= 0; i--)
        {
            if(drag[i].ownerItem == "")
            {
                Destroy(drag[i].gameObject);
                drag.RemoveAt(i);
            }
        }
    }

    public void OutputFood()
    {
        typeOutput = 1;
        InvenrotyEnabled();
    }
    public void OutputWeapon()
    {
        typeOutput = 2;
        InvenrotyEnabled();
    }


    public void RemoveItem(Drag drag)
    {
        Item it = drag.item;
        GameObject newObj = Instantiate<GameObject>(Resources.Load<GameObject>(it.pathPrefab));
        newObj.transform.position = transform.position + transform.forward + transform.up;
        food.Remove(it);
        weapon.Remove(it);
        InvenrotyEnabled();
    }   
    public void UseItem(Drag drag)
    {
        Item it = drag.item;
        if(it.typeItem == "Food")
        {
            heroStats.AddHealth(drag.item.addHealth);
            food.Remove(drag.item);
        }
        else if(it.typeItem == "Weapon")
        {
            GameObject newObj = Instantiate<GameObject>(Resources.Load<GameObject>(it.pathPrefab));
            newObj.transform.SetParent(rHand);
            newObj.transform.localPosition = it.position;
            newObj.transform.localRotation = Quaternion.Euler(it.rotation);
            newObj.GetComponent<Rigidbody>().isKinematic = true;
        }
        
        InvenrotyEnabled();
    }
}

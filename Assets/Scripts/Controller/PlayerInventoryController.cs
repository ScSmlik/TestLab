using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerInventoryController : MonoBehaviour
{
    //
    public InventoryObject inventory;
    public InventoryObject equipment;


    //

    private void Awake()
    {
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            inventory.Save();
            equipment.Save();
        }

        if(Input.GetKeyDown(KeyCode.O))
        {
            inventory.Load();
            equipment.Load();
        }

    }

    private void OnApplicationQuit() 
    {
        inventory.BackPack.Clear();
        equipment.BackPack.Clear();
    }



    public void PickUp(ItemObject item,int amount)
    {
        inventory.AddItem(new Item(item),amount);
    }

}

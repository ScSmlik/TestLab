using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEditor;

[CreateAssetMenu(fileName = "new Inventory", menuName = "Inventory System/Inventory/BackPack")]
public class InventoryObject : ScriptableObject
{
    //public string savePath;

    public Inventory BackPack;

    [System.NonSerialized]
    public ItemDatabaseObject Database;
    




    public bool AddItem(Item item, int amount)
    {
        if (EmptySlotCount <= 0)
            return false;
        InventorySlot slot = FindItemOnInventory(item);
        if(!Database.GetItem[item.Id].stackable || slot == null )
        {
            SetEmptySlot(item,amount);
            return true;
        }
        slot.AddAmount(amount);
        return true;
    }

    public InventorySlot FindItemOnInventory(Item item)
    {
        for(int i = 0; i < BackPack.Items.Length; i ++ )
        {
            if(BackPack.Items[i].item.Id == item.Id)
            {
                return BackPack.Items[i];
            }
        }
        return null;
    }

    public int EmptySlotCount
    {
        get
        {
            int counter = 0;
            for(int i = 0; i <BackPack.Items.Length; i ++ )
            {
                if (BackPack.Items[i].item.Id <= -1)
                    counter++;
            }
            return counter;
        }
    }

    public InventorySlot SetEmptySlot(Item item, int amount)
    {
        for (int i = 0; i < BackPack.Items.Length; i++)
        {
            if (BackPack.Items[i].item.Id <= -1)
            {
                BackPack.Items[i].UpdateSlot(item, amount);
                return BackPack.Items[i];
            }
        }
        return null;
    }//找到第一个空格，设置为相应的物品 


    //public void Save()
    //{
    //    string saveData = JsonUtility.ToJson(this, true);
    //    BinaryFormatter bf = new BinaryFormatter();
    //    FileStream file = File.Create(string.Concat(Application.persistentDataPath,savePath));
    //    bf.Serialize(file, saveData);
    //    file.Close();
    //}

    public void MoveItem(InventorySlot item1,InventorySlot item2)
    {
        if (item2.CanPlaceInSlot(item1.ItemObject) && item1.CanPlaceInSlot(item2.ItemObject))
        {
            InventorySlot t = new InventorySlot(item2.item, item2.amount);
            item2.UpdateSlot(item1.item, item1.amount);
            item1.UpdateSlot(t.item, t.amount);
        }

    }

    public void RemoveItem(Item item)
    {
        for(int i = 0; i < BackPack.Items.Length; i ++ )
        {
            if(BackPack.Items[i].item == item)
            {
                BackPack.Items[i].UpdateSlot(null,0);
            }
        }
    }


     [ContextMenu("Save")]
      public void Save()
    {
        string saveData = JsonUtility.ToJson(this.BackPack, true);
        PlayerPrefs.SetString("Inventory",saveData);
        PlayerPrefs.Save();
    }

    [ContextMenu("Load")]
    public void Load()
    {
        if (PlayerPrefs.HasKey("Inventory"))
        {
            JsonUtility.FromJsonOverwrite(PlayerPrefs.GetString("Inventory"), this.BackPack);
        }
    }

    [ContextMenu("Clear")]
    public void Clear()
    {
        BackPack.Clear();
    }
    //public void Load()
    //{
    //    if (File.Exists(string.Concat(Application.persistentDataPath, savePath)))
    //    {
    //        BinaryFormatter bf = new BinaryFormatter();
    //        FileStream file = File.Open(string.Concat(Application.persistentDataPath,savePath),FileMode.Open);
    //        JsonUtility.FromJsonOverwrite(bf.Deserialize(file).ToString(),this);
    //        file.Close(); 
    //    }
    //}



}

[System.Serializable]
public class Inventory
{
    public InventorySlot[] Items = new InventorySlot[24];
    public void Clear()
    {
        for(int i = 0; i < Items.Length; i ++ )
        {
            Items[i].RemoveItem();
        }
    }
}




[System.Serializable]
public class InventorySlot
{
    public ItemType[] AllowedItem = new ItemType[0];

    [System.NonSerialized]
    public UserInterface parent;
    public Item item;
    public int amount;

    public ItemObject ItemObject
    {
        get
        {
            if(item.Id >= 0)
            {
                return parent.inventory.Database.GetItem[item.Id];
            }
            return null;
        }
    }

    public InventorySlot()
    {
        item = new Item();
        amount = 0;
    }

    public InventorySlot(Item item,int amount)
    {
        this.item = item;
        this.amount = amount;
    }

    public void UpdateSlot(Item item, int amount)
    {
        this.item = item;
        this.amount = amount;
    }

    public void RemoveItem()
    {
        item = new Item();
        amount = 0;
    }


    public void AddAmount(int value)
    {
        amount += value;
    }

    public bool CanPlaceInSlot(ItemObject itemObject)
    {
        if (AllowedItem.Length <= 0 || itemObject  == null ||itemObject.data.Id < 0)
            return true;
        for(int i = 0; i < AllowedItem.Length; i ++ )
        {
            if (itemObject.type == AllowedItem[i])
                return true;
        }

        return false;
    }

}


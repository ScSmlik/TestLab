using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "new Food Object", menuName = "Inventory System/Items/Food")]
public class FoodObject : ItemObject
{
    //



    //
    private void Awake()
    {
        type = ItemType.Food;
    }



    //
}

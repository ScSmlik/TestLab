using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DynamicInterface : UserInterface
{
    //

    public GameObject inventoryPrefab;

    //每个物体之间的间隔
    public int X_SPACE_BETWEEN_ITEM;
    public int Y_SPACE_BETWEEN_ITEM;

    //第一个物体的起始位置
    public float X_Start = -339;
    public float Y_Start = 170;

    //每行显示多少物体  
    public int NUMBER_OF_COLUMN;

    //



    //
    public override void CreateSlots()
    {
        SlotsOnInterface = new Dictionary<GameObject, InventorySlot>();

        for (int i = 0; i < inventory.BackPack.Items.Length; i++)
        {
            var obj = Instantiate(inventoryPrefab, Vector3.zero, Quaternion.identity, transform);
            obj.GetComponent<RectTransform>().localPosition = GetPosition(i);

            AddEvent(obj, EventTriggerType.PointerEnter, delegate { OnEnter(obj); });
            AddEvent(obj, EventTriggerType.PointerExit, delegate { OnExit(obj); });
            AddEvent(obj, EventTriggerType.BeginDrag, delegate { OnDragStart(obj); });
            AddEvent(obj, EventTriggerType.EndDrag, delegate { OnDragEnd(obj); });
            AddEvent(obj, EventTriggerType.Drag, delegate { OnDrag(obj); });

            SlotsOnInterface.Add(obj, inventory.BackPack.Items[i]);

        }

    }



    private Vector3 GetPosition(int index)
    {
        Vector3 pos = new Vector3(X_Start + X_SPACE_BETWEEN_ITEM * (index % NUMBER_OF_COLUMN), Y_Start - Y_SPACE_BETWEEN_ITEM * (index / NUMBER_OF_COLUMN), 0);
        return pos;
    }

}

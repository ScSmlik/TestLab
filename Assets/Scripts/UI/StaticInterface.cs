using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class StaticInterface : UserInterface
{
    //0->ÓÒÊÖÎäÆ÷ 1->Í·¿ø 2->¿ø¼× 3->×óÊÖÎäÆ÷ 4-> Ð¬×Ó

    //
    public GameObject[] slots;

    //



    //
    public override void CreateSlots()
    {
        SlotsOnInterface = new Dictionary<GameObject, InventorySlot>();
        for(int i = 0; i < inventory.BackPack.Items.Length; i ++ )
        {
            var obj = slots[i];


            AddEvent(obj, EventTriggerType.PointerEnter, delegate { OnEnter(obj); });
            AddEvent(obj, EventTriggerType.PointerExit, delegate { OnExit(obj); });
            AddEvent(obj, EventTriggerType.BeginDrag, delegate { OnDragStart(obj); });
            AddEvent(obj, EventTriggerType.EndDrag, delegate { OnDragEnd(obj); });
            AddEvent(obj, EventTriggerType.Drag, delegate { OnDrag(obj); });

            SlotsOnInterface.Add(obj,inventory.BackPack.Items[i]);
             

        }


    }

}

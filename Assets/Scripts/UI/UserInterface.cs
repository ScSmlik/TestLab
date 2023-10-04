using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public abstract class UserInterface : MonoBehaviour
{
    //

    
    public InventoryObject inventory;



    public Dictionary<GameObject, InventorySlot> SlotsOnInterface = new Dictionary<GameObject, InventorySlot>();


    //
    private void Start()
    {
        for(int i = 0; i < inventory.BackPack.Items.Length; i ++ )
        {
            inventory.BackPack.Items[i].parent = this;
        }

        CreateSlots();
        AddEvent(gameObject, EventTriggerType.PointerEnter, delegate { OnEnterInterface(gameObject); });
        AddEvent(gameObject, EventTriggerType.PointerExit, delegate { OnEixtInterface(gameObject); });
    }

    private void Update()
    {
        SlotsOnInterface.UpdateSlotDisplay();
    }

    //



    public abstract void CreateSlots();

    protected void AddEvent(GameObject obj, EventTriggerType type, UnityAction<BaseEventData> action)
    {
        EventTrigger trigger = obj.GetComponent<EventTrigger>();
        var eventTrigger = new EventTrigger.Entry();
        eventTrigger.eventID = type;
        eventTrigger.callback.AddListener(action);
        trigger.triggers.Add(eventTrigger);
    }

    public void OnEnter(GameObject obj)
    {
        MouseData.HoveringSlot = obj;
    }

    public void OnEnterInterface(GameObject obj)
    {
        MouseData.ui = obj.GetComponent<UserInterface>();
    }

    public void OnEixtInterface(GameObject obj)
    {
        MouseData.ui = null;
    }

    public void OnExit(GameObject obj)
    {
        MouseData.HoveringSlot = null;

    }

    public void OnDragStart(GameObject obj)
    {


        MouseData.DraggingItem = CreateTempItem(obj);

    }

    public GameObject CreateTempItem(GameObject obj)
    {
        GameObject t = null;
        if(SlotsOnInterface[obj].item.Id >= 0)
        {
            t = new GameObject();
            var rt = t.AddComponent<RectTransform>();
            rt.sizeDelta = new Vector2(50, 50);
            t.transform.SetParent(transform.parent);
            var img = t.AddComponent<Image>();
            img.sprite = SlotsOnInterface[obj].ItemObject.uiDisplay;
            img.raycastTarget = false;
        }

        return t;

    }

    public void OnDragEnd(GameObject obj)
    {
        Destroy(MouseData.DraggingItem);
        if(MouseData.ui == null)
        {
            SlotsOnInterface[obj].RemoveItem();
            return;
        }
        if(MouseData.HoveringSlot)
        {
            InventorySlot mouseHoverSlotData = MouseData.ui.SlotsOnInterface[MouseData.HoveringSlot];
            inventory.MoveItem(SlotsOnInterface[obj],mouseHoverSlotData);
        }

    }

    public void OnDrag(GameObject obj)
    {
        if (MouseData.DraggingItem!= null)
            MouseData.DraggingItem.GetComponent<RectTransform>().position = Input.mousePosition;
    }





}
public static class MouseData
{
    public static UserInterface ui;
    public static GameObject DraggingItem;
    public static GameObject HoveringSlot;

}

public static class ExtensionMethods
{
    public static void UpdateSlotDisplay(this Dictionary<GameObject, InventorySlot> _slotsOnInterface)
    {
        foreach (KeyValuePair<GameObject, InventorySlot> slot in _slotsOnInterface)
        {
            if (slot.Value.item.Id >= 0)
            {
                slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().sprite = slot.Value.ItemObject.uiDisplay;//inventory.Database.GetItem[slot.Value.item.Id].uiDisplay;
                slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 1);
                slot.Key.GetComponentInChildren<TextMeshProUGUI>().text = slot.Value.amount == 1 ? " " : slot.Value.amount.ToString("n0");

            }
            else
            {
                slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().sprite = null;
                slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 0);
                slot.Key.GetComponentInChildren<TextMeshProUGUI>().text = "";

            }
        }
    }
}
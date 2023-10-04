using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class ItemController : MonoBehaviour,ISerializationCallbackReceiver
{
    //
    public ItemObject item;
    
    public void OnAfterDeserialize()
    {
        
    }

    public void OnBeforeSerialize()
    {
        
        GetComponentInChildren<SpriteRenderer>().sprite = item.uiDisplay;
        EditorUtility.SetDirty(GetComponentInChildren<SpriteRenderer>());
    }

    //


    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name == "Player")
        {
            other.gameObject.GetComponent<PlayerInventoryController>().PickUp(item, 1);
            Destroy(this.gameObject);
        }

    }

    //


}

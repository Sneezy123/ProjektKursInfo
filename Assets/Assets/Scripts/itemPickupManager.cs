using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class itemPickupManager : MonoBehaviour
{
    // Start is called before the first frame update
    public bool isHolding = false;
    private Transform currentItem;

    public void PickupItem(Transform item)
    {
        Rigidbody itemRB = item.GetComponent<Rigidbody>();
        Collider itemCollider = item.GetComponent<Collider>();

        item.SetParent(this.transform);
        itemRB.isKinematic = true;
        itemCollider.enabled = false;
        item.localPosition = Vector3.zero;
        item.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));

        isHolding = true;
        currentItem = item;
    }

    public void DropItem(Transform item)
    {
        Rigidbody itemRB = currentItem.GetComponent<Rigidbody>();
        Collider itemCollider = currentItem.GetComponent<Collider>();
        Debug.Log(currentItem);
        currentItem.parent = null;

        itemRB.isKinematic = false;
        itemCollider.enabled = true;
        
        // item.transform.localPosition = Vector3.zero;
        // item.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
        isHolding = false;
    }
}

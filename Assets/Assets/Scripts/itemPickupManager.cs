using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IItem
{
    public void OnCollisionEnter(Collision collision);
}


public class itemPickupManager : MonoBehaviour
{
    // Start is called before the first frame update
    public bool isHolding = false;
    public float throwFactor = 100f;
    private Transform currentItem;
    public Camera mainCam;

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

    public void DropItem()
    {
        Rigidbody itemRB = currentItem.GetComponent<Rigidbody>();
        Collider itemCollider = currentItem.GetComponent<Collider>();
        Debug.Log(currentItem);
        currentItem.parent = null;

        itemRB.isKinematic = false;
        itemRB.AddForce((mainCam.transform.forward * 5f + mainCam.transform.position - currentItem.position).normalized * throwFactor);
        itemCollider.enabled = true;
        
        // item.transform.localPosition = Vector3.zero;
        // item.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
        isHolding = false;
    }
}

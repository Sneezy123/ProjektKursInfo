using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class itemPickupManager : MonoBehaviour
{
    // Start is called before the first frame update
    public bool isHolding = false;

    public Rigidbody itemRB;
    public Collider itemCollider;
    void Start()
    {
    }

    public void PickupItem(GameObject item)
    {
        itemRB = item.GetComponent<Rigidbody>();
        itemCollider = item.GetComponent<Collider>();

        item.transform.SetParent(this.transform);
        itemRB.isKinematic = true;
        itemCollider.enabled = false;
        item.transform.localPosition = Vector3.zero;
        item.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));

        isHolding = true;
    }

    public void DropItem(GameObject item)
    {
        itemRB = item.GetComponent<Rigidbody>();
        itemCollider = item.GetComponent<Collider>();

        item.transform.parent.DetachChildren();
        itemRB.isKinematic = false;
        itemCollider.enabled = true;
        // item.transform.localPosition = Vector3.zero;
        // item.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));

        isHolding = false;
    }
}

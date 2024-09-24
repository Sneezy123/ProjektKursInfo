using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class itemPickupManager : MonoBehaviour
{
    // Start is called before the first frame update
    public bool isHolding = false;


    void Start()
    {
    }

    public void PickupItem(GameObject item)
    {
        item.transform.SetParent(this.transform);
        item.GetComponent<Rigidbody>().isKinematic = true;
        item.GetComponent<Collider>().enabled = false;
        item.transform.localPosition = Vector3.zero;
        item.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));

        isHolding = true;
    }

    public void DropItem(GameObject item)
    {
        item.transform.parent.DetachChildren();
        item.GetComponent<Rigidbody>().isKinematic = false;
        item.GetComponent<Collider>().enabled = true;
        // item.transform.localPosition = Vector3.zero;
        // item.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));

        isHolding = false;
    }
}

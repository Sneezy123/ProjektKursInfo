using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Crosshair : MonoBehaviour
{
    public PlayerMovementAdvanced playerscript;
    public scr_ItemBlueprint item;
    Image image;
    Color imgColor;
    public Color interactingColor;

    void Start()
    {
        image = GetComponent<Image>();
        imgColor = image.color;
    }

    void Update()
    {
        if (playerscript.hit.transform != null && playerscript.hit.transform.gameObject.TryGetComponent(out IInteractable interactObj) && playerscript.hit.distance <= item.InteractRange)
        {
            image.color = interactingColor;
        }
        else
        {
            image.color = imgColor;
        }

    }
}

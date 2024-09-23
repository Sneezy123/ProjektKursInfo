using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Crosshair : MonoBehaviour
{
    private PlayerMovementAdvanced playerScript;
    public scr_ItemBlueprint item;
    Image image;
    Color imgColor;
    public Color interactingColor;

    void Start()
    {
        image = GetComponent<Image>();
        imgColor = image.color;

        playerScript = PlayerMovementAdvanced.playerScript;
    }

    void Update()
    {
        if (playerScript.hit.transform != null && playerScript.hit.transform.gameObject.TryGetComponent(out IInteractable interactObj) && playerScript.hit.distance <= item.InteractRange)
        {
            image.color = interactingColor;
        }
        else
        {
            image.color = imgColor;
        }

    }
}

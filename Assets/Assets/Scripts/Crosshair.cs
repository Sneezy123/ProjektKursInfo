using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Crosshair : MonoBehaviour
{
    public scr_PlayerMovement playerScript;
    public scr_ItemBlueprint itemBlueprintScript;
    Image image;
    Color imgColor;
    public Color interactingColor;
    private Outline interactingOutline;
    private GameObject interactedObj;

    void Start()
    {
        image = GetComponent<Image>();
        imgColor = image.color;
    }

    void Update()
    {
        if (playerScript.hit.transform != null && playerScript.hit.transform.gameObject.TryGetComponent(out IInteractable interactObj) && playerScript.hit.distance <= itemBlueprintScript.InteractRange)
        {
            image.color = interactingColor;
            scr_ItemBlueprint.canInteract = true;
            interactedObj = playerScript.hit.transform.gameObject;

            // Outline Object
            if (!interactedObj.GetComponent<Outline>())
            {
                interactingOutline = interactedObj.AddComponent<Outline>();
                interactingOutline.OutlineMode = Outline.Mode.OutlineVisible;
                interactingOutline.OutlineColor = Color.white;
                interactingOutline.OutlineWidth = 5f;
            }
        }
        else
        {
            image.color = imgColor;
            scr_ItemBlueprint.canInteract = false;

            // Remove Outline
            if (interactedObj.GetComponent<Outline>())
            {
                Destroy(interactedObj.GetComponent<Outline>());
            }
        }

    }
}

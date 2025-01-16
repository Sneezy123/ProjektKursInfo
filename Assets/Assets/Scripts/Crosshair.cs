using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using cakeslice;
using UnityEngine.UI;

public class Crosshair : MonoBehaviour
{
    public scr_PlayerMovement playerScript;
    public scr_ItemBlueprint itemBlueprintScript;
    Image image;
    Color imgColor;
    public Color interactingColor;
    private cakeslice.Outline interactingOutline;
    private GameObject interactedObj;

    void Start()
    {
        image = GetComponent<Image>();
        imgColor = image.color;
    }

    void Update()
    {
        if (playerScript.hit.transform != null && playerScript.hit.transform.gameObject.TryGetComponent(out IPickupable pickupObj) && playerScript.hit.distance <= itemBlueprintScript.InteractRange)
        {
            image.color = interactingColor;
            scr_ItemBlueprint.canInteract = true;
            interactedObj = playerScript.hit.transform.gameObject;

            // Outline Object
            if (!interactedObj.GetComponent<cakeslice.Outline>())
            {
                interactingOutline = interactedObj.AddComponent<cakeslice.Outline>();
                interactingOutline.color = 1; // 1 is green
            }
        }
        else if (playerScript.hit.transform != null && playerScript.hit.transform.gameObject.TryGetComponent(out IInteractable interactObj) && playerScript.hit.distance <= itemBlueprintScript.InteractRange)
        {
            image.color = interactingColor;
            scr_ItemBlueprint.canInteract = true;
            interactedObj = playerScript.hit.transform.gameObject;

            // Outline Object
            if (!interactedObj.GetComponent<cakeslice.Outline>())
            {
                interactingOutline = interactedObj.AddComponent<cakeslice.Outline>();
                interactingOutline.color = 2; // 2 is white
            }
        }
        else if (playerScript.hit.transform != null && playerScript.hit.transform.gameObject.TryGetComponent(out IUsable useObj) && playerScript.hit.distance <= itemBlueprintScript.InteractRange)
        {
            image.color = interactingColor;
            scr_ItemBlueprint.canInteract = true;
            interactedObj = playerScript.hit.transform.gameObject;

            // Outline Object
            if (!interactedObj.GetComponent<cakeslice.Outline>())
            {
                interactingOutline = interactedObj.AddComponent<cakeslice.Outline>();
                interactingOutline.color = 0; // 0 is red
            }
        }
        else
        {
            image.color = imgColor;
            scr_ItemBlueprint.canInteract = false;

            // Remove Outline
            if (interactedObj != null && interactedObj.GetComponent<cakeslice.Outline>())
            {
                Destroy(interactedObj.GetComponent<cakeslice.Outline>());
            }
        }

    }
}

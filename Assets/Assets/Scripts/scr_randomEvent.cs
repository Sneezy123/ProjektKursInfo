using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class randomEvent : MonoBehaviour
{
    [Header("Random Settings")]
    public float delay = 30.0f;
    public float threshold = 10.0f;

    [Header("Audio")]
    public AudioSource voices;
    public AudioSource steps;
    
    public List<string> possibleEvents; // List of events to choose from
    public bool isEventActive = false; // Prevent multiple events from overlapping

    [Header("Script Reference")]
    public scr_DamageAndHealthSystem DmgHealthSystem;
    public scr_PlayerMovement PlayerController;

    void Update()
    {
        if (!isEventActive)
        {
            StartCoroutine(newRandomEvent(delay, threshold));
        }
    }

    public IEnumerator newRandomEvent(float delay, float threshold)
    {
        isEventActive = true;

        // Wait for the delay time
        yield return new WaitForSeconds(delay);

        // Generate a random value
        float randomValue = Random.value;

        if (randomValue <= threshold)
        {
            // Trigger a random event
            if (possibleEvents.Count > 0)
            {
                string selectedEvent = possibleEvents[Random.Range(0, possibleEvents.Count)];
                Debug.Log("Triggered Event: " + selectedEvent);
                ExecuteEvent(selectedEvent);
            }
        }
        else
        {
            Debug.Log("No event triggered. Random value: " + randomValue);
        }

        isEventActive = false;
    }

    private void ExecuteEvent(string eventName)
    {
        switch (eventName)
        {
            case "voices":
                Debug.Log("Event 1");
                voices.Play();
                break;

            case "steps":
                Debug.Log("Event 2");
                steps.Play();
                break;

            case "blur":
                Debug.Log("Event 3");
                // Add logic here
                break;

            case "chromatic":
                Debug.Log("Event 4");
                // Add logic here
                break;

            case "stamina drain":
                Debug.Log("Event 5");
                PlayerController.drainStaminaForScare(20);
                break;

            default:
                Debug.Log("Unknown event: " + eventName);
                break;
        }
    }
}

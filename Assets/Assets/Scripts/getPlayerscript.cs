using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class getPlayerscript : MonoBehaviour
{
    public static PlayerMovementAdvanced playerScript;
    void Start()
    {
        playerScript = GetComponent<PlayerMovementAdvanced>();
    }
}

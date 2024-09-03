using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

// Script not in use!!!!
// Test

public class scr_KeyBinds : MonoBehaviour
{
    [Header("Movement")]
	public KeyCode walkForward = KeyCode.W;
	public KeyCode walkLeft = KeyCode.A;
	public KeyCode walkBack = KeyCode.S;
	public KeyCode walkRight = KeyCode.D;

    public KeyCode sprintKey = KeyCode.LeftShift;
    public KeyCode crouchKey = KeyCode.LeftControl;
	
    [Header("Interactions")]
	public KeyCode interact = KeyCode.E;
}
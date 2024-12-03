using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

// Script not in use!!!!
// Test

public class Keybinds : MonoBehaviour
{
    [Header("Movement")]
        public static KeyCode walkForward = KeyCode.W;
        public static KeyCode walkLeft = KeyCode.A;
        public static KeyCode walkBack = KeyCode.S;
        public static KeyCode walkRight = KeyCode.D;

        public static KeyCode sprintKey = KeyCode.LeftShift;
        public static KeyCode crouchKey = KeyCode.LeftControl;
	
    [Header("Interactions")]
		public static KeyCode interact = KeyCode.E;
		public static KeyCode drop = KeyCode.Q;
		public static KeyCode flashLightToggle = KeyCode.F;

    [Header("User Interface")]
		public static KeyCode pauseMenu = KeyCode.Escape;
		public static KeyCode utilMenu = KeyCode.Tab;

}
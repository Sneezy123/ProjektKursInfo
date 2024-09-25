using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;

public class KeypadManager : MonoBehaviour
{
    private List<int> currentCombination = new List<int>{};
    public List<int> requiredCombination = new List<int>{1, 2, 3, 4};

    public keypaddoorMechanics keypaddoorMechanics;


    public void input(int number)
    {

        if (number == 10)
        {
            if (currentCombination.SequenceEqual(requiredCombination))
            {
                keypaddoorMechanics.isLocked = false;
                Debug.Log("Door unlocked");
                currentCombination.Clear();
            }
            else
            {
                Debug.Log("Wrong combination");
                currentCombination.Clear();
            }

        }
        else if (number == 11)
        {
            if (currentCombination.Count >= 1)
            {
                currentCombination.RemoveAt(currentCombination.Count - 1);
                Debug.Log(string.Join(",", currentCombination) + "\n");
            }
        }
        else
        {
            if (currentCombination.Count < 4)
            {
                currentCombination.Add(number);
                Debug.Log(string.Join(",", currentCombination) + "\n");
            }
        }
    }
}

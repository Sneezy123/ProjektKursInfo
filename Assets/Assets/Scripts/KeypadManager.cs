using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;

public class KeypadManager : MonoBehaviour
{
    private List<int> currentCombination = new List<int> { };
    public List<int> requiredCombination = new List<int> { 1, 2, 3, 4 };
    public List<int> stayCombination = new List<int> { };

    public AudioSource beep;


    public keypaddoorMechanics keypaddoorMechanics;
    public Material[] numMat;


    public void input(int number)
    {
        beep.Play();

        if (number == 10)
        {
            if (currentCombination.SequenceEqual(requiredCombination))
            {
                keypaddoorMechanics.isLocked = false;
                Debug.Log("Door unlocked");
                colorNumbers(1);
                currentCombination.Clear();
            }
            else
            {
                Debug.Log("Wrong combination");
                colorNumbers(0);
                currentCombination.Clear();
            }

        }
        else if (number == 11)
        {
            if (currentCombination.Count >= 1)
            {
                currentCombination.RemoveAt(currentCombination.Count - 1);
                stayCombination = currentCombination;
                Debug.Log(string.Join(",", currentCombination) + "\n");

            }
        }
        else
        {
            if (currentCombination.Count < 4)
            {
                currentCombination.Add(number);
                stayCombination = currentCombination;
                Debug.Log(string.Join(",", currentCombination) + "\n");
            }
        }
    }

    public void colorNumbers(int wrongRightNone)
    {
        for (int num = 0; num < 10; num++)
        {
            if (wrongRightNone == -1)
            {
                if (stayCombination.Contains(num))
                {
                    if (stayCombination.Count <= 4)
                    {
                        numMat[num].SetColor("_EmissionColor", Color.yellow);
                        numMat[num].EnableKeyword("_EMISSION");
                        Debug.Log("Material " + numMat + " set on number " + num + " to color " + numMat[num].GetColor("_Color"));
                    }
                }
                else
                {
                    numMat[num].SetColor("_Color", Color.white);
                    numMat[num].DisableKeyword("_EMISSION");
                }
            }
            else if (wrongRightNone == 0)
            {
                numMat[num].SetColor("_EmissionColor", Color.red);
                numMat[num].EnableKeyword("_EMISSION");
            }
            else if (wrongRightNone == 1)
            {
                numMat[num].SetColor("_EmissionColor", Color.green);
                numMat[num].EnableKeyword("_EMISSION");
            }
        }
    }
}

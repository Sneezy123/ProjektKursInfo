using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeypadManager : MonoBehaviour
{
    public int firstnumber;
    public int secondnumber;
    public int thirdnumber;
    public int fourthnumber;
    private int numbercounter = 0;
    public int number;
    public keypaddoorMechanics keypaddoorMechanics;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void input(int number)
    {
        
        if(number == 10)
        {
            if (firstnumber == 1 && secondnumber == 2 && thirdnumber == 3 && fourthnumber == 4)
            {
                keypaddoorMechanics.isLocked = false;
            }else{
                Debug.Log("Wrong combination");
            }
            
        }else{
            if (numbercounter == 0)
            {
                numbercounter++;
                firstnumber = number;
                Debug.Log("input:" + firstnumber);
            }else if (numbercounter == 1)
            {
                numbercounter++;
                secondnumber = number;
                Debug.Log("input:" + firstnumber + secondnumber);
            }else if (numbercounter == 2)
            {
                numbercounter++;
                thirdnumber = number;
                Debug.Log("input:" + firstnumber + secondnumber + thirdnumber);
            }else if (numbercounter == 3)
            {
                numbercounter++;
                fourthnumber = number;
                Debug.Log("input:" + firstnumber + secondnumber + thirdnumber + fourthnumber);
            }else
            {
                Debug.Log("numpad is full, now its resetted");
                firstnumber = 0;
                secondnumber = 0;
                thirdnumber = 0;
                fourthnumber = 0;
                numbercounter = 0;
            }
        }
    }
}

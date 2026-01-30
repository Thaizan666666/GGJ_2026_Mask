using UnityEngine;
using System.Collections;
using System;

public class S_Topping : MonoBehaviour
{
    public bool Katsuobushi;
    public bool Seaweed;
    public bool Mayonnaise;

    public void ResetTopping()
    {
        Katsuobushi = false;
        Seaweed = false;
        Mayonnaise = false;
    }

    public void SetTopping(E_Topping _Topping) { 
        if (_Topping == E_Topping.Katsuobushi)
        {
            Katsuobushi = true;
        }
        else if (_Topping == E_Topping.Seaweed)
        {
            Seaweed = true;
        }
        else if (_Topping == E_Topping.Mayonnaise)
        {
            Mayonnaise = true;
        }
    }

    public void SetKatsuobushi()
    {
        Katsuobushi = true;
    }

    public void SetSeaweed()
    {
        Seaweed = true;
    }

    public void SetMayonnaise()
    {
        Mayonnaise = true;
    }

}

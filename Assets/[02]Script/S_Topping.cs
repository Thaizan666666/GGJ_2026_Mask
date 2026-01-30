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
            SetKatsuobushi();
        }
        else if (_Topping == E_Topping.Seaweed)
        {
            SetSeaweed();
        }
        else if (_Topping == E_Topping.Mayonnaise)
        {
            SetMayonnaise();
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

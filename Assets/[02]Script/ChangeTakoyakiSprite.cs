using UnityEngine;
using UnityEngine.UI;

public class ChangeTakoyakiSprite : MonoBehaviour
{
    public S_Topping takoyakiToppings;
    public Sprite BowlF;   // only Katsuobushi
    public Sprite BowlS;   // only Seaweed
    public Sprite BowlSa;  // only Mayonnaise
    public Sprite BowlSF;  // Katsuobushi + Seaweed
    public Sprite BowlSaF; // Katsuobushi + Mayonnaise (fixed name)
    public Sprite BowlSS;  // Seaweed + Mayonnaise
    public Sprite BowlSSF; // Katsuobushi + Seaweed + Mayonnaise
    public Sprite Default;
    public Image Target_Image;

    private void Awake()
    {
        ChangeSpriteTakoyaki();
    }

    public void ChangeSpriteTakoyaki()
    {
        if (takoyakiToppings.Katsuobushi && takoyakiToppings.Seaweed && takoyakiToppings.Mayonnaise)
        {
            Target_Image.sprite = BowlSSF;
        }
        else if (takoyakiToppings.Katsuobushi && takoyakiToppings.Seaweed)
        {
            Target_Image.sprite = BowlSF;
        }
        else if (takoyakiToppings.Katsuobushi && takoyakiToppings.Mayonnaise)
        {
            Target_Image.sprite = BowlSaF; // Fixed to use correct variable
        }
        else if (takoyakiToppings.Seaweed && takoyakiToppings.Mayonnaise)
        {
            Target_Image.sprite = BowlSS;
        }
        else if (takoyakiToppings.Katsuobushi)
        {
            Target_Image.sprite = BowlF;
        }
        else if (takoyakiToppings.Seaweed)
        {
            Target_Image.sprite = BowlS;
        }
        else if (takoyakiToppings.Mayonnaise)
        {
            Target_Image.sprite = BowlSa;
        }
        else
        {
            Target_Image.sprite = Default; // Fixed to use Default variable instead of keyword
        }
    }
}
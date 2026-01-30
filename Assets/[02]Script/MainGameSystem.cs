using UnityEngine;

public class MainGameSystem : MonoBehaviour
{
    [HideInInspector]
    public S_Topping target_Topping;
    public S_Topping Current_Topping;
    [Space(5)]

    //Customer GameObject Parts

    //SO of Mask normal
    //SO of Mask Anamoly

    //SO of Body normal
    //SO of Body anamoly

    //SO of Scarf anamoly
    //SO of Fox ears for anamoly
    //SO of tail for anamoly
    //SO of flame for anamoly


    private bool ForceTwoType = true;
    [SerializeField]
    private E_CharacterType CustomerType;
    [SerializeField]
    private int CustomerCount = 0;
    private int DuplicateCount = 0;

    public void RandomTargetTopping() {
        Current_Topping.ResetTopping();
        int rand = Random.Range(1, 8);
        switch (rand)
        {
            case 1:
                target_Topping.Katsuobushi = true;
                break;
            case 2:
                target_Topping.Seaweed = true;
                break;
            case 3:
                target_Topping.Mayonnaise = true;
                break;
            case 4:
                target_Topping.Katsuobushi = true;
                target_Topping.Seaweed = true;
                break;
            case 5:
                target_Topping.Katsuobushi = true;
                target_Topping.Mayonnaise = true;
                break;
            case 6:
                target_Topping.Seaweed = true;
                target_Topping.Mayonnaise = true;
                break;
            case 7:
                target_Topping.Katsuobushi = true;
                target_Topping.Seaweed = true;
                target_Topping.Mayonnaise = true;
                break;
        }
    }

    public string GetOrder() {
        string order = "Order: ";

        if (CustomerType == E_CharacterType.Normal)
        {
            if (target_Topping.Katsuobushi)
            {
                order += "Katsuobushi ";
            }
            if (target_Topping.Seaweed)
            {
                order += "Seaweed ";
            }
            if (target_Topping.Mayonnaise)
            {
                order += "Mayonnaise ";
            }
            return order;
        }

        if (CustomerType == E_CharacterType.Anamoly)
        {
            order += "Anamoly Order ";
            if (target_Topping.Katsuobushi)
            {
                order += "Katsuobushi ";
            }
            if (target_Topping.Seaweed)
            {
                order += "Seaweed ";
            }
            if (target_Topping.Mayonnaise)
            {
                order += "Mayonnaise ";
            }

            order += "(Anamoly Special! Line)";

            return order;
        }

        return order;
    }

    public void RandomCustomer()
    {
        //for first customer should be normal
        if (CustomerCount == 0)
        {
            CustomerType = E_CharacterType.Normal;
            CustomerCount++;
            DuplicateCount = 0;
            return;
        }

        //random customer type
        E_CharacterType previousType = CustomerType;

        // If duplicate count exceeds 2, force a different customer type
        if (DuplicateCount >= 2)
        {
            E_CharacterType[] allTypes;

            if (!ForceTwoType)
                allTypes = (E_CharacterType[])System.Enum.GetValues(typeof(E_CharacterType));
            else
                allTypes = new E_CharacterType[] { E_CharacterType.Normal, E_CharacterType.Anamoly };

            // Create a list excluding the current type
            System.Collections.Generic.List<E_CharacterType> availableTypes = new System.Collections.Generic.List<E_CharacterType>();
            foreach (E_CharacterType type in allTypes)
            {
                if (type != CustomerType)
                {
                    availableTypes.Add(type);
                }
            }

            // Select random from available types
            int rand = Random.Range(0, availableTypes.Count);
            CustomerType = availableTypes[rand];
            DuplicateCount = 0;
        }
        else
        {
            // Random selection from all types
            E_CharacterType[] allTypes;

            if (!ForceTwoType)
                allTypes = (E_CharacterType[])System.Enum.GetValues(typeof(E_CharacterType));
            else
                allTypes = new E_CharacterType[] { E_CharacterType.Normal, E_CharacterType.Anamoly };

            int rand = Random.Range(0, allTypes.Length);
            CustomerType = allTypes[rand];

            //check if new customer type same as previous
            if (CustomerType == previousType)
            {
                //increase duplicate count
                DuplicateCount++;
            }
            else
            {
                //reset duplicate count when type changes
                DuplicateCount = 0;
            }
        }

        CustomerCount++;
    }

    //method set apppearance according to customer type
}

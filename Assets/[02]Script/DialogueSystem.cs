using System.Collections.Generic;
using UnityEngine;

public class DialogueSystem : MonoBehaviour
{
    [Header("Dialogue")]
    public string Greeting;

    [Header("SO Properties")]
    public SO_Greeting dialogueGreeting;

    public SO_Order_Normal dialogueOrderNormal;
    public SO_Order_Anomaly dialogueOrderAnomaly;

    public SO_Order dialogueOrder;

    public SO_Serve_Right dialogueServeRight;
    public SO_Serve_Wrong dialogueServeWrong;

    public SO_Getout dialogueGetout;

    public string GetRandomGreeting()
    {
        return dialogueGreeting.dialogue.RandomValue();
    }

    public string GetRandomOrder(E_CharacterType charType)
    {
        switch (charType) { 
            case E_CharacterType.Normal:
                return dialogueOrderNormal.orders.RandomValue();
            case E_CharacterType.Anamoly:
                return dialogueOrderAnomaly.orders.RandomValue();
            default:
                Debug.LogWarning("GetRandomOrder: Unknown character type.");
                return string.Empty;
        }
    }

    public string GetRandomServeDialogue(bool isRight)
    {
        if (isRight)
        {
            return dialogueServeRight.dialogue.RandomValue();
        }
        else
        {
            return dialogueServeWrong.dialogue.RandomValue();
        }
    }

    public string GetRandomGetoutDialogue()
    {
        return dialogueGetout.dialogue.RandomValue();
    }

    public string GetRandomOrderDialogue(int i)
    {
        return dialogueOrder.orders[i];
    }   

}



using NUnit.Framework.Constraints;
using System.IO.Compression;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MainGameSystem : MonoBehaviour
{
    [Header("Game Object")]
    public S_Topping target_Topping;
    public S_Topping Current_Topping;

    public CharacterMaker character_OBJ;

    [Header("MoveControll")]
    public MoveControll moveControll_OBJ;

    [Header("Dialogue")]
    public DialogueSystem dialogueSystem_OBJ;

    [Header("Dialogue UI")]
    public TextMeshProUGUI text_Dialogue;
    public GameObject DiaLogueUI;
    public float timetoShowDialogue = 3.0f;
    private float timeProgress = 0.0f;
    private bool shouldCountTime = false;

    [Header("Cool Down")]

    [Space(5)]

    public bool CanMakeOrder = false;
    public bool ShouldRandomNewOrder = false;
    public bool ShouldShowDialogue = false;
    public bool ShouldGetCustomerOut = false;

    [Space(5)]

    private bool ForceTwoType = true;
    [SerializeField]
    public E_CharacterType CustomerType;
    [SerializeField]
    private int CustomerCount = 0;
    private int DuplicateCount = 0;

    [SerializeField]
    private int FailConut = 0;

    private void Awake()
    {
        CustomerType = E_CharacterType.Normal;
        CustomerCount = 0;
        DuplicateCount = 0;
        FailConut = 0;
        
    }

    private void Start()
    {
        RandomInitial();
        character_OBJ.RandomSpriteSet();
    }

    private void Update()
    {
        if(shouldCountTime)
        {
            timeProgress += Time.deltaTime;
            if (timeProgress >= 2 && Mouse.current.leftButton.wasPressedThisFrame && moveControll_OBJ.stageEvent == StageEvent.Service_End)
            {
                // change to customer exit
                moveControll_OBJ.MoveTo(StageEvent.Customer_Exit);
                timeProgress = 0.0f;
                shouldCountTime = false;
                ActiveUiDialog(false);
            }
            else if (timeProgress >= 2 && Mouse.current.leftButton.wasPressedThisFrame)
            {
                //manual close dialogue
                timeProgress = 0.0f;
                shouldCountTime = false;
                ActiveUiDialog(false);
            }
            
            if (moveControll_OBJ.stageEvent == StageEvent.Service_End && timeProgress >= timetoShowDialogue)
            {
                // change to customer exit
                moveControll_OBJ.MoveTo(StageEvent.Customer_Exit);
                timeProgress = 0.0f;
                shouldCountTime = false;
                ActiveUiDialog(false);
            }
            else if (timeProgress >= timetoShowDialogue)
            {
                //for only closing dialogue
                timeProgress = 0.0f;
                shouldCountTime = false;
                ActiveUiDialog(false);
            }
            
        }

        if (moveControll_OBJ.stageEvent == StageEvent.Customer_Enter && moveControll_OBJ.IsMovementComplete()) {
            RandomTargetTopping();
            ActiveUiDialog(true);
            shouldCountTime = true;
            SetTextDialog(GetOrder());
            
            moveControll_OBJ.stageEvent = StageEvent.Service_Start;
        }

        if (moveControll_OBJ.stageEvent == StageEvent.Service_Start && !CanMakeOrder){
            CanMakeOrder = true;
        }

        if (moveControll_OBJ.stageEvent == StageEvent.Service_End && ShouldShowDialogue) 
        {
            ShouldRandomNewOrder = true;
            FailCheck();
            ActiveUiDialog(true);
            shouldCountTime = true;
            ShouldShowDialogue = false;
        }

        if (moveControll_OBJ.stageEvent == StageEvent.Customer_Enter && ShouldRandomNewOrder) {
            ActiveUiDialog(false);
            RandomInitial();
            ShouldRandomNewOrder = false;
            character_OBJ.RandomSpriteSet();
        }

        if (moveControll_OBJ.stageEvent == StageEvent.WarpToStartPoint && moveControll_OBJ.IsMovementComplete())
        {
            RandomInitial();
            character_OBJ.RandomSpriteSet();
            moveControll_OBJ.MoveTo(StageEvent.Customer_Enter);
        }

        if (moveControll_OBJ.stageEvent == StageEvent.Customer_Exit && moveControll_OBJ.IsMovementComplete() && !shouldCountTime) 
        { 
            moveControll_OBJ.MoveTo(StageEvent.WarpToStartPoint);
        }

        if (moveControll_OBJ.stageEvent == StageEvent.Service_End && ShouldGetCustomerOut)
        { 
            moveControll_OBJ.MoveTo(StageEvent.Customer_Exit);
            ShouldGetCustomerOut = false;   
        }
        



    }

    public bool FailCheck() { 
        return FailConut >= 3;
    }

    public bool CheckOrderShouldServe()
    {
        if (CustomerType == E_CharacterType.Anamoly)
        {
            Debug.Log("Anamoly always not serve");
            FailConut += 3;
            return false;
        }
        // Normal customer check
        return CheckOrderMatch();
    }

    public bool CheckShouldMakeCustomerOut()
    {
        if (CustomerType == E_CharacterType.Anamoly)
        {
            Debug.Log("Anamoly always make out");
            return true;
        }

        FailConut += 1;
        return false;
    }

    public bool CheckOrderMatch()
    {
        if (Current_Topping.Katsuobushi != target_Topping.Katsuobushi)
        {
            Debug.Log("Katsuobushi mismatch");
            return false;
        }
        if (Current_Topping.Seaweed != target_Topping.Seaweed)
        {
            Debug.Log("Seaweed mismatch");
            return false;
        }
        if (Current_Topping.Mayonnaise != target_Topping.Mayonnaise)
        {
            Debug.Log("Mayonnaise mismatch");
            return false;
        }

        Debug.Log("Order matched");
        return true;
    }

    public void RandomInitial()
    {
        RandomCustomer();
        RandomTargetTopping();
        Current_Topping.ResetTopping();
    }

    public void RandomTargetTopping() {
        Current_Topping.ResetTopping();
        int rand = Random.Range(1, 8);

        dialogueSystem_OBJ.OrderDialogue = dialogueSystem_OBJ.dialogueOrder.orders[rand - 1];

        switch (rand)
        {
            case 1:
                target_Topping.Katsuobushi = true;
                target_Topping.Seaweed = true;
                target_Topping.Mayonnaise = true;
                break;
            case 2:
                target_Topping.Katsuobushi = false;
                target_Topping.Seaweed = true;
                target_Topping.Mayonnaise = true;
                break;
            case 3:
                target_Topping.Katsuobushi = false;
                target_Topping.Mayonnaise = false;
                target_Topping.Seaweed = true;
                break;
            case 4:
                target_Topping.Katsuobushi = true;
                target_Topping.Seaweed = false;
                target_Topping.Mayonnaise = true;
                break;
            case 5:
                target_Topping.Katsuobushi = true;
                target_Topping.Seaweed = true;
                target_Topping.Mayonnaise = false;
                break;
            case 6:
                target_Topping.Katsuobushi = true;
                target_Topping.Seaweed = false;
                target_Topping.Mayonnaise = false;
                break;
            case 7:
                target_Topping.Katsuobushi = false;
                target_Topping.Seaweed = true;
                target_Topping.Mayonnaise = false;
                break;
        }
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
        
    }

    public string GetOrder() {
        string order = "Order: ";

        if (CustomerType == E_CharacterType.Normal)
        {
            order += dialogueSystem_OBJ.GetRandomOrder(E_CharacterType.Normal);
        }

        if (CustomerType == E_CharacterType.Anamoly)
        {
            order += dialogueSystem_OBJ.GetRandomOrder(E_CharacterType.Anamoly);
        }
        order += " ";
        order += dialogueSystem_OBJ.OrderDialogue;

        return order;
    }

    public void BTN_MakeCustomerOut() {
        if (CheckShouldMakeCustomerOut())
        {
            moveControll_OBJ.MoveTo(StageEvent.Customer_Exit);
            Debug.Log("Get out!!");
            ShouldGetCustomerOut = true;
        }
        else {
            moveControll_OBJ.MoveTo(StageEvent.Service_End);
            ShouldShowDialogue = true;
            text_Dialogue.text = dialogueSystem_OBJ.GetRandomGetoutDialogue();
            Debug.Log(dialogueSystem_OBJ.GetRandomGetoutDialogue());
            Debug.Log("You can't make that customer out");
        }

        RandomInitial();
    }

    public void CheckOrder()
    {

        if (!CheckOrderShouldServe())
        {
            //get wrong serve dialogue
            Debug.Log(dialogueSystem_OBJ.GetRandomServeDialogue(false));
            SetTextDialog(dialogueSystem_OBJ.GetRandomServeDialogue(false));
            ActiveUiDialog(true);

            FailConut++;
            //Debug.Log("No no no");
            RandomInitial();
        }
        else
        {
            //get right serve dialogue
            Debug.Log(dialogueSystem_OBJ.GetRandomServeDialogue(true));
            SetTextDialog(dialogueSystem_OBJ.GetRandomServeDialogue(true));
            //start Countdown to next customer


            RandomInitial();
        }

        
    }

    public void BTN_Serve() { 
        CheckOrder();
        SetShouldShowDialogue(true);
        moveControll_OBJ.MoveTo(StageEvent.Service_End);
    }

    public void TriggerEvent() { 
        
    }

    public void  EnableToppingButton() { 
        
    }

    public string GetState()
    {
        return "Customer Type: " + CustomerType.ToString() + ", Fail Count: " + FailConut.ToString();
    }
    //method set apppearance according to customer type


    public void ActiveUiDialog(bool active) {
        DiaLogueUI.SetActive(active);
    }

    public void SetTextDialog(string str) { 
        text_Dialogue.text = str;
    }

    public void CheckForEndGame() { }

    public void SetShouldShowDialogue(bool b) {
        ShouldShowDialogue = b;
    }
}

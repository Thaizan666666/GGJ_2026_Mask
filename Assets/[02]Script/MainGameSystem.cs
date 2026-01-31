using NUnit.Framework.Constraints;
using System.IO.Compression;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MainGameSystem : MonoBehaviour
{
    [Header("Game Object")]
    public S_Topping target_Topping;
    public S_Topping Current_Topping;
    public CharacterMaker character_OBJ;

    [Header("BTN")]
    public Button btn_serve;

    public Button btn_makeCustomerOut;
    public Button btn_Katsuobushi;
    public Button btn_Seaweed;
    public Button btn_Mayonnaise;

    private bool _isHaveDoh = false;
    public bool IsHaveDoh
    {
        get { return _isHaveDoh; }
        set { _isHaveDoh = value; }
    }

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

    // Cache for optimization
    private StageEvent currentStage;
    private bool isMovementComplete;
    private bool isLeftButtonPressed;

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
        // Cache frequently accessed values
        currentStage = moveControll_OBJ.stageEvent;
        isMovementComplete = moveControll_OBJ.IsMovementComplete();
        isLeftButtonPressed = Mouse.current.leftButton.wasPressedThisFrame;

        HandleDialogueTimer();
        HandleStageTransitions();
    }

    private void HandleDialogueTimer()
    {
        if (!shouldCountTime) return;

        timeProgress += Time.deltaTime;

        // Manual close (after 2 seconds)
        if (timeProgress >= 2 && isLeftButtonPressed)
        {
            CloseDialogue();

            if (currentStage == StageEvent.Service_End)
            {
                moveControll_OBJ.MoveTo(StageEvent.Customer_Exit);
            }
            return;
        }

        // Auto close
        if (timeProgress >= timetoShowDialogue)
        {
            CloseDialogue();

            if (currentStage == StageEvent.Service_End)
            {
                moveControll_OBJ.MoveTo(StageEvent.Customer_Exit);
            }
        }
    }

    private void HandleStageTransitions()
    {
        switch (currentStage)
        {
            case StageEvent.Customer_Enter:
                HandleCustomerEnter();
                break;

            case StageEvent.Service_Start:
                
                if (!CanMakeOrder)
                {
                    CanMakeOrder = true;
                }
                break;

            case StageEvent.Service_End:
                HandleServiceEnd();
                break;

            case StageEvent.WarpToStartPoint:
                if (isMovementComplete)
                {
                    PrepareNewCustomer();
                    moveControll_OBJ.MoveTo(StageEvent.Customer_Enter);
                }
                break;

            case StageEvent.Customer_Exit:
                CanMakeOrder = false;
                if (isMovementComplete && !shouldCountTime)
                {
                    moveControll_OBJ.MoveTo(StageEvent.WarpToStartPoint);
                }
                break;
        }
    }

    private void HandleCustomerEnter()
    {
        if (ShouldRandomNewOrder)
        {
            PrepareNewCustomer();
            ShouldRandomNewOrder = false;
            return;
        }

        if (isMovementComplete)
        {
            RandomTargetTopping();
            ShowDialogue(GetOrder());
            moveControll_OBJ.stageEvent = StageEvent.Service_Start;
        }
    }

    private void HandleServiceEnd()
    {
        if (ShouldShowDialogue)
        {
            ShouldRandomNewOrder = true;
            FailCheck();
            ShowDialogue(text_Dialogue.text); // Text already set by CheckOrder
            ShouldShowDialogue = false;
        }

        if (ShouldGetCustomerOut)
        {
            moveControll_OBJ.MoveTo(StageEvent.Customer_Exit);
            ShouldGetCustomerOut = false;
        }
    }

    private void PrepareNewCustomer()
    {
        ActiveUiDialog(false);
        RandomInitial();
        character_OBJ.RandomSpriteSet();
    }

    private void ShowDialogue(string message)
    {
        SetTextDialog(message);
        ActiveUiDialog(true);
        shouldCountTime = true;
    }

    private void CloseDialogue()
    {
        timeProgress = 0.0f;
        shouldCountTime = false;
        ActiveUiDialog(false);
    }

    public bool FailCheck()
    {
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

    public void RandomTargetTopping()
    {
        Current_Topping.ResetTopping();
        int rand = Random.Range(1, 8);

        dialogueSystem_OBJ.OrderDialogue = dialogueSystem_OBJ.dialogueOrder.orders[rand - 1];

        // Reset all to false first
        target_Topping.Katsuobushi = false;
        target_Topping.Seaweed = false;
        target_Topping.Mayonnaise = false;

        // Set based on pattern
        switch (rand)
        {
            case 1: // All toppings
                target_Topping.Katsuobushi = true;
                target_Topping.Seaweed = true;
                target_Topping.Mayonnaise = true;
                break;
            case 2: // Seaweed + Mayo
                target_Topping.Seaweed = true;
                target_Topping.Mayonnaise = true;
                break;
            case 3: // Seaweed only
                target_Topping.Seaweed = true;
                break;
            case 4: // Katsuobushi + Mayo
                target_Topping.Katsuobushi = true;
                target_Topping.Mayonnaise = true;
                break;
            case 5: // Katsuobushi + Seaweed
                target_Topping.Katsuobushi = true;
                target_Topping.Seaweed = true;
                break;
            case 6: // Katsuobushi only
                target_Topping.Katsuobushi = true;
                break;
            case 7: // Seaweed only (duplicate of case 3)
                target_Topping.Seaweed = true;
                break;
        }
    }

    public void RandomCustomer()
    {
        // First customer is always normal
        if (CustomerCount == 0)
        {
            CustomerType = E_CharacterType.Normal;
            CustomerCount++;
            DuplicateCount = 0;
            return;
        }

        E_CharacterType previousType = CustomerType;
        E_CharacterType[] availableTypes = GetAvailableCustomerTypes();

        // Force different type if too many duplicates
        if (DuplicateCount >= 2)
        {
            CustomerType = SelectDifferentType(availableTypes, CustomerType);
            DuplicateCount = 0;
        }
        else
        {
            // Random selection
            int rand = Random.Range(0, availableTypes.Length);
            CustomerType = availableTypes[rand];

            // Update duplicate count
            DuplicateCount = (CustomerType == previousType) ? DuplicateCount + 1 : 0;
        }
    }

    private E_CharacterType[] GetAvailableCustomerTypes()
    {
        if (!ForceTwoType)
            return (E_CharacterType[])System.Enum.GetValues(typeof(E_CharacterType));
        else
            return new E_CharacterType[] { E_CharacterType.Normal, E_CharacterType.Anamoly };
    }

    private E_CharacterType SelectDifferentType(E_CharacterType[] allTypes, E_CharacterType excludeType)
    {
        System.Collections.Generic.List<E_CharacterType> availableTypes = new System.Collections.Generic.List<E_CharacterType>();
        foreach (E_CharacterType type in allTypes)
        {
            if (type != excludeType)
            {
                availableTypes.Add(type);
            }
        }

        int rand = Random.Range(0, availableTypes.Count);
        return availableTypes[rand];
    }

    public string GetOrder()
    {
        string characterDialogue = dialogueSystem_OBJ.GetRandomOrder(CustomerType);
        return "Order: " + characterDialogue + " " + dialogueSystem_OBJ.OrderDialogue;
    }

    public void BTN_MakeCustomerOut()
    {
        //RandomInitial();
        if (CheckShouldMakeCustomerOut())
        {
            Debug.Log("Get out!!");
            ShouldGetCustomerOut = true;
            moveControll_OBJ.MoveTo(StageEvent.Customer_Exit);
        }
        else
        {
            Debug.Log("You can't make that customer out");
            text_Dialogue.text = dialogueSystem_OBJ.GetRandomGetoutDialogue();
            Debug.Log(dialogueSystem_OBJ.GetRandomGetoutDialogue());
            ShouldGetCustomerOut = false;
            ShouldShowDialogue = true;
            moveControll_OBJ.MoveTo(StageEvent.Service_End);
        }
    }

    public void CheckOrder()
    {
        bool orderCorrect = CheckOrderShouldServe();
        string dialogueText = dialogueSystem_OBJ.GetRandomServeDialogue(orderCorrect);

        Debug.Log(dialogueText);
        SetTextDialog(dialogueText);

        if (!orderCorrect)
        {
            FailConut++;
        }

        RandomInitial();
    }

    public void BTN_Serve()
    {
        CheckOrder();
        SetShouldShowDialogue(true);
        IsHaveDoh = false;
        moveControll_OBJ.MoveTo(StageEvent.Service_End);
    }

    public void TriggerEvent()
    {
        // Implementation needed
    }

    public string GetState()
    {
        return "Customer Type: " + CustomerType.ToString() + ", Fail Count: " + FailConut.ToString();
    }

    public void ActiveUiDialog(bool active)
    {
        DiaLogueUI.SetActive(active);
    }

    public void SetTextDialog(string str)
    {
        text_Dialogue.text = str;
    }

    public void CheckForEndGame()
    {
        // Implementation needed
    }

    public void SetShouldShowDialogue(bool b)
    {
        ShouldShowDialogue = b;
    }

    public void SetKatsuobushi(bool b)
    {
        if (_isHaveDoh)
        {
            Current_Topping.Katsuobushi = b;
            btn_Katsuobushi.interactable = false;
        }
    }

    public void SetSeaweed(bool b)
    {
        if (_isHaveDoh)
        {
            Current_Topping.Seaweed = b;
            btn_Seaweed.interactable = false;
        }
    }

    public void SetMayonnaise(bool b)
    {
        if (_isHaveDoh)
        {
            Current_Topping.Mayonnaise = b;
            btn_Mayonnaise.interactable = false;
        }
    }

    public void EnableBTNTopping(bool b) { 
        if (b)
        {
            btn_Katsuobushi.interactable = true;
            btn_Seaweed.interactable = true;
            btn_Mayonnaise.interactable = true;
        }
        else
        {
            btn_Katsuobushi.interactable = false;
            btn_Seaweed.interactable = false;
            btn_Mayonnaise.interactable = false;
        }
    }   
}
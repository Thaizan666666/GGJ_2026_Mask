using UnityEngine;
using UnityEngine.Rendering;

public class NewRandomCharacterSystem : MonoBehaviour
{
    E_CharacterType currentCharacterType = E_CharacterType.None;
    NewCharacterMaker characterMaker;

    private void Awake()
    {
        characterMaker = FindFirstObjectByType<NewCharacterMaker>();
    }

    public void RandomCharacterType() {
        // Get the number of enum values
        int enumCount = System.Enum.GetValues(typeof(E_CharacterType)).Length;
        currentCharacterType = (E_CharacterType)Random.Range(1, enumCount);
    }

    public void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            RandomCharacterType();
            Debug.Log("Selected: " + currentCharacterType);

            switch (currentCharacterType) { 
                case E_CharacterType.Normal:
                    characterMaker.SetupNormalCharacter();
                    break;
                case E_CharacterType.Anamoly:
                    characterMaker.SetupAnomolyCharacter();
                    break;
                default:
                    Debug.LogWarning("Invalid character type selected.");
                    break;
            }
        }
    }
}


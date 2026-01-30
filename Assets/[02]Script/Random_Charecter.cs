using UnityEngine;

public class Random_Charecter : MonoBehaviour
{
    E_Charecter e_Charecter;
    public bool isFinish;

    public void RandomCharacter()
    {
        // Get the number of enum values
        int enumCount = System.Enum.GetValues(typeof(E_Charecter)).Length;
        e_Charecter = (E_Charecter)Random.Range(0, enumCount);
    }
    public void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            RandomCharacter();
            Debug.Log("Selected: " + e_Charecter);
        }
    }
    public bool ISfinish(bool GetTako)
    {
        return true;
    }
}
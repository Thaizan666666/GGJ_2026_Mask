using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneManager : MonoBehaviour
{
    public void ChangeSceneTo(string name) { 
        SceneManager.LoadScene(name);
    }
}

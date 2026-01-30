using UnityEngine;
using UnityEngine.SceneManagement;

public class MainManu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene(1);
    }
    public void PlayAgain()
    {
        SceneManager.LoadScene(1);
    }
    public void Gameover()
    {
        SceneManager.LoadScene(2);
    }
    public void SceneMainManu()
    {
        SceneManager.LoadScene(0);
    }
}

using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuControls : MonoBehaviour
{
    public void PlayPressed()
    {
        Debug.Log("Scene changed");
        SceneManager.LoadScene("GameScene");
    }

    public void ExitPressed()
    {
        Debug.Log("Exit pressed");
        Application.Quit();
    }
}

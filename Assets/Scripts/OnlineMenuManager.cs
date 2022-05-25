using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OnlineMenuManager : MonoBehaviour
{
    public void StartHost()
    {
        SceneManager.LoadScene("GameScene");
        NetworkManager.Singleton.StartHost();
    }

    public void StartClient()
    {
        SceneManager.LoadScene("GameScene");
        NetworkManager.Singleton.StartClient();
    }

    public void StartServer()
    {
        SceneManager.LoadScene("GameScene");
        NetworkManager.Singleton.StartServer();
    }
}

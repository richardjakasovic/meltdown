using UnityEngine;
using UnityEngine.SceneManagement;

public class Bootstrap : MonoBehaviour
{
    public string gameSceneName = "PersistentScene"; 

    void Start()
    {
        SceneManager.LoadScene("MainMenuScene", LoadSceneMode.Additive);
    }
}
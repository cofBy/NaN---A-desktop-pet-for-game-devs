using UnityEngine;
using UnityEngine.SceneManagement;

public class launcher : MonoBehaviour
{
    public void Close()
    {
        Application.Quit();
    }

    public void Play()
    {
        SceneManager.LoadScene(1);
    }
}

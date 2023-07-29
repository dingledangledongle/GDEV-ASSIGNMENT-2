using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    public void Pressed()
    {
        SceneManager.LoadScene("Main");
    }
}

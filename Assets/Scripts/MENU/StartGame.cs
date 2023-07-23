using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    private MapGenerator mapGenerator;
    public void Pressed()
    {
        SceneManager.LoadScene(1);
        mapGenerator.GenerateGraph();
    }
}


using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagement : MonoBehaviour
{
    public void NextScene()
    {
       
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
       
        SceneManager.LoadScene(currentSceneIndex + 1);
    }
}

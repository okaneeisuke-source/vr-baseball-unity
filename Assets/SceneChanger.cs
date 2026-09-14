using UnityEngine;
using UnityEngine.SceneManagement;

public class KeyboardSceneChanger : MonoBehaviour
{
    [SerializeField] private string sceneAName = "darts";
    [SerializeField] private string sceneBName = "Experiment1";

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            string currentSceneName =
                SceneManager.GetActiveScene().name;

            if (currentSceneName == sceneAName)
            {
                SceneManager.LoadScene(sceneBName);
            }
            else if (currentSceneName == sceneBName)
            {
                SceneManager.LoadScene(sceneAName);
            }
        }
    }
}
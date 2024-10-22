using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    private static AsyncOperation loadScene;

    public static void StartLoadScene(string sceneName)
    {
        loadScene = SceneManager.LoadSceneAsync(sceneName);
        loadScene.allowSceneActivation = false;
    }

    public static void ActivateScene()
    {
        loadScene.allowSceneActivation = true;
    }

    public static float GetLoadProgress()
    {
        return loadScene.progress;
    }
}

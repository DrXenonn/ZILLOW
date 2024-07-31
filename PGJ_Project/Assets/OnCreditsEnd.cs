using System.Collections;
using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OnCreditsEnd : MonoBehaviour
{
    
    public void LoadMainMenu()
    {
        Time.timeScale = 1;
        StartCoroutine(LoadSceneAsyncAdditive(0));
    }


    private IEnumerator LoadSceneAsyncAdditive(int sceneIndex)
    {
        var asyncLoad = SceneManager.LoadSceneAsync(sceneIndex, LoadSceneMode.Additive);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex);
    }
}
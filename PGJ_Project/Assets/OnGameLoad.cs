using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OnGameLoad : MonoBehaviour
{
    [SerializeField] private Light[] Lights;

    private void Start()
    {
        Invoke(nameof(LateStart),0.5f);
    }

    private void LateStart()
    {
        foreach (var l in Lights)
        {
            l.enabled = true;
        }
    }
    
    public void OnPlay()
    {
        StartCoroutine(LoadSceneAsyncAdditive(1));
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

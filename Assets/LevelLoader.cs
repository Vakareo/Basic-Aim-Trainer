using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(this);
        if (SceneManager.sceneCount == 1)
        {
            StartCoroutine(LoadBoxLevel());
        }
    }


    IEnumerator LoadBoxLevel()
    {
        AsyncOperation loading = SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);
        while (!loading.isDone)
        {
            yield return null;
        }
    }
}

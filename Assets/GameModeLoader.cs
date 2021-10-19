using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameModeLoader : MonoBehaviour
{
    [SerializeField] GameObject[] gameModes;
    private int loadedIndex = -1;
    private GameObject loadedObject;
    private IGameMode loadedComponent;

    public void Cycle()
    {
        var newIndex = (loadedIndex + 1) % gameModes.Length;
        UnloadGamemode();
        LoadGamemode(newIndex);
    }
    private void Awake()
    {
        LoadGamemode(0);
    }

    private void UnloadGamemode()
    {
        if (loadedIndex != -1)
        {
            StopGamemode();
            loadedComponent = null;
            Destroy(loadedObject);
            loadedObject = null;
        }
    }

    public void StopGamemode()
    {
        loadedComponent.Stop();
    }

    public void PlayGamemode()
    {
        ResetAndPlayGamemode();
    }
    public bool LoadGamemode(int gamemode)
    {
        if (gamemode < 0 || gamemode >= gameModes.Length)
            return false;

        loadedIndex = gamemode;
        loadedObject = Instantiate(gameModes[gamemode], transform.position, Quaternion.identity, transform);
        if (!loadedObject.TryGetComponent<IGameMode>(out loadedComponent))
            return false;

        loadedComponent.Initialize();
        return true;
    }
    private void ResetAndPlayGamemode()
    {
        loadedComponent.Stop();
        loadedComponent.Play();
    }
}

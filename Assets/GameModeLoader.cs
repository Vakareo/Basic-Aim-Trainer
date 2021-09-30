using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameModeLoader : MonoBehaviour
{
    [SerializeField] GameObject[] gameModes;
    private int loadedIndex = -1;
    private GameObject loadedObject;
    private IGameMode loadedComponent;

    public void PlayFirstGamemode()
    {
        PlayGamemode(0);
    }
    public void StopGamemode()
    {
        loadedComponent.Stop();
    }

    public void PlayGamemode(int gamemode)
    {
        if (loadedIndex == gamemode)
        {
            ResetAndPlayGamemode();
        }
        else
        {
            LoadGamemode(gamemode);
            ResetAndPlayGamemode();
        }
    }
    private void LoadGamemode(int gamemode)
    {
        loadedIndex = gamemode;
        loadedObject = Instantiate(gameModes[gamemode], transform.position, Quaternion.identity, transform);
        loadedComponent = loadedObject.GetComponent<IGameMode>();
    }
    private void ResetAndPlayGamemode()
    {
        loadedComponent.Stop();
        loadedComponent.Play();
    }


}

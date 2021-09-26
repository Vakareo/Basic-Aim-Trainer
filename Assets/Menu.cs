using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    public GameObject eventSystem;
    // Start is called before the first frame update
    private void Awake()
    {
        DontDestroyOnLoad(this);
        DontDestroyOnLoad(eventSystem);
    }

    // Update is called once per frame
    void Update()
    {

    }
}

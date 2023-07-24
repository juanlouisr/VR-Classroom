using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class NetworkStartup : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (SceneTransitionManager.singleton?.InitiatlizeAsHost ?? true)
        {
            Debug.Log("Initalizing as Host");
            NetworkManager.Singleton.StartHost();
        }
        else
        {
            Debug.Log("Initalizing as Client");
            NetworkManager.Singleton.StartClient();
        }
    }
}

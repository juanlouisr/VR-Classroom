using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class NetworkStartup : MonoBehaviour
{
    // Start is called before the first frame update
    async void Start()
    {
        if (SceneTransitionManager.singleton?.InitiatlizeAsHost ?? true)
        {
            Debug.Log("Initalizing as Host");
            if (RelayManager.singleton.IsRelayEnabled) {
                await RelayManager.singleton.SetupRelay();
            }
            NetworkManager.Singleton.StartHost();
        }
        else
        {
            Debug.Log("Initalizing as Client");
            if (RelayManager.singleton.IsRelayEnabled) {
                await RelayManager.singleton.JoinRelay(SceneTransitionManager.singleton.JoinCode);
            }
            NetworkManager.Singleton.StartClient();
        }
    }
}

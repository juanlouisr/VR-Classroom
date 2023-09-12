using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class JoinCodeStartup : MonoBehaviour
{

    private TMP_Text textCode;
    // Start is called before the first frame update
    void Start()
    {
        textCode = GetComponentInChildren<TMP_Text>();
        RelayManager.singleton.OnRoomJoined += OnRoomJoinEvent;
    }

    void OnRoomJoinEvent(string joinCode)
    {
        textCode.text = joinCode;
    }

}

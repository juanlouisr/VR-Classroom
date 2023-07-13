using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class NetworkTeleportationProvider : TeleportationProvider
{
    // Start is called before the first frame update
    [SerializeField]
    public bool enableTeleporationProvider = true;

    public override bool QueueTeleportRequest(TeleportRequest teleportRequest)
    {
        if (!enableTeleporationProvider)
        {
            return false;
        }
        Debug.Log("Queuein teleport");

        return base.QueueTeleportRequest(teleportRequest);
    }

}

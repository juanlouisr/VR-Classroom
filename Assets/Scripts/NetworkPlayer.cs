using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.InputSystem.XR;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;

public class NetworkPlayer : NetworkBehaviour
{
// Start is called before the first frame update
    [SerializeField]
    XRRayInteractor[] m_RayInteractors;
    
    public override void OnNetworkSpawn()
    {
        DisableClientInput();
        RegisterNetworkGrabbable();
        base.OnNetworkSpawn();
    }

    public override void OnNetworkDespawn()
    {
        UnregisterNetworkGrabbable();
        base.OnNetworkDespawn();
    }


    private void DisableClientInput()
    {
        if (IsClient && !IsOwner)
        {
            var clientHead = GetComponentInChildren<TrackedPoseDriver>();
            var clientCamera = GetComponentInChildren<Camera>();
            var clientAudio = GetComponentInChildren<AudioListener>();
            var clientDynamicMove = GetComponent<NetworkDynamicMoveProvider>();
            var clientContinousTurn = GetComponent<NetworkContinousTurnProvider>();
            var clientSnapTurn = GetComponent<NetworkSnapTurnProvider>();
            var clientTeleport = GetComponent<NetworkTeleportationProvider>();
            var clientControllers = GetComponentsInChildren<ActionBasedController>();
            var controllerManagers = GetComponentsInChildren<ActionBasedControllerManager>();
            var avatarRendererController = GetComponentInChildren<AvatarRendererController>();

            clientCamera.enabled = false;
            clientAudio.enabled = false;
            clientHead.enabled = false;
            clientDynamicMove.enableInputAction = false;
            clientContinousTurn.enableInputAction = false;
            clientSnapTurn.enableInputAction = false;
            clientSnapTurn.enableTurnLeftRight = false;
            clientSnapTurn.enableTurnAround = false;
            clientTeleport.enableTeleporationProvider = false;
            avatarRendererController.isLocalPlayer = false;
            foreach (var controller in clientControllers)
            {
                controller.enableInputActions = false;
                controller.enableInputTracking = false;
            }
            foreach (var controllerManager in controllerManagers)
            {
                controllerManager.enabled = false;
            }
        }
    }

    private void RegisterNetworkGrabbable()
    {
        foreach (var m_RayInteractor in  m_RayInteractors) 
        {
            m_RayInteractor.selectEntered.AddListener(OnSelectGrabbable);
        }
    }

    private void UnregisterNetworkGrabbable()
    {
        foreach (var m_RayInteractor in m_RayInteractors)
        {
            m_RayInteractor.selectEntered.RemoveListener(OnSelectGrabbable);
        }
    }

    public void OnSelectGrabbable(SelectEnterEventArgs eventArgs)
    {
        if (IsClient && IsOwner)
        {
            Debug.Log("Select Grabbable Event");
            var selectedNetworkObj = eventArgs.interactableObject.transform.GetComponent<NetworkObject>();

            if (selectedNetworkObj != null)
            {
                RequestGrabbableOwnershipServerRpc(OwnerClientId, selectedNetworkObj);
            }
        }
    }

    [ServerRpc]
    public void RequestGrabbableOwnershipServerRpc(ulong networkClientId, NetworkObjectReference networkObjectReference)
    {
        if (networkObjectReference.TryGet(out NetworkObject networkObject))
        {
            networkObject.ChangeOwnership(networkClientId);
        }
        else
        {
            Debug.Log($"Unable to change ownership for clietnId {networkClientId}");
        }
    }
}

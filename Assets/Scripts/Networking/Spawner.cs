using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using Fusion.Sockets;
using System;
using SG;
public class Spawner : MonoBehaviour, INetworkRunnerCallbacks
{
    public NetworkPlayer playerPrefab;
    // As we are in shared topology, having the StateAuthority means we are the local user
    public bool IsLocalUser => playerPrefab.networkObject.HasStateAuthority;
    private SG_User sgUser;
    void Start()
    {
        sgUser = GameObject.Find("[SG_User]").GetComponent<SG_User>();
    }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        if (player == runner.LocalPlayer)
        {
            runner.Spawn(playerPrefab, transform.position, Quaternion.identity, player);

        }
    }

    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        // if (characterInputHandler == null && NetworkPlayer.Local != null)
        //     characterInputHandler = NetworkPlayer.Local.GetComponent<CharacterInputHandler>();

        // if (characterInputHandler != null)
        //     input.Set(characterInputHandler.GetNetworkInput());
        var data = new NetworkInputData();

        data.leftHand.wristPosistion=sgUser.leftHand.handModel.wristTransform.position;
        data.leftHand.wristRotation=sgUser.leftHand.handModel.wristTransform.rotation;


        data.leftHand.Thumb_CMC=sgUser.leftHand.handModel.thumbJoints[0].rotation;
        data.leftHand.Thumb_MCP=sgUser.leftHand.handModel.thumbJoints[1].rotation;
        data.leftHand.Thumb_IP=sgUser.leftHand.handModel.thumbJoints[2].rotation;
        data.leftHand.Thumb_FingerTip=sgUser.leftHand.handModel.thumbJoints[3].rotation;

        data.leftHand.Index_MCP=sgUser.leftHand.handModel.indexJoints[0].rotation;
        data.leftHand.Index_PIP=sgUser.leftHand.handModel.indexJoints[1].rotation;
        data.leftHand.Index_DIP=sgUser.leftHand.handModel.indexJoints[2].rotation;
        data.leftHand.Index_FingerTip=sgUser.leftHand.handModel.indexJoints[3].rotation;

        data.leftHand.Middle_MCP=sgUser.leftHand.handModel.middleJoints[0].rotation;
        data.leftHand.Middle_PIP=sgUser.leftHand.handModel.middleJoints[1].rotation;
        data.leftHand.Middle_DIP=sgUser.leftHand.handModel.middleJoints[2].rotation;
        data.leftHand.Middle_FingerTip=sgUser.leftHand.handModel.middleJoints[3].rotation;

        data.leftHand.Ring_MCP=sgUser.leftHand.handModel.ringJoints[0].rotation;
        data.leftHand.Ring_PIP=sgUser.leftHand.handModel.ringJoints[1].rotation;
        data.leftHand.Ring_DIP=sgUser.leftHand.handModel.ringJoints[2].rotation;
        data.leftHand.Ring_FingerTip=sgUser.leftHand.handModel.ringJoints[3].rotation;

        data.leftHand.Pinky_MCP=sgUser.leftHand.handModel.pinkyJoints[0].rotation;
        data.leftHand.Pinky_PIP=sgUser.leftHand.handModel.pinkyJoints[1].rotation;
        data.leftHand.Pinky_DIP=sgUser.leftHand.handModel.pinkyJoints[2].rotation;
        data.leftHand.Pinky_FingerTip=sgUser.leftHand.handModel.pinkyJoints[3].rotation;



        // data.leftHand.Thumb_CMC=sgUser.leftHand.handModel.FingerJoints[0][1].rotation;
        input.Set(data);
    }

    public void OnConnectedToServer(NetworkRunner runner) { Debug.Log("OnConnectedToServer"); }
    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player) { }
    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }
    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) { Debug.Log("OnShutdown"); }
    public void OnDisconnectedFromServer(NetworkRunner runner) { Debug.Log("OnDisconnectedFromServer"); }
    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) { Debug.Log("OnConnectRequest"); }
    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) { Debug.Log("OnConnectFailed"); }
    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }
    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList) { }
    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { }
    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) { }
    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data) { }
    public void OnSceneLoadDone(NetworkRunner runner) { }
    public void OnSceneLoadStart(NetworkRunner runner) { }
}

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
    private Transform headTransform;
    void Start()
    {
        sgUser = GameObject.Find("[SG_User]").GetComponent<SG_User>();
        headTransform=GameObject.Find("[SG_User]").GetComponent<SG.XR.SG_XR_Rig>().headTransfrom;
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
        data.headPos=headTransform.position;
        data.headRot=headTransform.rotation;
        data.leftHand.wristPosistion=sgUser.leftHand.handModel.wristTransform.position;
        data.leftHand.wristRotation=sgUser.leftHand.handModel.wristTransform.rotation;


        data.leftHand.Thumb_CMCRot=sgUser.leftHand.handModel.thumbJoints[0].rotation;
        data.leftHand.Thumb_MCPRot =sgUser.leftHand.handModel.thumbJoints[1].rotation;
        data.leftHand.Thumb_IPRot =sgUser.leftHand.handModel.thumbJoints[2].rotation;
        data.leftHand.Thumb_FingerTipRot =sgUser.leftHand.handModel.thumbJoints[3].rotation;

        data.leftHand.Index_MCPRot =sgUser.leftHand.handModel.indexJoints[0].rotation;
        data.leftHand.Index_PIPRot =sgUser.leftHand.handModel.indexJoints[1].rotation;
        data.leftHand.Index_DIPRot =sgUser.leftHand.handModel.indexJoints[2].rotation;
        data.leftHand.Index_FingerTipRot =sgUser.leftHand.handModel.indexJoints[3].rotation;

        data.leftHand.Middle_MCPRot =sgUser.leftHand.handModel.middleJoints[0].rotation;
        data.leftHand.Middle_PIPRot =sgUser.leftHand.handModel.middleJoints[1].rotation;
        data.leftHand.Middle_DIPRot =sgUser.leftHand.handModel.middleJoints[2].rotation;
        data.leftHand.Middle_FingerTipRot =sgUser.leftHand.handModel.middleJoints[3].rotation;

        data.leftHand.Ring_MCPRot =sgUser.leftHand.handModel.ringJoints[0].rotation;
        data.leftHand.Ring_PIPRot =sgUser.leftHand.handModel.ringJoints[1].rotation;
        data.leftHand.Ring_DIPRot =sgUser.leftHand.handModel.ringJoints[2].rotation;
        data.leftHand.Ring_FingerTipRot =sgUser.leftHand.handModel.ringJoints[3].rotation;

        data.leftHand.Pinky_MCPRot =sgUser.leftHand.handModel.pinkyJoints[0].rotation;
        data.leftHand.Pinky_PIPRot =sgUser.leftHand.handModel.pinkyJoints[1].rotation;
        data.leftHand.Pinky_DIPRot =sgUser.leftHand.handModel.pinkyJoints[2].rotation;
        data.leftHand.Pinky_FingerTipRot =sgUser.leftHand.handModel.pinkyJoints[3].rotation;

        data.leftHand.Thumb_CMCPos = sgUser.leftHand.handModel.thumbJoints[0].position;
        data.leftHand.Thumb_MCPPos = sgUser.leftHand.handModel.thumbJoints[1].position;
        data.leftHand.Thumb_IPPos = sgUser.leftHand.handModel.thumbJoints[2].position;
        data.leftHand.Thumb_FingerTipPos = sgUser.leftHand.handModel.thumbJoints[3].position;

        data.leftHand.Index_MCPPos = sgUser.leftHand.handModel.indexJoints[0].position;
        data.leftHand.Index_PIPPos = sgUser.leftHand.handModel.indexJoints[1].position;
        data.leftHand.Index_DIPPos = sgUser.leftHand.handModel.indexJoints[2].position;
        data.leftHand.Index_FingerTipPos = sgUser.leftHand.handModel.indexJoints[3].position;

        data.leftHand.Middle_MCPPos = sgUser.leftHand.handModel.middleJoints[0].position;
        data.leftHand.Middle_PIPPos = sgUser.leftHand.handModel.middleJoints[1].position;
        data.leftHand.Middle_DIPPos = sgUser.leftHand.handModel.middleJoints[2].position;
        data.leftHand.Middle_FingerTipPos = sgUser.leftHand.handModel.middleJoints[3].position;

        data.leftHand.Ring_MCPPos = sgUser.leftHand.handModel.ringJoints[0].position;
        data.leftHand.Ring_PIPPos = sgUser.leftHand.handModel.ringJoints[1].position;
        data.leftHand.Ring_DIPPos = sgUser.leftHand.handModel.ringJoints[2].position;
        data.leftHand.Ring_FingerTipPos = sgUser.leftHand.handModel.ringJoints[3].position;

        data.leftHand.Pinky_MCPPos = sgUser.leftHand.handModel.pinkyJoints[0].position;
        data.leftHand.Pinky_PIPPos = sgUser.leftHand.handModel.pinkyJoints[1].position;
        data.leftHand.Pinky_DIPPos = sgUser.leftHand.handModel.pinkyJoints[2].position;
        data.leftHand.Pinky_FingerTipPos = sgUser.leftHand.handModel.pinkyJoints[3].position;


        data.rightHand.wristPosistion = sgUser.rightHand.handModel.wristTransform.position;
        data.rightHand.wristRotation = sgUser.rightHand.handModel.wristTransform.rotation;


        data.rightHand.Thumb_CMCRot = sgUser.rightHand.handModel.thumbJoints[0].rotation;
        data.rightHand.Thumb_MCPRot = sgUser.rightHand.handModel.thumbJoints[1].rotation;
        data.rightHand.Thumb_IPRot = sgUser.rightHand.handModel.thumbJoints[2].rotation;
        data.rightHand.Thumb_FingerTipRot = sgUser.rightHand.handModel.thumbJoints[3].rotation;

        data.rightHand.Index_MCPRot = sgUser.rightHand.handModel.indexJoints[0].rotation;
        data.rightHand.Index_PIPRot = sgUser.rightHand.handModel.indexJoints[1].rotation;
        data.rightHand.Index_DIPRot = sgUser.rightHand.handModel.indexJoints[2].rotation;
        data.rightHand.Index_FingerTipRot = sgUser.rightHand.handModel.indexJoints[3].rotation;

        data.rightHand.Middle_MCPRot = sgUser.rightHand.handModel.middleJoints[0].rotation;
        data.rightHand.Middle_PIPRot = sgUser.rightHand.handModel.middleJoints[1].rotation;
        data.rightHand.Middle_DIPRot = sgUser.rightHand.handModel.middleJoints[2].rotation;
        data.rightHand.Middle_FingerTipRot = sgUser.rightHand.handModel.middleJoints[3].rotation;

        data.rightHand.Ring_MCPRot = sgUser.rightHand.handModel.ringJoints[0].rotation;
        data.rightHand.Ring_PIPRot = sgUser.rightHand.handModel.ringJoints[1].rotation;
        data.rightHand.Ring_DIPRot = sgUser.rightHand.handModel.ringJoints[2].rotation;
        data.rightHand.Ring_FingerTipRot = sgUser.rightHand.handModel.ringJoints[3].rotation;

        data.rightHand.Pinky_MCPRot = sgUser.rightHand.handModel.pinkyJoints[0].rotation;
        data.rightHand.Pinky_PIPRot = sgUser.rightHand.handModel.pinkyJoints[1].rotation;
        data.rightHand.Pinky_DIPRot = sgUser.rightHand.handModel.pinkyJoints[2].rotation;
        data.rightHand.Pinky_FingerTipRot = sgUser.rightHand.handModel.pinkyJoints[3].rotation;

        data.rightHand.Thumb_CMCPos = sgUser.rightHand.handModel.thumbJoints[0].position;
        data.rightHand.Thumb_MCPPos = sgUser.rightHand.handModel.thumbJoints[1].position;
        data.rightHand.Thumb_IPPos = sgUser.rightHand.handModel.thumbJoints[2].position;
        data.rightHand.Thumb_FingerTipPos = sgUser.rightHand.handModel.thumbJoints[3].position;

        data.rightHand.Index_MCPPos = sgUser.rightHand.handModel.indexJoints[0].position;
        data.rightHand.Index_PIPPos = sgUser.rightHand.handModel.indexJoints[1].position;
        data.rightHand.Index_DIPPos = sgUser.rightHand.handModel.indexJoints[2].position;
        data.rightHand.Index_FingerTipPos = sgUser.rightHand.handModel.indexJoints[3].position;

        data.rightHand.Middle_MCPPos = sgUser.rightHand.handModel.middleJoints[0].position;
        data.rightHand.Middle_PIPPos = sgUser.rightHand.handModel.middleJoints[1].position;
        data.rightHand.Middle_DIPPos = sgUser.rightHand.handModel.middleJoints[2].position;
        data.rightHand.Middle_FingerTipPos = sgUser.rightHand.handModel.middleJoints[3].position;

        data.rightHand.Ring_MCPPos = sgUser.rightHand.handModel.ringJoints[0].position;
        data.rightHand.Ring_PIPPos = sgUser.rightHand.handModel.ringJoints[1].position;
        data.rightHand.Ring_DIPPos = sgUser.rightHand.handModel.ringJoints[2].position;
        data.rightHand.Ring_FingerTipPos = sgUser.rightHand.handModel.ringJoints[3].position;

        data.rightHand.Pinky_MCPPos = sgUser.rightHand.handModel.pinkyJoints[0].position;
        data.rightHand.Pinky_PIPPos = sgUser.rightHand.handModel.pinkyJoints[1].position;
        data.rightHand.Pinky_DIPPos = sgUser.rightHand.handModel.pinkyJoints[2].position;
        data.rightHand.Pinky_FingerTipPos = sgUser.rightHand.handModel.pinkyJoints[3].position;

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

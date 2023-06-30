using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public struct HandJointInfo:INetworkStruct
{
    public Vector3 wristPosistion;
    public Quaternion wristRotation;
    public Quaternion Thumb_CMC,
        Thumb_MCP,
        Thumb_IP,
        Thumb_FingerTip,

        Index_MCP,
        Index_PIP,
        Index_DIP,
        Index_FingerTip,

        Middle_MCP,
        Middle_PIP,
        Middle_DIP,
        Middle_FingerTip,

        Ring_MCP,
        Ring_PIP,
        Ring_DIP,
        Ring_FingerTip,

        Pinky_MCP,
        Pinky_PIP,
        Pinky_DIP,
        Pinky_FingerTip;
}
public struct NetworkInputData : INetworkInput
{
    public HandJointInfo leftHand;
    public HandJointInfo rightHand;


}

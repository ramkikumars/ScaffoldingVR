using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public struct HandJointInfo:INetworkStruct
{
    public Vector3 wristPosistion;
    public Quaternion wristRotation;
    public Quaternion Thumb_CMCRot,
        Thumb_MCPRot,
        Thumb_IPRot,
        Thumb_FingerTipRot,

        Index_MCPRot,
        Index_PIPRot,
        Index_DIPRot,
        Index_FingerTipRot,

        Middle_MCPRot,
        Middle_PIPRot,
        Middle_DIPRot,
        Middle_FingerTipRot,

        Ring_MCPRot,
        Ring_PIPRot,
        Ring_DIPRot,
        Ring_FingerTipRot,

        Pinky_MCPRot,
        Pinky_PIPRot,
        Pinky_DIPRot,
        Pinky_FingerTipRot;
    // public Vector3 Thumb_CMCPos,
    //  Thumb_MCPPos,
    //  Thumb_IPPos,
    //  Thumb_FingerTipPos,

    //  Index_MCPPos,
    //  Index_PIPPos,
    //  Index_DIPPos,
    //  Index_FingerTipPos,

    //  Middle_MCPPos,
    //  Middle_PIPPos,
    //  Middle_DIPPos,
    //  Middle_FingerTipPos,

    //  Ring_MCPPos,
    //  Ring_PIPPos,
    //  Ring_DIPPos,
    //  Ring_FingerTipPos,

    //  Pinky_MCPPos,
    //  Pinky_PIPPos,
    //  Pinky_DIPPos,
    //  Pinky_FingerTipPos;
}
public struct NetworkInputData : INetworkInput
{
    public HandJointInfo leftHand;
    public HandJointInfo rightHand;


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
// NetworkTransform
public class CharacterMovementHandler : NetworkBehaviour
{
    [System.Serializable]
    public struct HandJointTransformations
    {
        public Transform foreArm,wrist,
         Thumb_CMC,
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
    [SerializeField]
    private HandJointTransformations rightTransforms;
    [SerializeField]
    private HandJointTransformations leftTransforms;

    private void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void FixedUpdateNetwork()
    {
        //Get the input from the network
        if (GetInput(out NetworkInputData data))
        {

            leftTransforms.wrist.position=data.leftHand.wristPosistion;
            leftTransforms.wrist.rotation=data.leftHand.wristRotation;
                leftTransforms.Thumb_CMC.rotation=data.leftHand.Thumb_CMC;
                leftTransforms.Thumb_MCP.rotation=data.leftHand.Thumb_MCP;
                leftTransforms.Thumb_IP.rotation=data.leftHand.Thumb_IP;
                leftTransforms.Thumb_FingerTip.rotation=data.leftHand.Thumb_FingerTip;

                leftTransforms.Index_MCP.rotation=data.leftHand.Index_MCP;
                leftTransforms.Index_PIP.rotation=data.leftHand.Index_PIP;
                leftTransforms.Index_DIP.rotation=data.leftHand.Index_DIP;
                leftTransforms.Index_FingerTip.rotation=data.leftHand.Index_FingerTip;

                leftTransforms.Middle_MCP.rotation=data.leftHand.Middle_MCP;
                leftTransforms.Middle_PIP.rotation=data.leftHand.Middle_PIP;
                leftTransforms.Middle_DIP.rotation=data.leftHand.Middle_DIP;
                leftTransforms.Middle_FingerTip.rotation=data.leftHand.Middle_FingerTip;

                leftTransforms.Ring_MCP.rotation=data.leftHand.Ring_MCP;
                leftTransforms.Ring_PIP.rotation=data.leftHand.Ring_PIP;
                leftTransforms.Ring_DIP.rotation=data.leftHand.Ring_DIP;
                leftTransforms.Ring_FingerTip.rotation=data.leftHand.Ring_FingerTip;

                leftTransforms.Pinky_MCP.rotation=data.leftHand.Pinky_MCP;
                leftTransforms.Pinky_PIP.rotation=data.leftHand.Pinky_PIP;
                leftTransforms.Pinky_DIP.rotation=data.leftHand.Pinky_DIP;
                leftTransforms.Pinky_FingerTip.rotation=data.leftHand.Pinky_FingerTip;


            // rightTransforms.wrist.position=data.rightHand.wristPosistion;
            // rightTransforms.wrist.rotation=data.rightHand.wristRotation;
        }
    }

    public void SetViewInputVector(Vector2 viewInput)
    {
    }
}

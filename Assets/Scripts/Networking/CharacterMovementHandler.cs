using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
// NetworkTransform
public class CharacterMovementHandler : NetworkBehaviour
{
    [System.Serializable]
    public struct HandInfo
    {
        public SkinnedMeshRenderer skinnedMesh;
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
    private HandInfo rightTransforms;
    [SerializeField]
    private HandInfo leftTransforms;
    private void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        if(GetComponent<NetworkObject>().HasStateAuthority){
            leftTransforms.skinnedMesh.enabled=false;
            rightTransforms.skinnedMesh.enabled=false;
        }
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
                leftTransforms.Thumb_CMC.rotation=data.leftHand.Thumb_CMCRot;
                leftTransforms.Thumb_MCP.rotation=data.leftHand.Thumb_MCPRot;
                leftTransforms.Thumb_IP.rotation=data.leftHand.Thumb_IPRot;
                leftTransforms.Thumb_FingerTip.rotation=data.leftHand.Thumb_FingerTipRot;

                leftTransforms.Index_MCP.rotation=data.leftHand.Index_MCPRot;
                leftTransforms.Index_PIP.rotation=data.leftHand.Index_PIPRot;
                leftTransforms.Index_DIP.rotation=data.leftHand.Index_DIPRot;
                leftTransforms.Index_FingerTip.rotation=data.leftHand.Index_FingerTipRot;

                leftTransforms.Middle_MCP.rotation=data.leftHand.Middle_MCPRot;
                leftTransforms.Middle_PIP.rotation=data.leftHand.Middle_PIPRot;
                leftTransforms.Middle_DIP.rotation=data.leftHand.Middle_DIPRot;
                leftTransforms.Middle_FingerTip.rotation=data.leftHand.Middle_FingerTipRot;

                leftTransforms.Ring_MCP.rotation=data.leftHand.Ring_MCPRot;
                leftTransforms.Ring_PIP.rotation=data.leftHand.Ring_PIPRot;
                leftTransforms.Ring_DIP.rotation=data.leftHand.Ring_DIPRot;
                leftTransforms.Ring_FingerTip.rotation=data.leftHand.Ring_FingerTipRot;

                leftTransforms.Pinky_MCP.rotation=data.leftHand.Pinky_MCPRot;
                leftTransforms.Pinky_PIP.rotation=data.leftHand.Pinky_PIPRot;
                leftTransforms.Pinky_DIP.rotation=data.leftHand.Pinky_DIPRot;
                leftTransforms.Pinky_FingerTip.rotation=data.leftHand.Pinky_FingerTipRot;
            leftTransforms.Thumb_CMC.position = data.leftHand.Thumb_CMCPos;
            leftTransforms.Thumb_MCP.position = data.leftHand.Thumb_MCPPos;
            leftTransforms.Thumb_IP.position = data.leftHand.Thumb_IPPos;
            leftTransforms.Thumb_FingerTip.position = data.leftHand.Thumb_FingerTipPos;

            leftTransforms.Index_MCP.position = data.leftHand.Index_MCPPos;
            leftTransforms.Index_PIP.position = data.leftHand.Index_PIPPos;
            leftTransforms.Index_DIP.position = data.leftHand.Index_DIPPos;
            leftTransforms.Index_FingerTip.position = data.leftHand.Index_FingerTipPos;

            leftTransforms.Middle_MCP.position = data.leftHand.Middle_MCPPos;
            leftTransforms.Middle_PIP.position = data.leftHand.Middle_PIPPos;
            leftTransforms.Middle_DIP.position = data.leftHand.Middle_DIPPos;
            leftTransforms.Middle_FingerTip.position = data.leftHand.Middle_FingerTipPos;

            leftTransforms.Ring_MCP.position = data.leftHand.Ring_MCPPos;
            leftTransforms.Ring_PIP.position = data.leftHand.Ring_PIPPos;
            leftTransforms.Ring_DIP.position = data.leftHand.Ring_DIPPos;
            leftTransforms.Ring_FingerTip.position = data.leftHand.Ring_FingerTipPos;

            leftTransforms.Pinky_MCP.position = data.leftHand.Pinky_MCPPos;
            leftTransforms.Pinky_PIP.position = data.leftHand.Pinky_PIPPos;
            leftTransforms.Pinky_DIP.position = data.leftHand.Pinky_DIPPos;
            leftTransforms.Pinky_FingerTip.position = data.leftHand.Pinky_FingerTipPos;

            // rightTransforms.wrist.position=data.rightHand.wristPosistion;
            // rightTransforms.wrist.rotation=data.rightHand.wristRotation;
        }
    }

    public void SetViewInputVector(Vector2 viewInput)
    {
    }
}

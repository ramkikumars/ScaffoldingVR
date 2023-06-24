using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SG;
using Photon.Pun;
public class MimicHand : MonoBehaviour,IPunObservable
{
    public enum HandSide{
        Right,
        Left
    }
    [SerializeField] HandSide handSide;
    // Start is called before the first frame update
    /// <summary> The forearm of the hand model, usually the parent of the wrist transform. </summary>
    public Transform foreArmTransform;
    /// <summary> The transform of the wrist. Should be distinct from the foreArmTransform if wrist animation is not required. </summary>
    public Transform wristTransform;

    /// <summary> The thumb joint transforms, preferably including the fingertip. < /summary>
    public Transform[] thumbJoints = new Transform[0];
    /// <summary> The index joint transforms, preferably including the fingertip. </summary>
    public Transform[] indexJoints = new Transform[0];
    /// <summary> The middle joint transforms, preferably including the fingertip. </summary>
    public Transform[] middleJoints = new Transform[0];
    /// <summary> The ring joint transforms, preferably including the fingertip. </summary>
    public Transform[] ringJoints = new Transform[0];
    /// <summary> The pinky joint transforms, preferably including the fingertip. </summary>
    public Transform[] pinkyJoints = new Transform[0];

    private SG_User sgUser;
    private Quaternion[][] jointRotations;
    private Vector3[][] jointPositions;
    private Transform[][] jointTransforms;
    Transform[][] res = new Transform[5][];
    private SG_TrackedHand trackedHand;
    public PhotonView photonView;
    public GameObject mesh;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(trackedHand.handAnimation.handModelInfo.wristTransform.rotation);
            stream.SendNext(trackedHand.handAnimation.handModelInfo.wristTransform.position);
            // jointRotations = User.leftHand.
            for (int f = 0; f < 5; f++)
            {
                for (int j = 0; j < 3; j++)
                {
                    stream.SendNext(jointTransforms[f][j].rotation);
                }
            }
        }
        else if(stream.IsReading)
        {
            wristTransform.rotation=(Quaternion)stream.ReceiveNext();
            wristTransform.position=(Vector3)stream.ReceiveNext();
            for (int f = 0; f < 5; f++)
            {
                for (int j = 0; j < 3; j++)
                {
                    res[f][j].rotation = (Quaternion)stream.ReceiveNext();
                }
            }
        }
    }

    void Start()
    {
        sgUser=GameObject.Find("[SG_User]").GetComponent<SG_User>();
        // photonView=GetComponent<PhotonView>();
        switch (handSide)
        {
            case HandSide.Left:
            trackedHand=sgUser.leftHand;
            break;

            case HandSide.Right:
            trackedHand=sgUser.rightHand;
            break;

        }
        res[0] = thumbJoints;
        res[1] = indexJoints;
        res[2] = middleJoints;
        res[3] = ringJoints;
        res[4] = pinkyJoints;
        if (photonView.IsMine)
        {
            mesh.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {



    }
}

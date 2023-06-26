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
    float currentTime = 0;
    double currentPacketTime = 0;
    double lastPacketTime = 0;

    Vector3 wristPositionAtLastPacket = Vector3.zero;
    Quaternion wristRotationAtLastPacket = Quaternion.identity;
    Quaternion[][] jointRotationAtLastPacket = new Quaternion[5][];

    Vector3 wristPosistionLatest=Vector3.zero;
    Quaternion wristRotationLatest=Quaternion.identity;
    Quaternion[][] jointRotationLatest = new Quaternion[5][];
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            jointTransforms = trackedHand.handModel.FingerJoints;
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
            wristRotationLatest=(Quaternion)stream.ReceiveNext();
            wristPosistionLatest=(Vector3)stream.ReceiveNext();
            for (int f = 0; f < 5; f++)
            {
                for (int j = 0; j < 3; j++)
                {
                    jointRotationLatest[f][j] = (Quaternion)stream.ReceiveNext();
                    currentTime = 0.0f;
                    lastPacketTime = currentPacketTime;
                    currentPacketTime = info.SentServerTime;
                    jointRotationAtLastPacket[f][j]=res[f][j].rotation;
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

        if (!photonView.IsMine)
        {
            //Lag compensation
            double timeToReachGoal = currentPacketTime - lastPacketTime;
            currentTime += Time.deltaTime;

            //Update remote player
            wristTransform.rotation=Quaternion.Lerp(wristRotationAtLastPacket, wristRotationLatest, (float)(currentTime / timeToReachGoal));
            wristTransform.position=Vector3.Lerp(wristPositionAtLastPacket, wristPosistionLatest, (float)(currentTime / timeToReachGoal));
            for (int f = 0; f < 5; f++)
            {
                for (int j = 0; j < 3; j++)
                {
                    res[f][j].rotation=Quaternion.Lerp(jointRotationAtLastPacket[f][j],jointRotationLatest[f][j],(float)(currentTime / timeToReachGoal));
                }
            }
        }
    }

    }

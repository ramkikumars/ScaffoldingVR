using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UltimateXR.Manipulation;
using SG;
using UltimateXR.Avatar;
using UltimateXR.Haptics;
public class FollowMe : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject[] followers = new GameObject[3];
    [SerializeField] private GameObject Grab1, Grab2, wingNut;
    private UxrGrabbableObject UxrGrabObj1, UxrGrabObj2;
    private SG_SimpleDrawer SgGrabObj1, SgGrabObj2;
    // private Transform followerParent;
    private Vector3 initialPos, initialPosVert;
    private Vector3[] initialFollowerPos;
    private Transform grabbedobj;
    private Transform[] followersParent;
    [SerializeField] private int n = 1;
    void Start()
    {
        UxrGrabObj1 = Grab1.GetComponent<UxrGrabbableObject>();
        SgGrabObj1 = Grab1.GetComponent<SG_SimpleDrawer>();
        UxrGrabObj2 = Grab2.GetComponent<UxrGrabbableObject>();
        SgGrabObj2 = Grab2.GetComponent<SG_SimpleDrawer>();
        AddMainpulationEvents(Grab1.transform);
        AddMainpulationEvents(Grab2.transform);
        initialPos = transform.position;
        // n = followers.Length;
        // n = 1;
        initialFollowerPos = new Vector3[n];
        followersParent = new Transform[n];
    }

    // Update is called once per frame
    void Update()
    {
        if ((!UxrGrabObj1.IsBeingGrabbed && !SgGrabObj1.IsGrabbed()) && (!UxrGrabObj2.IsBeingGrabbed && !SgGrabObj2.IsGrabbed()))
        {

            Vector3 pos = transform.position;
            Vector3 pos1 = followers[0].transform.TransformPoint(0, 0.2f, 0);
            pos.y = pos1.y;
            this.transform.position = pos;
        }
        if (UxrGrabObj1.IsBeingGrabbed || SgGrabObj1.IsGrabbed() || UxrGrabObj2.IsBeingGrabbed || SgGrabObj2.IsGrabbed())
        {
            ChangeLimit();
        }

    }
    private void ObjGrabbed(object obj1, object obj2)
    {
        initialPos = transform.position;
        initialPosVert = followers[0].transform.position;
        // Object obj = new Object();
        if (obj1.GetType() == typeof(UxrGrabbableObject))
        {
            UxrGrabbableObject obj = obj1 as UxrGrabbableObject;
            grabbedobj = obj.transform;
        }
        else if (obj1.GetType() == typeof(SG_SimpleDrawer))
        {
            SG_SimpleDrawer obj = obj1 as SG_SimpleDrawer;
            grabbedobj = obj.transform;
        }
        for (int i = 0; i < n; i++)
        {
            followersParent[i] = followers[i].transform.parent;
            // Debug.Log($"Vertical Parent Before Grabbing: {followersParent[i].name}");
            followers[i].transform.SetParent(grabbedobj);
            initialFollowerPos[i] = followers[i].transform.position;
            if (followers[i].GetComponent<Rigidbody>() != null)
            {
                followers[i].GetComponent<Rigidbody>().isKinematic = true;
            }
        }
        // initialFollowerPos[1] = followers[1].transform.position;
        // followers[1].GetComponent<Rigidbody>().isKinematic = true;
    }

    private void ObjReleased(object obj1, object obj2)
    {

        followers[0].transform.SetParent(followersParent[0]);
        // Debug.Log($"Vertical Parent After Releasing: {followers[0].transform.parent.name}");
        Vector3 tempPos = grabbedobj.transform.localPosition;
        tempPos.y = 0;
        grabbedobj.transform.localPosition = tempPos;
        for (int i = 0; i < n; i++)
        {
            followers[i].transform.SetParent(followersParent[i]);
            // if (followers[i].GetComponent<Rigidbody>() != null)
            // {
            //     followers[i].GetComponent<Rigidbody>().isKinematic = false;
            // }
            // this.transform.SetParent(followers[i].transform);
        }


    }
    private void ChangeLimit()
    {
        float diff = Vector3.Distance(initialPosVert, wingNut.transform.position);
        UxrGrabObj1._translationLimitsMin.y = -1f * (diff);
        UxrGrabObj2._translationLimitsMin.y = -1f * (diff);
        SgGrabObj1.pushDistance = -1f * (diff);
        SgGrabObj2.pushDistance = -1f * (diff);
    }
    private void AddMainpulationEvents(Transform obj)
    {

        // obj.GetComponent<UxrGrabbableObject>().Grabbed += ObjGrabbed;
        obj.GetComponent<UxrGrabbableObject>().Grabbed += ObjGrabbed;
        obj.GetComponent<UxrGrabbableObject>().Released += ObjReleased;
        obj.GetComponent<SG_SimpleDrawer>().ObjectGrabbed.AddListener(ObjGrabbed);
        obj.GetComponent<SG_SimpleDrawer>().ObjectReleased.AddListener(ObjReleased);
    }
}

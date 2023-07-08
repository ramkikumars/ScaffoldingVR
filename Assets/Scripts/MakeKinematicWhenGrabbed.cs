using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UltimateXR.Manipulation;
using SG;

public class MakeKinematicWhenGrabbed : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        ListenManipulationEvents(transform.GetChild(0));
    }

    private void ListenManipulationEvents(Transform obj)
    {
        obj.GetComponent<UxrGrabbableObject>().Grabbed += ObjGrabbed;
        obj.GetComponent<UxrGrabbableObject>().Released += ObjReleased;
        if (obj.GetComponent<SG_SimpleDrawer>() != null)
        {
            obj.GetComponent<SG_SimpleDrawer>().ObjectGrabbed.AddListener(ObjGrabbed);
            obj.GetComponent<SG_SimpleDrawer>().ObjectReleased.AddListener(ObjReleased);
        }
        else if (obj.GetComponent<SG_Rotater>() != null)
        {
            obj.GetComponent<SG_Rotater>().ObjectGrabbed.AddListener(ObjGrabbed);
            obj.GetComponent<SG_Rotater>().ObjectReleased.AddListener(ObjReleased);
        }
        else if (obj.GetComponent<SG_Grabable>() != null)
        {
            obj.GetComponent<SG_Grabable>().ObjectGrabbed.AddListener(ObjGrabbed);
            obj.GetComponent<SG_Grabable>().ObjectReleased.AddListener(ObjReleased);
        }

    }
    private void ObjGrabbed(object obj1, object obj2)
    {
        this.GetComponent<Rigidbody>().isKinematic = true;
    }


    private void ObjReleased(object obj1, object obj2)
    {
        this.GetComponent<Rigidbody>().isKinematic = false;
    }


}

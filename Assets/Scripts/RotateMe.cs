using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UltimateXR.Manipulation;
public class RotateMe : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject grab1, grab2, parentGrab1, parentGrab2;
    // [SerializeField] private  obj1, obj2;
    private UxrGrabbableObject uxrObj1, uxrObj2;
    void Start()
    {
        uxrObj1 = grab1.GetComponent<UxrGrabbableObject>();
        uxrObj2 = grab2.GetComponent<UxrGrabbableObject>();
        // uxrObj1.Grabbed += ObjGrabbed;
        // uxrObj1.Released += ObjReleased;
        // uxrObj2.Grabbed += ObjGrabbed;
        // uxrObj2.Released += ObjReleased;
    }

    // Update is called once per frame
    void Update()
    {
        if (uxrObj1.IsBeingGrabbed)
        {
            transform.localRotation = grab1.transform.localRotation;
            parentGrab2.transform.localRotation = grab1.transform.localRotation;
        }
        else if (uxrObj2.IsBeingGrabbed)
        {
            transform.localRotation = grab2.transform.localRotation;
            parentGrab1.transform.localRotation = grab2.transform.localRotation;
        }
    }
    // private void ObjGrabbed(object obj1, object obj2)
    // {

    // }
    // private void ObjReleased(object obj1, object obj2)
    // {

    // }
}

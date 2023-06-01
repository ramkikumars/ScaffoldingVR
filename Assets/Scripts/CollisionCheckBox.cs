using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UltimateXR.Manipulation;
public class CollisionCheckBox : MonoBehaviour
{
    // Start is called before the first frame update
    [System.NonSerialized]
    public bool collidedWithBox=false;
    [System.NonSerialized]
    public bool releaseTheObject = false;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.name=="Box"&&other.GetComponent<UxrGrabbableObject>().IsBeingGrabbed){
            // other.GetComponent<UxrGrabbableObject>().IsLockedInPlace=true;
            other.GetComponent<UxrGrabbableObject>().ReleaseGrabs(true);
            // other.transform.eulerAngles = new Vector3(0, 90, -90);
            other.transform.rotation=transform.rotation;
            other.transform.position=transform.position;
            other.transform.eulerAngles=new Vector3(transform.eulerAngles.x,transform.eulerAngles.y,-90);
            collidedWithBox=true;
        releaseTheObject = false;
        }

    }

    // private void OnTriggerStay(Collider other){
    //     if (other.name == "Box" && !releaseTheObject)
    //     {
    //         other.transform.eulerAngles = new Vector3(0, 90, -90);
    //         other.transform.position = transform.position;
    //     }
    // }
    private void OnTriggerExit(Collider other){
        collidedWithBox = false;
    }
}

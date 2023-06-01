using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UltimateXR.Manipulation;
public class CollisionCheckHolder : MonoBehaviour
{
    // Start is called before the first frame update
    [System.NonSerialized]
    public bool collidedWithHolder=false;
    [System.NonSerialized]
    public bool releaseTheObject=false;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Holder" && other.GetComponent<UxrGrabbableObject>().IsBeingGrabbed)
        {
            other.GetComponent<UxrGrabbableObject>().IsLockedInPlace=true;
            other.transform.position=new Vector3(other.transform.position.x,transform.position.y,other.transform.position.z);
            collidedWithHolder = true;
            releaseTheObject=false;
        }
    }
    // private void OnTriggerStay(Collider other)
    // {
    //     if (other.name == "Holder" && !releaseTheObject)
    //     {
    //         // other.transform.eulerAngles = new Vector3(0, 90, -90);
    //         other.transform.position = transform.position;
    //     }
    // }
    private void OnTriggerExit(Collider other)
    {
        collidedWithHolder = false;
    }

}

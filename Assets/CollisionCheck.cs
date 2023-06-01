using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UltimateXR.Manipulation;
public class CollisionCheck : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        // Debug.Log("Collide Object with Plane: "+other.name);
        if (other.name == "Box"&&other.GetComponent<UxrGrabbableObject>().IsBeingGrabbed)
        {
            other.GetComponent<UxrGrabbableObject>().IsLockedInPlace=true;
            // other.transform.rotation = transform.rotation;
        }
    }

    private void OnTriggerStay(Collider other){
        if (other.name == "Box" && other.GetComponent<UxrGrabbableObject>().IsBeingGrabbed)
        {
            other.transform.eulerAngles=new Vector3(0,90,-90);
            other.transform.position = transform.position;
            // other.transform.LookAt(transform.position, new Vector3(0, 1, 0));


        }
    }
}

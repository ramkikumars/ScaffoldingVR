using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hammer : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject follower;
    [SerializeField] private Transform headSnap;
    private Vector3 objPos;
    private Quaternion objRot;
    void Start()
    {

    }

    // Update is called once per frame
    private void FixedUpdate()
    {

        objPos = headSnap.transform.position;
        objRot = this.transform.rotation;
        follower.GetComponent<Rigidbody>().MovePosition(objPos);
        follower.GetComponent<Rigidbody>().MoveRotation(objRot);
    }
}

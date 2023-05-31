using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SG;
public class Tester : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject sgRot;
    void Start()
    {
        // sgRot.GetComponent<SG_Grabable>().ObjectGrabbed.AddListener(objgrab);
        sgRot.GetComponent<SG_Rotater>().ObjectGrabbed.AddListener(objgrab);
    }

    // Update is called once per frame
    void Update()
    {


    }
    private void objgrab(Object obj1, Object obj2)
    {
        Debug.Log("Grab Working");
    }
}

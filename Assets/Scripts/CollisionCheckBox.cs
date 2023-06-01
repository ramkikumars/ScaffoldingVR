using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UltimateXR.Manipulation;
public class CollisionCheckBox : MonoBehaviour
{
    // Start is called before the first frame update
    [System.NonSerialized]
    public bool collidedWithBox=false;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        collidedWithBox=true;
    }

    private void OnTriggerStay(Collider other){

    }
    private void OnTriggerExit(Collider other){
        collidedWithBox = false;
    }
}

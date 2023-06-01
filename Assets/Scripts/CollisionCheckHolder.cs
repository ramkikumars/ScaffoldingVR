using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UltimateXR.Manipulation;
public class CollisionCheckHolder : MonoBehaviour
{
    // Start is called before the first frame update
    [System.NonSerialized]
    public bool collidedWithHolder=false;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        collidedWithHolder=true;
    }

}

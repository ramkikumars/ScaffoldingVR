using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class horizontalscript : MonoBehaviour
{
    public GameObject cup1;
    public GameObject cup2;
    public GameObject cylinder;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // calculate the midpoint between another game object
        Vector3 midpoint = (cup1.transform.position + cup2.transform.position) / 2;
        // set the transform of the cylinder s center to the midpoint
        cylinder.transform.position = midpoint;
        // now when we move the cups, the cylinder will move with them
        // calculate the angle that the cups make and set the rotation of the cylinder to match
        // Vector3 direction = cup1.transform.position - cup2.transform.position;
        // cylinder.transform.rotation = Quaternion.LookRotation(direction);
        
    }
}

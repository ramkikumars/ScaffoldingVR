using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MEASURINGTAPE : MonoBehaviour
{
    public GameObject plane;
    public GameObject box;
    // make an array to store plane1 plane 2...
    public GameObject[] planes;
    
    void Start()
    {

    }


    void Update()
    {

        for (int i = 0; i < planes.Length; i++)
        {
            print(planes[0].transform.position);
            if (planes[i].transform.position.x >= 0.17)
            {
                planes[i].SetActive(true);
            }
            else
            {
                planes[i].SetActive(false);
            }

        }
    }
    public float Distance(GameObject plane, GameObject box)
    {
        return Vector3.Distance(plane.transform.position, box.transform.position);
    }
}

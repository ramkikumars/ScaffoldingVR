using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeCentreofMass : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject com;
    [SerializeField] private bool ResetCom, changeCOM;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (changeCOM)
        {
            GetComponent<Rigidbody>().centerOfMass = com.transform.localPosition;
        }
        if (ResetCom)
        {
            GetComponent<Rigidbody>().ResetCenterOfMass();
            ResetCom = false;
        }

    }
}

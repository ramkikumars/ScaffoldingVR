using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UltimateXR.Manipulation;
using SG;
using UltimateXR.Avatar;
using UltimateXR.Haptics;
public class Lift : MonoBehaviour
{
    // Start is called before the first frame update
    // [SerializeField] private GameObject followObj;
    [SerializeField] private GameObject verticalObj, bottomParent;
    Vector3 iniBottomLedgerPos;
    Vector3 iniverticalObjPos;
    private UxrGrabbableObject UxrBottomLedgerGrabbable;
    private SG_SimpleDrawer SgBottomLedgerGrabbable;
    LayerMask mask;
    RaycastHit hit1, hit2;
    void Start()
    {
        // mask = LayerMask.GetMask("bottom horizontal ledger");
        mask = ~mask;
        UxrBottomLedgerGrabbable = this.GetComponent<UxrGrabbableObject>();
        SgBottomLedgerGrabbable = this.GetComponent<SG_SimpleDrawer>();
        UxrBottomLedgerGrabbable.Grabbed += bottomLedgerGrabbed;
        UxrBottomLedgerGrabbable.Released += bottomLedgerReleased;
        SgBottomLedgerGrabbable.ObjectGrabbed.AddListener(bottomLedgerGrabbed);
        SgBottomLedgerGrabbable.ObjectReleased.AddListener(bottomLedgerReleased);
        // iniBottomLedgerPos = transform.position;
        // iniverticalObjPos = verticalObj.transform.position;
        // FindNearestVerical();
    }

    // Update is called once per frame
    void Update()
    {
        if (UxrBottomLedgerGrabbable.IsBeingGrabbed || SgBottomLedgerGrabbable.IsGrabbed())
        {
            float dist = transform.position.y - iniBottomLedgerPos.y;
            // verticalObj.transform.position = iniverticalObjPos + new Vector3(0, dist, 0);
            verticalObj.GetComponent<Rigidbody>().MovePosition(iniverticalObjPos + new Vector3(0, dist, 0));
        }

    }
    void bottomLedgerGrabbed(object obj1, object obj2)
    {
        iniBottomLedgerPos = transform.position;
        iniverticalObjPos = verticalObj.transform.position;
        bottomParent.GetComponent<Rigidbody>().isKinematic = true;
        verticalObj.GetComponent<Rigidbody>().useGravity = false;
        // verticalObj.GetComponent<Rigidbody>().isKinematic = true;

    }
    void bottomLedgerReleased(object obj1, object obj2)
    {
        bottomParent.GetComponent<Rigidbody>().isKinematic = false;
        verticalObj.GetComponent<Rigidbody>().useGravity = true;

    }
    // void FixedUpdate()
    // {
    //     RaycastHit hit;
    //     Vector3 pos = transform.position;
    //     pos.y += 0.1f;
    //     if (Physics.Raycast(pos, transform.TransformDirection(Vector3.left), out hit, 100.0f, mask))
    //     {
    //         print("Found an object - name: " + hit.transform.name);
    //         hit.transform.GetComponent<Renderer>().material.color = Color.blue;
    //     }

    // }

    void FindNearestVerical()
    {
        Vector3 pos = new Vector3(0, 0.43f, 0.08f);
        pos.y += 0.1f;
        Physics.Raycast(pos, transform.TransformDirection(Vector3.left), out hit1, 100.0f, mask);
        Physics.Raycast(pos, transform.TransformDirection(Vector3.right), out hit2, 100.0f, mask);
        float dist1 = Vector3.Distance(pos, hit1.transform.position);
        float dist2 = Vector3.Distance(pos, hit2.transform.position);
        if (dist1 < dist2)
        {
            hit1.transform.GetComponent<Renderer>().material.color = Color.blue;
            Debug.Log("dist1");
        }
        else if (dist2 < dist1)
        {

            hit2.transform.GetComponent<Renderer>().material.color = Color.blue;
            Debug.Log("dist1");
        }
    }

}

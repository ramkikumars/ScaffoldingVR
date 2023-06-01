using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UltimateXR.Manipulation;
using SG;
using TMPro;
public class RotationConstraint : MonoBehaviour
{
    // Start is called before the first frame update
    private UxrGrabbableObject uxrObj;
    private SG_Rotater sgObj;
    private Vector3 grabbedPos, localGrabbedPos;
    private string lastGrabbed, currentGrabbed;
    private float startAngle, endAngle, diff;
    public bool allowChange;
    // [SerializeField] private GameObject ball;
    // [SerializeField] private TextMeshProUGUI dText;
    void Start()
    {
        uxrObj = this.GetComponent<UxrGrabbableObject>();
        sgObj = this.GetComponent<SG_Rotater>();
        AddMainpulationEvents(this.transform);
        lastGrabbed = "snap 2";
        diff = 180f;
    }

    // Update is called once per frame
    void Update()
    {
        endAngle = this.transform.rotation.eulerAngles.y;
        // // Debug.Log($"Angle at releasing:{endAngle}");
        diff = Mathf.Abs(startAngle - endAngle);
        // dText.text = $"Diff: {diff}\nAllow Change{allowChange}";
        // // allowChange = Mathf.Approximately(diff, 180);
    }
    private void AddMainpulationEvents(Transform obj)
    {
        obj.GetComponent<UxrGrabbableObject>().Grabbed += ObjGrabbed1;
        obj.GetComponent<UxrGrabbableObject>().Released += ObjReleased;
        obj.GetComponent<SG_Rotater>().ObjectGrabbed.AddListener(ObjGrabbed1);
        obj.GetComponent<SG_Rotater>().ObjectReleased.AddListener(ObjReleased);
    }
    public void ObjGrabbed1(object obj1, object obj2)
    {
        // allowChange = Mathf.Approximately(diff, 180);
        if (obj1.GetType() == typeof(UxrGrabbableObject))
        {
            UxrGrabbableObject asobj1 = obj1 as UxrGrabbableObject;
            UxrManipulationEventArgs asobj2 = obj2 as UxrManipulationEventArgs;
            grabbedPos = asobj2.Grabber.transform.position;
            // ball.transform.position = obj.Grabber.transform.position;
            localGrabbedPos = transform.InverseTransformPoint(grabbedPos);
            if (localGrabbedPos.x < 0 && allowChange)
            {
                currentGrabbed = "snap1";
                if (allowChange)
                {
                    asobj1._rotationAngleLimitsMin.y = 180f;
                    asobj1._rotationAngleLimitsMax.y = 360f;
                    allowChange = false;
                }
                // Debu
            }
            else if (localGrabbedPos.x > 0 && allowChange)
            {
                currentGrabbed = "snap2";
                if (allowChange)
                {
                    asobj1._rotationAngleLimitsMin.y = 0f;
                    asobj1._rotationAngleLimitsMax.y = 180f;
                    allowChange = false;
                }
            }

        }
        else if (obj1.GetType() == typeof(SG_Rotater))
        {
            SG_Rotater asobj1 = obj1 as SG_Rotater;
            SG_GrabScript asobj2 = obj2 as SG_GrabScript;
            grabbedPos = asobj2.realGrabRefrence.position;
            // ball.transform.position = grabbedPos;
            localGrabbedPos = transform.InverseTransformPoint(grabbedPos);
            if (localGrabbedPos.x < 0)// && allowChange)
            {
                currentGrabbed = "snap1";
                if (allowChange)
                {
                    asobj1._rotationAngleLimitsMin.y = 180f;
                    asobj1._rotationAngleLimitsMax.y = 360f;
                    allowChange = false;
                }
                // Debug.Log("snap");
                // dText.text = "snap2";
            }
            else if (localGrabbedPos.x > 0)// && allowChange)
            {
                currentGrabbed = "snap2";
                if (allowChange)
                {
                    asobj1._rotationAngleLimitsMin.y = 0f;
                    asobj1._rotationAngleLimitsMax.y = 180f;
                    allowChange = false;
                }
                // dText.text = "snap1";
                // Debug.Log("snap1");
            }
        }
        if (currentGrabbed != lastGrabbed)
        {
            startAngle = this.transform.rotation.eulerAngles.y;
            Debug.Log($"Angle at first grab:{startAngle}");
            lastGrabbed = currentGrabbed;
        }
        // startAngle = this.transform.rotation.eulerAngles.y;
    }
    private void ObjReleased(object obj1, object obj2)
    {
        endAngle = this.transform.rotation.eulerAngles.y;
        Debug.Log($"Angle at releasing:{endAngle}");
        diff = Mathf.Abs(startAngle - endAngle);
        diff = Mathf.RoundToInt(diff);
        // allowChange = Mathf.Approximately(diff, 180) || Mathf.Approximately(diff, 0) || Mathf.Approximately(diff, 360);
        if (diff == 180 || diff == 0)
        {
            allowChange = true;
        }
    }
}

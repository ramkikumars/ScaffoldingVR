using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UltimateXR.Manipulation;
using SG;
public class RotateObjOnTriggerEnter : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private float minStepAngle = 5f;
    [SerializeField] private float maxStepAngle = 10f;
    [SerializeField] private float maxAngleToRotate = 50f;
    [SerializeField] private string targetTag = "Hammer";
    [SerializeField] private bool useVelocity = true;
    [SerializeField] private float minVelocity = 0;
    [SerializeField] private float maxVelocity = 2f;
    private GameObject hammer, childCup;
    private bool allowRotation = true;
    private float cumilativeAngle;
    public bool reachedLimit;
    private RigidbodyConstraints initialConstraints;
    void Start()
    {
        cumilativeAngle = 0;
        childCup = transform.GetChild(0).gameObject;
        hammer = GameObject.Find("Hammer");
        ListenManipulationEvents(hammer.transform);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag(targetTag))
        {
            VelocityEstimator estimator = other.gameObject.GetComponent<VelocityEstimator>();
            if (estimator && useVelocity)
            {
                float v = estimator.GetVelocityEstimate().magnitude;
                float value = Mathf.InverseLerp(minVelocity, maxVelocity, v);
                float angle = Mathf.Lerp(minStepAngle, maxStepAngle, value);
                cumilativeAngle += angle;
                Quaternion quatRot = new Quaternion();
                Vector3 eulerRot = childCup.transform.eulerAngles;
                eulerRot = childCup.transform.eulerAngles;
                eulerRot.y = cumilativeAngle;
                quatRot.eulerAngles = eulerRot;
                Debug.Log($"Cumilative Angle: {cumilativeAngle}");
                if (cumilativeAngle <= maxAngleToRotate)
                {
                    transform.GetComponent<Rigidbody>().MoveRotation(quatRot);
                    // childCup.transform.eulerAngles = rot;
                    reachedLimit = false;
                }
                else
                {
                    Debug.Log("Cup reached the max limit");
                    reachedLimit = true;
                }

            }

        }
    }

    private void HammerGrabbed(object obj1, object obj2)
    {
        initialConstraints = GetComponent<Rigidbody>().constraints;
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        GetComponent<Rigidbody>().isKinematic = false;
    }
    private void HammerReleased(object obj1, object obj2)
    {
        GetComponent<Rigidbody>().constraints = initialConstraints;
        GetComponent<Rigidbody>().isKinematic = true;
    }
    private void ListenManipulationEvents(Transform obj)
    {
        obj.GetComponent<UxrGrabbableObject>().Grabbed += HammerGrabbed;
        obj.GetComponent<UxrGrabbableObject>().Released += HammerReleased;
        obj.GetComponent<SG_Grabable>().ObjectGrabbed.AddListener(HammerGrabbed);
        obj.GetComponent<SG_Grabable>().ObjectReleased.AddListener(HammerReleased);
    }
}

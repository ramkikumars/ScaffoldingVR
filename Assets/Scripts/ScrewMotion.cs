using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Interhaptics.Utils;

using UltimateXR.Manipulation;

public class ScrewMotion : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject rotateObj, translateObj;
    [SerializeField] float pitch = 1;
    [SerializeField] private float duration;
    [SerializeField] private VelocityEstimator velocityEstimator;
    [SerializeField] private AudioHapticSource rotatingHaptics;
    [SerializeField] private AudioHapticSource squeakHaptics;
    private Vector3 tempVect, newPos;
    private float dist, unit_dist, intitialAngle, lastAngle, CurrentAngle, prevAngle, velocity, random;
    public bool getSmoothedVelocity = true;

    float lerpTime = 1;
    private UxrGrabbableObject grabObj => GetComponentInChildren<UxrGrabbableObject>();

    void Start()
    {

        intitialAngle = rotateObj.transform.localRotation.eulerAngles.y;
        lastAngle = intitialAngle;
        prevAngle = intitialAngle;
        unit_dist = pitch / 360f;

        // rb = GetComponent<Rigidbody>();
        grabObj.ConstraintsApplied += speedChange;


    }

    // Update is called once per frame
    void Update()
    {
        // if(rotateObj.Grabbed)
        // {
        //     translateObj.GetComponent<AudioSource>().Play();
        // }


        
        if (grabObj.IsBeingGrabbed)
        {
            Vector3 angularVelocity = UxrGrabManager.Instance.GetGrabbedObjectAngularVelocity(grabObj, getSmoothedVelocity);
            // SET IS KINEMATIC TO FALSE
            Debug.Log(angularVelocity.y);
            // if rounded abosolute value of angular velocity is less than 0.5, stop the audio


            if (Mathf.RoundToInt(Mathf.Abs(angularVelocity.y)) < 20)
            {
                Debug.Log("STOP");
                rotatingHaptics.Stop();
            }
            else if (!rotatingHaptics.audioSource.isPlaying)
            {
                rotatingHaptics.Play();
            }
        }
        if (rotateObj.transform.hasChanged)
        {

            // if (!rotatingHaptics.audioSource.isPlaying)
            // {
            //     rotatingHaptics.Play();
            // }
            CurrentAngle = Mathf.Round(rotateObj.transform.localRotation.eulerAngles.y);

            if (Mathf.Abs(CurrentAngle - lastAngle) >= 2)
            {
                tempVect.y = 2f * unit_dist;
                if (lastAngle > CurrentAngle)
                {
                    tempVect = -1 * tempVect;
                }
                newPos = translateObj.transform.localPosition + tempVect;

                translateObj.transform.localPosition = Vector3.Lerp(translateObj.transform.localPosition, newPos, lerpTime * Time.deltaTime);
                lastAngle = CurrentAngle;

            }
            // make a randomized call for random unlock but no two calls should be made within 5 seconds
            // if (Mathf.Abs(CurrentAngle - prevAngle) >= 50)
            // {
            //     random = Random.Range(0, 100);
            //     if (random > 95)
            //     {
            //         StartCoroutine(WaitAndUnlock(1f));
            //         prevAngle = CurrentAngle;
            //     }
            // }


            rotateObj.transform.hasChanged = false;

        }
        else
        {
            rotatingHaptics.Stop();
        }
    }
    private IEnumerator WaitAndUnlock(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        grabObj.IsLockedInPlace = true;
        squeakHaptics.Play();
        yield return new WaitForSeconds(seconds);
        squeakHaptics.Stop();
        grabObj.IsLockedInPlace = false;
        // lastAngle = CurrentAngle;
    }
    private void speedChange(object sender, UxrApplyConstraintsEventArgs e)
    {
        rotateObj.transform.eulerAngles.Set(0, 0, 0);
    }
}

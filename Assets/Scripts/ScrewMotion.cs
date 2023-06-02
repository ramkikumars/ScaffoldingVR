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
    float lerpTime = 1;
    private UxrGrabbableObject grabObj => GetComponentInChildren<UxrGrabbableObject>();
    public Rigidbody rb;
    void Start()
    {

        intitialAngle = rotateObj.transform.localRotation.eulerAngles.y;
        lastAngle = intitialAngle;
        prevAngle = intitialAngle;
        unit_dist = pitch / 360f;
        rb = GetComponent<Rigidbody>();
        
        

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
            velocity = velocityEstimator.GetAngularVelocityEstimate().magnitude;
            velocity = Mathf.Round(velocity * 100f) / 100f;
            Debug.Log("Ang Velocity:" + velocity);
            // SET IS KINEMATIC TO FALSE 
            rb.isKinematic = false;
            // if (velocity == 0)
            // {
            //     rotatingHaptics.Stop();
            // }
            // else if (!rotatingHaptics.audioSource.isPlaying){
            //     rotatingHaptics.Play();
            // }
        }
        else
        {
            // SET IS KINEMATIC TO TRUE
            rb.isKinematic = true;
            rotatingHaptics.Stop();
        }
        if (rotateObj.transform.hasChanged)
        {

            if (velocity <= 0.01)
            {
                rotatingHaptics.Stop();
            }
            else if (!rotatingHaptics.audioSource.isPlaying)

                rotatingHaptics.Play();
            if (rb.angularVelocity.magnitude < 5)
                Debug.Log(rb.angularVelocity.magnitude);

            // if (!squeakHaptics.audioSource.isPlaying)
            // {
            //     random = Random.Range(1, 10);
            //     if (random >= 5)
            //         squeakHaptics.Play();
            //     // rotatingHaptics.Play();
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

            // if (Mathf.Abs(CurrentAngle - prevAngle) >= 20)
            // {
            //     StartCoroutine(WaitAndUnlock(duration));
            //     prevAngle = CurrentAngle;
            // }

            // if(Random.Range(1, 10)>=5){

            //     StartCoroutine(WaitAndUnlock(duration));
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
}

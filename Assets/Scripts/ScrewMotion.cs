using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Interhaptics.Internal;

using UltimateXR.Manipulation;

public class ScrewMotion : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject rotateObj, translateObj;
    [SerializeField] float pitch = 1;
    [SerializeField] private float duration;
    [SerializeField] private VelocityEstimator velocityEstimator;

    // [SerializeField] private EventHapticSource rotatingHaptics;
    [SerializeField] private AudioSource rotatingAudio;
    // [SerializeField] private AudioSource squeakAudio;
    // [SerializeField] private EventHapticSource squeakHaptics;
    [SerializeField] private float speed;
    private Vector3 tempVect, newPos,prevAngle2;
    private float dist, unit_dist, intitialAngle, lastAngle, CurrentAngle, prevAngle,velocity,random;
    float lerpTime = 1;
    private UxrGrabbableObject grabObj => GetComponentInChildren<UxrGrabbableObject>();
    // private Quaternion prevAngle1;
    void Start()
    {

        intitialAngle = rotateObj.transform.localRotation.eulerAngles.y;
        lastAngle = intitialAngle;
        prevAngle = intitialAngle;
        unit_dist = pitch / 360f;

        // rb = GetComponent<Rigidbody>();

        grabObj.ConstraintsApplied+=speedChange;
        grabObj.Grabbed+=OnNutGrabbed;
        grabObj.Released+=OnNutReleased;

    }

    // Update is called once per frame
    void Update()
    {
        // if(rotateObj.Grabbed)
        // {
        //     translateObj.GetComponent<AudioSource>().Play();
        // }



        // if (grabObj.IsBeingGrabbed)
        // {
        //     Vector3 angularVelocity = UxrGrabManager.Instance.GetGrabbedObjectAngularVelocity(grabObj, getSmoothedVelocity);
        //     // SET IS KINEMATIC TO FALSE
        //     Debug.Log(angularVelocity.y);
        //     // if rounded abosolute value of angular velocity is less than 0.5, stop the audio


        //     if (Mathf.RoundToInt(Mathf.Abs(angularVelocity.y)) < 20)
        //     {
        //         Debug.Log("STOP");
        //         rotatingHaptics.Stop();
        //     }
        //     else if (!rotatingHaptics.audioSource.isPlaying)
        //     {
        //         rotatingHaptics.Play();
        //     }
        // }
        if (rotateObj.transform.hasChanged)
        {


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

        else{
            rotatingAudio.Stop();


        }
    }
    private IEnumerator WaitAndUnlock(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        grabObj.IsLockedInPlace = true;
        // squeakHaptics.Play();
        yield return new WaitForSeconds(seconds);
        // squeakHaptics.Stop();
        grabObj.IsLockedInPlace = false;
        // lastAngle = CurrentAngle;
    }

    private void speedChange(object sender, UxrApplyConstraintsEventArgs e){
        // float angle=rotateObj.transform.localEulerAngles.y;
        Vector3 angle =prevAngle2+new Vector3(0,Time.deltaTime*speed,0);
        // Debug.Log("SpeedChange");
        rotateObj.transform.localRotation=Quaternion.Euler(angle);
        prevAngle2=rotateObj.transform.eulerAngles;

    }


public void OnNutGrabbed(object sender, UxrManipulationEventArgs e)
    {
        Debug.Log("Nut Grabbed");
        rotatingAudio.Play();
    }

    public void OnNutReleased(object sender, UxrManipulationEventArgs e)
    {
        if(rotatingAudio.isPlaying){
            rotatingAudio.Pause();
        }
    }

}

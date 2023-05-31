using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UltimateXR;
using UltimateXR.Manipulation;
using Interhaptics.Utils;
using Interhaptics;
public class Digital_MT : MonoBehaviour
{

    [SerializeField] private GameObject box;
    [SerializeField] private GameObject holder;
    [SerializeField] private GameObject tape;
    [SerializeField] private TextMeshPro _text;
    [SerializeField] private AudioHapticSource tapeOpeningHaptics;
    [SerializeField] private AudioHapticSource tapeClosingHapticsRight;
    [SerializeField] private AudioHapticSource tapeClosingHapticsLeft;
    // [SerializeField] private AudioHapticSource tapeStretchingHaptics;
    [SerializeField] private VelocityEstimator velocityEstimator;

    private Vector3 boxPosition, holderPosition, tapePosition, scaleChange;
    private Vector3 globalBoxPosition, globalHolderPosition, globalTapePosition;
    [System.NonSerialized]
    public float measuredDist, maxDist;

    private UxrGrabbableObject holderGrab;
    private UxrGrabbableObject boxGrab;
    private float velocity;
    void Start()
    {
        holderGrab = holder.GetComponent<UxrGrabbableObject>();
        boxGrab = box.GetComponent<UxrGrabbableObject>();
        boxGrab.Grabbed += OnBoxGrabbed;
        boxGrab.Released+=OnBoxReleased;
        holderGrab.Released += OnHolderReleased;
        holderGrab.Grabbed += OnHolderGrabbed;



        scaleChange = new Vector3(1, 1, 1);
        maxDist = 120;


    }

    void Update()
    {
        globalBoxPosition = box.transform.position;
        globalHolderPosition = holder.transform.position;
        globalTapePosition = tape.transform.position;

        boxPosition = box.transform.InverseTransformPoint(globalBoxPosition);
        holderPosition = box.transform.InverseTransformPoint(globalHolderPosition);
        tapePosition = box.transform.InverseTransformPoint(globalTapePosition);

        measuredDist = Vector3.Distance(tapePosition, holderPosition);

        scaleChange.x = Mathf.Clamp(measuredDist, 1, maxDist * 100);
        tape.transform.localScale = scaleChange;

        _text.text = Mathf.Clamp(measuredDist - 2.00f, 0, maxDist * 100).ToString("F2");
        if(holderGrab.IsBeingGrabbed){
            velocity=velocityEstimator.GetVelocityEstimate().magnitude;
            Debug.Log("Holder Velocity,"+velocity);
        }
    }

    public void OnBoxGrabbed(object sender, UxrManipulationEventArgs e)
    {
        // Debug.Log("The box is grabbed by "+e.Grabber.Side);
        // Debug.Log("Box is Grabbed");
        // eventHapticSource.Play();
        // box.GetComponent<AudioSource>().Play();
    }
    public void OnBoxReleased(object sender, UxrManipulationEventArgs e)
    {
        boxGrab.IsLockedInPlace=false;
    }
    public void OnHolderGrabbed(object sender, UxrManipulationEventArgs e)
    {
        Debug.Log("Holder is Grabbed");
        // tapeStretchingHaptics.Play();
        // tapeOpeningHaptics.Play();
    }

    public void OnHolderReleased(object sender, UxrManipulationEventArgs e)
    {
        Debug.Log("Holder is Released");

        if(e.Grabber.Side.ToString()=="Right"){
            tapeClosingHapticsRight.Play();
            Debug.Log("Holder Grabbed with"+"Right Hand");
        }
        else if(e.Grabber.Side.ToString() == "Left"){
            tapeClosingHapticsLeft.Play();
            // tapeClosingHapticsLeft1.Play();
            Debug.Log("Holder Grabbed with" + "Left Hand");
        }
        // if (sender.Equals(holderGrab))
        // {
        //     // Choose element 0 in the array hapticBodyParts in eventHapticSource
        //     if (eventHapticSource.hapticBodyParts.Length > 0)
        //     {
        //         eventHapticSource.hapticBodyParts[0].GetComponent<EventHapticSource>().PlayEventVibration();
        //     }
        // }
        // else if (sender.Equals(boxGrab))
        // {
        //     // Choose element 1 in the array hapticBodyParts in eventHapticSource
        //     if (eventHapticSource.hapticBodyParts.Length > 1)
        //     {
        //         eventHapticSource.hapticBodyParts[1].GetComponent<EventHapticSource>().PlayEventVibration();
        //     }
        // }




        //wait for some seconds
        // WaitForSeconds wait = new WaitForSeconds(2);
        // play audio file


        // holder.GetComponent<Rigidbody>().isKinematic = false;

        // float distanceRatio = measured_dist / max_dist; // Calculate the ratio of measured distance to maximum distance
        // float lerpSpeed = Mathf.Lerp(0.1f, 1f, distanceRatio);
        // Vector3 startPosition = new Vector3(0.119996071f, 0, 0); // Define the starting position for the holder

        // holder.transform.localPosition = Vector3.Lerp(holder.transform.localPosition, startPosition, lerpSpeed);



        float distanceRatio = measuredDist / maxDist; // Calculate the ratio of measured distance to maximum distance
        // AudioSource.PlayClipAtPoint(audioClip, transform.position);


        float lerpSpeed = Mathf.Lerp(0.05f, 1f, distanceRatio); // Adjust the lerp speed based on the distance ratio

        Vector3 startPosition = new Vector3(0, 0, 0); // Define the start position for the holder

        Vector3 clampedPosition = Vector3.Lerp(holder.transform.localPosition, startPosition, lerpSpeed);

        // Clamp the clampedPosition within a certain range if needed
        float clampRange = 0.1f; // Example: clamping range of 0.1 units
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, startPosition.x - clampRange, startPosition.x + clampRange);
        clampedPosition.y = Mathf.Clamp(clampedPosition.y, startPosition.y - clampRange, startPosition.y + clampRange);
        clampedPosition.z = Mathf.Clamp(clampedPosition.z, startPosition.z - clampRange, startPosition.z + clampRange);

        holder.transform.localPosition = clampedPosition;

    }



}
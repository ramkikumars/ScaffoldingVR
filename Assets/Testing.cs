using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SG;

public class Testing : MonoBehaviour
{
    [SerializeField] private GameObject box;
    [SerializeField] private GameObject holder;

    private Vector3 globalBoxPosition, globalHolderPosition;
    private Vector3 boxPosition, holderPosition, tapePosition, scaleChange;
    public SG_SimpleDrawer grabable;
    // public SG_SimpleDrawer grabable1;
    public SG_SimpleDrawer boxGrab;
    [System.NonSerialized]
    public float measuredDist, maxDist;

    [Range(0, 100)] public int magnitude = 100;


    // find the game object with the tag "tapecomponent"
    public GameObject tapeComponent;

    /// <summary> To which fingers the vibration command will be sent. 0 = thumb, 4 = pinky. </summary>
    public bool[] fingers = new bool[5] { true, true, false, false, false };

    public bool state;
    public bool grabbed = false;
    private Vector3 initialPosition;
    protected SGCore.Haptics.SG_TimedBuzzCmd vibrationCmd;

    private Quaternion initialRotation;
    // private float initialScale;
    public VelocityEstimator velocityEstimator;
    private void Start()

    {
        Vector3 initialPosition = transform.position;
        Quaternion initialRotation = transform.rotation;
        maxDist = 120;
        vibrationCmd = new SGCore.Haptics.SG_TimedBuzzCmd(new SGCore.Haptics.SG_BuzzCmd(fingers, magnitude), 0.02f);

    }


    public bool comeback = false;

    public void Comeback()
    {
        Debug.Log("Cameback");
        grabbed = false;

        // remove the game component sg_SimpleDrawer from the tape component i dont want to make it false instead i want to remove that component
        // tapeComponent
        // SG_SimpleDrawer componentToRemove = tapeComponent.GetComponent<SG_SimpleDrawer>();
        // Destroy(componentToRemove);

        // // tapeComponent.GetComponent<SG_SimpleDrawer>().enabled = false;
        // // add game component sg_grabable to the tape component
        // tapeComponent.AddComponent<SG_Grabable>();

        // comeback = true;


    }
    public void Grabbed()
    {
        grabbed = true;
        comeback = false;
        // Destroy(tapeComponent.GetComponent<SG_Grabable>());
        // remove the game component sg_grabable from the tape component
        // SG_Grabable componentToRemove = tapeComponent.GetComponent<SG_Grabable>();
        // Destroy(componentToRemove);



        // // add game component sg_SimpleDrawer to the tape component
        // tapeComponent.AddComponent<SG_SimpleDrawer>();
        // // set the axis of the drawer component s move axis to be the x axis
        // tapeComponent.GetComponent<SG_SimpleDrawer>().moveAxis = SG_SimpleDrawer.DrawerAxis.X;
    }

    private void Update()
    {
        globalBoxPosition = box.transform.position;
        globalHolderPosition = holder.transform.position;

        boxPosition = box.transform.InverseTransformPoint(globalBoxPosition);
        holderPosition = box.transform.InverseTransformPoint(globalHolderPosition);
        measuredDist = Vector3.Distance(tapePosition, holderPosition);
        if (grabbed)
        {
            float trackedVelocity = velocityEstimator.GetVelocityEstimate().magnitude;
            // Debug.Log("Tracked Velocity: " + trackedVelocity);
            //     Vector3 velocity = SG_Grabable.GetTrackedVelocity();
            boxGrab.MakeItFree = false;
            print("trackedVelocity: " + trackedVelocity);
            if (trackedVelocity >= 0.01)
            {
                grabable.ScriptsGrabbingMe()[0].TrackedHand.SendCmd(vibrationCmd);
            }
        }


        // }

        // if (holderRel)
        //     {
        //         HolderReturn();
        //     }
        if (comeback)
        {
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
        else
        {
            if (grabbed == false)
            {
            transform.localPosition = new Vector3(-0.0500000007f, -0.0500000007f, 0);
            // transform.localPosition = new Vector3(-3.66857171f, 3.24736714f, 0);
            //Vector3(0,0,272.728668)
            // transform.localRotation = Quaternion.Euler(0, 0, 272.728668f);

            // Vector3(0,0,272.728668)
            transform.localRotation = Quaternion.Euler(0, 0, 272.728668f);
            }
        }



    }
    void OnCollisionEnter(Collision other)
    {




        if (other.gameObject.tag == "cube")
        {
            // keep holder back to initial position and orientation

            // transform.position = initialPosition;
            // transform.rotation = initialRotation;
            // Grabbed();
            boxGrab.MakeItFree = true;
            Debug.Log("collided");
            // vibrationCmd = new SGCore.Haptics.SG_TimedBuzzCmd(new SGCore.Haptics.SG_BuzzCmd(fingers, magnitude), 0.05f);
            // Vector3(-0.0500000007,-0.0500000007,0)
            transform.localPosition = new Vector3(-0.0500000007f, -0.0500000007f, 0);
            // transform.localPosition = new Vector3(-3.66857171f, 3.24736714f, 0);
            //Vector3(0,0,272.728668)
            // transform.localRotation = Quaternion.Euler(0, 0, 272.728668f);

            // Vector3(0,0,272.728668)
            transform.localRotation = Quaternion.Euler(0, 0, 272.728668f);
            comeback = false;
            // Vector3(-3.66857171,3.24736714,0) 


        }
    }
    // private void OnValidate()
    // {
    //     if (state)
    //     {
    //         StartCoroutine(ReturnToInitialPositionCoroutine());
    //     }

    // }


}
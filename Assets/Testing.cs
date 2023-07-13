using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SG;
using Fusion;
using System.Threading.Tasks;
public class Testing : NetworkBehaviour
{
    [SerializeField] private GameObject box;
    [SerializeField] private GameObject holder;

    private Vector3 globalBoxPosition, globalHolderPosition;
    private Vector3 boxPosition, holderPosition, tapePosition, scaleChange;
    public SG_SimpleDrawer grabable;
    // public SG_SimpleDrawer grabable1;
    public SG_SimpleDrawer boxGrab;
    public static SG_SimpleDrawer boxGrabs;
    [System.NonSerialized]
    public float measuredDist, maxDist;

    [Range(0, 100)] public int magnitude = 100;


    // find the game object with the tag "tapecomponent"
    public GameObject tapeComponent;

    /// <summary> To which fingers the vibration command will be sent. 0 = thumb, 4 = pinky. </summary>
    public bool[] fingers = new bool[5] { true, true, true, true, false };

    public bool state;
    public static bool grabbed = false;
    public static bool boxGrabbed = false;
    // public bool grabbedInspector=false;
    public static NetworkObject holderNobj;
    private Vector3 initialPosition;
    protected static SGCore.Haptics.SG_TimedBuzzCmd vibrationCmd;

    private Quaternion initialRotation;
    // private float initialScale;
    public VelocityEstimator velocityEstimator;
    public static bool comeback = false;
    // public bool comebackInspector=false;
    private void Start()

    {
        Vector3 initialPosition = transform.position;
        Quaternion initialRotation = transform.rotation;
        maxDist = 19;
        vibrationCmd = new SGCore.Haptics.SG_TimedBuzzCmd(new SGCore.Haptics.SG_BuzzCmd(fingers, magnitude), 0.02f);
        grabable.ObjectGrabbed.AddListener(HolderGrabbed);
        grabable.ObjectReleased.AddListener(HolderReleased);
        boxGrab.ObjectGrabbed.AddListener(BoxGrabbed);
        boxGrab.ObjectReleased.AddListener(BoxReleased);
        boxGrabs=boxGrab;
        holderNobj=this.GetComponent<NetworkObject>();
    }


    public void HolderGrabbed(SG_Interactable sgGrab,SG_GrabScript sgScript){
        Debug.Log("Holder Grabbed");
        grabbed = true;
        comeback = false;
        boxGrab.MakeItFree=false;
       Rpc_HolderGrabbed(Object.Runner);
    }

    public void HolderReleased(SG_Interactable sgGrab, SG_GrabScript sgScript)
    {
        Debug.Log("Holder Released");
                grabbed = false;
               comeback = true;
               boxGrab.MakeItFree=true;

    }
    public void BoxGrabbed(SG_Interactable sgGrab, SG_GrabScript sgScript)
    {

        boxGrabbed=true;
        Rpc_BoxGrabbed(Object.Runner);
    }
    public void BoxReleased(SG_Interactable sgGrab, SG_GrabScript sgScript)
    {
        boxGrabbed = true;
        Rpc_BoxRel(Object.Runner);

    }


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

        comeback = true;


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
        // if (grabbedInspector)
        // {
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
        // if (comebackInspector)
        // {
        if (comeback)
        {
            float distanceRatio = measuredDist / maxDist; // Calculate the ratio of measured distance to maximum distance
                                                          // AudioSource.PlayClipAtPoint(audioClip, transform.position);


            float lerpSpeed = Mathf.Lerp(0.05f, 1f, distanceRatio); // Adjust the lerp speed based on the distance ratio

            Vector3 startPosition = new Vector3(2f, 0, 0); // Define the start position for the holder

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
            // if (grabbedInspector==false)
            // {
            if (grabbed == false)
            {
            transform.localPosition = new Vector3(2f, 0f, 0f);
            // transform.localPosition = new Vector3(-3.66857171f, 3.24736714f, 0);
            //Vector3(0,0,272.728668)
            // transform.localRotation = Quaternion.Euler(0, 0, 272.728668f);

            // Vector3(0,0,272.728668)
            // transform.localRotation = Quaternion.Euler(0, 0, 272.728668f);
            }
        }



    }
     private void OnTriggerEnter(Collider other)
    {


        if (other.gameObject.tag == "cube")
        {
            // keep holder back to initial position and orientation

            // transform.position = initialPosition;
            // transform.rotation = initialRotation;
            // Grabbed();
            Debug.Log("collided with cube");
            // boxGrab.MakeItFree = true;
            // boxGrab.ScriptsGrabbingMe()[0].TrackedHand.SendCmd(vibrationCmd);

            // vibrationCmd = new SGCore.Haptics.SG_TimedBuzzCmd(new SGCore.Haptics.SG_BuzzCmd(fingers, magnitude), 0.05f);
            // Vector3(-0.0500000007,-0.0500000007,0)
            transform.localPosition = new Vector3(2f, 0, 0);
            // transform.localPosition = new Vector3(-3.66857171f, 3.24736714f, 0);
            //Vector3(0,0,272.728668)
            // transform.localRotation = Quaternion.Euler(0, 0, 272.728668f);

            // Vector3(0,0,272.728668)
            // transform.localRotation = Quaternion.Euler(0, 0, 272.728668f);
            comeback = false;
            Rpc_HolderRel(Object.Runner);
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
    [Rpc]
    public static void Rpc_HolderGrabbed(NetworkRunner runner)
    {
        grabbed = true;
        comeback = false;
        boxGrabs.MakeItFree=false;
    }

     [Rpc]
    public static void Rpc_HolderRel(NetworkRunner runner)
    {
         boxGrabs.MakeItFree = true;
        if(boxGrabbed==true){
        boxGrabs.SendImpactVibration(SG_HandSection.Wrist,80f);
        }
    }


    [Rpc]
    public static void Rpc_BoxGrabbed(NetworkRunner runner)
    {
        boxGrabbed=true;
        if(grabbed==false){
            if(!holderNobj.HasStateAuthority){
                ReqAuthorithy(holderNobj);
            }
        }

    }

     [Rpc]
    public static void Rpc_BoxRel(NetworkRunner runner)
    {
        boxGrabbed = false;
    }

    async static void ReqAuthorithy(NetworkObject o)
    {
        await WaitForStateAuthority(o);
    }



    public async static Task<bool> WaitForStateAuthority(NetworkObject o, float maxWaitTime = 8)
    {
        float waitStartTime = Time.time;
        o.RequestStateAuthority();
        while (!o.HasStateAuthority && (Time.time - waitStartTime) < maxWaitTime)
        {
            await System.Threading.Tasks.Task.Delay(1);
        }
        return o.HasStateAuthority;
    }

}
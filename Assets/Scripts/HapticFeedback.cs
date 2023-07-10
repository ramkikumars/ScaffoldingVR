using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UltimateXR.Manipulation;
using UltimateXR.Avatar;
using UltimateXR.Haptics;
using SG;
public class HapticFeedback : MonoBehaviour
{
    // Start is called before the first frame update
    [Range(0, 1)]
    [SerializeField] private float intensity;
    [SerializeField] private float duration;
    [SerializeField] private float hitWait;
    [SerializeField] private UxrHapticClipType UxrclipType;
    public float minVelocity = 0;
    public float maxVelocity = 2f;
    private string controllerName;
    private VelocityEstimator velocityEstimator;
    private float hitVelocity;
    // private UxrGrabbableObject grabObj => GetComponent<UxrGrabbableObject>();
    // <summary> The Interactable object that we will be sending vibration commands to. </summary>
    /// <remarks> Since SG_Grabable derives from SG_Interactable, this will work for grabables, as well as any other script that derives from SG_Interactable. </remarks>

    private SG_Grabable objectToVibrate => GetComponent<SG_Grabable>();
    /// <summary> The amplitude of the vibration. 0 = no vibration, 100 = full vibration. </summary>
    [Range(0, 100)] public int magnitude = 100;

    /// <summary> To which fingers the vibration command will be sent. 0 = thumb, 4 = pinky. </summary>
    public bool[] fingers = new bool[5] { true, true, true, true, true };

    /// <summary> The vibration command to be send. Cached so we do not need to regenerate it every frame. </summary>
    protected SGCore.Haptics.SG_TimedBuzzCmd vibrationCmd;
    private void OnEnable()
    {
        // grabObj.Grabbed += ObjGrabbed;
        objectToVibrate.ObjectGrabbed.AddListener(ObjGrabbed);
    }
    private void OnDisable()
    {
        // grabObj.Grabbed -= ObjGrabbed;
    }
    void Start()
    {
        // grabInteractable.selectEntered.AddListener(ObjGrabbed);
        //regenerate the vibration command. We'll make it 0.02f (20ms) long so there will be overlap between two frames for a continuous vibration.
        // vibrationCmd = new SGCore.Haptics.SG_TimedBuzzCmd(new SGCore.Haptics.SG_BuzzCmd(fingers, magnitude), 0.02f);
        velocityEstimator = GetComponent<VelocityEstimator>();

    }

    // private void OnTriggerEnter(Collider other)
    // {
    //     Debug.Log("Hit");

    //     if (grabObj.IsBeingGrabbed)
    //     {
    //         grabObj.IsLockedInPlace = true;

    //     }
    // }
    private void OnCollisionEnter(Collision other)
    {

        if (objectToVibrate.IsGrabbed())
        {
        Debug.Log("Hit");
            float v = velocityEstimator.GetVelocityEstimate().magnitude;
            magnitude = (int) Mathf.InverseLerp(minVelocity, maxVelocity, v)*100;
            // vibrationCmd = new SGCore.Haptics.SG_TimedBuzzCmd(new SGCore.Haptics.SG_BuzzCmd(fingers, magnitude), 0.5f);
            // objectToVibrate.ScriptsGrabbingMe()[0].TrackedHand.SendCmd(vibrationCmd);
            StartCoroutine(HitAndWait());
        }
    }
    private void ObjGrabbed(object obj1, object obj2)
    {
        // // Object obj = new Object();
        // if (obj1.GetType() == typeof(UxrGrabbableObject))
        // {
        //     // obj = obj1 as UxrGrabbableObject;
        //     controllerName = "Uxr";
        //     // Debug.Log($"Grabbed {obj.name} with UxR");
        // }
        // else if (obj1.GetType() == typeof(SG_Grabable))
        // {
        //     // obj = obj1 as SG_SimpleDrawer;
        //     controllerName = "Sg";
        //     // Debug.Log($"Grabbed {obj.name} with SG");
        // }
        velocityEstimator.BeginEstimatingVelocity();

    }

    IEnumerator HitAndWait()
    {
        // if (controllerName == "Uxr")
        // {
        //     grabObj.IsLockedInPlace = true;
        //     UxrAvatar.LocalAvatar.ControllerInput.SendGrabbableHapticFeedback(grabObj, UxrclipType, intensity, duration);
        //     // objectToVibrate.SendCmd(vibrationCmd);
        //     yield return new WaitForSeconds(hitWait);
        //     grabObj.IsLockedInPlace = false;
        // }
        // if (controllerName == "Sg")
        // {
            Debug.Log($"Mag:{magnitude}");
            //regenerate the vibration command. We'll make it 0.02f (20ms) long so there will be overlap between two frames for a continuous vibration.
            // vibrationCmd = new SGCore.Haptics.SG_TimedBuzzCmd(new SGCore.Haptics.SG_BuzzCmd(fingers, magnitude), 0.5f);
            // objectToVibrate.ScriptsGrabbingMe()[0].TrackedHand.SendCmd(vibrationCmd);
             vibrationCmd = new SGCore.Haptics.SG_TimedBuzzCmd(new SGCore.Haptics.SG_BuzzCmd(fingers, magnitude),0.1f);
            // objectToVibrate.ScriptsGrabbingMe()[0].TrackedHand.SendCmd(vibrationCmd);
            objectToVibrate.SendCmd(vibrationCmd);
            yield return new WaitForSeconds(0.1f);

        // }
    }


}

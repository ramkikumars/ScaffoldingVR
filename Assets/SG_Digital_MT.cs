using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using SG;
public class SG_Digital_MT : MonoBehaviour
{

    [SerializeField] private GameObject box;
    [SerializeField] private GameObject holder;
    [SerializeField] private GameObject tape;
    [SerializeField] private TextMeshPro _text;

    // [SerializeField] private AudioHapticSource tapeStretchingHaptics;
    [SerializeField] private VelocityEstimator velocityEstimator;

    private Vector3 boxPosition, holderPosition, tapePosition, scaleChange;
    private Vector3 globalBoxPosition, globalHolderPosition, globalTapePosition;
    [System.NonSerialized]
    public float measuredDist, maxDist;

    private SG_Grabable holderGrab;
    private SG_Grabable boxGrab;
    private float velocity;
    
    private bool holderRel;
    void Start()
    {
        holderGrab = holder.GetComponent<SG_SimpleDrawer>();
        boxGrab = box.GetComponent<SG_Grabable>();
        scaleChange = new Vector3(1, 1, 1);
        maxDist = 120;
        holderGrab.ObjectGrabbed.AddListener(OnHolderGrabbed);
        holderGrab.ObjectReleased.AddListener(OnHolderReleased);
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

        _text.text = Mathf.Clamp(measuredDist - 1, 0, maxDist * 100).ToString("F2");
        if (holderGrab.IsGrabbed())
        {
            velocity = velocityEstimator.GetVelocityEstimate().magnitude;
            velocity = Mathf.Round(velocity * 100f) / 100f;
            if (velocity <= 0.01)
            {
                // tapeOpeningHaptics.Stop();
            }
            else {

            }
        }
        // Debug.Log("Holder Velocity,"+velocity);
        if (holderRel)
        {
            HolderReturn();
        }
    }

    public void OnBoxGrabbed(SG_Interactable sGInteractable, SG_GrabScript sgGrabScript)
    {

    }
    public void OnBoxReleased(SG_Interactable sGInteractable, SG_GrabScript sgGrabScript)
    {
    }
    public void OnHolderGrabbed(SG_Interactable sGInteractable,SG_GrabScript sgGrabScript)
    {
        Debug.Log("Holder Grabbed");
        holderRel=false;
    }

    public void OnHolderReleased(SG_Interactable sGInteractable, SG_GrabScript sgGrabScript)
    {
        Debug.Log("Holder Released");
        holderRel=true;
    }

    private void HolderReturn()
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


}

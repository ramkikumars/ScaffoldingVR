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

    public float measuredDist, maxDist;
    public bool state;
    public bool grabbed = false;
    private Vector3 initialPosition;
    private Quaternion initialRotation;
    // private float initialScale;
    private void Start()

    {
        Vector3 initialPosition = transform.position;
        Quaternion initialRotation = transform.rotation;
        maxDist = 120;
    }


    public bool comeback = false;

    public void Comeback()
    {
        comeback = true;
    }
    public void Grabbed()
    {
        grabbed = true;
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
            // float trackedVelocity = grabable.GetTrackedVelocity();
            // Debug.Log("Tracked Velocity: " + trackedVelocity);
            //     Vector3 velocity = SG_Grabable.GetTrackedVelocity();

            //     if ( 0.01)
            //     {
            //         tapeOpeningHaptics.Stop();
            //     }
            //     else if (!tapeOpeningHaptics.audioSource.isPlaying)
            //         tapeOpeningHaptics.Play();
            // }
            // // Debug.Log("Holder Velocity,"+velocity);
            // if (holderRel)
            // {
            //     HolderReturn();
            // }
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

            transform.localPosition = new Vector3(-3.66857171f, 3.24736714f, 0);
            //Vector3(0,0,272.728668)
            transform.localRotation = Quaternion.Euler(0, 0, 272.728668f);

        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.tag == "cube")
            {
                // keep holder back to initial position and orientation

                // transform.position = initialPosition;
                // transform.rotation = initialRotation;
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

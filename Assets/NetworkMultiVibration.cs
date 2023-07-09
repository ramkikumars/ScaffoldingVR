using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class NetworkMultiVibration : NetworkBehaviour
{
    // Start is called before the first frame update
    public AudioClip clip;
    private AudioSource source;
    public string targetTag;

    public bool useVelocity = true;
    public float minVelocity = 0;
    public float maxVelocity = 2f;

    public bool randomizePitch = true;
    public float minPitch = 0.8f;
    public float maxPitch = 1.2f;
    private static Vector3 handPos;
    private Vector3 hammerPos;
    private static bool handPlaced=false;
    void Start()
    {
source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Hand"))
        {
        Debug.Log("Hand Placed in the Collider");
        handPos=other.transform.position;
        handPlaced=true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Hand"))
        {
            Debug.Log("Hand Released from the Collider");
            handPlaced = false;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        Debug.Log("Enter aud");
        if (other.gameObject.CompareTag(targetTag))
        {
            VelocityEstimator estimator = other.gameObject.GetComponent<VelocityEstimator>();
            if (estimator && useVelocity)
            {
                float v = estimator.GetVelocityEstimate().magnitude;
                float volume = Mathf.InverseLerp(minVelocity, maxVelocity, v);
                Debug.Log($"Hit with Velocity {v} and volume is {volume}");
                if (randomizePitch)
                {
                    source.pitch = Random.Range(minPitch, maxPitch);
                }
                source.PlayOneShot(clip, volume);
            }
            else
            {
                source.PlayOneShot(clip);
            }
            // Make an empty list to hold contact points
            ContactPoint[] contacts = new ContactPoint[10];
            // Get the contact points for this collision
            int numContacts = other.GetContacts(contacts);
            hammerPos=contacts[0].point;
            Rpc_GiveVibrationOtherHand(Object.Runner,hammerPos);
        }
    }

[Rpc]
    public static void Rpc_GiveVibrationOtherHand(NetworkRunner runner,Vector3 hammerPos)
    {
        if(handPlaced){
            float dist=Vector3.Distance(handPos,hammerPos);
            Debug.Log($"The distance between hammer and hand {dist}");
        }
    }

}

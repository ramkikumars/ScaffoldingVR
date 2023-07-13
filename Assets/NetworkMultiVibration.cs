using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using SG;
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
    private static Vector3 handPos=Vector3.zero;
    private Vector3 hammerPos=Vector3.zero;
    private static bool handPlaced=false;
    private static float dist;
    private static float hammerMag;
    public static SG_FixedRod fixedRod;
    public  SG_Waveform waveForm;
    public  static SG_Waveform waveForms;
    /// <summary> To which fingers the vibration command will be sent. 0 = thumb, 4 = pinky. </summary>
    public static bool[] fingers = new bool[5] { true, true, true, true, true };

    /// <summary> The vibration command to be send. Cached so we do not need to regenerate it every frame. </summary>
    protected static SGCore.Haptics.SG_TimedBuzzCmd vibrationCmd;
    void Start()
    {
fixedRod=GetComponent<SG_FixedRod>();
source = GetComponent<AudioSource>();
fixedRod.ObjectGrabbed.AddListener(ObjectGrabbed);
fixedRod.ObjectReleased.AddListener(ObjectReleased);
waveForms=waveForm;
// waveForm.index=true;
// waveForm.middle=true;
// waveForm.ring=true;
// waveForm.pinky=true;

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void ObjectGrabbed(SG_Interactable sgGrab,SG_GrabScript sgScript){
        handPlaced=true;
        handPos = sgGrab.transform.position;
    }
    public void ObjectReleased(SG_Interactable sgGrab, SG_GrabScript sgScript)
    {
        handPlaced = false;
        sgGrab.StopAllVibrations();
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
        // Debug.Log("Enter aud");
        if (other.gameObject.CompareTag(targetTag))
        {
            VelocityEstimator estimator = other.gameObject.GetComponent<VelocityEstimator>();
            if (estimator && useVelocity)
            {
                float v = estimator.GetVelocityEstimate().magnitude;
                float volume = Mathf.InverseLerp(minVelocity, maxVelocity, v);
                hammerMag=Mathf.InverseLerp(minVelocity, maxVelocity, v);
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

            Rpc_GiveVibrationOtherHand(Object.Runner,hammerPos,this);
        }
    }

    [Rpc]
    public static void Rpc_GiveVibrationOtherHand(NetworkRunner runner,Vector3 hammerPos,NetworkMultiVibration nmult)
    {
        if(handPlaced){

        //  dist=Vector3.Distance(handPos,hammerPos);
            float dist=Mathf.Abs(handPos.x-hammerPos.x);
            nmult.StartCoroutine(HitAndWait());
        }
    }
     static IEnumerator HitAndWait()
    {

            Debug.Log($"Dist bw hammer and hand");
        float distMag = Mathf.InverseLerp(1.1f, 0, dist) * 100f;
        // float totalMag=(distMag+hammerMag)/2;
        float clampedVal = Mathf.Clamp(distMag, 10, 100);
            // vibrationCmd = new SGCore.Haptics.SG_TimedBuzzCmd(new SGCore.Haptics.SG_BuzzCmd(fingers, (int)mag),0.5f);
            // fixedRod.ScriptsGrabbingMe()[0].TrackedHand.SendCmd(vibrationCmd);
            waveForms.magnitude=(int)clampedVal;
            fixedRod.SendCmd(waveForms);
            yield return null;

        // }
    }

}

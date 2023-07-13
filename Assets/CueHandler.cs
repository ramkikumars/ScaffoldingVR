using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SG;
using Fusion;

public class CueHandler : NetworkBehaviour
{
    // Start is called before the first frame update
    // public int set=1;
    // public int objNo=1;
    public bool reqAuth=false;
    public bool relAuth=false;
    public bool state;
    private NetworkCue networkCue;
    private string recentlySnapped;
    public NetworkObject nobj;
    [Networked]
    public int counter { get; set; }
    public VelocityEstimator velocityEstimator;
    // public Transform hammerPlane;
    // public Transform handPlane;
    public float velMag;
    public int val;
    [Range(0, 1)] public float drawer_slideValue = 0;
    void Start()
    {
        networkCue=GetComponent<NetworkCue>();

    }

    public override void FixedUpdateNetwork()
    {
        // if(state){
        //     counter+=1;
        //     state=false;
        // }
    }
    private void OnValidate()
    {
        // Rpc_ReqAuthority(Object.Runner,this);

        // if(state) counter += 1;
        // if(reqAuth) Object.RequestStateAuthority();
        // if(relAuth) Object.ReleaseStateAuthority();
        // Debug.Log(Mathf.InverseLerp(1,0,drawer_slideValue));
        // float dist=Vector3.Distance(hammerPlane.position, handPlane.position);
        // Debug.Log($"Dist bw:{dist}");
        // Debug.Log($"Inverse Lerp :{Mathf.InverseLerp(1.1f,0,dist)}");
        // float mag=Mathf.InverseLerp(1.1f, 0, dist)*100f;
        // float clampedVal=Mathf.Clamp(mag,20,100);
        // Debug.Log($"Final Mag Val:{clampedVal}");
        // Rpc_SetActiveSnapzone(Object.Runner,GameObject.Find("Cube"));
    }

    void Update(){
        velMag=velocityEstimator.GetVelocityEstimate().magnitude;
    }

    [Rpc]
        public static void Rpc_SetActiveSnapzone(NetworkRunner runner)
        {
            // // Debug.Log($"The Value is {a}");
            // cube.SetActive(false);
        }


}

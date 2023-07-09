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
    private NetworkCue networkCue;
    private string recentlySnapped;
    public NetworkObject nobj;
    [Networked]
    public int counter { get; set; }
    public VelocityEstimator velocityEstimator;
    public float velMag;
    public int val;
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
        if(reqAuth) Object.RequestStateAuthority();
        if(relAuth) Object.ReleaseStateAuthority();
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

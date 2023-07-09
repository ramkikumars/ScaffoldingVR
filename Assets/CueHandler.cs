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
    public bool state=false;
    private NetworkCue networkCue;
    private string recentlySnapped;
    public NetworkObject nobj;
    [Networked]
    public int counter { get; set; }
    public VelocityEstimator velocityEstimator;
    public float velMag;
    void Start()
    {
        networkCue=GetComponent<NetworkCue>();

    }

    public override void FixedUpdateNetwork()
    {
        if(state){
            counter+=1;
            state=false;
        }
    }
    private void OnValidate()
    {
        // Rpc_ReqAuthority(Object.Runner,this);

        if(state) counter += 1;
    }

    void Update(){
        velMag=velocityEstimator.GetVelocityEstimate().magnitude;
    }

}

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
    // public SG_SnapDropZone dropZone1,dropZone2,dropZone3,dropZone4;
    // public Transform[] Set1SnapPoints,Set2SnapPoints,Set3SnapPoints,Set4SnapPoints;
    // public SG_SnapDropZone[] dropZones1,dropZones2,dropZones3,dropZones4;
    // public Transform[] Set1SnapPoints=new Transform[4];
    private string recentlySnapped;
    public NetworkObject nobj;

    [Networked]
    public int counter{ get; set; }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    private void OnValidate(){
        // Rpc_ReqAuthority(Object.Runner,this);
        counter+=1;
    }



    [Rpc]
    public static void Rpc_ReqAuthority(NetworkRunner runner, CueHandler ngrab)
    {
        if (ngrab.nobj.HasStateAuthority)
        {
            // ngrab.ReqAuthorithy(ngrab.nobj);
            Debug.Log("This has state authority");
        }
    }


}

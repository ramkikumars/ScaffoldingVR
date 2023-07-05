using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class NetworkGrabbable : NetworkBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public override void Spawned()
    {
        base.Spawned();
        if (Object.HasStateAuthority)
        {
            Debug.Log("This Runner have StateAuthority");
        }
        else Debug.Log("This Runner doesn't have StateAuthority");
    }
}

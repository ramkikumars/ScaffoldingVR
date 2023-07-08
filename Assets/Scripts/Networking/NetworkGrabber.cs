using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using System.Threading.Tasks;
public class NetworkGrabber : NetworkBehaviour
{
    // Start is called before the first frame update
    [Networked]
    public NetworkGrabbable GrabbedObject { get; set; }
    private NetworkObject grabbalenetworkObject;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag=="NetworkGrabbable"){
        NetworkGrabbable grabbable = other.GetComponent<NetworkGrabbable>();
        ReqAuthorithy(grabbable.Object);
        Debug.Log("StateAuthority Changed");
        }
    }

    async void ReqAuthorithy(NetworkObject nobj)
    {
        await WaitForStateAuthority(nobj);
    }

    public async Task<bool> WaitForStateAuthority(NetworkObject o, float maxWaitTime = 8)
    {
        float waitStartTime = Time.time;
        o.RequestStateAuthority();
        while (!o.HasStateAuthority && (Time.time - waitStartTime) < maxWaitTime)
        {
            await System.Threading.Tasks.Task.Delay(1);
        }
        return o.HasStateAuthority;
    }

}

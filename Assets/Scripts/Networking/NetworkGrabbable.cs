using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using System.Threading.Tasks;
using SG;
public class NetworkGrabbable : NetworkBehaviour
{
    // Start is called before the first frame update
    public SG_Grabable sgGrabable;
    [Networked]
    public NetworkString<_16> currentGrabbed { get; set; }
    // [Networked]
    // public NetworkString<_16> secondGrabbed { get; set; }
    [Networked(OnChanged = nameof(OnChangedGrabber))]
    public NetworkBool changeGrabber{ get; set; }
    void Start()
    {
        sgGrabable.ObjectGrabbed.AddListener(ObjectGrabbed);
        sgGrabable.ObjectReleased.AddListener(ObjectReleased);
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

    private void ObjectGrabbed(SG_Interactable obj1,SG_GrabScript obj2){
        currentGrabbed = Object.Runner.LocalPlayer.ToString();
        Debug.Log($"{this.gameObject.name} was grabbed by {currentGrabbed}");
        if(!Object.HasStateAuthority){
                changeGrabber=!changeGrabber;
        }
    }
    private void ObjectReleased(SG_Interactable obj1, SG_GrabScript obj2)
    {
        currentGrabbed="";
    }

    async void ReqAuthorithy(NetworkObject o)
    {
        await WaitForStateAuthority(o);
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

    public static void OnChangedGrabber(Changed<NetworkGrabbable> changed)
    {
        changed.Behaviour.ChangedGrabber();
    }

    private void ChangedGrabber()
    {
        if(!Object.HasStateAuthority){
            sgGrabable.ReleaseSelf();
            ReqAuthorithy(Object);
        }
    }

}

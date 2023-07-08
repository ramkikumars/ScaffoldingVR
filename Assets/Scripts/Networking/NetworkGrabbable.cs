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
    public string currentGrabbed { get; set; }
    [Networked]
    public int grabberCount { get; set; } = 0;
    [Networked]
    public bool grabbedByOther { get; set; }
    // [Networked(OnChanged = nameof(OnChangedGrabber))]
    public bool changeGrabber { get; set; }
    private bool objGrabbed;
    private bool objReleased;

    public NetworkObject nobj;
    private static NetworkObject nobjs;
    void Start()
    {
        nobjs=nobj;
        sgGrabable.ObjectGrabbed.AddListener(ObjectGrabbed);
        sgGrabable.ObjectReleased.AddListener(ObjectReleased);
    }

    // Update is called once per frame
    void Update()
    {

    }
    public override void FixedUpdateNetwork()
    {

        if (objGrabbed)
        {
            currentGrabbed = nobj.Runner.LocalPlayer.ToString();
            Debug.Log($"{this.gameObject.name} was grabbed by {currentGrabbed}");

                if (grabberCount == 0)
                {
                    changeGrabber = !changeGrabber;
                }
                    grabberCount += 1;
            objGrabbed=false;
        }

        if (objReleased)
        {

            if (grabberCount == 2)
            {
                changeGrabber = !changeGrabber;
            }if(grabberCount>=1){
            grabberCount -= 1;
            }
            objReleased = false;
        }

    }


    public override void Spawned()
    {
        base.Spawned();
        if (nobj.HasStateAuthority)
        {
            // Debug.Log("This Runner have StateAuthority");
        }
        // else Debug.Log("This Runner doesn't have StateAuthority");
    }

    private void ObjectGrabbed(SG_Interactable obj1, SG_GrabScript obj2)
    {
        objGrabbed = true;
        grabberCount+=1;
        // if(grabbedByOther){

        // }else{
        //     grabbedByOther=false;
        // }
    }
    private void ObjectReleased(SG_Interactable obj1, SG_GrabScript obj2)
    {
        objReleased = true;
        grabberCount += 1;
    }




    async static void ReqAuthorithy(NetworkObject o)
    {
        await WaitForStateAuthority(o);
    }

    public async static Task<bool> WaitForStateAuthority(NetworkObject o, float maxWaitTime = 8)
    {
        float waitStartTime = Time.time;
        o.RequestStateAuthority();
        while (!o.HasStateAuthority && (Time.time - waitStartTime) < maxWaitTime)
        {
            await System.Threading.Tasks.Task.Delay(1);
        }
        return o.HasStateAuthority;
    }

    // public static void OnChangedGrabber(Changed<NetworkGrabbable> changed)
    // {
    //     changed.Behaviour.ChangedGrabber();
    // }

    // private void ChangedGrabber()
    // {

    //     if (!nobj.HasStateAuthority)
    //     {
    //         ReqAuthorithy(nobj);
    //     }
    // }



    [Rpc]
    public static void Rpc_ReqAuthority(NetworkRunner runner)
    {
        if (!nobjs.HasStateAuthority)
        {
            ReqAuthorithy(nobjs);
        }
    }


}

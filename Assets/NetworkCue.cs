using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using SG;
using Fusion.Photon.Realtime;
using System.Linq;
[OrderAfter]
public class NetworkCue : NetworkBehaviour
{
    // Start is called before the first frame update
    // [System.Serializable]
    // public struct ScaffoldingSingleSetObjects
    // {
    //     public GameObject baseJack,vertical,horizontal1,horizontal2;
    // }
    // [SerializeField]
    // public ScaffoldingSingleSetObjects set1,set2,set3,set4;
    // [Header("Scaffolding Objects")]
    //
    // private static GameObject [] basess,verticalss,horizontalLowerss,horizontalMiddless;
    // // private static GameObject[] set1s, set2s, set3s, set4s;
    // private static SG_SnapDropZone [] baseSnapZoness,verticalSnapZoness;
    // private static SG_SnapDropZone [][] horizontalSnapZoness;

    public GameObject[] sets;
    private static GameObject[] bases, verticals, horizontalLowers, horizontalMiddles;
    // private static GameObject =new GameObject
    private static SG_SnapDropZone[] baseSnapZones, verticalSnapZones;
    private static SG_SnapDropZone[,] horizontalLowerZones, horizontalMiddleZones;
    public static string recentlySnapped="";

    private bool objSnapped;
    private bool resetObjSnapped;
    private string recentlySnappedObj;
    // private SG_SnapDropZone[] baseSnapZones, verticalSnapZones;
    // private SG_SnapDropZone[][] horizontalSnapZones;
    public NetworkObject networkObject;
    void Start()
    {
        bases=new GameObject[4];
        verticals =new GameObject[4];
        horizontalLowers=new GameObject[4];
        horizontalMiddles=new GameObject[4];
        baseSnapZones=new SG_SnapDropZone[4];
        verticalSnapZones =new SG_SnapDropZone[4];
        horizontalLowerZones=new SG_SnapDropZone[4,2];
        horizontalMiddleZones=new SG_SnapDropZone[4,2];

        for (int i = 0; i <4; i++)
        {
            // GameObject basee=sets[i].transform.Find("Base").gameObject;
            bases[i] = sets[i].transform.Find("Base").gameObject;
            bases[i].SetActive(false);
            verticals[i] = sets[i].transform.Find("Ledgers/Vertical").gameObject;
            verticals[i].SetActive(false);
            horizontalLowers[i] = sets[i].transform.Find("Ledgers/Horizontals/horizontal 1").gameObject;
            horizontalLowers[i].SetActive(false);
            horizontalMiddles[i] = sets[i].transform.Find("Ledgers/Horizontals/horizontal 2").gameObject;
            horizontalMiddles[i].SetActive(false);

            baseSnapZones[i] = sets[i].transform.Find("SnapObjects/Base").GetComponent<SG_SnapDropZone>();
            baseSnapZones[i].ObjectSnapped.AddListener(ObjectSnapped);
            verticalSnapZones[i] = sets[i].transform.Find("SnapObjects/Vertical").GetComponent<SG_SnapDropZone>();
            verticalSnapZones[i].ObjectSnapped.AddListener(ObjectSnapped);

                horizontalLowerZones[i,0] = sets[i].transform.Find("SnapObjects/HorizontalLower/Grab1").GetComponent<SG_SnapDropZone>();
                horizontalLowerZones[i,1] = sets[i].transform.Find("SnapObjects/HorizontalLower/Grab2").GetComponent<SG_SnapDropZone>();

                horizontalMiddleZones[i,0] = sets[i].transform.Find("SnapObjects/HorizontalMiddle/Grab1").GetComponent<SG_SnapDropZone>();
                horizontalMiddleZones[i,1] = sets[i].transform.Find("SnapObjects/HorizontalMiddle/Grab2").GetComponent<SG_SnapDropZone>();
        }




    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void FixedUpdateNetwork()
    {

    }

    [Rpc]
    public static void Rpc_SwitchState(NetworkRunner runner, string objName, int idx, bool state)
    {
        switch (objName)
        {
            case "Base":
                bases[idx].SetActive(state);
                break;

            case "Vertical":
                verticals[idx].SetActive(state);
                break;

            case "HorizontalLower":
                horizontalLowers[idx].SetActive(state);
                break;

            case "HorizontalMiddle":
                horizontalMiddles[idx].SetActive(state);
                break;

        }
    }

    public void SwitchState(string objName, int idx, bool state1)
    {
        Rpc_SwitchState(Object.Runner, objName, idx, state1);
    }


    [Rpc]
    public static void Rpc_SetActiveSnapzone(NetworkRunner runner, string objName, int idx, bool state)
    {
        switch (objName)
        {
            case "Base":
                baseSnapZones[idx].enabled = state;
                break;

            case "Vertical":
                verticalSnapZones[idx].enabled = state;
                break;

            case "HorizontalLower":
                horizontalLowerZones[idx,0].enabled = state;
                horizontalLowerZones[idx,1].enabled = state;
                break;
            case "HorizontalMiddle":
                horizontalMiddleZones[idx,0].enabled = state;
                horizontalMiddleZones[idx,1].enabled = state;
                break;

        }
    }
// [Rpc]
//     public static void Rpc_SetActiveSnapzone(NetworkRunner runner)
//     {
//         switch (objName)
//         {
//             case "Base":
//                 baseSnapZones[idx].enabled = state;
//                 break;

//             case "Vertical":
//                 verticalSnapZones[idx].enabled = state;
//                 break;

//             case "HorizontalLower":
//                 horizontalLowerZones[idx, 0].enabled = state;
//                 horizontalLowerZones[idx, 1].enabled = state;
//                 break;
//             case "HorizontalMiddle":
//                 horizontalMiddleZones[idx, 0].enabled = state;
//                 horizontalMiddleZones[idx, 1].enabled = state;
//                 break;

//         }
//     }

    public void SetActiveSnapzone(string objName, int idx, bool state)
    {
        Rpc_SetActiveSnapzone(Object.Runner, objName, idx, state);

    }
[Rpc]
    public static void Rpc_ObjectSnapped(NetworkRunner runner, string snappedObj)
    {
        recentlySnapped=snappedObj;
    }

    IEnumerator Exercise1()
    {

        Debug.Log("Started Coroutine");
        yield return new WaitUntil(()=>(IsPlayerJoined()));

            for(int i=0;i<4;i++){

            SwitchState("Base", i, true);
            SetActiveSnapzone("Base", i, true);
            Debug.Log($"Waiting for Base {i} to be snapped");
            yield return new WaitUntil(() => (IsObjSnapped("Base")));
            Debug.Log($"Base {i} snapped");
            resetObjSnapped=true;
            SetActiveSnapzone("Base", i, false);
            SwitchState("Base", i, false);
        }
        for (int i = 0; i < 4; i++)
        {
            SwitchState("Vertical", i, true);
            SetActiveSnapzone("Vertical", i, true);
            yield return new WaitUntil(() => (IsObjSnapped("Vertical")));
            resetObjSnapped = true;
            SetActiveSnapzone("Vertical", i, false);
        }
        SwitchState("HorizontalLower",0,true);
        SetActiveSnapzone("HorizontalLower", 0, true);
        // yield return new WaitUntil(() => (IsObjSnapped("HGrab")));
        // resetObjSnapped = true;
        // yield return new WaitUntil(() => (IsObjSnapped("HGrab")));
        // resetObjSnapped = true;
        // SwitchState("HorizontalLower", 3, false);
        // SetActiveSnapzone("HorizontalLower", 3, false);

        // SwitchState("Base",0,true);
    }

    private void ObjectSnapped(SG_Grabable sgGrab)
    {
        objSnapped = true;
        recentlySnapped= sgGrab.name;
        if(!Object.HasStateAuthority){
        Rpc_ObjectSnapped(Object.Runner,sgGrab.name);
        }
        // Debug.Log($"Object Snapped{objSnapped}");
    }


    private bool IsPlayerJoined(){
        if(networkObject.Runner.ActivePlayers.Count() == 2){
            Debug.Log("2 Players has joined");
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool IsObjSnapped(string objName)
    {
        if(recentlySnapped==objName){
            recentlySnapped="";
            return true;
        }
        else{
            return false;

        }

    }

    public override void Spawned()
    {
        base.Spawned();
        Debug.Log(Object.HasStateAuthority);
        if(Object.HasStateAuthority){
        StartCoroutine(Exercise1());
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
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
    public  GameObject [] baseObj,verticalObj,horizontalObj;
    private static GameObject [] baseObjs,verticalObjs,horizontalObjs;
    private static Vector3 [] basessPos,verticalssPos,horizontalPos;
    private static Quaternion [] basessRot,verticalssRot,horizontalRot;
    // private static GameObject[] set1s, set2s, set3s, set4s;
    // private static SG_SnapDropZone [] baseSnapZoness,verticalSnapZoness;
    // private static SG_SnapDropZone [][] horizontalSnapZoness;

    public GameObject[] sets;
    private static GameObject[] bases, verticals, horizontalLowers, horizontalMiddles,parentCups,childCups;
    // private static GameObject =new GameObject
    private static SG_SnapDropZone[] baseSnapZones, verticalSnapZones;
    private static SG_SnapDropZone[,] horizontalLowerZones, horizontalMiddleZones;
    public static string recentlySnapped="";
    public static Vector3[] parentCupPos,childCupPos;
    public static Quaternion[] parentCupRot,childCupRot;
    private bool objSnapped;
    private bool resetObjSnapped;
    private string recentlySnappedObj;
    // private SG_SnapDropZone[] baseSnapZones, verticalSnapZones;
    // private SG_SnapDropZone[][] horizontalSnapZones;
    public NetworkObject networkObject;
    private static Coroutine routine;
    private static NetworkRunner nrunner;
    void Start()
    {
        nrunner=Runner;
        bases=new GameObject[4];
        verticals =new GameObject[4];
        horizontalLowers=new GameObject[4];
        horizontalMiddles=new GameObject[4];
        baseSnapZones=new SG_SnapDropZone[4];
        verticalSnapZones =new SG_SnapDropZone[4];
        horizontalLowerZones=new SG_SnapDropZone[4,2];
        horizontalMiddleZones=new SG_SnapDropZone[4,2];

        baseObjs=new GameObject[4];
        verticalObjs=new GameObject[4];
        horizontalObjs=new GameObject[1];

        basessPos=new Vector3[4];
        basessRot=new Quaternion[4];
        verticalssPos=new Vector3[4];
        verticalssRot=new Quaternion[4];
        horizontalPos=new Vector3[4];
        horizontalRot=new Quaternion[4];

        childCupPos=new Vector3[16];
        childCupRot=new Quaternion[16];

        parentCupPos=new Vector3[16];
        parentCupRot=new Quaternion[16];
        parentCups=GameObject.FindGameObjectsWithTag("parent_cup");
        childCups=GameObject.FindGameObjectsWithTag("movable_cup");
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
            horizontalLowerZones[i,0].ObjectSnapped.AddListener(ObjectSnapped);
                horizontalLowerZones[i,1] = sets[i].transform.Find("SnapObjects/HorizontalLower/Grab2").GetComponent<SG_SnapDropZone>();
            horizontalLowerZones[i,1].ObjectSnapped.AddListener(ObjectSnapped);


                horizontalMiddleZones[i,0] = sets[i].transform.Find("SnapObjects/HorizontalMiddle/Grab1").GetComponent<SG_SnapDropZone>();
            horizontalLowerZones[i, 0].ObjectSnapped.AddListener(ObjectSnapped);
                horizontalMiddleZones[i,1] = sets[i].transform.Find("SnapObjects/HorizontalMiddle/Grab2").GetComponent<SG_SnapDropZone>();
            horizontalMiddleZones[i, 1].ObjectSnapped.AddListener(ObjectSnapped);

        }


        for(int i=0;i<baseObj.Length;i++){
        baseObjs[i]= baseObj[i];
        basessPos[i] = baseObj[i].transform.position;
        basessRot[i] = baseObj[i].transform.rotation;
        }

        for (int i = 0; i < verticalObj.Length; i++)
        {
        verticalObjs[i] = verticalObj[i];
        verticalssPos[i] = verticalObj[i].transform.position;
        verticalssRot[i] = verticalObj[i].transform.rotation;
        }
        for (int i = 0; i < horizontalObj.Length; i++)
        {

        horizontalObjs[i]= horizontalObj[i];
        horizontalPos[i] = horizontalObj[i].transform.position;
        horizontalRot[i] = horizontalObj[i].transform.rotation;
        }
        for(int i=0;i<16;i++){
            parentCupPos[i]=parentCups[i].transform.localPosition;
            parentCupRot[i]=parentCups[i].transform.localRotation;
            childCupPos[i]=childCups[i].transform.localPosition;
            childCupRot[i]=childCups[i].transform.localRotation;
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            print("Initial Setup Done");
            // InitialSetup(this);
            // Rpc_InitialSetup(Object.Runner,this);
            Object.Runner.Shutdown();
            Scene scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
            // ReloadScene(Object.Runner);
            // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
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
    public static void Rpc_InitialSetup(NetworkRunner runner, NetworkCue networkCue)
    {

        //    networkCue.StartCoroutine(ReloadScene(runner));
        InitialSetup(networkCue);
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
            recentlySnapped="";
            yield return new WaitUntil(() => (IsObjSnapped("Base")));
            recentlySnapped = "";
            Debug.Log($"Base {i} snapped");
            // resetObjSnapped=true;
            SetActiveSnapzone("Base", i, false);
            SwitchState("Base", i, false);
            yield return new WaitForSeconds(1f);
        }
        for (int i = 0; i < 4; i++)
        {
            SwitchState("Vertical", i, true);
            SetActiveSnapzone("Vertical", i, true);
            Debug.Log($"Waiting for Vertical {i} to be snapped");
            recentlySnapped="";
            yield return new WaitUntil(() => (IsObjSnapped("Vertical")));
            Debug.Log($"Vertical {i} snapped");
            recentlySnapped="";
            // resetObjSnapped = true;
            SetActiveSnapzone("Vertical", i, false);
            SwitchState("Vertical", i, false);
             yield return new WaitForSeconds(1f);
        }
        SwitchState("HorizontalLower",0,true);
        SetActiveSnapzone("HorizontalLower", 0, true);
        recentlySnapped="";
        yield return new WaitUntil(() => (IsObjSnapped("HGrab")));
        recentlySnapped="";
        yield return new WaitForSeconds(1f);
        yield return new WaitUntil(() => (IsObjSnapped("HGrab")));
        recentlySnapped="";
        SwitchState("HorizontalLower", 0, false);
        SetActiveSnapzone("HorizontalLower", 0, false);

        // SwitchState("Base",0,true);
    }

    private void ObjectSnapped(SG_Grabable sgGrab)
    {
        objSnapped = true;
        if(!Object.HasStateAuthority){
        Rpc_ObjectSnapped(Object.Runner,sgGrab.name);
        }
        else{
        recentlySnapped= sgGrab.name;

        }
        Debug.Log($"Object Snapped{objSnapped}");
    }


    private bool IsPlayerJoined(){
        if(networkObject.Runner.ActivePlayers.Count() > 1){
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
            recentlySnapped = "";
            return true;
        }
        else{
            return false;

        }

    }
    private static void InitialSetup(NetworkCue networkCue){
        for (int i = 0; i < baseObjs.Length; i++)
        {
             baseObjs[i].transform.position=basessPos[i];
             baseObjs[i].transform.rotation=basessRot[i];
        }

        for (int i = 0; i < verticalObjs.Length; i++)
        {
            verticalObjs[i].transform.position=verticalssPos[i];
             verticalObjs[i].transform.rotation=verticalssRot[i];
        }
        for (int i = 0; i < horizontalObjs.Length; i++)
        {
            horizontalObjs[i].transform.position=horizontalPos[i];
             horizontalObjs[i].transform.rotation=horizontalRot[i];
        }
        for(int i=0;i<4;i++){
            baseSnapZones[i].enabled=false;
            verticalSnapZones[i].enabled=false;
            horizontalLowerZones[i,0].enabled=false;
            horizontalLowerZones[i,1].enabled=false;
            horizontalMiddleZones[i,0].enabled=false;
            horizontalMiddleZones[i,1].enabled=false;
                bases[i].SetActive(false);

                verticals[i].SetActive(false);

                horizontalLowers[i].SetActive(false);

                horizontalMiddles[i].SetActive(false);
        }
        for (int i = 0; i < 16; i++)
        {
            parentCups[i].transform.localPosition=parentCupPos[i] ;
            parentCups[i].transform.localRotation=parentCupRot[i] ;
            childCups[i].transform.localPosition=childCupPos[i] ;
            childCups[i].transform.localRotation=childCupRot[i] ;
        }

        if (routine != null)
        {
            networkCue.StopCoroutine(routine);
            routine = null;
        }
        routine=networkCue.StartCoroutine(networkCue.Exercise1());

    }
    public override void Spawned()
    {
        base.Spawned();
        Debug.Log(Object.HasStateAuthority);

        // StartCoroutine(Exercise1());
        if(Object.HasStateAuthority){
        StartCoroutine(Exercise1());
        }
    }
    static IEnumerator  ReloadScene(NetworkRunner runner)
    {
        // nrunner.SetActiveScene(SceneManager.GetSceneByName("LoadingScene").buildIndex);
        runner.Shutdown();
        yield return new WaitUntil(() => (runner.IsShutdown));
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
        yield return null;
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SG;

public class CueHandler : MonoBehaviour
{
    // Start is called before the first frame update
    // public int set=1;
    // public int objNo=1;
    // public bool state=false;
    private NetworkCue networkCue;
    // public SG_SnapDropZone dropZone1,dropZone2,dropZone3,dropZone4;
    // public Transform[] Set1SnapPoints,Set2SnapPoints,Set3SnapPoints,Set4SnapPoints;
    public SG_SnapDropZone[] dropZones1,dropZones2,dropZones3,dropZones4;
    // public Transform[] Set1SnapPoints=new Transform[4];
    private string recentlySnapped;
    void Start()
    {
        networkCue=GetComponent<NetworkCue>();
        for(int i=0;i<4;i++){

            // if(i != 0) networkCue.set1[i].SetActive(false);
            // networkCue.set2[i].SetActive(false);
            // networkCue.set3[i].SetActive(false);
            // networkCue.set4[i].SetActive(false);
        }
            for(int j=0;j<=6;j++){
            // dropZones1[j].ObjectSnapped.AddListener(ObjSnapped);
            // dropZones2[j].ObjectSnapped.AddListener(ObjSnapped);
            // dropZones3[j].ObjectSnapped.AddListener(ObjSnapped);
            // dropZones4[j].ObjectSnapped.AddListener(ObjSnapped);
            }

    }

    // Update is called once per frame
    void Update()
    {

    }
    // private void OnValidate(){
    //     GetComponent<NetworkCue>().SwitchState(set,objNo,state);
    // }

    IEnumerator Exercise1(){
        // networkCue.SwitchState(1,0,true);
        dropZones1[0].enabled=true;
        yield return new WaitUntil(() => (IsObjSnapped("Base")));
        dropZones1[0].enabled=false;
        dropZones1[1].enabled = true;
        yield return new WaitUntil(() => (IsObjSnapped("Vertical")));
        dropZones1[1].enabled = false;
        dropZones1[3].enabled=true;
        dropZones1[4].enabled=true;
    }


    public bool IsObjSnapped(string objName){
        if(objName==recentlySnapped){

        return true;
        }
        else return false;
    }

    // public void ObjSnapped(SG_Grabable sgGrab,SG_GrabScript sh){
    //     SG.SG_Tra
    // }
    private int GetObjIdx(string objName){
        int idx=0;
        switch(objName)
        {
            case "Base":
            idx=0;
            break;

            case "Vertical":
                idx = 1;
                break;

            case "HGrab11":
                idx = 2;
                break;

            case "HGrab12":
                idx = 2;
                break;

            case "HGrab21":
                idx = 3;
                break;

            case "HGrab22":
                idx = 3;
                break;

        }
        return idx;

    }
}

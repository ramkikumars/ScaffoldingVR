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
    public SG_SnapDropZone dropZone1;
    public Transform[] Set1SnapPoints=new Transform[4];
    void Start()
    {
        networkCue=GetComponent<NetworkCue>();
        for(int i=0;i<4;i++){

            if(i != 0) networkCue.set1[i].SetActive(false);
            networkCue.set2[i].SetActive(false);
            networkCue.set3[i].SetActive(false);
            networkCue.set4[i].SetActive(false);
        }

        dropZone1.ObjectDetected.AddListener(ObjectDetectedZone1);
    }

    // Update is called once per frame
    void Update()
    {

    }
// private void OnValidate(){
//     GetComponent<NetworkCue>().SwitchState(set,objNo,state);
// }

private void ObjectDetectedZone1(SG_Grabable sgGrabObj){
    string detectedObjName=sgGrabObj.name;
    if(detectedObjName=="Base"||detectedObjName == "Vertical"){
    dropZone1.snapPoint=Set1SnapPoints[GetObjIdx(sgGrabObj.name)];
    }
    else{
    dropZone1.snapPoint=sgGrabObj.gameObject.GetComponent<CollisionChecker>().collidedGameObject.transform;
    }
    Debug.Log($"Drop Zone 1 Detected{sgGrabObj.name}");

}
private void ObjectDetectedZone2(SG_Grabable sgGrabObj){

}
private void ObjectDetectedZone3(SG_Grabable sgGrabObj){

}
private void ObjectDetectedZone4(SG_Grabable sgGrabObj){

}
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

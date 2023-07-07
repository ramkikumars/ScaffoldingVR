using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CueHandler : MonoBehaviour
{
    // Start is called before the first frame update
    public int set=1;
    public int objNo=1;
    public bool state=false;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
private void OnValidate(){
    GetComponent<NetworkCue>().SwitchState(set,objNo,state);
}
}

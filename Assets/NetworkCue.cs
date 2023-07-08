using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
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
    [SerializeField]
    public GameObject [] set1,set2,set3,set4;
    public static GameObject[] set1s, set2s, set3s, set4s;
    void Start()
    {
        set1s=set1;
        set2s=set2;
        set3s=set3;
        set4s=set4;
    }

    // Update is called once per frame
    void Update()
    {

    }

    [Rpc]
    public static void Rpc_SwitchState(NetworkRunner runner, int set,int objidx,bool state) {
        switch (set)
        {
            case 1:
            set1s[objidx].SetActive(state);
            break;

            case 2:
                set2s[objidx].SetActive(state);
                break;
            case 3:
                set3s[objidx].SetActive(state);
                break;
            case 4:
                set4s[objidx].SetActive(state);
                break;

                        }
    }

    public void SwitchState(int set, int objidx, bool state){
        Rpc_SwitchState(Object.Runner,set, objidx, state);
    }
}

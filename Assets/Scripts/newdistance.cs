using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using TMPro.Examples;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class newdistance : MonoBehaviour
{
    [SerializeField] private Transform tip;
    [SerializeField] private Transform end;
    [SerializeField] private GameObject[] rulerParts;
    private float currentmax;

    // Update is called once per frame
    void Update()
    {
        float dist = Vector3.Distance(tip.position, end.position);

        int index = (int)(dist * 10);
        print(dist + " " + index);

        int listIndex = 0;
        foreach (GameObject rulerPart in rulerParts)
        {
            if (listIndex <= index&&dist>0)
                rulerPart.SetActive(true);
            else
                rulerPart.SetActive(false);

            listIndex++;
        }
        // for(int i=0;i<=index;i++){
        //     rulerParts[i].SetActive(true);
        // }
        // foreach (GameObject rulerPart in rulerParts){
        // if (dist < currentmax){
        //     rulerPart.SetActive(false);
        //     currentmax = dist;
        //     }
        // else if(dist > currentmax && dist < 10){
        //     rulerPart.SetActive(true);
        //     currentmax = dist;
        // }
        // }

    }
}
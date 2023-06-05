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


    // Update is called once per frame
    void Update()
    {
        float dist = Vector3.Distance(tip.position, end.position);

        int index = (int)(dist * 10);
        print(dist + " " + index);

        int listIndex = 0;
        foreach (GameObject rulerPart in rulerParts)
        {
            if (listIndex <= index)
                rulerPart.SetActive(true);
            else
                rulerPart.SetActive(false);

            listIndex++;
        }

    }
}
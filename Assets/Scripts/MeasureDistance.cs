using UnityEngine;
using TMPro;
using System;
using System.Collections.Generic;
public class MeasureDistance : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject plane1, plane2, sphere;
    [SerializeField] Transform anchor1, anchor2;
    [SerializeField] private TextMeshPro distText;
    [SerializeField] private LineRenderer line1, line2;
    [SerializeField] private Color targetStateColor;
    [SerializeField] private Vector3 offset = new Vector3(0, 0, -0.1f);
    public float targetDist = 0.5f;
    [Range(0f, 0.09f)]
    private float tolerance = 0.001f;
    private Color defaultColor;
    private Vector3 plane1Pos, plane2Pos, midPos, plane1ToSphere, plane2ToSphere, transformOffset, tempVect;
    private float dist, sphereRadius;
    private Material sphereMaterial;
    [System.NonSerialized]
    public bool reachedTarget;
    // private void OnEnable()
    // {
    //     UpdatePos();
    //     CheckTargetDist();
    // }
    void Start()
    {
        line1.useWorldSpace = true;
        line2.useWorldSpace = true;
        // offset = new Vector3(0, 0, -0.1f);
        sphereRadius = sphere.transform.localScale.x / 2f;
        // Debug.Log(sphereRadius);
        sphereMaterial = sphere.GetComponent<Renderer>().material;
        defaultColor = sphereMaterial.color;

        // plane1.transform.rotation = anchor1.rotation;
        // plane2.transform.rotation = anchor2.rotation;
        // plane1.transform.parent = anchor1;
        // plane2.transform.parent = anchor2;
        anchor1.transform.hasChanged = false;
        anchor2.transform.hasChanged = false;
        // Debug.Log($"Target dist{targetDist}");
        reachedTarget = false;
        UpdatePos();
        CheckTargetDist();
    }
    // Update is called once per frame
    void Update()
    {

        UpdatePos();
        if (anchor1.transform.hasChanged || anchor2.transform.hasChanged)
        {
            CheckTargetDist();

            anchor1.transform.hasChanged = false;
            anchor2.transform.hasChanged = false;
        }
    }
    void UpdatePos()
    {
        tempVect = plane1.transform.position;
        tempVect.y = anchor1.position.y;
        plane1.transform.position = tempVect;
        tempVect = plane2.transform.position;
        tempVect.y = anchor2.position.y;
        plane2.transform.position = tempVect;
        plane1Pos = anchor1.transform.TransformPoint(offset);
        plane2Pos = anchor2.transform.TransformPoint(offset);
        line1.SetPosition(0, plane1Pos);
        line2.SetPosition(0, plane2Pos);
        dist = Vector3.Distance(plane1Pos, plane2Pos);
        distText.text = dist.ToString("F3");
        midPos = Vector3.Lerp(plane1Pos, plane2Pos, 0.5f);
        sphere.transform.position = midPos;
        plane1ToSphere = midPos;
        plane1ToSphere.y -= sphereRadius;
        plane2ToSphere = midPos;
        plane2ToSphere.y += sphereRadius;
        line1.SetPosition(1, plane1ToSphere);
        line2.SetPosition(1, plane2ToSphere);

    }
    private void CheckTargetDist()
    {
        if (tolerance != 0f)
        {
            if (targetDist - tolerance <= dist && dist <= targetDist + tolerance)
            {
                sphereMaterial.color = targetStateColor;
                reachedTarget = true;
            }
            else
            {
                sphereMaterial.color = defaultColor;
            }
        }
        else
        {
            if (targetDist == dist)
            {
                sphereMaterial.color = targetStateColor;
                reachedTarget = true;
            }
            else
            {
                sphereMaterial.color = defaultColor;
            }
        }
    }

}

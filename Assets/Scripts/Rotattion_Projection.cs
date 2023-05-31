using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotattion_Projection : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject real_pt, base_pt, proj_pt, cube; //right_hand;
    Vector3 real_pos, base_pos, proj_pos, local_proj_pos, rot;
    // Quaternion rot;
    float angle;
    void Start()
    {
        Debug.Log(transform.TransformPoint(Vector3.right));
    }

    // Update is called once per frame
    void Update()
    {
        base_pos = transform.position;
        base_pt.transform.position = base_pos;
        real_pt.transform.position = real_pt.transform.position;
        proj_pos = real_pt.transform.position;
        proj_pos.y = base_pos.y;
        proj_pt.transform.position = proj_pos;
        local_proj_pos = transform.InverseTransformPoint(proj_pos).normalized;
        // angle = Mathf.Atan2(local_proj_pos.x, local_proj_pos.z) * Mathf.Rad2Deg;
        // // angle = Vector3.Angle(Vector3.forward, local_proj_pos);
        // Debug.Log("Angele: " + angle);
        // Quaternion myRotation = Quaternion.identity;
        // myRotation.eulerAngles = new Vector3(0, angle, 0);
        Quaternion myRotation = Quaternion.FromToRotation(Vector3.right, local_proj_pos);
        cube.transform.rotation = myRotation;
        Debug.Log("Angle: " + myRotation.eulerAngles);
    }
}

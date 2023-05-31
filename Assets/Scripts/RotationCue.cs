using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationCue : MonoBehaviour
{
    // Start is called before the first frame update
    private Quaternion start, end, initialRot;
    private float startAngle, endAngle, currentAngle, initialAngle;
    private float currentLerpTime;
    private string setDir;
    private Transform arrow;
    // [SerializeField] private float upperLimit = 100f;
    // private bool flag = false;
    [SerializeField] private float lerpTime = 5f;
    [SerializeField] private TrailRenderer trail;
    void Start()
    {
        // start=new Quaternion();
        // start = Quaternion.AngleAxis(startAngle, Vector3.up);
        // end = Quaternion.AngleAxis(endAngle, Vector3.up);
        initialRot = transform.rotation;
        initialAngle = initialRot.eulerAngles.y;
        arrow = this.transform.Find("Arrow");
        ChangeDirection("CW");
    }

    // Update is called once per frame
    void Update()
    {
        // transform.rotation = Quaternion.RotateTowards(transform.rotation, end, turningRate * Time.deltaTime);
        transform.rotation = Quaternion.Lerp(start, end, currentLerpTime / lerpTime) * initialRot;
        currentLerpTime += Time.deltaTime;
        currentAngle = Mathf.Abs(transform.rotation.eulerAngles.y - initialAngle);
        // Debug.Log(currentAngle);
        if ((currentLerpTime / lerpTime) >= 1f)
        {
            currentLerpTime = 0;
            // trail.enabled = false;
        }
        if (setDir == "CW")
        {

            if (currentAngle >= 160f)
            {
                trail.emitting = false;
                // Debug.Log($"Reached End Range");
                // flag = false;
            }
            if (currentAngle <= 30f && currentAngle >= 10f && !trail.emitting)//&& transform.rotation.eulerAngles.y > upperLimit && flag)
            {
                trail.emitting = true;
                // Debug.Log("Reached Start Range");
            }
        }
        if (setDir == "CCW")
        {
            if (currentAngle <= 30f)
            {
                trail.emitting = false;
                Debug.Log($"Reached End Range");
                // flag = false;
            }
            if (currentAngle >= 150f && currentAngle <= 170f && !trail.emitting)//&& transform.rotation.eulerAngles.y > upperLimit && flag)
            {
                trail.emitting = true;
                Debug.Log("Reached Start Range");
            }
        }

        // if(){}


    }
    public void ChangeDirection(string dir)
    {
        Vector3 rot = arrow.rotation.eulerAngles;
        if (dir == "CW")
        {
            rot.y = 180f;
            startAngle = 0f;
            endAngle = -180f;
        }
        if (dir == "CCW")
        {
            rot.y = 0f;
            startAngle = -180f;
            endAngle = 0f;
        }
        setDir = dir;
        arrow.rotation = Quaternion.Euler(rot);
        start = Quaternion.AngleAxis(startAngle, Vector3.up);
        end = Quaternion.AngleAxis(endAngle, Vector3.up);
    }


}

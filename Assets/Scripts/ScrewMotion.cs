using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrewMotion : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject rotateObj, translateObj;
    [SerializeField] float pitch = 1;
    private Vector3 tempVect, newPos;
    private float dist, unit_dist, intitialAngle, lastAngle, CurrentAngle;
    float lerpTime = 1;
    void Start()
    {
        intitialAngle = rotateObj.transform.localRotation.eulerAngles.y;
        lastAngle = intitialAngle;
        unit_dist = pitch / 360f;
    }

    // Update is called once per frame
    void Update()
    {
        if (rotateObj.transform.hasChanged)
        {
            CurrentAngle = Mathf.Round(rotateObj.transform.localRotation.eulerAngles.y);
            if (Mathf.Abs(CurrentAngle - lastAngle) >= 2)
            {
                tempVect.y = 2f * unit_dist;
                if (lastAngle > CurrentAngle)
                {
                    tempVect = -1 * tempVect;
                }
                newPos = translateObj.transform.localPosition + tempVect;
                translateObj.transform.localPosition = Vector3.Lerp(translateObj.transform.localPosition, newPos, lerpTime * Time.deltaTime);
                lastAngle = CurrentAngle;
            }
            rotateObj.transform.hasChanged = false;

        }
    }
}

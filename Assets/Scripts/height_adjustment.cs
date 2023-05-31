using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UltimateXR.Manipulation;
using UltimateXR.Avatar;
using System;
using UltimateXR.Haptics;

public class height_adjustment : MonoBehaviour

{

    [SerializeField] private GameObject wingnut;
    [SerializeField] private GameObject wingnut_parent;
    [SerializeField] private UxrGrabbableObject wingnutGrabbable;
    [SerializeField] private TextMeshPro heightText;
    [SerializeField] float haptic_amplitude = 0.6f;
    [SerializeField] float pitch = 1;
    [SerializeField] LineRenderer heightLine;
    [SerializeField] bool haptics;
    [SerializeField] UxrHapticClipType hapticClipType = UxrHapticClipType.Click;
    private Vector3 wingnutPos, tempVect, newPos, fromVec, toVec, rotAxis;
    private Rigidbody wingnut_rb;
    private float dist, unit_dist, intitialAngle, angleRotated, lastAngle, newDist, CurrentAngle;
    float lerpTime = 1;
    // int _linearSpeed = 1;
    // int dir_change = 1;
    void Start()
    {


        heightLine.SetPosition(0, new Vector3(0, 0, 0));
        unit_dist = pitch / 360f;
        // basejackPos = basejack.transform.position;
        wingnut_rb = wingnut.GetComponent<Rigidbody>();
        wingnutGrabbable.Grabbed += wingnut_grabbed;
        wingnutGrabbable.Released += wingnut_released;
        intitialAngle = wingnut.transform.localRotation.eulerAngles.y;
        lastAngle = intitialAngle;
        wingnutPos = wingnut.transform.localPosition;
        heightLine.SetPosition(1, wingnutPos);
        intitialAngle = wingnut.transform.localRotation.eulerAngles.y;
        rotAxis = Vector3.up;
        fromVec = Vector3.forward;
    }


    void Update()
    {
        if (wingnut.transform.hasChanged)
        {
            // CurrentAngle = Mathf.Round(UnityEditor.TransformUtils.GetInspectorRotation(wingnut.transform).y);
            CurrentAngle = Mathf.Round(wingnut.transform.localRotation.eulerAngles.y);
            // toVec = transform.forward;
            // float anglerot = Mathf.Round(Vector3.Angle(fromVec, toVec));
            // angleRotated = Mathf.Abs(intitialAngle - CurrentAngle);
            // Debug.Log(CurrentAngle);
            newDist = angleRotated * unit_dist;
            // Debug.Log(newDist);
            // tempVect = tempVect * _linearSpeed * Time.deltaTime;
            if (Mathf.Abs(CurrentAngle - lastAngle) >= 2)
            {
                tempVect.y = 2f * unit_dist;
                if (lastAngle > CurrentAngle)
                {
                    tempVect = -1 * tempVect;
                }
                newPos = wingnut_parent.transform.localPosition + tempVect;
                heightLine.SetPosition(1, newPos);
                heightText.text = (newPos).y.ToString("F2");
                // wingnutGrabbable.InitialLocalPosition = newPos;
                // wingnutGrabbable.InitialLocalEulerAngles = wingnut.transform.localEulerAngles;
                // wingnutGrabbable.ConstrainTransform(false, false, transform.localPosition, transform.localRotation);
                // tempVect = tempVect.normalized * 2 * Time.deltaTime;
                // wingnut_rb.MovePosition(wingnut.transform.TransformPoint(tempVect));
                // wingnut.transform.localPosition = Vector3.Lerp(wingnut.transform.localPosition, tempVect, lerpTime * Time.deltaTime);
                wingnut_parent.transform.localPosition = Vector3.Lerp(wingnut_parent.transform.localPosition, newPos, lerpTime * Time.deltaTime);

                if (haptics)
                {
                    UxrAvatar.LocalAvatar.ControllerInput.SendGrabbableHapticFeedback(wingnutGrabbable, hapticClipType, haptic_amplitude);
                }
                lastAngle = CurrentAngle;
                // wingnut.transform.localPosition += tempVect;
            }
            wingnut.transform.hasChanged = false;

        }

    }
    private void wingnut_grabbed(object sender, UxrManipulationEventArgs e)
    {
        // wingnutGrabbable.TranslationConstraint = UxrTranslationConstraintMode.Locked;
        wingnutPos = wingnut.transform.localPosition;
        intitialAngle = wingnut.transform.localRotation.eulerAngles.y;
        // wingnutGrabbable.IsLockedInPlace = true;
    }
    private void wingnut_released(object sender, UxrManipulationEventArgs e)
    {
        // wingnutGrabbable.TranslationConstraint = UxrTranslationConstraintMode.RestrictLocalOffset;
        // wingnut.transform.localPosition += tempVect;
        // wingnutGrabbable.IsLockedInPlace = false;
        // newPos = wingnut.transform.TransformPoint(newPos);
        // wingnut_rb.MovePosition(newPos * Time.deltaTime * 2);
        // wingnutGrabbable.InitialLocalPosition = wingnut.transform.localPosition;
    }
    public static float Clamp0360(float angle)
    {
        {
            angle %= 360;
            if (angle > 180)
                return angle - 360;

            return angle;
        }
    }
}

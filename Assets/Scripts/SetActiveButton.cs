using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using System;

[Serializable]
// public class BoolEvent : UnityEvent<bool> { }
// public class ButtonOff : UnityEvent<bool> { }
public class SetActiveButton : MonoBehaviour
{
    private string firstColliderName = "Sphere 1";
    private string secondColliderName = "Sphere 2";
    private bool firstCollided = false;
    private bool buttonState = false;
    // public BoolEvent ButtonOn, ButtonOff;
    [SerializeField] GameObject pointer1, pointer2, pointerAnchor1, pointerAnchor2, objToActivate;
    [SerializeField] private TextMeshPro buttonText;

    private void Start()
    {
        pointer1.transform.position = pointerAnchor1.transform.position;
        // pointer2.transform.position = pointerAnchor2.transform.position;
        pointer1.transform.parent = pointerAnchor1.transform;
        // pointer2.transform.parent = pointerAnchor2.transform;
        ChangeButtonState();
    }
    private void OnCollisionEnter(Collision other)
    {
        string colliderName = other.gameObject.name;
        if (colliderName == firstColliderName)
        {
            firstCollided = true;
        }

        if (colliderName == secondColliderName && firstCollided)
        {
            buttonState = !buttonState;
            ChangeButtonState();
            firstCollided = false;
        }
    }
    private void ChangeButtonState()
    {
        if (buttonState)
        {
            GetComponent<SpriteRenderer>().color = Color.clear;
            buttonText.text = "Measure: On";
            objToActivate.SetActive(true);
            // ButtonOn.Invoke(true);
        }
        else
        {
            GetComponent<SpriteRenderer>().color = Color.red;
            buttonText.text = "Measure: Off";
            objToActivate.SetActive(false);
            // ButtonOff.Invoke(false);
        }
    }
    // private void OnButtonPress()
    // {

    // }
    // private void OnButton()
    // {

    // }

}

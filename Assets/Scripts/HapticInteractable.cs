using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.Events;
[System.Serializable]
public class Haptic
{
    [Range(0, 1)]
    public float intensity;
    public float duration;
    public UnityEvent TriggerEvent;
    public void SetListener(XRBaseController controller)
    {
        TriggerEvent.AddListener(delegate { TriggerHaptic(controller); });
        Debug.Log("Listener Added");
    }
    private void TriggerHaptic(XRBaseController controller)
    {
        if (intensity > 0)
        {
            controller.SendHapticImpulse(intensity, duration);
        }
    }

}
public class HapticInteractable : MonoBehaviour
{
    // Start is called before the first frame update
    public Haptic[] haptics;
    private XRBaseController controller;
    void Start()
    {
        XRGrabInteractable grabInteractable = GetComponent<XRGrabInteractable>();
        XRBaseInteractable interactable = GetComponent<XRBaseInteractable>();
        interactable.selectEntered.AddListener(ObjGrabbed);
    }

    // Update is called  once per frame
    public void ObjGrabbed(BaseInteractionEventArgs eventArgs)
    {
        if (eventArgs.interactorObject is XRBaseControllerInteractor controllerInteractor)
        {
            controller = controllerInteractor.xrController;
        }
        foreach (Haptic x in haptics)
        {
            x.SetListener(controller);
        }
    }

}

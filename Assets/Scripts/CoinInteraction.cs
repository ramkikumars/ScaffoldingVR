using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class CoinInteraction : MonoBehaviour
{
    // Start is called before the first frame update
    private Material objMaterial;
    private Color originalColor;
    [SerializeField] private bool coinState = false;
    [SerializeField] private Color onColor, offColor;
    public UnityEvent StateOn, StateOff;
    void Start()
    {
        objMaterial = GetComponent<Renderer>().material;
        originalColor = objMaterial.color;
        ChangeState();
    }



    // private void OnCollisionEnter(Collision other)
    // {
    //     coinState = !coinState;
    //     // Debug.Log("Collided");
    //     ChangeState();
    // }
    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Collider")
        {
            Debug.Log("Coin hit with Collider");
            coinState = !coinState;
            ChangeState();
        }
    }
    private void ChangeState()
    {
        if (coinState)
        {
            StateOn.Invoke();
            objMaterial.color = onColor;

        }
        else
        {
            StateOff.Invoke();
            objMaterial.color = offColor;
        }
    }
}

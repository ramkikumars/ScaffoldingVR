using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsRig : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Transform playerHead;
    [SerializeField] private CapsuleCollider bodyCollider;

    [SerializeField] private float bodyHeightMin = 0.5f;
    [SerializeField] private float bodyHeightMax = 2;
    [SerializeField] private Vector3 newPos;
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        newPos = playerHead.position;
        newPos.y = 0f;
        transform.position = newPos;
        bodyCollider.height = Mathf.Clamp(playerHead.localPosition.y, bodyHeightMin, bodyHeightMax);
        // bodyCollider.center = new Vector3(playerHead.localPosition.x, bodyCollider.height / 2, playerHead.localPosition.z);
    }
}

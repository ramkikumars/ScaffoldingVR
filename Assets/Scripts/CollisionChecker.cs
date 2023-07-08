using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionChecker : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject collidedGameObject;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    //Upon collision with another GameObject, this GameObject will reverse direction
    private void OnTriggerEnter(Collider other)
    {
        collidedGameObject=other.gameObject;
        Debug.Log($"Collided with{collidedGameObject.name}");
    }
}

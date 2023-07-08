using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtEachOther : MonoBehaviour
{
    public Transform obj1; // Reference to the target object
    public Transform obj2;
    private void Update()
    {
        // Calculate the direction from this object to the target
        Vector3 direction = obj1.position - obj2.position;

        // // Project the direction onto the XZ plane
        // direction.y = 0;

        if (direction != Vector3.zero)
        {
            // Rotate this object to look at the target
            obj1.rotation = Quaternion.LookRotation(direction);
            obj1.localRotation*=Quaternion.Euler(-90f, 0, 0);
            // Rotate the target objsect to look at this object
            obj2.rotation = Quaternion.LookRotation(direction);
            obj2.localRotation *= Quaternion.Euler(-90f, 0, 0);
        }
    }
}

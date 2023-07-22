using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtEachOther : MonoBehaviour
{
    public GameObject obj1; // Reference to the target object
    public GameObject obj2;
    public GameObject horizontal;
    private void Update()
    {
        // Calculate the direction from this object to the target
        Vector3 direction = obj1.transform.position - obj2.transform.position;

        // // Project the direction onto the XZ plane
        // direction.y = 0;

        if (direction != Vector3.zero)
        {
            // Rotate this object to look at the target
            obj1.transform.rotation = Quaternion.LookRotation(direction);
            obj1.transform.localRotation*=Quaternion.Euler(-90f, 0, 0);
            // Rotate the target objsect to look at this object
            obj2.transform.rotation = Quaternion.LookRotation(direction);
            obj2.transform.localRotation *= Quaternion.Euler(-90f, 0, 0);
            horizontal.transform.position=(obj1.transform.position+obj2.transform.position)/2;
            horizontal.transform.rotation=Quaternion.LookRotation(direction);
            horizontal.transform.rotation*=Quaternion.Euler(0,90f,0);

        }
    }
}

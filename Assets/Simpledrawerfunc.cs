using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SG;

public class Simpledrawerfunc : MonoBehaviour
{
    public SG_SimpleDrawer simpleDrawer;

    private void Update()
    {
        float inputY = Input.GetAxis("Vertical"); // Get input along the Y-axis (up/down arrow keys or W/S keys)

        // Calculate the target position for the drawer based on the input
        Vector3 targetPosition = transform.position + Vector3.up * inputY;

        // Use the SimpleDrawer object to restrict the movement along the Y-axis
        SG_SimpleDrawer.CalculateDrawerTarget(simpleDrawer, targetPosition, out Vector3 restrictedPos, out Quaternion targetRot, out float drawerDist);

        // Move the game object to the restricted position
    }
}

// using System.Nonser;
// using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG
{
    /// <summary> A SG_Interactable that rotates along a single (local) axis. </summary>
    public class SG_FixedRod : SG_Grabable
    {


        //--------------------------------------------------------------------------------------------------------------------------------------------------------
        // Member Variables

        /// <summary> Local(!) axis along which to move. </summary>

        private Vector3 initialPos;
        private Quaternion initialRot;
        protected override void SetupScript()
        {
            base.SetupScript();
            // RigidbodyConstraints rotation_y_axis = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
            // this.SetPhysicsbody(false, this.physicsBody != null ? this.physicsBody.isKinematic : true, rotation_y_axis); //unlock the movement, freeze the rotation.
            this.UpdateRigidbodyDefaults(); //should always return to this.
            // RecalculateBaseLocation();
        }



        protected override void MoveToTargetLocation(Vector3 targetPosition, Quaternion targetRotation, float dT)
        {
            List<GrabArguments> heldBy = this.grabbedBy;
            Vector3 realPosition = heldBy[0].GrabScript.realGrabRefrence.position;

            transform.position = initialPos;
            transform.rotation = initialRot;

        }


        //--------------------------------------------------------------------------------------------------------------------------------------------------------
        // Drawer Functions



        // /// <summary> Set the current location of the drawer as "0" for the drawerDistance. </summary>
        // public void RecalculateBaseLocation()
        // {
        //     SG.Util.SG_Util.CalculateBaseLocation(this.MyTransform, out localStartPos, out localStartRot);
        // }

        // /// <summary> Returns the position & rotation of the Drawer's base. </summary>
        // public void GetBaseLocation(out Vector3 basePos, out Quaternion baseRot)
        // {
        //     SG.Util.SG_Util.GetCurrentBaseLocation(this.MyTransform, localStartPos, localStartRot, out basePos, out baseRot);
        // }




        // public void CalculateRotationTarget(Vector3 realPositon, out Vector3 targetPos, out Quaternion targetRot)
        // {

        //    Vector3 diff=realPositon-refObject.position;
        //    Vector3 normalisedDiff=diff.normalized;
        //    targetPos=(normalisedDiff*distBwObjects)+refObject.position;
        //    targetRot=transform.rotation;
        // }

        protected override void Start()
        {
            base.Start();
            initialPos = transform.position;
            initialRot = transform.rotation;
            // distBwObjects = Vector3.Distance(transform.position, refObject.position);

        }


    }

}
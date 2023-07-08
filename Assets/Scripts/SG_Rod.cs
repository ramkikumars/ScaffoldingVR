// using System.Nonser;
// using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UltimateXR.Extensions.Unity.Math;
using UltimateXR.Extensions.System.Math;
using UnityEngine.Events;
using System.IO;
using UltimateXR.Manipulation;
namespace SG
{
    /// <summary> A SG_Interactable that rotates along a single (local) axis. </summary>
    public class SG_Rod : SG_Grabable
    {


        //--------------------------------------------------------------------------------------------------------------------------------------------------------
        // Member Variables

        /// <summary> Local(!) axis along which to move. </summary>
        [Header("Drawer Components")]


        [SerializeField]
        private Transform refObject;
        private float distBwObjects;
        // public GameObject ball1;
        // public Vector3 dir;
        //--------------------------------------------------------------------------------------------------------------------------------------------------------
        // SG_Grabable Overrides
                protected override void SetupScript()
        {
            base.SetupScript();
            // RigidbodyConstraints rotation_y_axis = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
            // this.SetPhysicsbody(false, this.physicsBody != null ? this.physicsBody.isKinematic : true, rotation_y_axis); //unlock the movement, freeze the rotation.
            this.UpdateRigidbodyDefaults(); //should always return to this.
            // RecalculateBaseLocation();
        }


        // /// <summary> Updates the drawer's position </summary>
        // /// <param name="dT"></param>
        // protected override void UpdateLocation(float dT)
        // {
        //     List<GrabArguments> heldBy = this.grabbedBy;
        //     if (heldBy.Count > 0) //I'm actually grabbed by something
        //     {
        //         Vector3 targetPosition, realPosition; Quaternion targetRotation;
        //         CalculateTargetLocation(heldBy, out targetPosition, out targetRotation);
        //         ball1.transform.position = targetPosition;
        //         realPosition = heldBy[0].GrabScript.realGrabRefrence.position;
        //         Vector3 projPos; Quaternion projRot;
        //         CalculateRotationTarget(targetPosition, out projPos, out projRot);
        //         MoveToTargetLocation(projPos, projRot, dT);
        //     }
        //     else if (this.IsMovedByPhysics) //I have a physicsBody
        //     {
        //         // throw new System.NotImplementedException("The SG_Drawer feature is not yet available for non-Kinematic Rigidbodies.");
        //     }
        // }

        protected override void MoveToTargetLocation(Vector3 targetPosition, Quaternion targetRotation, float dT)
        {
            List<GrabArguments> heldBy = this.grabbedBy;
            Vector3 realPosition = heldBy[0].GrabScript.realGrabRefrence.position;
            // ball1.transform.position=realPosition;
            Vector3 diff = realPosition- refObject.position;
            Vector3 normalisedDiff = diff.normalized*distBwObjects;
            transform.position=normalisedDiff+refObject.position;
            // refObject.rotation=Quaternion.LookRotation(diff);
            // refObject.localRotation*=Quaternion.Euler(-90f, 0, 0);
            // transform.rotation=Quaternion.LookRotation(diff);
            // transform.localRotation*=Quaternion.Euler(-90f,0,0);

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

        public void ObjGrabbed(Object obj1, Object obj2)
        {



        }
        public void ObjReleased(object obj1, object obj2)
        {

        }


        protected override void Awake()
        {
            base.Awake();
        }

        protected override void Start()
        {
            base.Start();
           distBwObjects=Vector3.Distance(transform.position,refObject.position);
        }


    }

}
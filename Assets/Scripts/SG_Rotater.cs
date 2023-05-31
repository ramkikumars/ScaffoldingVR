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
    public class SG_Rotater : SG_Grabable
    {


        //--------------------------------------------------------------------------------------------------------------------------------------------------------
        // Member Variables

        /// <summary> Local(!) axis along which to move. </summary>
        [Header("Drawer Components")]

        protected Vector3 localStartPos = Vector3.zero;
        protected Quaternion localStartRot = Quaternion.identity;
        protected Quaternion grabbedRot = Quaternion.identity;
        [System.NonSerialized]
        public Vector3 ref_pos;
        private float _singleRotationAngleGrab, _singleRotationAngleCumulative;
        [SerializeField] private Vector3 rotationAxis = Vector3.back;
        // private GameObject ball1, ball2, ball3, ball4, ball5;
        // [NonSerialized] public Vector3 realPos;
        // [SerializeField] private Vector3 _translationLimitsMin = Vector3.zero;
        // [SerializeField] private Vector3 _translationLimitsMax = Vector3.zero;
        // [SerializeField] private bool _translationLimitsReferenceIsParent = true;
        // [SerializeField] private Transform _translationLimitsParent;
        public Vector3 _rotationAngleLimitsMin = Vector3.zero;
        public Vector3 _rotationAngleLimitsMax = Vector3.zero;
        // [SerializeField] private GameObject dText;//, ball2, ball3, ball4, ball5;
        // [SerializeField] private TextMeshPro dText;//, ball2, ball3, ball4, ball5;
        // [SerializeField] public Transform refPosistion;

        //--------------------------------------------------------------------------------------------------------------------------------------------------------
        // SG_Grabable Overrides

        protected override void SetupScript()
        {
            base.SetupScript();
            // RigidbodyConstraints rotation_y_axis = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
            // this.SetPhysicsbody(false, this.physicsBody != null ? this.physicsBody.isKinematic : true, rotation_y_axis); //unlock the movement, freeze the rotation.
            this.UpdateRigidbodyDefaults(); //should always return to this.
            RecalculateBaseLocation();
        }


        /// <summary> Updates the drawer's position </summary>
        /// <param name="dT"></param>
        protected override void UpdateLocation(float dT)
        {
            List<GrabArguments> heldBy = this.grabbedBy;
            if (heldBy.Count > 0) //I'm actually grabbed by something
            {
                Vector3 targetPosition, realPosition; Quaternion targetRotation;
                CalculateTargetLocation(heldBy, out targetPosition, out targetRotation);
                realPosition = heldBy[0].GrabScript.realGrabRefrence.position;
                Vector3 projPos; Quaternion projRot;
                CalculateRotationTarget(realPosition, out projPos, out projRot);
                // ball1.transform.position = realPosition;
                // ball1.GetComponentInChildren<TextMesh>().text = "Real";
                // ball2.transform.position = projPos;
                // ball2.GetComponentInChildren<TextMesh>().text = "BasePos";
                MoveToTargetLocation(projPos, projRot, dT);
            }
            else if (this.IsMovedByPhysics) //I have a physicsBody
            {
                // throw new System.NotImplementedException("The SG_Drawer feature is not yet available for non-Kinematic Rigidbodies.");
            }
        }


        //--------------------------------------------------------------------------------------------------------------------------------------------------------
        // Drawer Functions



        /// <summary> Set the current location of the drawer as "0" for the drawerDistance. </summary>
        public void RecalculateBaseLocation()
        {
            SG.Util.SG_Util.CalculateBaseLocation(this.MyTransform, out localStartPos, out localStartRot);
        }

        /// <summary> Returns the position & rotation of the Drawer's base. </summary>
        public void GetBaseLocation(out Vector3 basePos, out Quaternion baseRot)
        {
            SG.Util.SG_Util.GetCurrentBaseLocation(this.MyTransform, localStartPos, localStartRot, out basePos, out baseRot);
        }




        public void CalculateRotationTarget(Vector3 realPositon, out Vector3 targetPos, out Quaternion targetRot)
        {
            Vector3 basePos;
            Quaternion baseRot;
            GetBaseLocation(out basePos, out targetRot);
            baseRot = targetRot;
            Quaternion ls = lastRotation;
            // // Project targetPosition onto the movement "plane"
            // // Vector3 projectedPos = SG.Util.SG_Util.ProjectOnTransform(nextPositon, basePos, targetRot);
            // Vector3 projectedPos = realPositon - transform.position;
            // Vector3 fromDir = ref_pos - transform.position;
            // Vector3 toDir = projectedPos;
            // Quaternion rot = Quaternion.FromToRotation(fromDir.normalized, toDir.normalized);//* baseRot;
            // Quaternion rot1 = ClampRotation(rot, lastRotation, baseRot, _rotationAngleLimitsMin, _rotationAngleLimitsMax);
            Vector3 grabDirection = realPositon - transform.position;
            Vector3 initialGrabDirection = ref_pos - transform.position;
            Quaternion initialLocalRotation = baseRot;
            // rotationAxis = Vector3.back;
            Vector3 projectedGrabDirection = Vector3.ProjectOnPlane(grabDirection, rotationAxis);
            Vector3 projectedInitialGrabDirection = Vector3.ProjectOnPlane(initialGrabDirection, rotationAxis);
            float angle = Vector3.SignedAngle(projectedInitialGrabDirection, projectedGrabDirection, rotationAxis);
            float angleDelta = angle - _singleRotationAngleGrab.ToEuler180();

            // Keep track of turns below/above -360/360 degrees.

            if (angleDelta > 180.0f)
            {
                _singleRotationAngleGrab -= 360.0f - angleDelta;
            }
            else if (angleDelta < -180.0f)
            {
                _singleRotationAngleGrab += 360.0f + angleDelta;
            }
            else
            {
                _singleRotationAngleGrab += angleDelta;
            }

            // Clamp inside valid range

            float rotationAngle = (_singleRotationAngleCumulative + _singleRotationAngleGrab).Clamped(_rotationAngleLimitsMin[1], _rotationAngleLimitsMax[1]);
            _singleRotationAngleGrab = rotationAngle - _singleRotationAngleCumulative;

            // Rotate using absolute current rotation to preserve precision

            // transform.localRotation = initialLocalRotation * Quaternion.AngleAxis(rotationAngle, rotationAxis);
            Quaternion rot = initialLocalRotation * Quaternion.AngleAxis(rotationAngle, rotationAxis);
            // Quaternion rot = Quaternion.AngleAxis(rotationAngle, rotationAxis);
            targetPos = basePos;
            targetRot = rot;
            // dText.text = $" signed angle: {angle}<br>_singgleangle: {_singleRotationAngleGrab}<br>clamped angle: {rotationAngle}<br>final: {rot.eulerAngles}";
        }

        public void ObjGrabbed(Object obj1, Object obj2)
        {

            Vector3 realPos = this.grabbedBy[0].GrabScript.realGrabRefrence.position;
            Vector3 localRealPos = transform.InverseTransformPoint(realPos);
            localRealPos.y = 0;
            // localRealPos.z = 0;
            if (localRealPos.x > 0)
            {
                ref_pos = transform.TransformPoint(new Vector3(0.09f, 0, 0));
            }
            else if (localRealPos.x < 0)
            {
                ref_pos = transform.TransformPoint(new Vector3(-0.09f, 0, 0));
            }
            ref_pos = transform.TransformPoint(localRealPos);
            // ball3.transform.position = realPos;
            // ball3.GetComponentInChildren<TextMesh>().text = "Ref Pos";

        }
        public void ObjReleased(object obj1, object obj2)
        {
            _singleRotationAngleCumulative += _singleRotationAngleGrab;
            _singleRotationAngleGrab = 0.0f;
        }


        protected override void Awake()
        {
            base.Awake();
        }

        protected override void Start()
        {
            base.Start();
            ref_pos = transform.TransformPoint(Vector3.right);
            this.ObjectGrabbed.AddListener(ObjGrabbed);
            this.ObjectReleased.AddListener(ObjReleased);
            // transform.GetComponent<SG_Rotater>().ObjectGrabbed.AddListener(ObjGrabbed);
            // transform.GetComponent<SG_Rotater>().ObjectReleased.AddListener(ObjReleased);
            // this.ObjectGrabbed
            // ball1 = GameObject.Find("ball1");
            // ball2 = GameObject.Find("ball2");
            // ball3 = GameObject.Find("ball3");
            // ball4 = GameObject.Find("ball4");
            // ball5 = GameObject.Find("ball5");
            // ball.GetComponentInChildren<TextMeshPro>().text = "ref pos";
        }


    }

}
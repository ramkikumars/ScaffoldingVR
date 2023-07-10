//======= Copyright (c) Valve Corporation, All rights reserved. ===============
//
// Purpose: Estimates the velocity of an object based on change in position
//
//=============================================================================

using UnityEngine;
using System.Collections;
using SG;
using Fusion;

//-------------------------------------------------------------------------
public class VelocityEstimator : NetworkBehaviour
{
	[Tooltip( "How many frames to average over for computing velocity" )]
	public int velocityAverageFrames = 5;
	[Tooltip( "How many frames to average over for computing angular velocity" )]
	public int angularVelocityAverageFrames = 11;
	public bool estimateOnAwake = false;
	public bool useLocalPosistion=false;
	private Coroutine routine;
	private int sampleCount;
	private Vector3[] velocitySamples;
	private Vector3[] angularVelocitySamples;
	public float currentVelocity;
	public SG_Grabable TrackedObject;
	public bool starRoutine;
    Vector3 previousPosition;
    Quaternion previousRotation;
	void Start(){
        // TrackedObject.ObjectGrabbed.AddListener(ObjectGrabbed);
        // TrackedObject.ObjectReleased.AddListener(ObjectReleased);
        sampleCount = 0;
        velocitySamples = new Vector3[velocityAverageFrames];
        angularVelocitySamples = new Vector3[angularVelocityAverageFrames];
        previousPosition = Vector3.zero;
        if (useLocalPosistion)
        {
            previousPosition = transform.localPosition;
        }
        else
        {
            previousPosition = transform.position;
        }
        previousRotation = transform.rotation;
	}
	//-------------------------------------------------
	public void BeginEstimatingVelocity()
	{
		FinishEstimatingVelocity();

		routine = StartCoroutine( EstimateVelocityCoroutine() );
	}


	//-------------------------------------------------
	public void FinishEstimatingVelocity()
	{
		if ( routine != null )
		{
			StopCoroutine( routine );
			routine = null;
		}
	}

    // private void ObjectGrabbed(SG_Interactable obj1, SG_GrabScript obj2)
    // {
    //     velocitySamples = new Vector3[velocityAverageFrames];
    //     angularVelocitySamples = new Vector3[angularVelocityAverageFrames];
    //     BeginEstimatingVelocity();
    // }
    // private void ObjectReleased(SG_Interactable obj1, SG_GrabScript obj2)
    // {
    //     FinishEstimatingVelocity();
    // }
	//-------------------------------------------------
	public Vector3 GetVelocityEstimate()
	{
		// Compute average velocity
		Vector3 velocity = Vector3.zero;
		int velocitySampleCount = Mathf.Min( sampleCount, velocitySamples.Length );
		if ( velocitySampleCount != 0 )
		{
			for ( int i = 0; i < velocitySampleCount; i++ )
			{
				velocity += velocitySamples[i];
			}
			velocity *= ( 1.0f / velocitySampleCount );
		}

		return velocity;
	}


	//-------------------------------------------------
	public Vector3 GetAngularVelocityEstimate()
	{
		// Compute average angular velocity
		Vector3 angularVelocity = Vector3.zero;
		int angularVelocitySampleCount = Mathf.Min( sampleCount, angularVelocitySamples.Length );
		if ( angularVelocitySampleCount != 0 )
		{
			for ( int i = 0; i < angularVelocitySampleCount; i++ )
			{
				angularVelocity += angularVelocitySamples[i];
			}
			angularVelocity *= ( 1.0f / angularVelocitySampleCount );
		}

		return angularVelocity;
	}


	//-------------------------------------------------
	public Vector3 GetAccelerationEstimate()
	{
		Vector3 average = Vector3.zero;
		for ( int i = 2 + sampleCount - velocitySamples.Length; i < sampleCount; i++ )
		{
			if ( i < 2 )
				continue;

			int first = i - 2;
			int second = i - 1;

			Vector3 v1 = velocitySamples[first % velocitySamples.Length];
			Vector3 v2 = velocitySamples[second % velocitySamples.Length];
			average += v2 - v1;
		}
		average *= ( 1.0f / Time.deltaTime );
		return average;
	}


	//-------------------------------------------------
	void Awake()
	{
		velocitySamples = new Vector3[velocityAverageFrames];
		angularVelocitySamples = new Vector3[angularVelocityAverageFrames];

		if ( estimateOnAwake )
		{
			BeginEstimatingVelocity();
		}
	}


	//-------------------------------------------------
	private IEnumerator EstimateVelocityCoroutine()
	{
		sampleCount = 0;
		Debug.Log("Entered Velocity Estimator Coroutine");
        Vector3 previousPosition = Vector3.zero;
        if(useLocalPosistion){
		previousPosition = transform.localPosition;
		}
		else{
            previousPosition = transform.position;
		}
		Quaternion previousRotation = transform.rotation;
		while ( true )
		{
			// yield return new WaitForEndOfFrame();
			yield return null;

			float velocityFactor = 1.0f / Time.deltaTime;

			int v = sampleCount % velocitySamples.Length;
			int w = sampleCount % angularVelocitySamples.Length;
			sampleCount++;

			// Estimate linear velocity

			if(useLocalPosistion){
			velocitySamples[v] = velocityFactor * ( transform.localPosition - previousPosition );}
			else {
			velocitySamples[v] = velocityFactor * ( transform.position - previousPosition );}


			// Estimate angular velocity
			Quaternion deltaRotation = transform.rotation * Quaternion.Inverse( previousRotation );

			float theta = 2.0f * Mathf.Acos( Mathf.Clamp( deltaRotation.w, -1.0f, 1.0f ) );
			if ( theta > Mathf.PI )
			{
				theta -= 2.0f * Mathf.PI;
			}

			Vector3 angularVelocity = new Vector3( deltaRotation.x, deltaRotation.y, deltaRotation.z );
			if ( angularVelocity.sqrMagnitude > 0.0f )
			{
				angularVelocity = theta * velocityFactor * angularVelocity.normalized;
			}

			angularVelocitySamples[w] = angularVelocity;

			previousPosition = transform.position;
			previousRotation = transform.rotation;
		}
	}

	void Update(){
		currentVelocity=GetVelocityEstimate().magnitude;
		if(starRoutine){
			BeginEstimatingVelocity();
		}

    //     // while (true)
    //     // {
    //         // yield return new WaitForEndOfFrame();
    //         // yield return null;

    //         float velocityFactor = 1.0f / Time.deltaTime;

    //         int v = sampleCount % velocitySamples.Length;
    //         int w = sampleCount % angularVelocitySamples.Length;
    //         sampleCount++;

    //         // Estimate linear velocity

    //         if (useLocalPosistion)
    //         {
    //             velocitySamples[v] = velocityFactor * (transform.localPosition - previousPosition);
    //         }
    //         else
    //         {
    //             velocitySamples[v] = velocityFactor * (transform.position - previousPosition);
    //         }


    //         // Estimate angular velocity
    //         Quaternion deltaRotation = transform.rotation * Quaternion.Inverse(previousRotation);

    //         float theta = 2.0f * Mathf.Acos(Mathf.Clamp(deltaRotation.w, -1.0f, 1.0f));
    //         if (theta > Mathf.PI)
    //         {
    //             theta -= 2.0f * Mathf.PI;
    //         }

    //         Vector3 angularVelocity = new Vector3(deltaRotation.x, deltaRotation.y, deltaRotation.z);
    //         if (angularVelocity.sqrMagnitude > 0.0f)
    //         {
    //             angularVelocity = theta * velocityFactor * angularVelocity.normalized;
    //         }

    //         angularVelocitySamples[w] = angularVelocity;

    //         previousPosition = transform.position;
    //         previousRotation = transform.rotation;
        }

		public override void Spawned()
    {
        base.Spawned();
		velocitySamples = new Vector3[velocityAverageFrames];
		angularVelocitySamples = new Vector3[angularVelocityAverageFrames];

			BeginEstimatingVelocity();
        
    }
	
}

using Assets.Scripts;
using MainEnvironment.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class FlyingBehaviour : MonoBehaviour
{
    public SteamVR_Action_Boolean triggerAction = SteamVR_Input.GetBooleanAction("InteractUI");
    public Hand hand;
    public float ThrustForce;
    public Vector3 GravityForce = Physics.gravity;
    public GameObject Player;
    public Vector3 CurrentMaxBoundary;
    public Vector3 CurrentMinBoundary;
    public bool InvertDirection = true;

    private bool ApplyForce;
    private Vector3 PreviousMaxBoundary;
    private Vector3 PreviousMinBoundary;

    private void Start()
    {
    }

    private void Update()
    {
        if (triggerAction.GetStateDown(hand.trackedObject.inputSource))
        {
            ApplyForce = true;
        }
        else if(triggerAction.GetStateUp(hand.trackedObject.inputSource))
        {
            ApplyForce = false;
        }

        if(ApplyForce)
        {
            AddThrust();
        }
        else
        {
            ApplyGravity();
        }
    }

    private void AddThrust()
    {
        if (WithinBounds(Player.transform.position, out Vector3 maxPos))
        { 
            var force = (ThrustForce * Time.deltaTime) * hand.transform.forward;
            if (InvertDirection)
            {
                this.Player.transform.position -= force;
            }
            else
            {
                this.Player.transform.position += force;
            }
        }
        else
        {
            this.Player.transform.position = maxPos;
        }
    }
    private void ApplyGravity()
    {
        if (Player.transform.position.y <= 0)
        {
            this.Player.transform.position = new Vector3(this.Player.transform.position.x, 0, this.Player.transform.position.z);
        }
        else
        {
            var force = GravityForce * Time.deltaTime;
            this.Player.transform.position += force;
        }
    }

    public void UpdateBounds(Vector3 min, Vector3 max)
    {
        this.PreviousMaxBoundary = this.CurrentMaxBoundary;
        this.PreviousMinBoundary = this.CurrentMinBoundary;
        this.CurrentMaxBoundary = max;
        this.CurrentMinBoundary = min;
    }
    public void ResetBounds()
    {
        var tempMin = this.CurrentMinBoundary;
        var tempMax = this.CurrentMaxBoundary;
        this.CurrentMaxBoundary = this.PreviousMaxBoundary;
        this.CurrentMinBoundary = this.PreviousMinBoundary;
    }

    private bool WithinBounds(Vector3 position, out Vector3 maxPos)
    {
        bool withinBounds = true;
        maxPos = new Vector3();
		//if the current bounds are not set then just say user is within bounds
		if (this.CurrentMaxBoundary.magnitude <= 0 || this.CurrentMinBoundary.magnitude <= 0)
		{
			if(position.x < this.CurrentMinBoundary.x)
			{
				withinBounds = false;
				maxPos.x = this.CurrentMinBoundary.x;
			}
			else if(position.x > this.CurrentMaxBoundary.x)
			{
				withinBounds = false;
				maxPos.x = this.CurrentMaxBoundary.x;
			}
			else
			{
				maxPos.x = position.x;
			}

			if (position.y < this.CurrentMinBoundary.y)
			{
				withinBounds = false;
				maxPos.y = this.CurrentMinBoundary.y;
			}
			else if (position.y > this.CurrentMaxBoundary.y)
			{
				withinBounds = false;
				maxPos.y = this.CurrentMaxBoundary.y;
			}
			else
			{
				maxPos.y = position.y;
			}

			if (position.z < this.CurrentMinBoundary.z)
			{
				withinBounds = false;
				maxPos.z = this.CurrentMinBoundary.z;
			}
			else if (position.z > this.CurrentMaxBoundary.z)
			{
				withinBounds = false;
				maxPos.z = this.CurrentMaxBoundary.z;
			}
			else
			{
				maxPos.z = position.z;
			}
		}
        return withinBounds;
    }
}

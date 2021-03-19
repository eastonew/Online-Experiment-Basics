using Assets.Scripts;
using MainEnvironment.Core;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Valve.VR;
using Valve.VR.Extras;

public class GrabbingBehaviour : MonoBehaviour
{
	public SteamVR_LaserPointer Pointer;
	public SteamVR_Action_Boolean interactWithUI = SteamVR_Input.GetBooleanAction("InteractUI");
	public Material HighlightMaterial;
	public GameObject ScalingPoint;
	public ScalingBehaviour ScalingPlayer;

	private Material OriginalMaterial;
	
	public State CurrentState { get; set; }
    public enum State
    {
        Standard,
        Selected,
        Grabbed
    }

	public void Start()
    {
        Pointer.PointerIn += OnPointerIn;
        Pointer.PointerOut += OnPointerOut;
        this.CurrentState = State.Standard;
        this.OriginalMaterial = GetComponent<MeshRenderer>().sharedMaterial;
		if(this.ScalingPlayer != null)
		{
			this.ScalingPlayer.Register(this);
		}
    }
	
	public void Update()
    {
        if (this.CurrentState == State.Selected)
        {
            //need to highlight the item
            GetComponent<MeshRenderer>().sharedMaterial = HighlightMaterial;
            if (interactWithUI.GetStateDown(Pointer.pose.inputSource))
            {
                this.CurrentState = State.Grabbed;
                //need hit position of pointer when the trigger was pulled - for now use the one given when the pointer enters the sculpture;
            }
        }
        else if (this.CurrentState == State.Grabbed)
        {
            //as the controller is moved then we just need to track the movement with this game object
            //we also need to ensure that the sculpture stays on the floor 
            if (interactWithUI.GetStateUp(Pointer.pose.inputSource))
            {
                this.CurrentState = State.Standard;
                GetComponent<MeshRenderer>().sharedMaterial = OriginalMaterial;
            }
        }
    }
	
	private void OnPointerIn(object sender, PointerEventArgs e)
    {
        if (e.target == gameObject.transform)
        {
            if (this.CurrentState != State.Grabbed)
            {
                this.CurrentState = State.Selected;
            }
        }
        else
        {
            if (this.CurrentState != State.Grabbed)
            {
                this.CurrentState = State.Standard;
            }
        }
    }

    private void OnPointerOut(object sender, PointerEventArgs e)
    {
        if (this.CurrentState == State.Selected)
        {
            this.CurrentState = State.Standard;
            GetComponent<MeshRenderer>().sharedMaterial = OriginalMaterial;
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class RotationBehaviour : MonoBehaviour
{
    public SteamVR_Action_Boolean rightRotateAction;
    public SteamVR_Action_Boolean leftRotateAction;
	public Vector3 RotationAngle = new Vector3(0, 90, 0);

    public Hand hand;

    private bool ObjectGrabbed = false;
    private GrabbingBehaviour GrabbedObject;


    private void OnEnable()
    {
        if (hand == null)
            hand = this.GetComponent<Hand>();

        if (rightRotateAction == null || leftRotateAction == null)
        {
            Debug.LogError("<b>[SteamVR Interaction]</b> No rotate action assigned", this);
            return;
        }

        rightRotateAction.AddOnChangeListener(OnRightRotateActionChange, hand.handType);
        leftRotateAction.AddOnChangeListener(OnLeftRotateActionChange, hand.handType);
    }
    private void Start()
    {
        this.GrabbedObject = GetComponent<GrabbingBehaviour>();
    }
    private void Update()
    {
        //add a constraint that we need this pedestal to be in the grabbed state before we can rotate
        ObjectGrabbed = this.GrabbedObject.CurrentState == GrabbedObject.State.Grabbed;
    }

    private void OnDisable()
    {
        if (rightRotateAction != null)
            rightRotateAction.RemoveOnChangeListener(OnRightRotateActionChange, hand.handType);

        if (leftRotateAction != null)
            leftRotateAction.RemoveOnChangeListener(OnLeftRotateActionChange, hand.handType);
    }

    private void OnRightRotateActionChange(SteamVR_Action_Boolean actionIn, SteamVR_Input_Sources inputSource, bool pressed)
    {
        if (pressed && ObjectGrabbed)
        {
            RotateElement(true);
        }
    }

    private void OnLeftRotateActionChange(SteamVR_Action_Boolean actionIn, SteamVR_Input_Sources inputSource, bool pressed)
    {
        if (pressed && ObjectGrabbed)
        {
            RotateElement(false);
        }
    }

    public void RotateElement(bool rotateRight)
    {
        var currentRot = this.transform.rotation;
        if (rotateRight)
        {
            this.transform.rotation = currentRot * Quaternion.Euler(RotationAngle);
        }
        else
        {
            this.transform.rotation = currentRot * Quaternion.Euler(RotationAngle * -1);
        }
    }
}

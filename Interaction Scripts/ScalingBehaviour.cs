using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class ScalingBehaviour : MonoBehaviour
{
    public List<GrabbingBehaviour> AllItems { get; set; }
    public SteamVR_Action_Boolean GripAction = SteamVR_Input.GetBooleanAction("GrabGrip");
    public Hand LeftHand;
    public Hand RightHand;
    public FlyingBehaviour Flying;
    /// <summary>
    ///this is the distance the controllers need to be dragged apart before triggering the user to scale into the sculpture
    /// </summary>
    public float ScaleInDistanceThreshold;
    /// <summary>
    /// This is the maximum distance between the controllers before triggering the user to scale out of the sculpture.
    /// </summary>
    public float ScaleOutDistanceThreshold;
    public GameObject PlayerObject;
    public float ScalingFactor;

    private bool LeftHandDown;
    private bool RightHandDown;
    private bool Scaled;
    private GrabbingBehaviour SelectedItem;
    private float? StartDistance;
    private float OriginalThrustForce;
    private Vector3 OriginalGravityForce;

    // Start is called before the first frame update
    void Start()
    {
    }

    public void Register(GrabbingBehaviour ped)
    {
        if(this.AllItems == null)
        {
            this.AllItems = new List<GrabbingBehaviour>();
        }
        this.AllItems.Add(ped);
    }

    // Update is called once per frame
    void Update()
    {

        if (GripAction.GetStateDown(LeftHand.trackedObject.inputSource))
        {
            LeftHandDown = true;
        }
        if (GripAction.GetStateUp(LeftHand.trackedObject.inputSource))
        {
            LeftHandDown = false;
        }

        if (GripAction.GetStateDown(RightHand.trackedObject.inputSource))
        {
            RightHandDown = true;
        }
        if (GripAction.GetStateUp(RightHand.trackedObject.inputSource))
        {
            RightHandDown = false;
        }

        SelectedItem = AllItems.FirstOrDefault(a => a.CurrentState == GrabbingBehaviour.State.Selected);

        if(RightHandDown && LeftHandDown && SelectedItem != null && !Scaled)
        {
            if(StartDistance == null)
            {
                StartDistance = Vector3.Distance(RightHand.transform.position, LeftHand.transform.position);
            }
            
            var currentDistance = Vector3.Distance(RightHand.transform.position, LeftHand.transform.position);
            if (Mathf.Abs(currentDistance - StartDistance.Value) > ScaleInDistanceThreshold)
            {
                Scaled = true;
                SteamVR_Fade.View(Color.black, 0.5f);
                IEnumerator fadeIn()
                {
                    yield return new WaitForSeconds(0.5f);
                    SteamVR_Fade.View(Color.clear, 0.5f);
                    //update position & Scale
                    PlayerObject.transform.position = SelectedItem.ScalingPoint.transform.position;
                    PlayerObject.transform.localScale = new Vector3(ScalingFactor, ScalingFactor, ScalingFactor);
                    OriginalThrustForce = Flying.ThrustForce;
                    OriginalGravityForce = Flying.GravityForce;
                    //reduce the intensity of the thrust and gravity forces
                    Flying.ThrustForce = OriginalThrustForce * ScalingFactor;
                    Flying.GravityForce = OriginalGravityForce * ScalingFactor;
                    var bounds = SelectedItem.GetComponent<Renderer>().bounds;
                    //this takes into account the sculpture and thickness of the base
                    var minBounds = new Vector3(bounds.min.x, bounds.min.y + 1, bounds.min.z);
                    var maxBounds = new Vector3(bounds.max.x, bounds.max.y + SelectedItem.SculptureHeight, bounds.max.z);
                    Flying.UpdateBounds(minBounds, maxBounds);
                    //also need to set the bounds
                    StartDistance = null;
                    yield return new WaitForSeconds(0.5f);
                }
                StartCoroutine(fadeIn());
            }
        }
        else if(RightHandDown && LeftHandDown && Scaled)
        {
            if (StartDistance == null)
            {
                StartDistance = Vector3.Distance(RightHand.transform.position, LeftHand.transform.position);
            }

            var currentDistance = Vector3.Distance(RightHand.transform.position, LeftHand.transform.position);

            if (Mathf.Abs(currentDistance - StartDistance.Value) < ScaleOutDistanceThreshold)
            {
                
                SteamVR_Fade.View(Color.black, 0.5f);
                IEnumerator scaleOut()
                {
                    yield return new WaitForSeconds(0.5f);
                    SteamVR_Fade.View(Color.clear, 3);
                    //update position & Scale
                    PlayerObject.transform.position = new Vector3(0, 0, 0);
                    PlayerObject.transform.localScale = new Vector3(1, 1, 1);
                    Flying.ThrustForce = OriginalThrustForce;
                    Flying.GravityForce = OriginalGravityForce;
                    Flying.ResetBounds();
                    StartDistance = null;
                    yield return new WaitForSeconds(3);
                    Scaled = false;
                }
                StartCoroutine(scaleOut());
            }
        }
        else
        {
            StartDistance = null;
        }
    }
}

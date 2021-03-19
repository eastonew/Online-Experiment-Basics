using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.Extras;
using Valve.VR.InteractionSystem;

public class TranslationBehaviour : MonoBehaviour
{
    public float FloorHeight;
    public float MaxDistance = 10;
    public GrabbingBehaviour GrabbingObject { get; set; }

    private float Coefficient = 0.5f;
    
    // Start is called before the first frame update
    void Start()
    {
        this.GrabbingObject = GetComponent<GrabbingBehaviour>();
    }

    void Update()
    {
        if(this.GrabbingObject.CurrentState == GrabbingObject.State.Grabbed)
        {
            var newPos = CalculateLaserFloorHitPoint();
            if (!float.IsNaN(newPos.x))
            {
                //TODO - need to position object relative to it's original position
                //TODO - maybe we need to track position change and alter the sculpture pos by the change, rather than setting it to the calculated value
                this.gameObject.transform.position = newPos;
            }
        }
    }

    private float GetFloorIntersectVal(float startY, float gravityY, float velocityY)
    {
        var s1 = (FloorHeight - startY - velocityY) / gravityY;
        var s2 = s1 / Coefficient;
        if(s2 < 0)
        {
            //TODO - Issue with having the controller pointed below horizontal - if no checks this moves sculpture below horizontal
        }
        var x = Mathf.Sqrt(s2);
        return x;
    }

    private Vector3 CalculateLaserFloorHitPoint()
    {
        
        Vector3 gravity = Physics.gravity;
        Vector3 startPos = this.PedestalInfo.Pointer.gameObject.transform.position;
        Vector3 velocity = this.PedestalInfo.Pointer.gameObject.transform.forward * MaxDistance;
        var x = GetFloorIntersectVal(startPos.y, gravity.y, velocity.y);
        Vector3 arcPos = startPos + (velocity + (0.5f * Mathf.Pow(x, 2)) * gravity);
        //TODO - add check for within bounds
        //this also doesn't take into account that a user might be outside the bounds of the environment
        return arcPos;
    }
}

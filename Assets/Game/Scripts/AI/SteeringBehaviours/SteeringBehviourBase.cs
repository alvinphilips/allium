using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SteeringBehviourBase : MonoBehaviour
{

    public float weight = 1f;
    
    public bool bUseMouseInput = true;

    public abstract Vector3 CalculateForce();

    [HideInInspector] public SteeringAgent steeringAgent;

    protected bool bMouseClicked = false;

    protected void CheckMouseInput()
    {
        bMouseClicked = false;

        Debug.Log("Checking Mouse Input");

        if(Input.GetMouseButtonDown(0) && bUseMouseInput) 
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if(Physics.Raycast(ray, out hit, 100) )
            {
                target = hit.point;
                bMouseClicked = true;
            }
        }
    }

    protected virtual void OnDrawGizmos()
    {
        if(steeringAgent != null)
        {
            DebugExtension.DrawArrow(transform.position, target, Color.red);
            DebugExtension.DrawArrow(transform.position, steeringAgent.velocity, Color.blue);
        }

        DebugExtension.DrawPoint(target, Color.magenta);
    }
}

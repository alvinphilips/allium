using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SB_ObstacleAvoidance : SteeringBehviourBase
{
    //Feelers
    public List<Feeler> feelers = new List<Feeler>();
    public LayerMask layerMask;

    public float avoidWeight = 1f;

    public override Vector3 CalculateForce()
    {
        RaycastHit hit;
        Ray ray;

        foreach (Feeler feeler in feelers)
        {
            Vector3 feelerPos = transform.rotation * feeler.offset + transform.position;
            ray = new Ray(feelerPos, transform.forward);

            if(Physics.Raycast(ray, out hit, feeler.distance, layerMask)) 
            {
                Vector3 forceDir = Vector3.Project(hit.point - transform.position, transform.forward);
                float multiplier = 1f + ((feeler.distance - forceDir.magnitude) / feeler.distance);

                Vector3 forcePos = forceDir + transform.position;
                forceDir = (forcePos - hit.point).normalized * multiplier * (1f / avoidWeight);
                return forceDir;
            }
        }

        return Vector3.zero;
    }

    protected override void OnDrawGizmos()
    {
        foreach (Feeler feeler in feelers)
        {
            Vector3 feelerPos = transform.rotation * feeler.offset + transform.position;
            Debug.DrawLine(feelerPos, transform.forward * feeler.distance + feelerPos, Color.blue);
        }
    }
}

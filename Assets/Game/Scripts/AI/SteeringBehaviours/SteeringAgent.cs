using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Obstacle Avoidance
    [System.Serializable]
    public class Feeler
    {
        public float distance;
        public Vector3 offset;
    }

public class SteeringAgent : MonoBehaviour { 
    public enum SummingMethod
    {
        Weighted,
        Prioritized
    };

    public SummingMethod summingMethod = SummingMethod.Weighted;

    public float mass = 1f;
    public float maxSpeed = 1f;
    public float maxForce = 10f;

    public Vector3 velocity = Vector3.zero;

    private List<SteeringBehviourBase> steeringBehaviours = new List<SteeringBehviourBase>();

    //In other words rotation speed (or time it should take to rotate)
    public float angularDampeningTime = 5f;
    
    //Angle after which you can snap rotation and it wontlook bad
    public float deadZone = 10f;

    public bool bReachedGoal = false;

    private void Start()
    {
        steeringBehaviours.AddRange(GetComponents<SteeringBehviourBase>());
        foreach (var t in steeringBehaviours)
        {
            t.steeringAgent = this;
        }
    }

    private void Update()
    {
        Vector3 steeringForce = CalculateSteeringForce();

        steeringForce.y = 0f;
        
        if(bReachedGoal)
        {
            velocity = Vector3.zero;
        }
        else
        {
            Vector3 acceleration = steeringForce / mass;

            velocity = velocity + (acceleration * Time.deltaTime);
            
            velocity.y = 0f;

            velocity = Vector3.ClampMagnitude(velocity, maxSpeed);

            transform.position += (velocity * Time.deltaTime);

            if (velocity.magnitude > 0f)
            {
                float angle = Vector3.Angle(transform.forward, velocity);

                if (Mathf.Abs(angle) <= deadZone)
                {
                    transform.LookAt(transform.position + velocity);
                }
                else
                {
                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(velocity), angularDampeningTime * Time.deltaTime);
                }
            }
        }

        
    }

    public Vector3 CalculateSteeringForce()
    {
        Vector3 totalForce = Vector3.zero;

        foreach (SteeringBehviourBase steeringBehviour in steeringBehaviours)
        {
            if(steeringBehviour.enabled)
            {
                switch(summingMethod)
                {
                    case SummingMethod.Weighted:
                        {
                            totalForce = totalForce + (steeringBehviour.CalculateForce() * steeringBehviour.weight);
                            totalForce = Vector3.ClampMagnitude(totalForce, maxForce);
                            break;
                        }
                    case SummingMethod.Prioritized:
                        {
                            Vector3 steeringForce = (steeringBehviour.CalculateForce() * steeringBehviour.weight);
                            if(!AccumulateForce(ref totalForce, steeringForce))
                            {
                                return totalForce;
                            }
                            break;
                        }

                }
            }
        }

        return totalForce;
    }

    public bool AccumulateForce(ref Vector3 destVector, Vector3 forceToAdd)
    {
        float magSoFar = destVector.magnitude;

        float remainingMag = maxForce - magSoFar;

        if(remainingMag <= 0)
        {
            return false;
        }

        float magToAdd = forceToAdd.magnitude;

        if(magToAdd < remainingMag)
        {
            destVector = destVector + forceToAdd;
        }
        else
        {
            destVector = destVector + (forceToAdd.normalized * remainingMag);
        }

        return true;
    }
}

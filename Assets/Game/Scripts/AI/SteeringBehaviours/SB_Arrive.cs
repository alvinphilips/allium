using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SB_Arrive : SteeringBehviourBase
{
    protected Vector3 desiredVelocity;

    public float slowDownDistance = 2f;
    public float stoppingDistance = 0.1f;

    public override Vector3 CalculateForce()
    {
        CheckMouseInput();
        return CalculateArriveForce();
    }

    private Vector3 CalculateArriveForce()
    {
        Vector3 vectorToTarget = steeringAgent.target - steeringAgent.transform.position;

        float distance = vectorToTarget.magnitude;

        steeringAgent.bReachedGoal = false;

        if(distance > slowDownDistance)
        {
            desiredVelocity = (steeringAgent.target - transform.position).normalized;
            desiredVelocity = desiredVelocity * steeringAgent.maxSpeed;
            return (desiredVelocity - steeringAgent.velocity);

        }
        else if(distance > stoppingDistance && distance <= slowDownDistance) 
        {
            vectorToTarget.Normalize();
            float speed = steeringAgent.maxSpeed * (distance / slowDownDistance);
            speed = (speed < steeringAgent.maxSpeed ? speed : steeringAgent.maxSpeed);
            desiredVelocity = (speed / distance) * vectorToTarget;
            return desiredVelocity - steeringAgent.velocity;
        }

        steeringAgent.bReachedGoal = true;
        return Vector3.zero;
    }
}

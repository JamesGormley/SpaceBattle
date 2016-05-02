using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Pursuit : MonoBehaviour
{

    public Vector3 velocity;
    public Vector3 acceleration;
    public Vector3 force;
    public float mass = 1.0f;
    public float maxSpeed = 200.0f;
    public float maxForce = 100.0f;

    public bool seekEnabled;
    public Vector3 targetObject;
    



    public int waypointCount = 10;
    private List<Vector3> waypoints = new List<Vector3>();
    int current;

    [Header("Arrive")]
    public bool arriveEnabled;
    Vector3 arriveTargetPosition;
    public float slowingDistance = 100;

    [Header("Offset Pursue")]
    public bool offsetPursueEnabled = false;
    public GameObject offsetPursueTarget = null;
    Vector3 offset;
    Vector3 offsetPursueTargetPos;

    [Header("Flee")]
    public bool fleeEnabled;
    public GameObject fleeFromMe;
    public float fleeRange = 150.0f;


    // Use this for initialization
    void Start()
    {

        arriveEnabled = false;
        


        if (offsetPursueEnabled)
        {
            offset = transform.position - offsetPursueTarget.transform.position;
            offset = Quaternion.Inverse(offsetPursueTarget.transform.rotation) * offset;
            //offset = new Vector3(50, 0, 0);
        }

    }


    // Update is called once per frame
    void Update()
    {


        force = Vector3.zero;

        if (seekEnabled)
        {
            //distance to next waypoint
            float dist = Vector3.Distance(transform.position, waypoints[current]);
            if (dist < 150.0f)
            {
                //Increment variable to the next waypoint
                current = (current + 1) % waypoints.Count;
                //Debug.Log(current);
            }
            //if heading for last waypoint turn on arrive
            if (current == waypoints.Count - 1)
            {
                arriveEnabled = true;
                seekEnabled = false;
            }
            // Move towards 
            force += Seek(waypoints[current]);
        }

        //if (arriveEnabled)
        //{
        //    force += Arrive(vec2point2);
        //}

        if (offsetPursueEnabled)
        {
            force += OffsetPursue(offsetPursueTarget, offset);
        }

        if (fleeEnabled)
        {
            force += Flee(fleeFromMe.transform.position, fleeRange);
        }

        force = Vector3.ClampMagnitude(force, maxForce);

        acceleration = force / mass;
        velocity += acceleration * Time.deltaTime;
        velocity = Vector3.ClampMagnitude(velocity, maxSpeed);
        if (velocity.magnitude > float.Epsilon)
        {
            transform.forward = velocity;
        }

        transform.position += velocity * Time.deltaTime;


        //transform.forward.Normalize();
        //transform.LookAt(transform.position + transform.forward);
    }

    Vector3 Seek(Vector3 target)
    {
        Vector3 toTarget = target - transform.position;
        toTarget.Normalize();
        Vector3 desired = toTarget * maxSpeed;
        return desired - velocity;
    }


    public Vector3 OffsetPursue(GameObject leader, Vector3 offset)
    {
        Vector3 target = leader.transform.TransformPoint(offset);
        Vector3 toTarget = transform.position - target;
        float dist = toTarget.magnitude;
        float lookAhead = dist / maxSpeed;

        offsetPursueTargetPos = target + (lookAhead * leader.GetComponent<NabooPath>().velocity);
        return Arrive(offsetPursueTargetPos);
    }


    public Vector3 Arrive(Vector3 targetPos)
    {
        Vector3 toTarget = targetPos - transform.position;

        //float slowingDistance = 15.0f;
        float distance = toTarget.magnitude;
        if (distance < 100.0f)
        {
            velocity = Vector3.zero;
            return Vector3.zero;
        }
        float ramped = maxSpeed * (distance / slowingDistance);

        float clamped = Mathf.Min(ramped, maxSpeed);
        Vector3 desired = clamped * (toTarget / distance);

        return desired - velocity;
    }


    public Vector3 Flee(Vector3 targetPos, float range)
    {
        Vector3 desiredVelocity;
        desiredVelocity = transform.position - targetPos;
        if (desiredVelocity.magnitude > range)
        {
            return Vector3.zero;
        }
        desiredVelocity.Normalize();
        desiredVelocity *= maxSpeed;
        //Debug.Log("Flee");
        return desiredVelocity - velocity;
    }


}
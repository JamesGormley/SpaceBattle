using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NabooPath : MonoBehaviour {

    public Vector3 velocity;
    public Vector3 acceleration;
    public Vector3 force;
    public float mass = 1.0f;
    public float maxSpeed = 200.0f;
    public float maxForce = 100.0f;

    public bool seekEnabled;
    public Vector3 targetObject;


    Vector3 vec1point1, vec1point2, vec2point1, vec2point2;

    public GameObject line1Point1 = null; // line1Point2, line2Point1, line2Point2 = null;
    public GameObject line1Point2 = null;
    public GameObject line2Point1 = null;
    public GameObject line2Point2 = null;

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
    public float fleeRange = 150.0f;
    public Vector3 fleeTargetPosition;


    // Use this for initialization
    void Start() {

        arriveEnabled = false;
        
        vec1point1 = line1Point1.transform.position;
        vec1point2 = line1Point2.transform.position;
        vec2point1 = line2Point1.transform.position;
        vec2point2 = line2Point2.transform.position;

        calcWayPoints(vec1point1, vec1point2, vec2point1, vec2point2);
        


        if (offsetPursueEnabled)
        {
            offset = transform.position - offsetPursueTarget.transform.position;
            offset = Quaternion.Inverse(offsetPursueTarget.transform.rotation) * offset;
            //offset = new Vector3(50, 0, 0);
        }

    }


    // Update is called once per frame
    void Update() {


        force = Vector3.zero;

        if (seekEnabled)
        {
            //distance to next waypoint
            float dist = Vector3.Distance(transform.position, waypoints[current]);
            if (dist <150.0f)
            {
                //Increment variable to the next waypoint
                current = (current + 1) % waypoints.Count;
                //Debug.Log(current);
            }
            //if heading for last waypoint turn on arrive
            if(current == waypoints.Count - 1)
            {
                arriveEnabled = true;
                seekEnabled = false;
            }
            // Move towards 
            force += Seek(waypoints[current]);
        }

        if (arriveEnabled)
        {
            force += Arrive(vec2point2);
        }

        if (offsetPursueEnabled)
        {
            force += OffsetPursue(offsetPursueTarget, offset);
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
        Debug.Log("Flee");
        return desiredVelocity - velocity;
    }





    public Vector3 calcBeizerCurve(float t,Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
    {
        //Beizer curve function
        //[x,y]=(1–t)^3P0 + 3(1–t)^2tP1 + (1–t)t^2P2 + t3P3
        //P are control points, t between 0 and 1. 
        //In two point curve, t = 0 at P0 and t = 1 at P1. 
        //T always between 1 and 0. We're using 4 points here

        //  *****  This code referenced from http://devmag.org.za/2011/04/05/bzier-curves-a-tutorial/ *****
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        float uuu = uu * u;
        float ttt = tt * t;

        Vector3 p = uuu * p0; //first term
        p += 3 * uu * t * p1; //second term
        p += 3 * u * tt * p2; //third term
        p += ttt * p3; //fourth term

        return p;
        
        // ***** End of reference *****
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        // Straight line between these two vector3s. Make identical and the same as the first vector3 in q1
        Vector3 q0 = calcBeizerCurve(0, vec1point1, vec1point1, vec1point1, vec1point1);

        // Line curves between the 2 points used to make q1
        // First point must match with second point used to make q0
        // i starts equal to one as we want t to be 1
        for (int i = 1; i <= waypointCount; i++)
        {
            float t = i / (float)waypointCount;
            Vector3 q1 = calcBeizerCurve(t, vec1point1, vec1point2, vec2point1, vec2point2);

            Gizmos.DrawLine(q0, q1);
            q0 = q1;
        }

        Gizmos.DrawLine(vec1point1, vec1point2);
        Gizmos.DrawLine(vec2point1, vec2point2);

    }

    public List<Vector3> calcWayPoints( Vector3 v1, Vector3 v2, Vector3 v3, Vector3 v4)
    {
        // Straight line between these two vector3s. Make identical and the same as the first vector3 in q1 to create start point
        Vector3 q0 = calcBeizerCurve(0, v1, v1, v1, v1);

        // Line curves between the 2 points used to make q1
        // First point must match with second point used to make q0
        // i starts equal to one as we want t to be 1
        for (int i = 1; i <= waypointCount; i++)
        {
            float t = i / (float)waypointCount;
            //This is the second line in the Bezier curve
            Vector3 q1 = calcBeizerCurve(t, v1, v2, v3, v4);
            //Add to our list of waypoints
            waypoints.Add(q1);
            q0 = q1;
        }

        return waypoints;

    }

}

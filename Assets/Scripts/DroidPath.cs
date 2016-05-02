using UnityEngine;
using System.Collections;

public class DroidPath : MonoBehaviour
{

    public Vector3 velocity;
    public Vector3 acceleration;
    public Vector3 force;
    public float mass = 1.0f;
    public float maxSpeed = 200.0f;
    public float maxForce = 100.0f;

    //timetolive is the time after which fighters auto destroy
    public float timeToLive = 8.0f;
    private float destroyTime;

    public bool seekEnabled;
    public Vector3 targetObject;
    

    [Header("Wander")]
    public bool wanderEnabled = false;
    public float wanderRadius = 10.0f;
    public float wanderJitter = 1.0f;
    public float wanderDistance = 15.0f;
    private Vector3 wanderTargetPos;
    
    // Use this for initialization
    void Start()
    {
        destroyTime = Time.time + timeToLive;

        Wander();
        wanderTargetPos = Random.insideUnitSphere * wanderRadius;

    }

    // Update is called once per frame
    void Update()
    {
        //This destroys the gameObject after a certain amount of time in seconds. Dictated by timeToLive variable
        if (Time.time > destroyTime)
            Destroy(this.gameObject);

        force = Vector3.zero;

        if (seekEnabled)
        {
            force += Seek(targetObject);
        }

        if (wanderEnabled)
        {   
            force += Seek(wanderTargetPos);
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

    }

    public Vector3 Seek(Vector3 target)
    {
        Vector3 toTarget = target - transform.position;
        toTarget.Normalize();
        Vector3 desired = toTarget * maxSpeed;
        return desired - velocity;
    }


    public Vector3 Wander()
    {
        float jitterTimeSlice = wanderJitter * Time.deltaTime;

        Vector3 toAdd = Random.insideUnitSphere * jitterTimeSlice;
        wanderTargetPos += toAdd;
        wanderTargetPos.Normalize();
        wanderTargetPos *= wanderRadius;

        Debug.Log(wanderTargetPos);

        Vector3 localTarget = wanderTargetPos + Vector3.forward * wanderDistance;
        Vector3 worldTarget = transform.TransformPoint(localTarget);
        return (worldTarget - transform.position);
    }
}


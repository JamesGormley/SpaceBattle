using UnityEngine;
using System.Collections;

public class SimpleSeek : MonoBehaviour {

    public Vector3 velocity;
    public Vector3 acceleration;
    public Vector3 force;
    public float mass = 1.0f;
    public float maxSpeed = 200.0f;
    public float maxForce = 100.0f;

    public bool seekEnabled = false;
    public Vector3 target;

    public GameObject laserBolt;
    


    // Use this for initialization
    void Start () {

    }

    // Update is called once per frame
    void Update() {

        //Calc each frame 
        force = Vector3.zero;

        if (seekEnabled)
        {
            force += Seek(target);
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

    //Fire/Instantiate projectile on vector towards target (after reload time reached)
    void shootLaser()
    {
        //Reload time calculated and waited for before instantiating bullit
        //nextShot = Time.time + reloadSpeed;
        GameObject laser = (GameObject)Instantiate(laserBolt, this.transform.position, Quaternion.identity);

    }

}

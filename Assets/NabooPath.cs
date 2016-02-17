using UnityEngine;
using System.Collections;

public class NabooPath : MonoBehaviour {
    Boid boid;
    public float radius = 50;
    // Use this for initialization
    void Start () {
        boid = GetComponent<Boid>();
        boid.seekEnabled = true;
        PickNewTarget();
    }

    void PickNewTarget()
    {
        Vector3 target = Random.insideUnitSphere;
        target.y = 0;
        target *= radius;
    }


    // Update is called once per frame
    void Update () {

        if (Vector3.Distance(transform.position, boid.seekTargetPosition) < 1.0f)
        {
            PickNewTarget();
        }

    }


}

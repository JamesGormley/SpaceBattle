using UnityEngine;
using System.Collections;

public class DistanceMonitor : MonoBehaviour
{

    public GameObject targetObject;

    //distance to object (spacesation)
    private float distToTarget;
    //distance at which to trigger the end of the scene
    public float triggerDistance = 2750.0f;

    // Update is called once per frame
    void Update()
    {
        //distance from ship to target 
        distToTarget = Vector3.Distance(transform.position, targetObject.transform.position);
        Debug.Log(distToTarget);

        if (distToTarget < triggerDistance)
        {
            Application.LoadLevel(4);
        }
    }
}

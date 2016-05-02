using UnityEngine;
using System.Collections;

//Script to make camera follow target
public class CamFollow : MonoBehaviour {

    public GameObject lookTarget;
	
    //distance from camera to main ship
    private float camDist;
    //distance from camera to exit scene when reached 
    public float maxCamDist = 2750.0f;

    // Update is called once per frame
    void Update () {
        //look at ship as it passes
        transform.LookAt(lookTarget.transform.position);

        //distance from camera to leap ship
        camDist = Vector3.Distance(transform.position, lookTarget.transform.position);
        Debug.Log(camDist);

        if( camDist > maxCamDist)
        {
            Application.LoadLevel(1);
        }
    }
}

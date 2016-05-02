using UnityEngine;
using System.Collections;

public class LaserBolt : MonoBehaviour {

    //Speed and Range variables. Available in Unity Editor
    public float speed = 10;

    LineRenderer lineRend;

	// Use this for initialization
	void Start () {

        lineRend = GetComponent<LineRenderer>();
	}


    // Update is called once per frame
    void Update () {

        //update laser position
        //lineRend.SetPosition(0, startPos.position);
        //lineRend.SetPosition(1, endPos.position);

        lineRend.SetPosition(0, transform.position + transform.forward);
        lineRend.SetPosition(1, transform.position - transform.forward);

    }
}

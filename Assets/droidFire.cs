using UnityEngine;
using System.Collections;

public class droidFire : MonoBehaviour {

    public GameObject enemyFighter;

	// Use this for initialization
	void Start () {


	}
	
	// Update is called once per frame
	void Update () {


        Vector3 toTarget= enemyFighter.transform.position - transform.position;
        toTarget.Normalize();

        float dot = Vector3.Dot(transform.forward, toTarget);

        if (dot < 0)
        {
            Debug.Log("Behind");
        }
        else
        {
            Debug.Log("In front");

        }

    }





    System.Collections.IEnumerator fireProjectile()
    {
        while (true)
        {
            // Use a line renderer
            GameObject lazer = new GameObject();
            lazer.transform.position = transform.position;
            lazer.transform.rotation = transform.rotation;
            LineRenderer line = lazer.AddComponent<LineRenderer>();
            lazer.AddComponent<LaserBolt>();
            line.material = new Material(Shader.Find("Particles/Additive"));
            line.SetColors(Color.red, Color.blue);  
            line.SetWidth(10.0f, 10.0f);
            line.SetVertexCount(2);
            yield return new WaitForSeconds(1.0f);
        }
    }
}

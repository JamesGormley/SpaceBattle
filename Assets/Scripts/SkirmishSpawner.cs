using UnityEngine;
using System.Collections;

public class SkirmishSpawner : MonoBehaviour {

    public GameObject droidFighter;
    public GameObject repFighter;

    public float sceneTime = 5.0f;
    private float endTime;

    public int numFighters;
    public int n1Speed = 100;

    //These are the limits of the area ships can spawn in
    public int minX, maxX, minY, maxY, minZ, maxZ;
    //x between -1000 and -3000
    //y between 0 and 1500
    //z between -3000 and 1000 works well

    private Vector3[] repPositions;


    // Use this for initialization
    void Start () {

        endTime = Time.time + sceneTime;

        //holder for the positions the republic fighters spawn in
        repPositions = new Vector3[numFighters];

        //instantiate the republic fighters
        for (int i = 0; i < numFighters; i++)
        {
            Vector3 pos = new Vector3(Random.Range(minX, maxX), Random.Range(minY, maxY), Random.Range(minZ, maxZ));
            // The +1000 are hardcoded in here to send the republic fighters into the spawn location of the trade federation
            spawnRepublicFighters(repFighter, pos, (GameObject.Find("dfighter").transform), new Vector3(Random.Range(minX + 1000, maxX + 1000), Random.Range(minY, maxY), Random.Range(minZ, maxZ)));

            repPositions[i] = pos;
        }

        //need to mirror area the ships can spawn in across the Z axis
        //if minX is -5000 and maxX is -3000 -> the values we need are minX = -3000 and max X -1000
        // minX becomes maxX
        int xDifference = maxX - minX;
        int temp = maxX;
        maxX = maxX + xDifference;
        minX = temp;

        //instantiate the trade federation fighters
        for (int i = 0; i < numFighters; i++)
        {
            Vector3 pos = new Vector3(Random.Range(minX, maxX), Random.Range(minY, maxY), Random.Range(minZ, maxZ));
            
            spawnFighterMod(droidFighter, pos, (GameObject.Find("Planet").transform), repPositions[i]);
            
        }

        

        

    }
	
	// Update is called once per frame
	void Update () {
	
        if(Time.time > endTime)
        {
            Application.LoadLevel(3);
        }

	}

    //Method to spawn fighters for scene 3
    //Used instantiating fighters in a large scattered formation
    public void spawnFighterMod(GameObject starFighter, Vector3 pos, Transform lookAt, Vector3 target)
    { 
        //Instantiate fighter and attach script
        GameObject fighter = Instantiate(starFighter, pos, Quaternion.identity) as GameObject;

        fighter.transform.LookAt(lookAt);

        fighter.GetComponent<DroidPath>().timeToLive = 20.0f;
        fighter.GetComponent<DroidPath>().seekEnabled = true;
        fighter.GetComponent<DroidPath>().wanderEnabled = false;
        fighter.GetComponent<DroidPath>().targetObject = target;    
        fighter.GetComponent<DroidPath>().Seek(target);

    }

    public void spawnRepublicFighters(GameObject starFighter, Vector3 pos, Transform lookAt, Vector3 target)
    {
        //Instantiate fighter and attach script
        GameObject fighter = Instantiate(starFighter, pos, Quaternion.identity) as GameObject;
        fighter.transform.LookAt(lookAt);

        fighter.GetComponent<SimpleSeek>().seekEnabled = true;
        fighter.GetComponent<SimpleSeek>().target = target;    

        //fighter.AddComponent<DroidPath>();

    }
}

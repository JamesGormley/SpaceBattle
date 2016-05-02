using UnityEngine;
using System.Collections;

public class SpawnFighters : MonoBehaviour {

    public GameObject droidFighter;

    DroidPath dp;
    
    //Place fighters will spawns
    public GameObject spawnPos;

    public bool noFighters = true;
    private float nextSpawn = 0.1f;
    public float spawnInterval = 0.5f;
    public int numFighters = 10;
    int count = 0;

	// Use this for initialization
	void Start () {

        //initial spawn time
        nextSpawn += Time.time + spawnInterval;

    }
	
	// Update is called once per frame
	void Update () {


        if (noFighters)
        {
            //spawn fighter when nextspawn time has elapsed --- nextspawn dictated in spawnfighter method
            if (Time.time >= nextSpawn)
            { 
                spawnFighter(droidFighter, dp, spawnPos.transform.position);
                count++;

                //stop spawning fighters when max number reached
                if (count == numFighters)
                {
                    noFighters = false;
                    //open next scene
                    Application.LoadLevel(2);
                }
                
            }
        }

    }

    //Method to spawn fighters
    public void spawnFighter(GameObject droidFighter, DroidPath droidPath, Vector3 pos )
    {
        //Instantiate fighter and attach script
        GameObject fighter = Instantiate(droidFighter, pos, Quaternion.identity) as GameObject;


        fighter.GetComponent<DroidPath>().seekEnabled = true;
        fighter.GetComponent<DroidPath>().wanderEnabled = true;
        fighter.GetComponent<DroidPath>().Seek(pos);
        fighter.GetComponent<DroidPath>().timeToLive = 8;

        nextSpawn = Time.time + spawnInterval;
    }
}

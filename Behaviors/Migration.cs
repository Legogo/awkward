using UnityEngine;
using System.Collections;

/*
 * Permet de déplacer un objet d'un pt A a un pt B
 * */

public class Migration : MonoBehaviour {
	
	public Vector3 destination;
	float speed = 5f;
	
	bool atDestination = false;
	
	float MIGRATION_DEATH_TIMER = 5f;
	float deathMigrationTimer = 0f;
	
	void Awake(){
		enabled = false;
	}
	
	public void spawn(Vector3 startFrom, Vector3 destination){
		transform.position = startFrom;
    setDestination(destination);
	}
	
	public void kill(){
		enabled = false;
		GameObject.Destroy(this);
	}
	
	public void setDestination(Vector3 newDestination){
		destination = newDestination;
		atDestination = false;
		deathMigrationTimer = MIGRATION_DEATH_TIMER;
		
		//Debug.Log("new destination for "+name+" is "+destination);
		
		enabled = true;
	}
	
	void Update () {
		if(!atDestination){
			transform.position = Vector3.MoveTowards(transform.position, destination, speed * Time.deltaTime);
			if(isAtDestination()) atDestinationEvent();
		}else{
			deathMigrationTimer -= Time.deltaTime;
			if(deathMigrationTimer <= 0f){
				Debug.Log("Migration killed "+name);
				GameObject.Destroy(gameObject);
			}
		}
	}
	
	void atDestinationEvent(){
		atDestination = true;
		transform.position = destination;
	}
	
	bool isAtDestination(){
		return Vector3.Distance(transform.position, destination) < (speed * Time.deltaTime);
	}
}

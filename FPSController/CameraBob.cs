using UnityEngine;
using System.Collections;

public class CameraBob : MonoBehaviour
{
	Vector3 originPosition;

	public float slantSpeed = 10f;
	public float slantAmplitude = 0.5f; // taille de l'angle, 0.5
	float slantRatio = 0f; // counter
	float slantResult = 0f;

	public float verticalSpeed = 20f; // 20
	public float verticalAmplitude = 0.05f; // 0.05
	float verticalRatio = 0f; // counter

	public AnimationCurve progressionCurve;

	public Movement playerMovement; // to connect to behavior

	private void Start() {

		originPosition = transform.localPosition;

		if(playerMovement == null){
      Debug.LogWarning("Camera bob need a <Movement> script");
			enabled = false;
			return;
		}

		slantRatio = Random.Range(0f,Mathf.PI);
		verticalRatio = Random.Range(0f, Mathf.PI);
	}
	
	private void Update() {
		applySlanting();
		applyVertical();
	}

	void applySlanting(){
		if(playerMovement.isCrouching())	return;

		slantRatio += Time.deltaTime * slantSpeed;
		if(slantRatio > Mathf.PI * 2f)	slantRatio = 0f;
		slantResult = Mathf.Cos(slantRatio);
		transform.rotation = transform.parent.rotation * Quaternion.AngleAxis(slantResult * slantAmplitude * playerSpeed(), Vector3.forward);
	}

	void applyVertical(){
		if(playerMovement.isCrouching())	verticalRatio += Time.deltaTime * verticalSpeed * 4f;
		else verticalRatio += Time.deltaTime * verticalSpeed;
		
		if(verticalRatio > Mathf.PI * 2f)	verticalRatio = 0f;
    transform.localPosition = originPosition + (Vector3.up * Mathf.Cos(verticalRatio) * verticalAmplitude * playerSpeed());
	}

	float playerSpeed(){
		return progressionCurve.Evaluate(playerMovement.getSpeed() * 0.1f);
	}

	public bool DEBUG_DRAW = false;
	void OnGUI(){
		if(!DEBUG_DRAW)	return;
		GUI.TextArea(new Rect(10,50,300,100), "playerSpeed ? "+playerSpeed()+"\nslant ? "+slantResult);
	}
}

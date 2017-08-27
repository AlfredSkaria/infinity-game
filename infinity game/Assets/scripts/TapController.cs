using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class TapController : MonoBehaviour {

	public delegate void PlayerDelegate();
	public static event PlayerDelegate OnPlayerDied;
	public static event PlayerDelegate OnPlayerScored;

	public float tapForce = 10;
	public float tiltSmooth = 5;
	public Vector3 startPos;
	private	Rigidbody2D rigidbody1;
	Quaternion downRotation;
	Quaternion forwardRotation;

	GameManager game;

	// Use this for initialization
	void Start () {
		rigidbody1 = GetComponent<Rigidbody2D>();
		downRotation = Quaternion.Euler (0,0,-90);
		forwardRotation = Quaternion.Euler (0, 0, 35);
		game = GameManager.Instance;
		rigidbody1.simulated = false;

	}

	void OnEnable(){
		GameManager.OnGameStarted += OnGameStarted;
		GameManager.OnGameOverConfirmed += OnGameOverConfirmed;
	}

	void OnDisable(){
		GameManager.OnGameStarted -= OnGameStarted;
		GameManager.OnGameOverConfirmed -= OnGameOverConfirmed;
	}

	void OnGameStarted(){
		rigidbody1.velocity = Vector3.zero;
		rigidbody1.simulated = true;
	}

	void OnGameOverConfirmed(){

		transform.localPosition = startPos;
		transform.rotation = Quaternion.identity;
	}
	
	// Update is called once per frame
	void Update () {
		if (game.GameOver)
			return;
		if (Input.GetMouseButtonDown (0)) {
			//Time.timeScale += 1;
			transform.rotation = forwardRotation; 
			rigidbody1.velocity = Vector3.zero;
			rigidbody1.AddForce (Vector2.up * tapForce, ForceMode2D.Force);
		}

		transform.rotation = Quaternion.Lerp(transform.rotation, downRotation, tiltSmooth*Time.deltaTime);
	}

	void OnTriggerEnter2D(Collider2D col){
		if (col.gameObject.tag == "ScoreZone") {
		//register a score count
			OnPlayerScored(); // event sent to GameManager
			//playsound
		}

		if (col.gameObject.tag == "DeadZone") {
			rigidbody1.simulated = false;
			//register a deadevent
			OnPlayerDied(); //event sent to GameManager
			//play sound
		}
	}
}

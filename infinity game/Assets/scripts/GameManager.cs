using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameManager : MonoBehaviour {

	public delegate void GameDelegate();
	public static event GameDelegate OnGameStarted;
	public static event GameDelegate OnGameOverConfirmed;
	public static GameManager Instance;

	public GameObject StartPage;
	public GameObject GameOverPage;
	public GameObject CountdownPage;
	public Text ScoreText;

	enum PageState{
		None,
		Start,
		GameOver,
		Countdown
	}

	int score = 0;
	bool gameOver = true;

	public bool GameOver {get { return gameOver;}}

	public int Score {get { return score;}}

	void Awake(){
		Instance = this;
	}

	void OnEnable(){
		CountDownText.OnCountdownFinished += OnCountdownFinished;
		TapController.OnPlayerDied += OnPlayerDied;
		TapController.OnPlayerScored += OnPlayerScored;
	}

	void OnPlayerDied ()
	{
		gameOver = true;
		int savedScore = PlayerPrefs.GetInt ("HighScore");
		if (score > savedScore) {
			PlayerPrefs.SetInt ("HighScore", score);
		}

		SetPageState (PageState.GameOver);
	}
	void OnPlayerScored(){
		score++;
		ScoreText.text = score.ToString ();
	}

	void OnCountdownFinished ()
	{
		SetPageState (PageState.None);	
		OnGameStarted ();   //event sent to TapController
		score = 0;
		gameOver = false;
	}

	void OnDisable(){
		CountDownText.OnCountdownFinished -= OnCountdownFinished;
		TapController.OnPlayerDied -= OnPlayerDied;
		TapController.OnPlayerScored -= OnPlayerScored;
	}

	void SetPageState(PageState state){
		switch (state) {
		case PageState.None:
			StartPage.SetActive (false);
			GameOverPage.SetActive (false);
			CountdownPage.SetActive (false);
			break;
		
		case PageState.Start:
			StartPage.SetActive (true);
			GameOverPage.SetActive (false);
			CountdownPage.SetActive (false);
			break;

		case PageState.GameOver:
			StartPage.SetActive (false);
			GameOverPage.SetActive (true);
			CountdownPage.SetActive (false);
			break;
		case PageState.Countdown:
			StartPage.SetActive (false);
			GameOverPage.SetActive (false);
			CountdownPage.SetActive (true);
			break;
		
		}
	}


	public void ConfirmGameOver(){
		//activated when replay button is hit

		OnGameOverConfirmed(); //event sent to TapController
		ScoreText.text = "0";
		SetPageState (PageState.Start);
	}

	public void StartGame(){
		//activated when play button is hit
		SetPageState(PageState.Countdown);
	}
}

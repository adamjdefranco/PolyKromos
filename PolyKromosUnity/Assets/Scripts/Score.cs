using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Score : MonoBehaviour {

	public GameObject player;
	public int score;
	public bool updateScore;
	public Text scoreText;
	public float zDistanceAtScoreIncrement;

	public static Score singleton;

	//Singleton
	void Awake() {
		if (singleton != null) {
			Object.Destroy(singleton);
		} else {
			singleton = this;
		}

		DontDestroyOnLoad(this);
	}

	void Start () {
		score = 0;
		zDistanceAtScoreIncrement = 0;
		updateScore = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (updateScore && !HasAlreadyScoredAtThisBarrierGroup()) {
			score++;
            PlayerPrefsController.singleton.SetCurrentAmountOfAchievementByAchievementType("Points");
			updateScore = false;
			scoreText.text = score.ToString ();

            // Reset the counter that tells PlayerController when to increment the player's speed
            PlayerController.singleton.IncrementPointsSinceLastSpeedIncrement();

            //Generate a fireball or do nothing
            FireballGeneration.singleton.GenerateFireball();
		}
	}

	// Returns true if the player has scored a point within 1 unit of where they are now, used to insure that you cannot score multiple points at the same barrier group
	public bool HasAlreadyScoredAtThisBarrierGroup() {
		//Debug.Log ("Player Distance: " + zDistanceAtScoreIncrement + "Last Distance: " + zDistanceAtScoreIncrement);
		bool hasAlreadyScored = player.gameObject.transform.position.z - zDistanceAtScoreIncrement < 1;
		if (hasAlreadyScored) {
			updateScore = false;
		} else { 
			zDistanceAtScoreIncrement = player.gameObject.transform.position.z;
		}
		return hasAlreadyScored;
	}

	public void resetScore() {
		score = 0;
		zDistanceAtScoreIncrement = 0;
		updateScore = false;
		scoreText.text = score.ToString ();
	}
}

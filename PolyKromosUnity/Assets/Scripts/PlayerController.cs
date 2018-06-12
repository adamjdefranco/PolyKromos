using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

	#region PublicVariables

	//GameObjects
	public GameObject[] MainMenuButtons;
	public GameObject PowerupParticleSystemParent;
	public GameObject powerupParticleSystem;
	public GameObject inGameCanvas;
	public GameObject mainMenuCanvas;
	public GameObject groundSeg;
	public GameObject ParticleSystemAndFloor;
	public GameObject currentGroundSegment;
	public GameObject FireballDeathParticleSystem;
    public GameObject ButtonInstructionsText;

    public FireballGeneration fireballControl;

	public float speedForTurning;
	public float memoizedSpeed;
	public float speedIncreasing;
	public float minimumForwardSpeed;
	public float scoreForParticleSystemToTurnOff;
	public float forward;
	public float zDistance;
	public float ballsVelocity;
	public float TiltSpeed = 100.0f;

	public Image RedButton;
	public Image BlueButton;
	public Image YellowButton; 

	public Material rainbow;
	public Material normal;

	public bool spawnNewTrack;
    public bool isInPowerupMode;

	#endregion

	#region PrivateVariables

	private bool hasGameStarted;
	private bool redDown;
	private bool yellowDown;
	private bool blueDown;
    private bool isInEndGameState;

	private GameObject pastGroundSegment;

    private int pointsSinceLastSpeedIncrement;
	private float zDistanceToDeleteOldSegment;
    private float previousZDistance;
    private float distanceForIncrementingDistancePowerup;
    private float mobileTiltAngle;
    private Vector3 mobileTiltMovement;

	private Rigidbody rb;
	private Renderer rend;

	#endregion

	public float GetSpeedIncreasing() {
		return speedIncreasing;
	}

	public static PlayerController singleton;

	//Singleton 
	void Awake() {
		if (singleton != null) {
			Object.Destroy(singleton);
		} else {
			singleton = this;
		}
	}

	void Start () {
        previousZDistance = this.transform.position.z;
        distanceForIncrementingDistancePowerup = 0;
		isInPowerupMode = false;
		rb = GetComponent<Rigidbody>();
		redDown = false;
		yellowDown = false;
		blueDown = false;
		forward = 0.0f;
		scoreForParticleSystemToTurnOff = 0;
		zDistance = 50;
		zDistanceToDeleteOldSegment = 105f;
		ballsVelocity = speedIncreasing;
		memoizedSpeed = speedIncreasing;
		hasGameStarted = false;
		rend = GetComponent<Renderer>();
        isInEndGameState = false;
        pointsSinceLastSpeedIncrement = 0;

		powerupParticleSystem.SetActive (false);
		PowerupParticleSystemParent.SetActive (false);
	}

	public void reset() {
		TurnOffPowerup ();
		rb = GetComponent<Rigidbody>();
		redDown = false;
		yellowDown = false;
		blueDown = false;
		forward = 0.0f;
		zDistance = 50;
		scoreForParticleSystemToTurnOff = 0;
        distanceForIncrementingDistancePowerup = 0;
		zDistanceToDeleteOldSegment = 105f;
		speedIncreasing = 10f;
		ballsVelocity = 10f;
		memoizedSpeed = 10f;
		hasGameStarted = false;
        isInEndGameState = false;
        pointsSinceLastSpeedIncrement = 0;
	}

	public void resetPositionOfPlayer() {
		transform.position = new Vector3 (0f, 0.2f, 3f);
        previousZDistance = this.transform.position.z;
		DestroyObjectsForGameReset ();
		pastGroundSegment = (GameObject)Instantiate (groundSeg, new Vector3 (0, 0, 0), Quaternion.identity);
		pastGroundSegment.tag = "GroundSegment";
		PowerupParticleSystemParent.SetActive (false);
		powerupParticleSystem.SetActive (false);
	}

	public void StopPlayerFromMovingOrLosingPointsDuringEndGame() {
		forward = 0.0f;
	}

	public IEnumerator WaitFor(float time) {
		yield return new WaitForSeconds (time);
	}

	// Transition from main menu to start of game
	public void StartGame() {
        Sounds.singleton.PlayStartGame();

		// Animate the camera backwords to the start of the first ground segment
		CameraAnimationsController.singleton.GetComponent<Animator> ().Play ("AnimateCameraFromMainMenuToStartGame");

        //TODO: Move these steps to UIController
        //ButtonInstructionsText.SetActive(true);
		mainMenuCanvas.GetComponent<Animator> ().Play ("AnimateOutMainMenu");

		// Begin the wait for the countdown before starting a game
		//print("Pre Wait for game: " + CameraAnimationsController.singleton.transform.position);
		StartCoroutine(WaitToStartGame ());
		//print("post Wait for game: " + CameraAnimationsController.singleton.transform.position);
	}

	public void DisableMainMenuCanvas() {
		//mainMenuCanvas.GetComponent<CanvasGroup> ().alpha = 1;
		mainMenuCanvas.SetActive (false);
		mainMenuCanvas.GetComponent<CanvasGroup> ().alpha = 1;
	}

	public void EnableMainMenuCanvas() {
		mainMenuCanvas.SetActive (true);
	}

	// Turns on the invicibility powerup, turns it off after four points have been obtained.
	public void TurnOnPowerup() {
		SwitchPlayerMaterialToRainbow ();
		isInPowerupMode = true;
		PowerupParticleSystemParent.SetActive (true);
		powerupParticleSystem.SetActive (true);
		scoreForParticleSystemToTurnOff = Score.singleton.score + 4;
		// You may not obtain another powerup/heart/clock while in powerup mode
		SetBarrierParticleSystemVisibility(false, "Powerup");
		SetBarrierParticleSystemVisibility(false, "HeartsParticleSystem");
		SetBarrierParticleSystemVisibility(false, "ClocksParticleSystem");
        PlayerPrefsController.singleton.SetCurrentAmountOfAchievementByAchievementType("RainbowPowerups");
	}

	// Shut off powerup mode
	public void TurnOffPowerup() {
		PowerupParticleSystemParent.SetActive (false);
		powerupParticleSystem.SetActive (false);
		SwitchPlayerMaterialToNormal ();
		isInPowerupMode = false;
        SetBarrierParticleSystemVisibility(true, "Powerup");
		if (UIController.singleton.GetCurrentHeartsFillAmount () != 1) {
			SetBarrierParticleSystemVisibility (true, "HeartsParticleSystem");
		} 
		if(speedIncreasing > minimumForwardSpeed) {
			SetBarrierParticleSystemVisibility (true, "ClocksParticleSystem");
		}
	}

	// Make the player go one unit slower, triggered by going through a clocks powerup
	public void DecrementSpeedFromClocksPowerup() {
        PlayerPrefsController.singleton.SetCurrentAmountOfAchievementByAchievementType("ClockPowerups");
		speedIncreasing -= 1.5f;
		memoizedSpeed = speedIncreasing;
        pointsSinceLastSpeedIncrement = 0;
		if (speedIncreasing == minimumForwardSpeed) {
			SetBarrierParticleSystemVisibility (false, "ClocksParticleSystem");
		}
	}

	public float GetCurrentGroundSegmentYValue() {
		return zDistance;
	}

    public void SetEndGameState(bool IsEndGame) {
        isInEndGameState = IsEndGame;
    }

    public bool GetEndGameState() {
        return isInEndGameState;
    }

    public void IncrementPointsSinceLastSpeedIncrement() {
        pointsSinceLastSpeedIncrement++;
    }

	public IEnumerator WaitToStartGame() {
		//Debug.Log ("Here1");
		//yield return new WaitForSeconds(5);
		//print("pre wait for secs: " + CameraAnimationsController.singleton.transform.position);
		yield return new WaitForSeconds(3);
		//print("post wait for secs: " + CameraAnimationsController.singleton.transform.position);
		UIController.singleton.EditCountDownNumber (3);
		Sounds.singleton.PlayCountdownTick ();
		UIController.singleton.ShowCountDown (true);
		yield return new WaitForSeconds(1);
		UIController.singleton.EditCountDownNumber (2);
		Sounds.singleton.PlayCountdownTick ();
		yield return new WaitForSeconds(1);
		UIController.singleton.EditCountDownNumber (1);
		Sounds.singleton.PlayCountdownTick ();
		yield return new WaitForSeconds(1);
		UIController.singleton.ShowCountDown (false);
		Sounds.singleton.PlayCountdownBegin ();
		forward = 0.3f;
		hasGameStarted = true;
	}

    void Update() {
        // These next few lines increment the distance achievement amount by 1 every time the player has traveled by 1 unit
        distanceForIncrementingDistancePowerup += this.transform.position.z - previousZDistance;
        previousZDistance = this.transform.position.z;
        if (distanceForIncrementingDistancePowerup > 1) {
            distanceForIncrementingDistancePowerup--;
            PlayerPrefsController.singleton.SetCurrentAmountOfAchievementByAchievementType("Distance");
        }

        if (PowerupParticleSystemParent.activeInHierarchy) {
			PowerupParticleSystemParent.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - 0.2f);
        }

        if (gameObject.transform.position.z > zDistance && !UIController.singleton.isEndGamePanelOn()) {
            pastGroundSegment = (GameObject)Instantiate(groundSeg, new Vector3(0, 0, zDistance + 50f), Quaternion.identity);
            pastGroundSegment.tag = "GroundSegment";
            zDistance += 100f;
            //Debug.Log ("zdist from playerController" + zDistance);
        } else if (gameObject.transform.position.z > zDistanceToDeleteOldSegment) {
            currentGroundSegment = pastGroundSegment;
            DeleteGameObjectsBehindPlayer();
            zDistanceToDeleteOldSegment += 100f;
        }

        // Increases the players speed by 1 if they have collected 20 points since the last speed increment or decrement (Can be moved to a method, call from score)
        if (pointsSinceLastSpeedIncrement > 9) {
            speedIncreasing += .75f;
            memoizedSpeed = speedIncreasing;
            if (speedIncreasing > minimumForwardSpeed && !isInPowerupMode) {
                SetBarrierParticleSystemVisibility(true, "ClocksParticleSystem");
            }
            pointsSinceLastSpeedIncrement = 0;
        }

        //There has got to be a way to do this outside of update? do it in the score class maybe? then check when it's been incremented?
        if (isInPowerupMode && Score.singleton.score > scoreForParticleSystemToTurnOff) {
            TurnOffPowerup();
        }

        #if UNITY_STANDALONE_OSX
        //print("true")
        if (!isInPowerupMode && !UIController.singleton.isPaused && !isInEndGameState) {
            if (Input.GetKey(KeyCode.Z) && Input.GetKey(KeyCode.X)) {
                // Orange
                //rend.material.color = new Color32(255,100,0, 150);
                rend.material.color = RainbowColors.singleton.getOrange();
            } else if (Input.GetKey(KeyCode.Z) && Input.GetKey(KeyCode.C)) {
                // Purple
                rend.material.color = RainbowColors.singleton.getViolet();
            } else if (Input.GetKey(KeyCode.C) && Input.GetKey(KeyCode.X)) {
                rend.material.color = RainbowColors.singleton.getGreen();
            } else if (Input.GetKey(KeyCode.Z)) {
                rend.material.color = RainbowColors.singleton.getRed();
            } else if (Input.GetKey(KeyCode.X)) {
                rend.material.color = RainbowColors.singleton.getYellow();
            } else if (Input.GetKey(KeyCode.C)) {
                rend.material.color = RainbowColors.singleton.getBlue();
            } else {
                if (UIController.singleton.ThemeColor == "Light") {
                    rend.material.color = RainbowColors.singleton.getWhite();
                } else {
                    rend.material.color = RainbowColors.singleton.getBlack();
                }
            }
        }

        #endif

        #if UNITY_IOS
		if(!isInPowerupMode && !Object.FindObjectOfType<UIController>().isPaused && !isInEndGameState) {
            if (redDown && yellowDown) {
                // Orange
                rend.material.color = RainbowColors.singleton.getOrange(); //new Color32(255,100,0, 1);
            } else if (redDown && blueDown) {
                // Purple
                rend.material.color = RainbowColors.singleton.getViolet();  //new Color32(100,0,255, 1);
            } else if (yellowDown && blueDown) {
                rend.material.color = RainbowColors.singleton.getGreen(); //Color.green;
            } else if (redDown) {
                rend.material.color = RainbowColors.singleton.getRed();  //Color.red; // Red
            } else if (yellowDown) {
                rend.material.color = RainbowColors.singleton.getYellow(); //Color.yellow; // Yellow
            } else if (blueDown) {
                rend.material.color = RainbowColors.singleton.getBlue(); // Color.blue; // Blue
            } else {
                if(UIController.singleton.ThemeColor == "Light") {
                    rend.material.color = RainbowColors.singleton.getWhite();
                } else {
                    rend.material.color = RainbowColors.singleton.getBlack();
                }
            }
        }

        #endif

		#if UNITY_ANDROID
		if(!isInPowerupMode && !Object.FindObjectOfType<UIController>().isPaused && !isInEndGameState) {
			if (redDown && yellowDown) {
				// Orange
				rend.material.color = RainbowColors.singleton.getOrange(); //new Color32(255,100,0, 1);
			} else if (redDown && blueDown) {
				// Purple
				rend.material.color = RainbowColors.singleton.getViolet();  //new Color32(100,0,255, 1);
			} else if (yellowDown && blueDown) {
				rend.material.color = RainbowColors.singleton.getGreen(); //Color.green;
			} else if (redDown) {
				rend.material.color = RainbowColors.singleton.getRed();  //Color.red; // Red
			} else if (yellowDown) {
				rend.material.color = RainbowColors.singleton.getYellow(); //Color.yellow; // Yellow
			} else if (blueDown) {
				rend.material.color = RainbowColors.singleton.getBlue(); // Color.blue; // Blue
			} else {
				if(UIController.singleton.ThemeColor == "Light") {
					rend.material.color = RainbowColors.singleton.getWhite();
				} else {
					rend.material.color = RainbowColors.singleton.getBlack();
				}
			}
		}

		#endif
    }

    void FixedUpdate() {
        float moveHorizontal;

        if (!isInEndGameState && !(transform.position.x > 2.5 || transform.position.x < -2.5)) {
            moveHorizontal = Input.GetAxis("Horizontal");
        } else {
            moveHorizontal = 0;
        }

        Vector3 movement = new Vector3(moveHorizontal * 0.5f, 0.0f, forward);
        //Vector3 turns = new Vector3 (moveHorizontal, 0, 0);

        if (transform.position.x > 2.5 || transform.position.x < -2.5) {
            rb.AddForce(new Vector3(0, -40f, 0));
        } else {
            rb.velocity = movement * speedIncreasing;
        }

        if (transform.position.y < -0.5f && !UIController.singleton.isEndGamePanelOn() && !isInEndGameState) {
            rb.velocity = new Vector3(0, 0, 0);
            UIController.singleton.EndGameActions();
        }


        #if UNITY_IOS


        // IPHONE INPUT
        if (!isInEndGameState && !(transform.position.x > 2.5 || transform.position.x< -2.5)) {
            mobileTiltAngle = Input.acceleration.x;
        } else {
            moveHorizontal = 0;
        }

        mobileTiltAngle *= Time.deltaTime;
        mobileTiltAngle *= TiltSpeed* 13;

        mobileTiltMovement = new Vector3(mobileTiltAngle, 0.0f, 0.0f);

        if(hasGameStarted && !GameObject.FindObjectOfType<UIController>().isPaused && !(transform.position.x > 2.5 || transform.position.x< -2.5) && !isInEndGameState) {
            rb.AddForce(mobileTiltMovement* speedIncreasing);
        }

        if(!GameObject.FindObjectOfType<UIController>().isPaused && !(transform.position.x > 2.5 || transform.position.x< -2.5) && !isInEndGameState) {
            rb.velocity = movement* speedIncreasing;
        }


        #endif

		#if UNITY_ANDROID


		// ANDROID INPUT
		if (!isInEndGameState && !(transform.position.x > 2.5 || transform.position.x< -2.5)) {
		mobileTiltAngle = Input.acceleration.x;
		} else {
		moveHorizontal = 0;
		}

		mobileTiltAngle *= Time.deltaTime;
		mobileTiltAngle *= TiltSpeed* 13;

		mobileTiltMovement = new Vector3(mobileTiltAngle, 0.0f, 0.0f);

		if(hasGameStarted && !GameObject.FindObjectOfType<UIController>().isPaused && !(transform.position.x > 2.5 || transform.position.x< -2.5) && !isInEndGameState) {
		rb.AddForce(mobileTiltMovement* speedIncreasing);
		}

		if(!GameObject.FindObjectOfType<UIController>().isPaused && !(transform.position.x > 2.5 || transform.position.x< -2.5) && !isInEndGameState) {
		rb.velocity = movement* speedIncreasing;
		}


		#endif
    }


	#region EditPlayerMaterial

	public void SwitchPlayerMaterialToRainbow() {
		GetComponent<Renderer> ().material = rainbow;
	}

	public void SwitchPlayerMaterialToNormal() {
		GetComponent<Renderer> ().material = normal;
	}

	#endregion

	#region DestorySegmentsBehindPlayer

	private void DeleteGameObjectsBehindPlayer() {
		DeleteObjectsWithTag("Barrier");
		DeleteObjectsWithTag ("BarrierInner");
		DeleteObjectsWithTag ("BarrierDestroyedParticleSystem");
		DeleteObjectsWithTag ("Powerup");
		DeleteObjectsWithTag ("HeartsParticleSystem");
		DeleteObjectsWithTag ("ClocksParticleSystem");
		DeletePastGroundSegments ();
	}

	private void DeleteObjectsWithTag(string tag) {
		GameObject[] gameObjects = GameObject.FindGameObjectsWithTag (tag);
		for (int i = 0; i < gameObjects.Length; i++) {
			if (gameObjects [i].transform.position.z < zDistanceToDeleteOldSegment) {
				Destroy (gameObjects [i]);
			}
		}
	}

	//This could be included in the last segment but for ease of reading taken out due to needing to change the zdistance check
	private void DeletePastGroundSegments() {
		GameObject[] gameObjects = GameObject.FindGameObjectsWithTag ("GroundSegment");
		for (int i = 0; i < gameObjects.Length; i++) {
			if (gameObjects [i].transform.position.z + 5.0f < zDistanceToDeleteOldSegment && gameObjects [i].transform.position.z != 0) {
				Destroy (gameObjects [i]);
			}
		}
	}

	public void SetBarrierParticleSystemVisibility(bool visible, string tag) {
		GameObject[] particleSystemsInScene = GameObject.FindGameObjectsWithTag(tag);
		for (int i = 0; i < particleSystemsInScene.Length; i++) {
            if (!visible) {
				particleSystemsInScene[i].GetComponent<ParticleSystem>().Stop();
			} else if (particleSystemsInScene[i].transform.position.z > this.transform.position.z + 1f){
				particleSystemsInScene[i].GetComponent<ParticleSystem>().Play();
            }
		}
	}

	#endregion

	#region DestroyObjectsOnGameRestart

	//TODO: Clean this up
	// Destroys all of the barriers in the scene
	private void DestroyObjectsForGameReset() {
		DeleteObjectsWithTagForReset("Barrier");
		DeleteObjectsWithTagForReset("BarrierDestroyedParticleSystem");
		DeleteObjectsWithTagForReset("BarrierInner");
		DeleteObjectsWithTagForReset("Powerup");
		DeleteObjectsWithTagForReset("HeartsParticleSystem");
		DeleteObjectsWithTagForReset ("ClocksParticleSystem");
		DeleteObjectsWithTagForReset ("Fireball");
		DeleteObjectsWithTagForReset ("GroundSegment");
	}

	private void DeleteObjectsWithTagForReset(string tag) {
		GameObject[] gameObjects = GameObject.FindGameObjectsWithTag (tag);
		for (int i = 0; i < gameObjects.Length; i++) {
				Destroy (gameObjects [i]);
		}
	}

	#endregion

	// Fireball collisons - player loses a heart
	void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == "Fireball") {
			if (!isInPowerupMode) {
				UIController.singleton.DecrementHeart ();
				Sounds.singleton.PlayIncorrectColorSound ();
				if(Sounds.singleton.GetSoundsAreOn()) Handheld.Vibrate ();
				CameraAnimationsController.singleton.ShakeCamera ();
			} else {
				ActivateFireballDeathParticleSystem (other.transform.position);
				Destroy (other.gameObject);
                Sounds.singleton.PlayCorrectColorSound();
                PlayerPrefsController.singleton.SetCurrentAmountOfAchievementByAchievementType("Demolishers");
			}
		}
	}

	private void ActivateFireballDeathParticleSystem(Vector3 pos) {
		GameObject.Instantiate (FireballDeathParticleSystem, pos, Quaternion.identity);
	}

    //On end game, make sure all fireballs explode so that when you lerp back to the main menu you do not see fireballs that have just been generated. Also it looks pretty cool.
    public void ExplodeAllFireballsInScene() {
        GameObject[] fireballsInScene = GameObject.FindGameObjectsWithTag("Fireball");
        foreach(GameObject fireball in fireballsInScene) {
            //Only delete/create explosion for the parent object. The child objects of the fireball are also tagged with the fireball name.
            if(fireball.name.Contains("FireBall")) {
                GameObject.Instantiate(FireballDeathParticleSystem, fireball.transform.position, Quaternion.identity);
                Destroy(fireball);
            }
        }
    }

	#region ButtonControls

	// Red Button
	public void redClicked() { redDown = true; }
	public void redReleased() { redDown = false; }

	// Yellow Button
	public void yellowClicked() { yellowDown = true; }
	public void yellowReleased() { yellowDown = false; }

	// BlueButton
	public void blueClicked() { blueDown = true; }
	public void blueReleased() { blueDown = false; }

	#endregion
}
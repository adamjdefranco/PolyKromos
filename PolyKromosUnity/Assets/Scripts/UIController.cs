using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;

public class UIController : MonoBehaviour {

    public GameObject BackgroundSquares;

	public GameObject InGameCanvas;

    public Sprite PolyKromosLogoTextGray;
    public Sprite PolyKromosLogoTextWhite;

    //Sound Icon 
    public Image SoundIcon;
    public Image SoundIconOffSlash;
    public Image SoundIconInGame;
    public Image SoundIconOffSlashInGame;
    public Sprite SoundIconOn;
    public Sprite SoundIconOff;
    public bool soundsOn;

    public ParticleSystem ClockParticleSystem;
    public Material ClockParticleSystemLightMode;
    public Material ClockParticleSystemDarkMode;

	public bool EndGameAnimateToMainMenu;
	public float ScrollerLerpSpeed = 8.5f;
	public string ThemeColor = "Light";
	public Material GroundSegmentMaterial;

	public Button LeftAchievementButton;
	public Button RightAchievementButton;

	public GameObject BackgroundRectanglesLight;
	public GameObject BackgroundRectanglesDark;

	public GameObject SecretAchievementLeft;
	public GameObject SecretAchievementMiddle;
	public GameObject SecretAchievementRight;

	public Image[] SecretAchievementThemeColorImages;

    public Text[] SecretAchievementLabel;
    public Text[] SecretAchievementPictureLabel;

	public Text[] MainMenuText;

    public Text AvoidObstaclesInstructionText;

	public Image[] MainMenuImagesLightestGray;
	public Image[] MainMenuImagesDarkerGray;

	public Image TiltToSteerImage;
	public Image DoNotCrossBarrier;

    public Image[] DividerImages;

	public Text InGameScoreText;

	public ScrollRect achievementScrollRect;
	private bool scrollLeft = false;
	private bool scrollRight = false;
	private bool scrollToCenter = false;

    public GameObject AchievementIconZone;
    public GameObject moveableAchievementZoom;
    public Image moveableAchievementImage;

	public static UIController singleton;

    private int visibleAchievementIndex;
    public Text achievementLabel;
	private float heartsFillAmount;
	public Image hearts;
	public RectTransform heartsTransform;
	public RectTransform miniAchievementIconsZone;

	public GameObject RedButton;
	public GameObject YellowButton;
	public GameObject BlueButton;
	public GameObject pauseButton;
	public GameObject countDown;
	public GameObject InstructionsPictures;
	public GameObject achievements;
	//public GameObject endGamePanel;
	public GameObject endGameStats;
	//public GameObject endGameBackground;
	public GameObject mainMenuCanvas;
	public GameObject logo;

	public bool isPaused;

	public GameObject playButton;
	public GameObject scoresButton;
	public GameObject achievementsButton;
	public GameObject InstructionsButton;

	public GameObject[] pageCircles;

	public Image[] achievementImagesLeft;
	public Text[] achievementTextLeft;
	public Image[] achievementImagesMiddle;
	public Text[] achievementTextMiddle;
	public Image[] achievementImagesRight;
	public Text[] achievementTextRight;

	public GameObject[] achievementIconLockImagesLeft;
	public GameObject[] achievementIconLockImagesMiddle;
	public GameObject[] achievementIconLockImagesRight;

	public GameObject PolyKromosTitleText;
	//public GameObject HighScoreText;

	public GameObject topScores;
	public GameObject backButton;

	public Text[] topScoresText;
	public Text EndGameScoreText;
	public Text EndGameBestScoreText;

	private bool endGameStatsCurrentlyDisplayed = false;
	private bool highScoresCurrentlyDisplayed = false;
	private bool achievementsCurrentlyDisplayed = false;
	private bool instructionsCurrentlyDisplayed = true;

	private bool swipeTimerOn = false;
	private float swipeTimer;

	private int PowerupsCollectedInRound;
	public Text PowerupsCollectedInRoundText;

	private int AchievementsCollectedInRound;
	public Text AchievementsCollectedInRoundText;

	private bool leftButtonPressedDown;
	private bool rightButtonPressedDown;
    //RectTransform zoomAchievementRectTransform;

	private bool invalidLeftPressedDown = false;
	private bool invalidRightPressedDown = false;

    public RectTransform RightPanelRect;
    public RectTransform LeftButtonsRect;

    public GameObject[] InstructionsMoreInfoText;

	public RectTransform[] MainMenuButtonTextTransforms;
	public RectTransform MainMenuColorWheelLogo;
	public RectTransform MainMenuPolyKromosText;

	//Singleton
	void Awake() {

		if (singleton != null) {
			Object.Destroy(singleton);
		} else {
			singleton = this;
		}
			
		DontDestroyOnLoad(this);
	}

	public Animator AnimatePlayButton;
	// Use this for initialization
	void Start () {
		isPaused = false;
        visibleAchievementIndex = 0;
		heartsFillAmount = 1f;
		PowerupsCollectedInRound = 0;

		bool isiPhoneX = UnityEngine.iOS.Device.generation == UnityEngine.iOS.DeviceGeneration.iPhoneX;
		//bool isiPhoneX = true;

        // Set menu UI to fit the resolution of the device
        float screenResolution = (float)Screen.width / Screen.height;
        if(screenResolution > 1.4) {
            RightPanelRect.anchorMin = new Vector2(0.36f, 0.05f);
            RightPanelRect.anchorMax = new Vector2(0.99f, 0.70f);

            LeftButtonsRect.anchorMin = new Vector2(-0.10f, 0.05f);
            LeftButtonsRect.anchorMax = new Vector2(0.35f, 0.70f);
        } else {
            RightPanelRect.anchorMin = new Vector2(0.36f, 0.15f);
            RightPanelRect.anchorMax = new Vector2(0.99f, 0.65f);

            LeftButtonsRect.anchorMin = new Vector2(-0.10f, 0.15f);
            LeftButtonsRect.anchorMax = new Vector2(0.35f, 0.65f);
        }

		if (isiPhoneX) {
			foreach (RectTransform rect in MainMenuButtonTextTransforms) {
				rect.anchorMin = new Vector2(0.40f, 0.2f);
			}
			MainMenuColorWheelLogo.anchorMin = new Vector2 (0.06f, 0.1f);
			MainMenuColorWheelLogo.anchorMax = new Vector2 (0.23f, 0.9f);

			MainMenuPolyKromosText.anchorMin = new Vector2 (0.17f, 0f);
			MainMenuPolyKromosText.anchorMax = new Vector2 (0.87f, 0.78f);
		} 

	}
	
	// Update is called once per frame
	void Update () {
		if (scrollLeft) {
			achievementScrollRect.horizontalNormalizedPosition = Mathf.Lerp (achievementScrollRect.horizontalNormalizedPosition, 0.0f, ScrollerLerpSpeed * Time.deltaTime);
			if (achievementScrollRect.horizontalNormalizedPosition <= 0.001f && achievementScrollRect.horizontalNormalizedPosition >= -0.001f) {
				scrollLeft = false;
				achievementScrollRect.horizontalNormalizedPosition = 0.5f;
				swipeTimerOn = false;
				UpdateVisibleAchievements();
				achievementScrollRect.enabled = true;
			}
		}

		else if (scrollRight) {
			achievementScrollRect.horizontalNormalizedPosition = Mathf.Lerp (achievementScrollRect.horizontalNormalizedPosition, 1.0f, ScrollerLerpSpeed * Time.deltaTime);
			if (achievementScrollRect.horizontalNormalizedPosition >= 0.999f && achievementScrollRect.horizontalNormalizedPosition <= 1.001f) {
				scrollRight = false;
				achievementScrollRect.horizontalNormalizedPosition = 0.5f;
				swipeTimerOn = false;
				UpdateVisibleAchievements ();
				achievementScrollRect.enabled = true;
			}
		}

		else if (scrollToCenter) {
			achievementScrollRect.horizontalNormalizedPosition = Mathf.Lerp (achievementScrollRect.horizontalNormalizedPosition, 0.5f, ScrollerLerpSpeed * Time.deltaTime);
			if (achievementScrollRect.horizontalNormalizedPosition >= 0.499f && achievementScrollRect.horizontalNormalizedPosition <= 0.501f) {
				scrollToCenter = false;
				achievementScrollRect.horizontalNormalizedPosition = 0.5f;
				swipeTimerOn = false;
				UpdateVisibleAchievements ();
				achievementScrollRect.enabled = true;
			}
		}

		if (swipeTimerOn) {
			swipeTimer += Time.deltaTime;
		}

	}

	public void resetGame() {
		//Stores score in playerprefs
		PlayerPrefsController.singleton.addScore(Score.singleton.score);

		//turnOnEndGamePanel ();
		//PlayerController.singleton.reset ();
		PowerupsCollectedInRound = 0;
        AchievementsCollectedInRound = 0;
		hearts.fillAmount = 1;
        heartsFillAmount = 1;

		Score.singleton.resetScore();
	}

	public void DecrementHeart() {
		heartsFillAmount -= .25f;
		hearts.fillAmount = heartsFillAmount;
		// Game over actions
		if (hearts.fillAmount == 0) {
            EndGameActions();
		} else {
			PlayerController.singleton.SetBarrierParticleSystemVisibility (true, "HeartsParticleSystem");
		}
	}

    public void EndGameActions() {
        PlayerController.singleton.SetEndGameState(true);
        PlayerController.singleton.ExplodeAllFireballsInScene();
        UpdateMainMenuWithEndGameStats();
        PlayerController.singleton.StopPlayerFromMovingOrLosingPointsDuringEndGame();
        //Sounds.singleton.PlayEndGame();
        StartCoroutine("LerpFromInGameToMainMenu");
    }

	public void IncrementPowerupsCollectedInRound() {
		PowerupsCollectedInRound++;
	}

	public void IncrementAchievementsCollectedInRound() {
		AchievementsCollectedInRound++;
	}

	private IEnumerator LerpFromInGameToMainMenu() {
		yield return new WaitForSeconds (1.0f);
		//Sounds.singleton.PlayEndGame();
		InGameCanvas.GetComponent<Animator> ().Play ("AnimateOutInGameCanvas");
		CameraController.singleton.StartLerpFromInGameToMainMenu ();
	}

	public void UpdateMainMenuWithEndGameStats() {
		// New high score
		if (Score.singleton.score > PlayerPrefsController.singleton.getTopScoreAtIndex (1)) {
			EndGameScoreText.text = "New High Score!";
			EndGameBestScoreText.text = Score.singleton.score.ToString ();
		} else {
			EndGameScoreText.text = Score.singleton.score.ToString ();
			EndGameBestScoreText.text = PlayerPrefsController.singleton.getTopScoreAtIndex (1).ToString ();
		}

		endGameStats.GetComponent<CanvasGroup> ().alpha = 1.0f;

		PowerupsCollectedInRoundText.text = PowerupsCollectedInRound.ToString ();
		AchievementsCollectedInRoundText.text = AchievementsCollectedInRound.ToString ();

		highScoresCurrentlyDisplayed = false;
		achievementsCurrentlyDisplayed = false;
		instructionsCurrentlyDisplayed = false;
		endGameStatsCurrentlyDisplayed = true;
		endGameStats.SetActive (true);
		topScores.SetActive (false);
		achievements.SetActive (false);
		InstructionsPictures.SetActive (false);

		InstructionsButton.GetComponent<RectTransform> ().anchorMin = new Vector2(0.0f, 0.0f);
		InstructionsButton.GetComponent<RectTransform> ().anchorMax = new Vector2(0.9f, 0.18f);

		scoresButton.GetComponent<RectTransform> ().anchorMin = new Vector2(0.0f, 0.545f);
		scoresButton.GetComponent<RectTransform> ().anchorMax = new Vector2(0.9f, 0.725f);

		achievementsButton.GetComponent<RectTransform> ().anchorMin = new Vector2(0.0f, 0.275f);
		achievementsButton.GetComponent<RectTransform> ().anchorMax = new Vector2(0.9f, 0.455f);
	}

	public void AnimateMainMenuCanvasInAfterLerp() {
		mainMenuCanvas.GetComponent<CanvasGroup> ().alpha = 0f;
		mainMenuCanvas.SetActive (true);
		mainMenuCanvas.GetComponent<Animator> ().Play ("AnimateInMainMenuCanvas");
		InGameCanvas.SetActive (false);
		CameraController.singleton.SetCameraPositionToMainMenu ();
		resetGame ();
		PlayerController.singleton.reset ();
		PlayerController.singleton.resetPositionOfPlayer ();
	}

	public void IncrementHearts() {
        PlayerPrefsController.singleton.SetCurrentAmountOfAchievementByAchievementType("HeartPowerups");
		heartsFillAmount += 0.25f;
		hearts.fillAmount = heartsFillAmount;
		if (hearts.fillAmount == 1.0f) {
			PlayerController.singleton.SetBarrierParticleSystemVisibility (false, "HeartsParticleSystem");
		}
	}

	public float GetCurrentHeartsFillAmount() {
		return heartsFillAmount;
	}

	public void MoveToMainScene() {
		SceneManager.LoadScene ("MainScene");
	}

	public void PauseClicked() {
        if (PlayerController.singleton.GetEndGameState()) return;
		if(!isPaused) {
			pauseButton.GetComponent<Text> ().text = "Paused";
			PlayerController.singleton.speedIncreasing = 0;

			GameObject[] Fireballs = GameObject.FindGameObjectsWithTag ("Fireball");
			for (int i = 0; i < Fireballs.Length; i++) {
				if (Fireballs [i].GetComponentInParent<FireballMovement> () != null) {
					Fireballs [i].GetComponentInParent<FireballMovement> ().speed = 0f;
				}
			}

		} else {
			pauseButton.GetComponent<Text> ().text = "";
			PlayerController.singleton.speedIncreasing = PlayerController.singleton.memoizedSpeed;
			GameObject[] Fireballs = GameObject.FindGameObjectsWithTag ("Fireball");
			for (int i = 0; i < Fireballs.Length; i++) {
				if (Fireballs [i].GetComponentInParent<FireballMovement> () != null) {
					Fireballs [i].GetComponentInParent<FireballMovement> ().speed = -5f;
				}
			}
		}

		isPaused = !isPaused;
	}

	public void SetMainMenuCanvasGroupToVisible() {
		mainMenuCanvas.GetComponent<CanvasGroup> ().alpha = 1;
	}


	public void HighScoresButtonClicked() {
		if (!highScoresCurrentlyDisplayed) {
            Sounds.singleton.PlayMenuClick();
			scoresButton.GetComponent<Animator> ().Play ("TabOutScoreButton");
			topScores.SetActive (true);
			updateTopScoresToBeDisplayed ();
			if (achievementsCurrentlyDisplayed) {
				achievementsButton.GetComponent<Animator> ().Play ("TabInAchievementsButton");
				achievements.GetComponent<Animator> ().Play ("AnimateOutAchievements");
			}
			if (instructionsCurrentlyDisplayed) {
				InstructionsButton.GetComponent<Animator> ().Play ("TabInInstructionsButton");
				InstructionsPictures.GetComponent<Animator> ().Play ("AnimateInstructionsOut");
			}
			if (endGameStatsCurrentlyDisplayed) {
				endGameStats.GetComponent<Animator> ().Play ("AnimateOutEndGameStats");
			}

			topScores.GetComponent<Animator>().Play("AnimateInTopScores");

			highScoresCurrentlyDisplayed = true;
			instructionsCurrentlyDisplayed = false;
			achievementsCurrentlyDisplayed = false;
			endGameStatsCurrentlyDisplayed = false;
		}
		//AnimateFromHomeToTopScoresOrAchievements ();

	}

	public void AchievementsButtonClicked() {
		//Call update to pictures
		UpdateVisibleAchievements();
		if (!achievementsCurrentlyDisplayed) {
            Sounds.singleton.PlayMenuClick();
			achievementsButton.GetComponent<Animator> ().Play ("TabOutAchievementsButton");
			achievements.SetActive (true);

			if (highScoresCurrentlyDisplayed) {
				scoresButton.GetComponent<Animator> ().Play ("TabInScoreButton");
				topScores.GetComponent<Animator>().Play("AnimateOutTopScores");
			}
			if (instructionsCurrentlyDisplayed) {
				InstructionsButton.GetComponent<Animator> ().Play ("TabInInstructionsButton");
				InstructionsPictures.GetComponent<Animator> ().Play ("AnimateInstructionsOut");
			}
			if (endGameStatsCurrentlyDisplayed) {
				endGameStats.GetComponent<Animator> ().Play ("AnimateOutEndGameStats");
			}

			achievements.GetComponent<Animator> ().Play ("AnimateInAchievements");

			achievementsCurrentlyDisplayed = true;
			instructionsCurrentlyDisplayed = false;
			highScoresCurrentlyDisplayed = false;
			endGameStatsCurrentlyDisplayed = false;
		}
	}

	public void InstructionsButtonClicked() {
		if (!instructionsCurrentlyDisplayed) {
            Sounds.singleton.PlayMenuClick();
			InstructionsPictures.SetActive (true);

			InstructionsButton.GetComponent<Animator> ().Play ("TabOutInstructionsButton");
			if (highScoresCurrentlyDisplayed) {
				scoresButton.GetComponent<Animator> ().Play ("TabInScoreButton");
				topScores.GetComponent<Animator>().Play("AnimateOutTopScores");
			}
			if (achievementsCurrentlyDisplayed) {
				achievementsButton.GetComponent<Animator> ().Play ("TabInAchievementsButton");
				achievements.GetComponent<Animator> ().Play ("AnimateOutAchievements");
			}
			if (endGameStatsCurrentlyDisplayed) {
				endGameStats.GetComponent<Animator> ().Play ("AnimateOutEndGameStats");
			}

			InstructionsPictures.GetComponent<Animator> ().Play ("AnimateInstructionsIn");

			instructionsCurrentlyDisplayed = true;
			achievementsCurrentlyDisplayed = false;
			highScoresCurrentlyDisplayed = false;
			endGameStatsCurrentlyDisplayed = false;
		}
	}

    public void TurnOffOtherRightPanels(string panelToStayActive) {
        topScores.SetActive(panelToStayActive.Equals("TopScores"));
        InstructionsPictures.SetActive(panelToStayActive.Equals("Instructions"));
        achievements.SetActive(panelToStayActive.Equals("Achievements"));
        endGameStats.SetActive(false);
    }

    public bool isEndGamePanelOn() {
        //return endGamePanel.activeInHierarchy;
		return false;
    }

    public void PlayHeartsAnimationForAchievement(){
        heartsTransform.GetComponent<Animator>().Play("AnimateHeartsDown");
    }

	public void turnOnEndGamePanel() {
		//endGamePanel.SetActive (true);
		//endGameBackground.SetActive (true);
		EndGameScoreText.text = Score.singleton.score.ToString ();
		EndGameBestScoreText.text = PlayerPrefsController.singleton.getTopScoreAtIndex (1).ToString();
		//endGameBackground.GetComponent<Animator> ().Play ("AnimateEndGameBackgroundIn");
		//endGamePanel.GetComponent<Animator> ().Play ("AnimateEndGameStateIn");
	}
		
	public void PlayAgainOnClick() {
		//endGamePanel.SetActive (false);
		//Maybe set the image/text anchor values. Unless you end up just going with a logo which is prob the better call. 
		//resetGame();
		//PlayerController.singleton.resetPositionOfPlayer ();
		//endGameBackground.SetActive (true);
		//TODO: Change this to be an animation of the new end game screen out. It should make sure to call the methods that this animation calls. 
		//endGameBackground.GetComponent<Animator> ().Play ("AnimateEndGameBackgroundInForReplay");
		EndGameAnimateToMainMenu = false;

		GameObject.FindObjectOfType<PlayerController> ().reset ();
		PlayerController.singleton.GetComponent<PlayerController> ().resetPositionOfPlayer ();

		//endGamePanel.GetComponent<Animator> ().Play ("AnimateOutEndGamePanel");
		StartCoroutine(PlayerController.singleton.WaitToStartGame ());

		//TODO: HERE IS WHERE YOU SHOULD RESTART THE GAME STUFF I BELIEVE. Right now, once you play again you'll see a bunch of barriers way far ahead. those should be deleted. 
	}

	/*public void MainMenuFromGameOnClick() {
		//resetGame ();

		InGameCanvas.SetActive (false);
		EndGameAnimateToMainMenu = true;
		UpdateVisibleAchievements();
        updateTopScoresToBeDisplayed();
		//endGameBackground.SetActive (true);
		//endGameBackground.GetComponent<Animator> ().Play ("AnimateEndGameBackgroundInForMainMenu");
		InGameCanvas.GetComponent<CanvasGroup>().alpha = 0;
		CameraAnimationsController.singleton.SetCameraPositionToMainMenu ();
		GameObject.FindObjectOfType<PlayerController> ().EnableMainMenuCanvas ();

		//endGamePanel.GetComponent<Animator> ().Play ("AnimateOutEndGamePanel");
	}*/

	public void EndOfEndGameAnimateOutActions() {
		if (EndGameAnimateToMainMenu) {
			SetMainMenuCanvasGroupToVisible ();
			//CameraController.singleton.SetCameraPositionToMainMenu ();
			//InGameCanvas.SetActive (false);
			//GameObject.FindObjectOfType<PlayerController> ().EnableMainMenuCanvas ();
			GameObject.FindObjectOfType<PlayerController> ().reset ();
			GameObject.FindObjectOfType<PlayerController> ().resetPositionOfPlayer ();
		}
	}

	public void TurnOffEndGamePanel() {
		//endGamePanel.SetActive (false);
	}

	private void updateTopScoresToBeDisplayed() {
		//print (topScoresText.Length);
		int scoreAtIndex;
		for (int i = 1; i <= topScoresText.Length; i++) {
			scoreAtIndex = PlayerPrefsController.singleton.getTopScoreAtIndex (i);
			topScoresText [i - 1].text = scoreAtIndex.ToString();
		}
	}

	public void EditCountDownNumber(int numberToDisplay) {
		countDown.GetComponent<Text> ().text = numberToDisplay.ToString ();
	}

    public void ShowCountDown(bool setVisible) {
        countDown.gameObject.SetActive(setVisible);
        //Debug.Log ("Countdown set visible: " + setVisible);
    }

    public void RightAchievementButtonClicked() {

		bool rightButtonCurrentlyPressedDownByFirstFinger = RectTransformUtility.RectangleContainsScreenPoint (RightAchievementButton.gameObject.GetComponent<RectTransform> (), Input.GetTouch (0).position);

		//Don't you dare push that button when I'm already scrolling to the next page for you learn some patience please. 
		//Also there has to be this correction factor for some reason, even thought I'm explicitly setting the position to 0.5f at the end of a page move? Floats are stupid. 
		if(achievementScrollRect.horizontalNormalizedPosition >= 0.499f && achievementScrollRect.horizontalNormalizedPosition <= 0.501f && Input.touchCount == 1 && !swipeTimerOn && rightButtonCurrentlyPressedDownByFirstFinger) {
			achievementScrollRect.enabled = false;
        	visibleAchievementIndex = (visibleAchievementIndex + 1) % 15;
			AnimatePageCircles (visibleAchievementIndex, visibleAchievementIndex == 0 ? 14 : visibleAchievementIndex - 1);
        	//UpdateVisibleAchievementByPage();
			scrollRight = true;
		}
		if (Input.touchCount == 1) {
			swipeTimerOn = false;
		}
    }

    public void LeftAchievementButtonClicked() {

		bool leftButtonCurrentlyPressedDownByFirstFinger = RectTransformUtility.RectangleContainsScreenPoint (LeftAchievementButton.gameObject.GetComponent<RectTransform> (), Input.GetTouch (0).position);

		if (achievementScrollRect.horizontalNormalizedPosition >= 0.499f && achievementScrollRect.horizontalNormalizedPosition <= 0.501f && Input.touchCount == 1 && !swipeTimerOn && leftButtonCurrentlyPressedDownByFirstFinger) {
            //print("here");
			//Annoying fix for C#s % being a remainder and not modulo -_-
			visibleAchievementIndex = visibleAchievementIndex == 0 ? 14 : visibleAchievementIndex - 1;
			AnimatePageCircles (visibleAchievementIndex, (visibleAchievementIndex + 1) % 15);
			scrollLeft = true;
		}
		if (Input.touchCount == 1) {
			swipeTimerOn = false;
		}
    }

	private void AnimatePageCircles(int circleIn, int circleOut) {
		pageCircles[circleIn].GetComponent<Animator>().Play("AnimatePageCircleIn");
		pageCircles[circleOut].GetComponent<Animator>().Play("AnimatePageCircleOut");
	}

    public void SetZoomedImageActive(bool active) {
		//TODO: Need something here and probably in surrounding methods to see if the position came from a drag outside the miniAchievementIconsZone
		if (Input.touchCount == 1 && RectTransformUtility.RectangleContainsScreenPoint(miniAchievementIconsZone, Input.GetTouch(0).position)) {
            moveableAchievementZoom.SetActive(active);
        }
    }

	public void UpdateVisibleAchievements() {
		//Loop through all left and right pages as well
		int indexForLeft = visibleAchievementIndex == 0 ? 14 : visibleAchievementIndex - 1;
		int indexForRight = (visibleAchievementIndex + 1) % 15;

		//Update the right panel header to have the name of the current achievement
		achievementLabel.text = Achievement.singleton.getLabelByIndex (visibleAchievementIndex);

		// For each page (left, right, and middle), update what should be displayed on that page 
		for (int i = 0; i < achievementImagesMiddle.Length; i++) {
			UpdateVisibleAchievementsByPage (i, achievementImagesLeft, achievementTextLeft, indexForLeft, SecretAchievementLeft, (indexForLeft == 14), achievementIconLockImagesLeft);
			UpdateVisibleAchievementsByPage (i, achievementImagesMiddle, achievementTextMiddle, visibleAchievementIndex, SecretAchievementMiddle, (visibleAchievementIndex == 14), achievementIconLockImagesMiddle);
			UpdateVisibleAchievementsByPage (i, achievementImagesRight, achievementTextRight, indexForRight, SecretAchievementRight, (indexForRight == 14), achievementIconLockImagesRight);
		}
			
	}

	//Updates an individual page of the achievements scrollable (Left, Middle, Or Right "Panels")
	private void UpdateVisibleAchievementsByPage(int achievementIncrement, Image[] achievementImages, Text[] achievementText, int indexOfAchievement, GameObject secretAchievement, bool isPageSecretAchievementPage, GameObject[] achievementIconLockImages) {
		//If the page is for the secret achievement then turn on the secret achievement button and turn off all other achievements
		if (isPageSecretAchievementPage) {
			achievementImages [achievementIncrement].transform.parent.gameObject.SetActive (false);
			secretAchievement.SetActive (true);

            // If the player has unlocked all of the achievements, change the button to reflect that
            if (PlayerPrefsController.singleton.HaveAllAchievementsBeenUnlocked()) {
                foreach (Text t in SecretAchievementPictureLabel) {
                    t.gameObject.SetActive(false);
                }
                foreach (Text t in SecretAchievementLabel) {
                    if (ThemeColor == "Light") {
                        t.text = "Click To Enable*Dark Mode";
                    } else {
                        t.text = "Click To Enable*Light Mode";
                    }
					t.text = t.text.Replace ("*", "\n");
                }
            }
		} else {
			//Make sure the secret achievement is off and the other images are active in the case of switching from a secret achievement page
			achievementImages [achievementIncrement].transform.parent.gameObject.SetActive (true);
			secretAchievement.SetActive (false);

			//Update the text of the achievements with the achievement increment if it is already unlocked, or question marks if it is not yet unlocked. Also, turn off the lock image if the achievement has been unlocked.
			if (achievementIncrement <= Achievement.singleton.getHighestLevelOfAchievement (indexOfAchievement)) {
				achievementText [achievementIncrement].text = Achievement.singleton.getAchievementIncrementAtIndex (indexOfAchievement, achievementIncrement).ToString ();
				achievementIconLockImages [achievementIncrement].SetActive (false);

				//Update the visuals of the achievement
				achievementImages [achievementIncrement].gameObject.SetActive(true);
				achievementImages [achievementIncrement].sprite = Achievement.singleton.getAchievementImageByIndex (indexOfAchievement);
				achievementImages [achievementIncrement].color = Achievement.singleton.getColor (indexOfAchievement);
                achievementImages[achievementIncrement].color = new Color(achievementImages[achievementIncrement].color.r,
                                                                          achievementImages[achievementIncrement].color.g,
                                                                          achievementImages[achievementIncrement].color.b,
                                                                          .88f);
				achievementImages [achievementIncrement].preserveAspect = false;
			} else {
				achievementImages [achievementIncrement].gameObject.SetActive(false);
				achievementText [achievementIncrement].text = "???";
				achievementIconLockImages [achievementIncrement].SetActive (true);

			}
		}
	}

	public void SpinLogo() {
		Sounds.singleton.PlayMenuClick ();
		logo.GetComponent<Animator> ().Play ("SpinLogo");
	}

	public void LeftButtonEnter() {
		if (achievementScrollRect.horizontalNormalizedPosition <= 0.501f && achievementScrollRect.horizontalNormalizedPosition >= 0.499f && Input.touchCount == 1) {
            Sounds.singleton.PlayMenuClick();
			leftButtonPressedDown = true;
			achievementScrollRect.enabled = false;
		} else {
			invalidLeftPressedDown = true;
		}
	}

	public void LeftButtonExit() {
		if (leftButtonPressedDown) {
			visibleAchievementIndex = visibleAchievementIndex == 0 ? 14 : visibleAchievementIndex - 1;
			AnimatePageCircles (visibleAchievementIndex, (visibleAchievementIndex + 1) % 15);
			scrollLeft = true;
			leftButtonPressedDown = false;
		}
		invalidLeftPressedDown = false;
	}

	public void RightButtonEnter() {
		if (achievementScrollRect.horizontalNormalizedPosition <= 0.501f && achievementScrollRect.horizontalNormalizedPosition >= 0.499f && Input.touchCount == 1) {
            Sounds.singleton.PlayMenuClick();
			rightButtonPressedDown = true;
			achievementScrollRect.enabled = false;
		} else {
			invalidRightPressedDown = true;
		}
	}

	public void RightButtonExit() {
		if (rightButtonPressedDown) {
			rightButtonPressedDown = false;
			achievementScrollRect.enabled = false;
			visibleAchievementIndex = (visibleAchievementIndex + 1) % 15;
			AnimatePageCircles (visibleAchievementIndex, visibleAchievementIndex == 0 ? 14 : visibleAchievementIndex - 1);
			scrollRight = true;
		}
		invalidRightPressedDown = false;
	}

	public void TriggerLeftRightOrCenterScroll(Vector2 value) {
		//TODO: Change this to be there is no touch inside the scrollable zone.
		if (!scrollLeft && !scrollRight && !scrollToCenter && !leftButtonPressedDown && !rightButtonPressedDown) {
			if (Input.touchCount == 0 || (Input.touchCount == 1 && (invalidLeftPressedDown || invalidRightPressedDown))) {
				if (achievementScrollRect.horizontalNormalizedPosition <= 0.3f) {
					visibleAchievementIndex = visibleAchievementIndex == 0 ? 14 : visibleAchievementIndex - 1;
					AnimatePageCircles (visibleAchievementIndex, (visibleAchievementIndex + 1) % 15);
					achievementScrollRect.enabled = false;
					scrollLeft = true;
					//print ("Inside need to scroll left");
				} else if (achievementScrollRect.horizontalNormalizedPosition >= 0.7f) {
					visibleAchievementIndex = (visibleAchievementIndex + 1) % 15;
					AnimatePageCircles (visibleAchievementIndex, visibleAchievementIndex == 0 ? 14 : visibleAchievementIndex - 1);
					achievementScrollRect.enabled = false;
					scrollRight = true;
					//Okay I'm pretty proud of this. Basically, check if the swipe timer has been going and there are no longer fingers down. Check to see if it's been a quick swipe and change pages if it's a little bit onto the next page.
				} else if (swipeTimerOn) {
					//print ("Inside swipe timer was on need to turn off");
					swipeTimerOn = false;
					//print (swipeTimer);
					if (swipeTimer < 0.25f) {
						if (achievementScrollRect.horizontalNormalizedPosition <= 0.45f) {
							visibleAchievementIndex = visibleAchievementIndex == 0 ? 14 : visibleAchievementIndex - 1;
							AnimatePageCircles (visibleAchievementIndex, (visibleAchievementIndex + 1) % 15);
							achievementScrollRect.enabled = false;
							scrollLeft = true;
						} else if (achievementScrollRect.horizontalNormalizedPosition >= 0.55f) {
							visibleAchievementIndex = (visibleAchievementIndex + 1) % 15;
							AnimatePageCircles (visibleAchievementIndex, visibleAchievementIndex == 0 ? 14 : visibleAchievementIndex - 1);
							achievementScrollRect.enabled = false;
							scrollRight = true;
						} else {
							achievementScrollRect.enabled = false;
							scrollToCenter = true;
						}
					}
					swipeTimer = 0.0f;
				} else {
					//print ("Inside need to scroll center");
					achievementScrollRect.enabled = false;
					scrollToCenter = true;
				}
				//Turn on a tracker to see how long a swipe is taking if not currently in the process of scrolling between pages.
			} else if (Input.touchCount == 1 && !swipeTimerOn) {
				//print ("Inside need to turn swipe timer on");
				swipeTimerOn = true;
			} 
		} else if (leftButtonPressedDown || rightButtonPressedDown) {
			//print ("inside reset horizontal pos jndfgkjnsdfg");
				achievementScrollRect.horizontalNormalizedPosition = 0.5f;
		}
	}

    // Toggles on and off the more info text on the instructions page
    public void ToggleInstructionsMoreInfoText(int InstructionsPage) {
        Sounds.singleton.PlayMenuClick();
        InstructionsMoreInfoText[InstructionsPage].SetActive(!InstructionsMoreInfoText[InstructionsPage].activeInHierarchy);
    }

	// On Click to change to dark mode or light mode
	public void ChangeThemeColor() {
        if (PlayerPrefsController.singleton.HaveAllAchievementsBeenUnlocked())
        {
            if (ThemeColor == "Light")
            {
                //SkyboxMaterial.
                RenderSettings.skybox.SetColor("_Tint", RainbowColors.singleton.getSkyboxDark());
                GroundSegmentMaterial.color = RainbowColors.singleton.getGroundSegmentDark();
                BackgroundRectanglesLight.SetActive(false);
                BackgroundRectanglesDark.SetActive(true);

                Image PolyKromosLogoText = PolyKromosTitleText.GetComponent<Image>();
                PolyKromosLogoText.sprite = PolyKromosLogoTextWhite;
                PolyKromosLogoText.color = new Color(PolyKromosLogoText.color.r, PolyKromosLogoText.color.g, PolyKromosLogoText.color.b, .82f);

                EndGameScoreText.color = RainbowColors.singleton.getPolyKromosTitleTextDark();
                EndGameBestScoreText.color = RainbowColors.singleton.getPolyKromosTitleTextDark();
                PowerupsCollectedInRoundText.color = RainbowColors.singleton.getPolyKromosTitleTextDark();
                AchievementsCollectedInRoundText.color = RainbowColors.singleton.getPolyKromosTitleTextDark();
                foreach (Image go in MainMenuImagesLightestGray)
                {
                    go.color = RainbowColors.singleton.getMainMenuButtonsColorDark();
                }
                foreach (Image go in MainMenuImagesDarkerGray)
                {
                    go.color = RainbowColors.singleton.getMainMenuDarkerGrayDark();
                }
                foreach (Text text in MainMenuText)
                {
                    text.color = RainbowColors.singleton.getMainMenuTextDark();
                }
                foreach (Text textScores in topScoresText)
                {
                    textScores.color = RainbowColors.singleton.getMainMenuTextDark();
                }
                achievementLabel.color = RainbowColors.singleton.getMainMenuTextDark();
                InGameScoreText.color = RainbowColors.singleton.getPolyKromosTitleTextDark();
                foreach (Image satci in SecretAchievementThemeColorImages)
                {
                    satci.color = RainbowColors.singleton.getPolyKromosTitleTextDark();
                }
                TiltToSteerImage.color = RainbowColors.singleton.getPolyKromosTitleTextDark();
                foreach (Image divider in DividerImages)
                {
                    divider.color = RainbowColors.singleton.getPolyKromosTitleTextDark();
                }
                DoNotCrossBarrier.color = RainbowColors.singleton.getPolyKromosTitleTextDark();
                foreach (Text t in SecretAchievementLabel)
                {
                    t.text = "Click To Enable*Light Mode";
					t.text = t.text.Replace ("*", "\n");
                    t.color = RainbowColors.singleton.getMainMenuTextDark();
                }
                ClockParticleSystem.GetComponent<ParticleSystemRenderer>().material = ClockParticleSystemDarkMode;
                AvoidObstaclesInstructionText.text = "Avoid enemies and white barriers";
                ThemeColor = "Dark";
            }
            else if (ThemeColor == "Dark")
            {
                RenderSettings.skybox.SetColor("_Tint", RainbowColors.singleton.getSkyboxLight());
                GroundSegmentMaterial.color = RainbowColors.singleton.getGroundSegmentLight();
                BackgroundRectanglesDark.SetActive(false);
                BackgroundRectanglesLight.SetActive(true);

                Image PolyKromosLogoText = PolyKromosTitleText.GetComponent<Image>();
                PolyKromosLogoText.sprite = PolyKromosLogoTextGray;
                PolyKromosLogoText.color = new Color(PolyKromosLogoText.color.r, PolyKromosLogoText.color.g, PolyKromosLogoText.color.b, .92f);

                EndGameScoreText.color = RainbowColors.singleton.getPolyKromosTitleTextLight();
                EndGameBestScoreText.color = RainbowColors.singleton.getPolyKromosTitleTextLight();
                PowerupsCollectedInRoundText.color = RainbowColors.singleton.getPolyKromosTitleTextLight();
                AchievementsCollectedInRoundText.color = RainbowColors.singleton.getPolyKromosTitleTextLight();
                foreach (Image go in MainMenuImagesLightestGray)
                {
                    go.color = RainbowColors.singleton.getMainMenuButtonsColorLight();
                }
                foreach (Image go in MainMenuImagesDarkerGray)
                {
                    go.color = RainbowColors.singleton.getMainMenuDarkerGrayLight();
                }
                foreach (Text text in MainMenuText)
                {
                    text.color = RainbowColors.singleton.getMainMenuTextLight();
                }
                foreach (Text textScores in topScoresText)
                {
                    textScores.color = RainbowColors.singleton.getMainMenuTextLight();
                }
                achievementLabel.color = RainbowColors.singleton.getMainMenuTextLight();
                InGameScoreText.color = RainbowColors.singleton.getPolyKromosTitleTextLight();
                foreach (Image satci in SecretAchievementThemeColorImages)
                {
                    satci.color = RainbowColors.singleton.getPolyKromosTitleTextLight();
                }
                TiltToSteerImage.color = RainbowColors.singleton.getPolyKromosTitleTextLight();
                foreach (Image divider in DividerImages)
                {
                    divider.color = RainbowColors.singleton.getPolyKromosTitleTextLight();
                }
                DoNotCrossBarrier.color = RainbowColors.singleton.getPolyKromosTitleTextLight();
                foreach (Text t in SecretAchievementLabel)
                {
                    t.text = "Click To Enable*Dark Mode";
					t.text = t.text.Replace ("*", "\n");
                    t.color = RainbowColors.singleton.getMainMenuTextLight();
                }
                ClockParticleSystem.GetComponent<ParticleSystemRenderer>().material = ClockParticleSystemLightMode;
                AvoidObstaclesInstructionText.text = "Avoid enemies and black barriers";
                ThemeColor = "Light";
            }

            PlayerController.singleton.reset();
            PlayerController.singleton.resetPositionOfPlayer();
        }


	}

    public string GetThemeColor() {
        return ThemeColor;
    }

    // Toggles the sound along with the sound icon sprites
    public void ToggleSound() {
        soundsOn = !soundsOn;
        SoundIcon.sprite = soundsOn ? SoundIconOn: SoundIconOff;
        SoundIconInGame.sprite = soundsOn ? SoundIconOn : SoundIconOff;
        SoundIconOffSlash.gameObject.SetActive(!soundsOn);
        SoundIconOffSlashInGame.gameObject.SetActive(!soundsOn);
        Sounds.singleton.toggleSoundsAreOn();
    }
	

}


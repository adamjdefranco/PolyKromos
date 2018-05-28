using UnityEngine;
using System.Collections;

public class BarrierProperties : MonoBehaviour {

	#region PublicVariables

	public GameObject particleSystemForBarrier;
	public GameObject HeartsIncrementBarrierPowerupSystem;
	public GameObject ClocksBarrierParticleSystem;
	public GameObject BarrierDestroyedParticleSystem;

	public Material skullMaterial;
    public Material ClockParticleSystemLightMode;
    public Material ClockParticleSystemDarkMode;

	//public RainbowColors ColorReference;

	public bool isPowerup;
	public bool isHeartsIncrement;
	public bool isClocksPowerup;

	#endregion

	#region PrivateVariables

	private GameObject powerup;
	private GameObject hearts;
	private GameObject clocks;

	private bool isSingleBarrier;
	private bool isDoubleBarrier;
	private bool isTripleBarrier;
	private bool isQuadrupleBarrier;
    private bool isUnpassableBarrier;
	private bool isFirstBarrierInRound;

	enum colorBarrierTypes { Red, Orange, Yellow, Green, Blue, Violet, Black, White };
	private colorBarrierTypes colorOfBarrier;

	#endregion

	void Start () {

		this.tag = "BarrierInner";

		// Determine how many barriers are in a row
		isDoubleBarrier = (gameObject.name.Contains ("Double"));
		isTripleBarrier = (gameObject.name.Contains("Triple"));
		isQuadrupleBarrier = (gameObject.name.Contains ("Quadruple"));
		isSingleBarrier = !isDoubleBarrier && !isTripleBarrier && !isQuadrupleBarrier;

		isPowerup = false;
		isUnpassableBarrier = false;

		// The first barrier should always be white or black (depending on theme) to get the player to understand the concept of passing through a barrier with the same color
		isFirstBarrierInRound = (this.transform.position.z == 5);

		int RandomNumBetweenOneToSix = Random.Range (0, 8);

		// Orange barrier
		if (RandomNumBetweenOneToSix == 0 && !isFirstBarrierInRound) {
			GetComponent<Renderer> ().material.color = RainbowColors.singleton.getOrange(); 
			this.colorOfBarrier = colorBarrierTypes.Orange;
		// Violet barrier
		} else if (RandomNumBetweenOneToSix == 1 && !isFirstBarrierInRound) {
			GetComponent<Renderer> ().material.color = RainbowColors.singleton.getViolet(); 
			this.colorOfBarrier = colorBarrierTypes.Violet;
		// Green barrier
		} else if (RandomNumBetweenOneToSix == 2 && !isFirstBarrierInRound) {
			GetComponent<Renderer> ().material.color = RainbowColors.singleton.getGreen (); 
			this.colorOfBarrier = colorBarrierTypes.Green;
		// Red barrier
		} else if (RandomNumBetweenOneToSix == 3 && !isFirstBarrierInRound) {
			GetComponent<Renderer> ().material.color = RainbowColors.singleton.getRed(); 
			this.colorOfBarrier = colorBarrierTypes.Red;
		// Yellow barrier
		} else if (RandomNumBetweenOneToSix == 4 && !isFirstBarrierInRound) {
			GetComponent<Renderer> ().material.color = RainbowColors.singleton.getYellow();
			this.colorOfBarrier = colorBarrierTypes.Yellow;
		// Orange Blue
		} else if (RandomNumBetweenOneToSix == 5 && !isFirstBarrierInRound) {
			GetComponent<Renderer> ().material.color = RainbowColors.singleton.getBlue(); 
			this.colorOfBarrier = colorBarrierTypes.Blue;
		// Unpassable (Black or white depending on theme) barrier
		} else if (RandomNumBetweenOneToSix == 6 && !isSingleBarrier && CanBarrierBeUnpassable() && !isFirstBarrierInRound) {
			if (UIController.singleton.ThemeColor == "Light") {
				GetComponent<Renderer> ().material.color = RainbowColors.singleton.getBlack ();
				this.colorOfBarrier = colorBarrierTypes.Black;
			} else {
				GetComponent<Renderer> ().material.color = RainbowColors.singleton.getWhite ();
				this.colorOfBarrier = colorBarrierTypes.White;
			}
			isUnpassableBarrier = true;
		// White or black passable barrier (depending on theme)
		} else {
			if (UIController.singleton.ThemeColor == "Light") {
				GetComponent<Renderer> ().material.color = RainbowColors.singleton.getWhite ();
				this.colorOfBarrier = colorBarrierTypes.White;
			} else {
				GetComponent<Renderer> ().material.color = RainbowColors.singleton.getBlack ();
				this.colorOfBarrier = colorBarrierTypes.Black;
			}
		}

		// Probability that the barrier is a powerup barrier (Normally 30)
		int randNumForPowerupGeneration = Random.Range (0, 80);
		if (randNumForPowerupGeneration < 4 && !isSingleBarrier && !isUnpassableBarrier && transform.position.z != 0) {
			powerup = (GameObject)Instantiate (particleSystemForBarrier, new Vector3 (transform.position.x, 0.72f, transform.position.z), Quaternion.Euler(-90.0f, 0f, 0f));
			if (isTripleBarrier) {
				ParticleSystem.ShapeModule shape = powerup.GetComponent<ParticleSystem> ().shape;
				shape.scale = new Vector3(1.667f, 0.25f, 0f);
                ParticleSystem.EmissionModule emission = powerup.GetComponent<ParticleSystem>().emission;
                emission.rateOverTime = 110;
			} else if (isQuadrupleBarrier) {
				ParticleSystem.ShapeModule shape = powerup.GetComponent<ParticleSystem> ().shape;
				shape.scale = new Vector3(1.25f, 0.25f, 0f);
			} else if (isDoubleBarrier)	{
				ParticleSystem.ShapeModule shape = powerup.GetComponent<ParticleSystem> ().shape;
				shape.scale = new Vector3(2.5f, 0.25f, 0f);
                ParticleSystem.EmissionModule emission = powerup.GetComponent<ParticleSystem>().emission;
                emission.rateOverTime = 170;
			}

			powerup.tag = "Powerup";
            // If this powerup is being create while the player is in rainbow powerup mode, stop the particle system
            if(PlayerController.singleton.isInPowerupMode) {
                powerup.GetComponent<ParticleSystem>().Stop();
            }
			isPowerup = true;
            // PlayerController.singleton.GetSpeedIncreasing() > 13
		} else if (randNumForPowerupGeneration < 6 && !isSingleBarrier && !isUnpassableBarrier && transform.position.z != 0 && UIController.singleton.hearts.fillAmount != 1.0f && Score.singleton.score > 40) {
			hearts = (GameObject)Instantiate (HeartsIncrementBarrierPowerupSystem, new Vector3 (transform.position.x, transform.position.y + 0.65f, transform.position.z), Quaternion.Euler(-90.0f, 0f, 0f));
			if (isTripleBarrier) {
				ParticleSystem.ShapeModule shape = hearts.GetComponent<ParticleSystem> ().shape;
				shape.scale = new Vector3(1.467f, 0.25f, 0f);
				ParticleSystem.EmissionModule emission = hearts.GetComponent<ParticleSystem> ().emission;
				//emission.rateOverTime = 60;
			} else if (isQuadrupleBarrier) {
				ParticleSystem.ShapeModule shape = hearts.GetComponent<ParticleSystem> ().shape;
				shape.scale = new Vector3(1.05f, 0.25f, 0f);
				ParticleSystem.EmissionModule emission = hearts.GetComponent<ParticleSystem> ().emission;
				//emission.rateOverTime = 40;
			} else if (isDoubleBarrier)	{
				ParticleSystem.ShapeModule shape = hearts.GetComponent<ParticleSystem> ().shape;
				shape.scale = new Vector3(2.3f, 0.25f, 0f);
			}

			hearts.tag = "HeartsParticleSystem";
            // If this powerup is being create while the player is in rainbow powerup mode, stop the particle system
            if (PlayerController.singleton.isInPowerupMode) {
                hearts.GetComponent<ParticleSystem>().Stop();
            }
			isHeartsIncrement = true;
        } else if (randNumForPowerupGeneration == 6 && !isSingleBarrier && !isUnpassableBarrier && transform.position.z != 0 && Score.singleton.score > 60) {
			clocks = (GameObject)Instantiate (ClocksBarrierParticleSystem, new Vector3 (transform.position.x, 0.84f, transform.position.z), Quaternion.Euler(-90.0f, 0f, 0f));
            ParticleSystem.ShapeModule shape = clocks.GetComponent<ParticleSystem>().shape;
            ParticleSystem.EmissionModule emission = clocks.GetComponent<ParticleSystem>().emission;
            clocks.GetComponent<ParticleSystemRenderer>().material = (UIController.singleton.GetThemeColor().Equals("Light")) ? ClockParticleSystemLightMode : ClockParticleSystemDarkMode;
			if (isTripleBarrier) {
				shape.scale = new Vector3(1.467f, 0.25f, 0f);
			} else if (isQuadrupleBarrier) {
				shape.scale = new Vector3(1.05f, 0.25f, 0f);
			} else if (isDoubleBarrier)	{
				shape.scale = new Vector3(2.3f, 0.25f, 0f);
			}

			clocks.tag = "ClocksParticleSystem";
            // If this powerup is being create while the player is in rainbow powerup mode, stop the particle system
            if (PlayerController.singleton.isInPowerupMode) {
                clocks.GetComponent<ParticleSystem>().Stop();
            }
			isClocksPowerup = true;
		}

		// Set the opacity of the barrier
		Color tempColor = new Color (GetComponent<Renderer> ().material.color.r, GetComponent<Renderer> ().material.color.g, GetComponent<Renderer> ().material.color.b, .98f);
		GetComponent<Renderer> ().material.color = tempColor;

	}

	void OnTriggerEnter(Collider other) {
		GameObject otherGO = other.gameObject;

        string colorPassedThrough = gameObject.transform.GetComponent<BarrierProperties>().colorOfBarrier.ToString();

        if (other.gameObject.tag.Equals("Fireball")) return;

		// Incorrect Color
		if (!PlayerController.singleton.isInPowerupMode && !isUnpassableBarrier &&!CheckForEqualityOfColors (other.gameObject.GetComponent<Renderer> ().material.color, gameObject.GetComponent<Renderer> ().material.color)) {
            Sounds.singleton.PlayIncorrectColorSound();
			if(Sounds.singleton.GetSoundsAreOn()) Handheld.Vibrate ();
			CameraAnimationsController.singleton.ShakeCamera ();
			UIController.singleton.DecrementHeart ();
		//Powerup Mode
		} else if (PlayerController.singleton.isInPowerupMode) {
            Sounds.singleton.PlayCorrectColorSound();
			Score.singleton.updateScore = true;
            PlayerPrefsController.singleton.SetCurrentAmountOfAchievementByAchievementType(colorPassedThrough);
			MakeBarrierDustAndDestroy();
		// Do Not Cross Barrier
		} else if (isUnpassableBarrier){
            Sounds.singleton.PlayIncorrectColorSound();
			if(Sounds.singleton.GetSoundsAreOn()) Handheld.Vibrate ();
			CameraAnimationsController.singleton.ShakeCamera ();
			UIController.singleton.DecrementHeart ();
		// Correct Color
		} else {
			Score.singleton.updateScore = true;

			// Rainbow Powerup Barrier
			if (this.GetComponent<BarrierProperties> ().isPowerup) {
                Sounds.singleton.PlayPowerupSound();
				if(Sounds.singleton.GetSoundsAreOn()) Handheld.Vibrate ();
				PlayerController.singleton.TurnOnPowerup ();
				UIController.singleton.IncrementPowerupsCollectedInRound ();
			//Hearts increment Barrier, only take action if the player has less than full hearts
			} else if (this.isHeartsIncrement && UIController.singleton.GetCurrentHeartsFillAmount () != 1.0f) {
                Sounds.singleton.PlayPowerupSound();
				TurnOffPowerupParticleSystem ();
				if(Sounds.singleton.GetSoundsAreOn()) Handheld.Vibrate ();
				UIController.singleton.IncrementHearts ();
				UIController.singleton.IncrementPowerupsCollectedInRound ();
                //Clock Powerup Barrier
			} else if (this.isClocksPowerup && PlayerController.singleton.GetSpeedIncreasing() > PlayerController.singleton.minimumForwardSpeed) {
                Sounds.singleton.PlayPowerupSound();
				TurnOffPowerupParticleSystem ();
				if(Sounds.singleton.GetSoundsAreOn()) Handheld.Vibrate ();
				PlayerController.singleton.DecrementSpeedFromClocksPowerup ();
				UIController.singleton.IncrementPowerupsCollectedInRound ();
            } else {
                Sounds.singleton.PlayCorrectColorSound();
            }
			
            PlayerPrefsController.singleton.SetCurrentAmountOfAchievementByAchievementType(colorPassedThrough);
			//TODO: Turn this off and figure out if it is necessary? 
			PlayerController.singleton.speedIncreasing = PlayerController.singleton.memoizedSpeed;
			MakeBarrierDustAndDestroy();
		}
	}

	private void MakeBarrierDustAndDestroy() {
		GameObject BarrierDustGO = (GameObject)Instantiate (BarrierDestroyedParticleSystem, new Vector3 (transform.position.x, transform.position.y, transform.position.z), Quaternion.Euler(270f, 0f, 0f));
		BarrierDustGO.tag = "BarrierDestroyedParticleSystem";
		ParticleSystem BarrierDustPS = BarrierDustGO.GetComponent<ParticleSystem> ();
		ParticleSystem.MainModule BarrierDustPSMM = BarrierDustPS.main;
		Color BarrierColor = gameObject.GetComponent<Renderer> ().material.color;
		BarrierDustPS.GetComponent<Renderer>().material.color = new Color(BarrierColor.r, BarrierColor.g, BarrierColor.b, 0.85f);
		ParticleSystem.ShapeModule shape = BarrierDustPS.GetComponent<ParticleSystem> ().shape;
		ParticleSystem.EmissionModule emission = BarrierDustPS.GetComponent<ParticleSystem> ().emission;
		if (isSingleBarrier) {
			shape.scale = new Vector3 (5f, .3f, .75f);
			emission.rateOverTime = 200;
		} if (isTripleBarrier) {
			shape.scale = new Vector3 (1.5f, .3f, .75f);
		}  if (isQuadrupleBarrier) {
			shape.scale = new Vector3 (1.25f, .3f, .75f);
		}
		Destroy (this.gameObject);
	}

	// Returns true if the two colors passed in are equal, false otherwise
	private bool CheckForEqualityOfColors(Color firstColor, Color otherColor) {
		return firstColor.r == otherColor.r && firstColor.g == otherColor.g && firstColor.b == otherColor.b;
	}

	// Determines whether a barrier can be black
	private bool CanBarrierBeUnpassable() {

		// Number of sibling barriers that are unpassable
		int numUnpassableBarriersInRow = 0;

		// Single barrier case - cannot be unpassable or else player could not pass through without powerup
		if (transform.parent == null) {
			return false;
		}

		// Loops through all other barrier in the row and increments numUnpassableBarriersInRow if a sibling barrier is unpassable
		foreach (Transform child in transform.parent) {
			if (child.name != this.name) {
				if (child.GetComponent<BarrierProperties> () != null && child.GetComponent<BarrierProperties> ().isUnpassableBarrier) {
					numUnpassableBarriersInRow++;
				}
			}
		}
			
		// A barrier cannot be black if every other sibling barrier is black because the player could not pass this row without a powerup
		if (isDoubleBarrier && numUnpassableBarriersInRow == 1) {
			return false;
		} else if (isTripleBarrier && numUnpassableBarriersInRow == 2) {
			return false;
		} else if (isQuadrupleBarrier && numUnpassableBarriersInRow == 3) {
			return false;
		} else {
			return true;
		}
	}

	private void TurnOffPowerupParticleSystem() {
		if (isHeartsIncrement) {
			hearts.GetComponent<ParticleSystem> ().Stop ();
		} else if (isClocksPowerup) {
			clocks.GetComponent<ParticleSystem> ().Stop ();
		}
	}


}

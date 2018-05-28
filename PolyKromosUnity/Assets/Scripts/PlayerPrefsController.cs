using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerPrefsController : MonoBehaviour {

	// The top ten scores are stored in PlayerPrefs as TopPersonalScores1-numberOfTopScores
	private int numberOfTopScores = 10;

	private string[] colorNamesArray = { "Red", "Orange", "Yellow", "Green", "Blue", "Violet", "Black", "White" };

    // Increments for each achievement to be thrown
    public int[] incrementsForColorBarrierAchievements;// = {10, 100, 250, 500, 750, 1000, 2500, 5000, 7500, 10000};
    public int[] incrementsForDemolisherHitsAndBlackBarriers;// = {5, 10, 20, 30, 50, 75, 100, 200, 350, 500};
    public int[] incrementsForRainbowPowerups;// = {5, 10, 20, 30, 50, 75, 100, 150, 200, 250};
    public int[] incrementsForHeartPowerups;
    public int[] incrementsForSlowdownPowerups;
    public int[] incrementsForLifetimeDistance;// = {100, 1000, 2500, 5000, 7500, 10000, 50000, 100000, 250000, 500000};
    public int[] incrementsForLifetimePoints;// = { 100, 250, 500, 750, 1000, 2500, 5000, 10000, 25000, 50000 };

	public GameObject achievementNotification;
	public static PlayerPrefsController singleton;

	public Queue achievementsWaitingToBeThrown;

	void Awake() {
		if (singleton != null) {
			Object.Destroy(singleton);
		} else {
			singleton = this;
		}

		achievementsWaitingToBeThrown = new Queue ();
	}

	// Use this for initialization
	void Start () {
        //PlayerPrefs.DeleteAll();
        //setAchievementsToSpecificNumber();

        //SetAllAchievementsTo(0);
        //PlayerPrefs.SetFloat("lifetimeDistance", 0);
        //print("Distance: " + GetLifetimeDistance());
        //print("Powerups: " + GetPowerups());
        //print("Powerups highest: " + GetHighestLevelOfAchievementPowerups());
        //PlayerPrefs.SetFloat("lifetimeDistance", 0);
        //PlayerPrefs.SetInt("powerups", 0);
        //PlayerPrefs.SetInt("hitByDemolisher", 0);
        //PlayerPrefs.SetInt("lifetimePoints", 0);



        for(int i = 1; i <= numberOfTopScores; i++) {
			// If the new score is greater than the one at position i, insert it into that position in the pseudoarray
            //PlayerPrefs.SetInt("TopPersonalScores" + i.ToString(), 0);
		}

        //printTopScores();
        printNumberOfBarriersPassedThroughForEachColor();
			
	}

    //Given an achievement type, this method will check if an achievement has just been unlocked and throw a notification if it has
    public void CheckIfAchievementHasJustBeenUnlocked(string achievementType) {
        int[] achievementIncrementsArray = GetIncrementsArrayByAchievementType(achievementType);
        // I can't think of a descriptive name for this. Basically, this is the number of barriers you've passed through so far, or the number of demolishers you've hit so far, or the distance you've traveled so far, etc. 
        int currentAmountOfCertainAchievement = GetCurrentAmountOfAchievementByAchievementType(achievementType);

        for (int i = achievementIncrementsArray.Length - 1; i >= 0; i--) {
            // Current amount is at the increment level i
            if(currentAmountOfCertainAchievement >= achievementIncrementsArray[i]) {
                // If the highest level of achievement has just been passed, increment the highest level of achievement for that achievement and throw a notification that an achievement has just been reached
                // The minus one is necessary because Unity defaults player prefs ints to 0, so this method returning -1 is saying that no achievements have been obtained while anything 0 or higher is the index of the highest level achievement obtained.
                if(i > (GetHighestLevelOfAchievementByAchievementType(achievementType) - 1)) {
                    IncrementHighestLevelOfAchievementObtainedByAchievementType(achievementType);
                    TurnOnAchievementNotification(achievementType, false);
                }
            }
        }
    }

    public void ThrowSecretAchievementNotificationIfNeeded() {
        if (!HaveAllAchievementsBeenUnlocked() && CalculateIfAllAchievementsHaveBeenUnlocked()) {
            TurnOnAchievementNotification("SecretAchievement", false);
            SetAllAchievementsUnlocked();
        }
    }

    // Gets the amount of a certain achievement. For example, how many of a certain color barrier have been passed through, the distance the player has traveled, the demolishers the player has hit, etc.
    private int GetCurrentAmountOfAchievementByAchievementType(string achievementType) {
        return PlayerPrefs.GetInt("achievementAmount" + achievementType);
    }

    // Set the amount of a certain achievement. For example, how many of a certain color barrier have been passed through, the distance the player has traveled, the demolishers the player has hit, etc.
    public void SetCurrentAmountOfAchievementByAchievementType(string achievementType) {
        // Only increment achievements if the player hasn't unlocked every achievement yet. This prevents losers who spend their whole life playing this game from causing an integer overflow.
        if (!HaveAllAchievementsBeenUnlocked()) {
            // Increment the achievement amount
            PlayerPrefs.SetInt("achievementAmount" + achievementType, GetCurrentAmountOfAchievementByAchievementType(achievementType) + 1);
            // Check if an achievement has just been unlocked and throw a notification if so
            CheckIfAchievementHasJustBeenUnlocked(achievementType);
            // Check if all of the achievements have been unlocked and set a playerpref int to reflect that if so
            if(CalculateIfAllAchievementsHaveBeenUnlocked()) {
                TurnOnAchievementNotification("SecretAchievement", false);
                SetAllAchievementsUnlocked();
            }
        }
    }

    // Gets the amount of a certain achievement. For example, how many of a certain color barrier have been passed through, the distance the player has traveled, the demolishers the player has hit, etc.
    private int[] GetIncrementsArrayByAchievementType(string achievementType) {
        if (isAchievementTypeAColorBarrier(achievementType)) {
            return incrementsForColorBarrierAchievements;
        } else if (achievementType == "Demolishers" || achievementType == "Black") {
            return incrementsForDemolisherHitsAndBlackBarriers;
        } else if (achievementType == "Points") {
            return incrementsForLifetimePoints;
        } else if (achievementType == "RainbowPowerups") {
            return incrementsForRainbowPowerups;
        } else if (achievementType == "HeartPowerups") {
            return incrementsForHeartPowerups;
        } else if (achievementType == "ClockPowerups") {
            return incrementsForSlowdownPowerups;
        } else if (achievementType == "Distance") {
            return incrementsForLifetimeDistance;
        }
        return new int[0];
    }

    // Increments a playerpref int that reflects the highest increment of a certain achievement that has been obtained
    private void IncrementHighestLevelOfAchievementObtainedByAchievementType(string achievementType) {
        PlayerPrefs.SetInt("highestLevelOfAchievementObtained" + achievementType, GetHighestLevelOfAchievementByAchievementType(achievementType) + 1);
    }

    // Returns the highest increment of a certain achievement that has been obtained.
    public int GetHighestLevelOfAchievementByAchievementType(string achievementType){
        return PlayerPrefs.GetInt("highestLevelOfAchievementObtained" + achievementType);
    }

	// Turn on an achievement notification for the given color
	public void TurnOnAchievementNotification(string achievementType, bool fromQueue) {

        AchievementNotification achievementNotificationProperties = achievementNotification.GetComponent<AchievementNotification> ();

		//If you are calling this method as a result of the achievement notification ending and having more to display in the queue, you need to pop the queue for the next achievement to display
		if (fromQueue) {
			achievementType = achievementsWaitingToBeThrown.Dequeue().ToString();
		}

		// If the achievement notification is already on the screen, queue the achievement trying to display to be displayed once the achievement notification is available 
		// The value fromQueue lets this method know that if this is being called from the queue being dequeued, it should not queue the achievement calling it, but rather display it
		if (achievementNotification.activeInHierarchy && !fromQueue) {
			achievementsWaitingToBeThrown.Enqueue (achievementType);
		//Throws the achievement to the screen if there is no achievement currently displayed, or there is one "displayed" (but off screen) and the next one is being called from the queue
		} else {
            Sounds.singleton.PlayAchievementUnlockedSound();
			bool isColorAchievement = isAchievementTypeAColorBarrier (achievementType);

            if (!achievementType.Equals("SecretAchievement")) {
                string foregroundImageColor = (isColorAchievement || achievementType.Equals("Black")) ? achievementType : "White";
                achievementNotificationProperties.setForegroundImageColor(foregroundImageColor);
                achievementNotificationProperties.setForegroundImage(achievementType);
            } else {
                achievementNotificationProperties.setForegroundImageColor("Black");
                achievementNotificationProperties.setForegroundImage("Black");
            }

            achievementNotificationProperties.specificAchievementText.GetComponent<Text>().text = Achievement.singleton.getAchievementTypeToAchievementLabel(achievementType);

            if (!achievementType.Equals("SecretAchievement")) {
                //This will set the text indicating the amount of a certain achievement obtained by looking at the increments array for the achievement that was just passed
                int[] incrementsArray = GetIncrementsArrayByAchievementType(achievementType);
                achievementNotificationProperties.setForegroundText(incrementsArray[GetHighestLevelOfAchievementByAchievementType(achievementType) - 1].ToString());
            } else {
                achievementNotificationProperties.setForegroundText("???");
            }

			UIController.singleton.IncrementAchievementsCollectedInRound ();

			// Feedback from people said they would rather this not vibrate as they think they've lost a heart.
			//if(Sounds.singleton.GetSoundsAreOn()) Handheld.Vibrate ();

			achievementNotification.SetActive (true);
		}
		 
	}

	// Returns true if achievementType is a color achievement, otherwise returns false 
	private bool isAchievementTypeAColorBarrier(string achievementType) {
		foreach (string color in colorNamesArray) {
			if(color.Equals(achievementType) && !color.Equals("Black")) {
				return true;
			}
		}
		return false;
	}

    // Returns true if there are no achievements waiting to throw a notification and false otherwise
    public bool isAchievementsWaitingToBeThrownQueueEmpty() {
        return achievementsWaitingToBeThrown.Count == 0;
    }

    // Returns the amount of achievements that can be obtained for each achievement type
    public int getSizeOfAchievementIncrementsArray() {
        return incrementsForColorBarrierAchievements.Length;
    }

    // Calculates if all achievements have been unlocked
    private bool CalculateIfAllAchievementsHaveBeenUnlocked() {
        int highestAchievementIndex = getSizeOfAchievementIncrementsArray();
        foreach(string color in colorNamesArray) {
            if(GetHighestLevelOfAchievementByAchievementType(color) != highestAchievementIndex) {
                return false;
            }
        }

        return (GetHighestLevelOfAchievementByAchievementType("Demolishers") == highestAchievementIndex)
            && (GetHighestLevelOfAchievementByAchievementType("Points") == highestAchievementIndex)
            && (GetHighestLevelOfAchievementByAchievementType("RainbowPowerups") == highestAchievementIndex)
            && (GetHighestLevelOfAchievementByAchievementType("ClockPowerups") == highestAchievementIndex)
            && (GetHighestLevelOfAchievementByAchievementType("HeartPowerups") == highestAchievementIndex)
            && (GetHighestLevelOfAchievementByAchievementType("Distance") == highestAchievementIndex);
    }

    // Returns true if all achievements have been unlocked and false if they have not
    public bool HaveAllAchievementsBeenUnlocked() {
        return PlayerPrefs.GetInt("AllAchievementsUnlocked") == 1;
    }

    // Set the AllAchievementsUnlocked PlayerPref to true
    private void SetAllAchievementsUnlocked() {
        PlayerPrefs.SetInt("AllAchievementsUnlocked", 1);
    }

    #region Testing
    private void SetAllAchievementsTo(int num) {
        for (int i = 0; i < colorNamesArray.Length; i++) {
            string playerPrefToPrint = "achievementAmount" + colorNamesArray[i];
            PlayerPrefs.SetInt(playerPrefToPrint, num);
            //print(colorNamesArray[i] + ": " + PlayerPrefs.GetInt("highestLevelAchievementObtained" + colorNamesArray[i]));
        }
    }

    // Prints the top 10 scores 
    public void printTopScores() {
        for (int i = 1; i <= numberOfTopScores; i++) {
            // If the new score is greater than the one at position i, insert it into that position in the pseudoarray
            //print(i.ToString() + ". " + PlayerPrefs.GetInt("TopPersonalScores" + i.ToString()));
        }
    }

    private void printNumberOfBarriersPassedThroughForEachColor() {
        for (int i = 0; i < colorNamesArray.Length; i++) {
            string playerPrefToPrint = "achievementAmount" + colorNamesArray[i];
            //print(colorNamesArray[i] + ": " + PlayerPrefs.GetInt(playerPrefToPrint));
            //print(colorNamesArray[i] + ": " + PlayerPrefs.GetInt("highestLevelOfAchievementObtained" + colorNamesArray[i]));
        }

    }

    private void setAchievementsToSpecificNumber() {
        for (int i = 0; i < colorNamesArray.Length; i++) {
            string playerPrefToPrint = "achievementAmount" + colorNamesArray[i];
            PlayerPrefs.SetInt(playerPrefToPrint, 10000);
            PlayerPrefs.SetInt("highestLevelOfAchievementObtained" + colorNamesArray[i], 10);
        }
        PlayerPrefs.SetInt("achievementAmountDemolishers", 500);
        PlayerPrefs.SetInt("achievementAmountPoints", 49999);
        PlayerPrefs.SetInt("achievementAmountDistance", 500000);
        PlayerPrefs.SetInt("achievementAmountRainbowPowerups", 250);
        PlayerPrefs.SetInt("achievementAmountClockPowerups", 250);
        PlayerPrefs.SetInt("achievementAmountHeartPowerups", 250);

        PlayerPrefs.SetInt("highestLevelOfAchievementObtainedDemolishers", 10);
        PlayerPrefs.SetInt("highestLevelOfAchievementObtainedPoints", 9);
        PlayerPrefs.SetInt("highestLevelOfAchievementObtainedDistance", 10);
        PlayerPrefs.SetInt("highestLevelOfAchievementObtainedRainbowPowerups", 10);
        PlayerPrefs.SetInt("highestLevelOfAchievementObtainedClockPowerups", 10);
        PlayerPrefs.SetInt("highestLevelOfAchievementObtainedHeartPowerups", 10);

        PlayerPrefs.SetInt("AllAchievementsUnlocked", 0);
    }

    #endregion

    #region Score
    // Returns the number 1-10 if new high score, returns -1 otheriwse
    public int addScore(int score) {
        // Iterate through the list of top scores
        for (int i = 1; i <= numberOfTopScores; i++) {
            // If the new score is greater than the one at position i, insert it into that position in the pseudoarray
            if (score > PlayerPrefs.GetInt("TopPersonalScores" + i.ToString())) {
                insertScoreIntoArrayAtPosition(score, i);
                return i;
            }
        }
        //The score is not larger than any in the top ten list. Return -1
        return -1;
    }

    // Inserts a new high score into the array of top scores
    private void insertScoreIntoArrayAtPosition(int score, int index) {
        for (int i = numberOfTopScores; i > index; i--) {
            // Shifts down the top scores until you get to the position of the index
            PlayerPrefs.SetInt("TopPersonalScores" + i.ToString(), PlayerPrefs.GetInt("TopPersonalScores" + ((i - 1).ToString())));
        }
        PlayerPrefs.SetInt("TopPersonalScores" + index.ToString(), score);
    }

    // Returns the top (index)th score
    public int getTopScoreAtIndex(int index) {
        return (PlayerPrefs.GetInt("TopPersonalScores" + index.ToString()));
    }

    #endregion

}

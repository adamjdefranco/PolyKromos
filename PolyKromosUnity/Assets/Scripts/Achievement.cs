using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Achievement : MonoBehaviour {

    public static Achievement singleton;

    private string label;
    private int highestLevelOfAchievement;
    private Color color;

    public Sprite[] achievementImages;

    private string[] achievementTypes = { "Red", "Orange", "Yellow", "Green", "Blue", "Violet", "White", "Black", "Points", "Demolishers", "Distance", "RainbowPowerups", "HeartPowerups", "ClockPowerups", "SecretAchievement" };
    private string[] achievementTypeLabels = { "Red Barriers", "Orange Barriers", "Yellow Barriers", "Green Barriers", "Blue Barriers", "Violet Barriers", "White Barriers", "Black Barriers", "Lifetime Points", "Enemies Destroyed", "Lifetime Distance", "Rainbow Powerups", "Heart Powerups", "Slowdown Powerups", "Secret Achievement" };

	void Awake() {
		if (singleton != null) {
			Object.Destroy(singleton);
		} else {
			singleton = this;
		}
	}

    private string getStringOfAchievementFromIndex(int index) {
        return achievementTypes[index];
    }

    public string getAchievementTypeToAchievementLabel(string achievementType) {
        for (int i = 0; i < achievementTypes.Length; i++) {
            if (achievementTypes[i].Equals(achievementType)) return achievementTypeLabels[i];
        }
        return "Error";
    }

    private int getIndexOfAchievementFromString(string achievement) {
        for (int i = 0; i < achievementTypes.Length; i++) {
            if (achievementTypes[i].Equals(achievement)) return i;
        }
        return -1;
    }

    public string getLabelByIndex(int index) {
        return achievementTypeLabels[index];
    }

    public string getLabelByString(string achievementType) {
        for (int i = 0; i < achievementTypeLabels.Length; i++) {
            if(achievementTypeLabels[i].Equals(achievementType)) return getLabelByIndex(i);
        }
        return "Error";
    }

    public Sprite getAchievementImageByIndex(int index) {
        if (index <= 7) {
            return achievementImages[0];
        } else {
            return achievementImages[index - 7];
        }
    }

    public Sprite getAchievementImageByString(string achievementType) {
        return getAchievementImageByIndex(getIndexOfAchievementFromString(achievementType));
    }

    public Color getColor(int index) {
		switch (index)
		{
			case 0: return RainbowColors.singleton.getRed();
			case 1: return RainbowColors.singleton.getOrange();
			case 2: return RainbowColors.singleton.getYellow();
			case 3: return RainbowColors.singleton.getGreen();
			case 4: return RainbowColors.singleton.getBlue();
			case 5: return RainbowColors.singleton.getViolet();
            case 6: return RainbowColors.singleton.getWhite();
			case 7: return RainbowColors.singleton.getBlack();
            default: return new Color(1, 1, 1, 1);
		}
    }

    // The minus 1 is necessary because Unity defaults player prefs to 0 so -1 denotes that not achievement has been obtained whereas anything higher than that is the index of the level of achievement obtained.
    public int getHighestLevelOfAchievement(int index) {
        return PlayerPrefsController.singleton.GetHighestLevelOfAchievementByAchievementType(getStringOfAchievementFromIndex(index)) - 1;
    }

    public int getAchievementIncrementAtIndex(int index, int achievementIncrementIndex) {
        if(index < 7) {
            return PlayerPrefsController.singleton.incrementsForColorBarrierAchievements[achievementIncrementIndex];
        } else if (index == 8) {
            return PlayerPrefsController.singleton.incrementsForLifetimePoints[achievementIncrementIndex];
        } else if (index == 9 || index == 7) {
            return PlayerPrefsController.singleton.incrementsForDemolisherHitsAndBlackBarriers[achievementIncrementIndex];
		} else if (index == 10) {
			return PlayerPrefsController.singleton.incrementsForLifetimeDistance[achievementIncrementIndex];
		} else if (index == 11) {
            return PlayerPrefsController.singleton.incrementsForRainbowPowerups[achievementIncrementIndex];
        } else if (index == 12) {
            return PlayerPrefsController.singleton.incrementsForHeartPowerups[achievementIncrementIndex];
        } else if (index == 13) {
            return PlayerPrefsController.singleton.incrementsForSlowdownPowerups[achievementIncrementIndex];
        } else {
            return -1;
        }
    }
}

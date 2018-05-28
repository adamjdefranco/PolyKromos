using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AchievementNotification : MonoBehaviour {

	public GameObject specificAchievementText;
	public GameObject backgroundImage;
	public GameObject foregroundImage;
	public GameObject foregroundText;

	public void turnOff() {
        PlayerPrefsController.singleton.ThrowSecretAchievementNotificationIfNeeded();
		if (PlayerPrefsController.singleton.isAchievementsWaitingToBeThrownQueueEmpty()) {
			gameObject.SetActive (false);
		} else {
			PlayerPrefsController.singleton.TurnOnAchievementNotification ("", true);
		}
	}

	public void playHeartsAnimation() {
		UIController.singleton.PlayHeartsAnimationForAchievement ();
	}

	public void setForegroundText(string foregroundTextString) {
		this.foregroundText.GetComponent<Text> ().text = foregroundTextString;
	}

	public void setForegroundImageColor(string color) {
		foregroundImage.GetComponent<Image> ().color = RainbowColors.singleton.getColorFromString (color);
	}

    public void setForegroundImage(string index) {
        foregroundImage.GetComponent<Image>().sprite = Achievement.singleton.getAchievementImageByString(index);
    }

}

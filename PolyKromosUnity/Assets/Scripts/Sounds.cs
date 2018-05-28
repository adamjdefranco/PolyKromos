using UnityEngine;
using System.Collections;

public class Sounds : MonoBehaviour {

	public AudioSource powerup;
	public AudioSource correctColor;
	public AudioSource incorrectColor;
	public AudioSource fireball;
    public AudioSource mainMusic;
    public AudioSource achievement;
    public AudioSource menuClick;
    public AudioSource startGame;
	public AudioSource countdownTick;
	public AudioSource countdownBegin;
	public AudioSource endGame;

    //public AudioSource[] PentatonicNotes;

	public static Sounds singleton;

    private bool soundsAreOn = true; 

	//Singleton
	void Awake() {
		if (singleton != null) {
			Object.Destroy(singleton);
		} else {
			singleton = this;
		}
        mainMusic.Play();
	}

	public bool GetSoundsAreOn() {
		return soundsAreOn;
	}

    public void PlayMenuClick(){
        if (soundsAreOn) menuClick.Play();
    }

	public void PlayCountdownTick(){
		if (soundsAreOn) countdownTick.Play();
	}

	public void PlayCountdownBegin(){
		if (soundsAreOn) countdownBegin.Play();
	}

    public void PlayStartGame() {
        if (soundsAreOn) startGame.Play();
    }

	public void PlayEndGame() {
		if (soundsAreOn) endGame.Play();
	}

	public void PlayPowerupSound() {
        if (soundsAreOn) powerup.Play () ;
	}

    public void PlayAchievementUnlockedSound() {
        if (soundsAreOn) achievement.Play();
    }

	public void PlayCorrectColorSound() {
        if (soundsAreOn) correctColor.Play(); 
	}

	public void PlayIncorrectColorSound() {
        if (soundsAreOn) incorrectColor.Play ();
	}

	public void PlayFireballSound() {
        if (soundsAreOn) fireball.Play ();
	}

    public void toggleSoundsAreOn() {
        soundsAreOn = !soundsAreOn;
        toggleMainMusic();
    }

    public void toggleMainMusic() {
		mainMusic.volume = (soundsAreOn) ? 0.6f : 0.0f;
    }
}

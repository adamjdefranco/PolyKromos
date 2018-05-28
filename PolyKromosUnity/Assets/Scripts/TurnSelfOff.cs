using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnSelfOff : MonoBehaviour {
    public void TurnOffButtonInstructionsText() {
        this.gameObject.SetActive(false);
    }
}










/*   void FixedUpdate() {
        // These next few lines increment the distance achievement amount by 1 every time the player has traveled by 1 unit
        distanceForIncrementingDistancePowerup += this.transform.position.z - previousZDistance;
        previousZDistance = this.transform.position.z;
        if (distanceForIncrementingDistancePowerup > 1) {
            distanceForIncrementingDistancePowerup--;
            PlayerPrefsController.singleton.SetCurrentAmountOfAchievementByAchievementType("Distance");
        }

        // TODO: Why are you doing this, it should just be a child of the player
        if (powerupParticleSystem.activeInHierarchy) {
            powerupParticleSystem.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - 0.2f);
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


            /*speedIncreasing += 1;
            if (speedIncreasing > minimumForwardSpeed && !isInPowerupMode) {
                SetBarrierParticleSystemVisibility(true, "ClocksParticleSystem");
            }
            memoizedSpeed = speedIncreasing;
        }

        // Increases the players speed by 1 if they have collected 20 points since the last speed increment or decrement
        if(pointsSinceLastSpeedIncrement > 9) {
            speedIncreasing += .75f;
            memoizedSpeed = speedIncreasing;
            if (speedIncreasing > minimumForwardSpeed && !isInPowerupMode) {
                SetBarrierParticleSystemVisibility(true, "ClocksParticleSystem");
            }
            pointsSinceLastSpeedIncrement = 0;
        }

        float moveHorizontal;

        if (!isInEndGameState && !(transform.position.x > 2.5 || transform.position.x< -2.5)) { 
            moveHorizontal = Input.GetAxis("Horizontal");
        } else {
            moveHorizontal = 0;
        }

        //There has got to be a way to do this outside of update? do it in the score class maybe? then check when it's been incremented?
        if (isInPowerupMode && Score.singleton.score > scoreForParticleSystemToTurnOff) {

            TurnOffPowerup();
        }

        Vector3 movement = new Vector3(moveHorizontal * 0.5f, 0.0f, forward);
        //Vector3 turns = new Vector3 (moveHorizontal, 0, 0);

        if (transform.position.x > 2.5 || transform.position.x< -2.5) {
            rb.AddForce(new Vector3(0, -40f, 0));
        } else {
            rb.velocity = movement* speedIncreasing;
        }

        if (transform.position.y< -0.5f && !UIController.singleton.isEndGamePanelOn() && !isInEndGameState) {
            rb.velocity = new Vector3(0, 0, 0);
UIController.singleton.EndGameActions();
        }

        #if UNITY_STANDALONE_OSX
        //print("true")
        if(!isInPowerupMode && !UIController.singleton.isPaused) {
            if (Input.GetKey (KeyCode.Z) && Input.GetKey (KeyCode.X)) {
                // Orange
                //rend.material.color = new Color32(255,100,0, 150);
                rend.material.color = RainbowColors.singleton.getOrange();
            } else if (Input.GetKey (KeyCode.Z) && Input.GetKey (KeyCode.C)) {
                // Purple
                rend.material.color = RainbowColors.singleton.getViolet();
            } else if (Input.GetKey (KeyCode.C) && Input.GetKey (KeyCode.X)) {
                rend.material.color = RainbowColors.singleton.getGreen();
            } else if (Input.GetKey (KeyCode.Z)) {
                rend.material.color = RainbowColors.singleton.getRed();
            } else if (Input.GetKey (KeyCode.X)) {
                rend.material.color = RainbowColors.singleton.getYellow();
            } else if (Input.GetKey (KeyCode.C)) {
                rend.material.color = RainbowColors.singleton.getBlue();
            } else {
                if(UIController.singleton.ThemeColor == "Light") {
                    rend.material.color = RainbowColors.singleton.getWhite();
                } else {
                    rend.material.color = RainbowColors.singleton.getBlack();
                }
            }
        }

        //Color tempColor = new Color(rend.material.color.r, rend.material.color.g, rend.material.color.b, .2f);
        //rend.material.color = tempColor;

        #endif 

        #if UNITY_IOS
        if(!isInPowerupMode && !Object.FindObjectOfType<UIController>().isPaused) {
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
    }*/
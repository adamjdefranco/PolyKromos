using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CameraController : MonoBehaviour {

	public GameObject player;

	public bool updateCamera;
	public GameObject InGameCanvas;

	public GameObject RedButton;
	public GameObject YellowButton;
	public GameObject BlueButton;
	public GameObject Hearts;

    public GameObject BackgroundSquares;

	public static CameraController singleton;

	private float LerpToMainMenuFromInGameSpeed = 2f;
	private bool LerpToMainMenuFromInGame;
	private Vector3 NewPositionForLerpingToMainMenu;
	private float ZValueToLerpTo;

	//Singleton
	void Awake() {
		if (singleton != null){
			Object.Destroy(singleton);
		} else {
			singleton = this;
		}
	}

	void Start () {
		updateCamera = false;
		LerpToMainMenuFromInGame = false;
	}

    // Have camera follow the player, or have the camera lerp to the main menu on end game
	void LateUpdate () {
		if (updateCamera) {
            transform.position = new Vector3 (0, 2f, (player.transform.position.z - 3.0f));
		} else if (LerpToMainMenuFromInGame) {
			ZValueToLerpTo = PlayerController.singleton.GetCurrentGroundSegmentYValue() + 55f;
			NewPositionForLerpingToMainMenu = new Vector3 (
				this.transform.position.x, 
				this.transform.position.y, 
				Mathf.Lerp (this.transform.position.z, ZValueToLerpTo, LerpToMainMenuFromInGameSpeed * Time.deltaTime));
			this.transform.position = NewPositionForLerpingToMainMenu;
			if (this.transform.position.z > ZValueToLerpTo - 5f) {
				UIController.singleton.AnimateMainMenuCanvasInAfterLerp ();
				LerpToMainMenuFromInGame = false;
			}
		}
	}

    // Begin the process of moving from the game back to the main menu
	public void StartLerpFromInGameToMainMenu() {
		updateCamera = false;
		LerpToMainMenuFromInGame = true;
	}

    // Corrects the camera position to a fixed point after it has leped to the main menu
	public void SetCameraPositionToMainMenu() {
		this.transform.position = new Vector3 (0f, 2f, 110f);

        // TODO: This is a patch for the problem where the background particle systems dissapear if you get over ~200 points
        if (Score.singleton.score > 199) {
            BackgroundSquares.SetActive(false);
            BackgroundSquares.SetActive(true);
        }


	}
		
}
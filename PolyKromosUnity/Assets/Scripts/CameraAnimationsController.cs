using UnityEngine;
using System.Collections;

public class CameraAnimationsController : MonoBehaviour {

	public KeyCode ScreenshotKey = KeyCode.Tab;

	public static CameraAnimationsController singleton;

	// Only to be used for taking promotional screenshots. Comment out for release.
	/*void LateUpdate() {
		if(Input.GetKey(ScreenshotKey)){
			ScreenCapture.CaptureScreenshot(Screen.width + "x" + Screen.height + Time.time + ".png");
		}
	}*/

	//Singleton
	void Awake() {
		if (singleton != null) {
			Object.Destroy(singleton);
		} else {
			singleton = this;
		}
	}

	public void StartGameActions() {

		CameraController.singleton.transform.position = new Vector3(0,2,0);
		this.transform.localPosition = Vector3.zero; 
		//this.transform.position = Vector3.zero; 

		CameraController.singleton.updateCamera = true;
		//TODO: Move all of this to the UI Helper.
		UIController.singleton.InGameCanvas.GetComponent<CanvasGroup>().alpha = 0;
		UIController.singleton.InGameCanvas.SetActive (true);
		UIController.singleton.InGameCanvas.GetComponent<Animator> ().Play ("AnimateInInGameCanvas");
	}	

	public void ShakeCamera() {
		gameObject.GetComponent<Animator> ().Play ("CameraShake");
	}

	public void DisableMainMenuCanvas() {
		PlayerController.singleton.DisableMainMenuCanvas ();
	}

	/*public void SetCameraPositionToMainMenu() {
		CameraController.singleton.updateCamera = false;
		this.transform.position = new Vector3 (0f, 2f, 110f);
	}*/

}

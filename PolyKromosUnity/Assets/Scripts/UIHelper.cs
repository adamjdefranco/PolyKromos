using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIHelper : MonoBehaviour {

	//public GameObject PlayerControl;
	//public GameObject UIControl; 
	//public GameObject CameraControl;
	//public GameObject InGameCanvas;

	/*private void callResetPlayerPosition() {
		GameObject.FindObjectOfType<PlayerController> ().reset ();
		PlayerControl.GetComponent<PlayerController> ().resetPositionOfPlayer ();
		UIControl.GetComponent<UIController> ().TurnOffEndGamePanel ();
	}

	public void NavigateToMainMenuFromInGame() {
		CameraControl.GetComponent<CameraController> ().SetCameraPositionToMainMenu ();
		InGameCanvas.SetActive (false);
		GameObject.FindObjectOfType<PlayerController> ().EnableMainMenuCanvas ();
		UIControl.GetComponent<UIController> ().TurnOffEndGamePanel ();
		UIControl.GetComponent<UIController> ().SetMainMenuCanvasGroupToVisible ();
		GameObject.FindObjectOfType<PlayerController> ().reset ();
		GameObject.FindObjectOfType<PlayerController> ().resetPositionOfPlayer ();
	}*/


	public void EndOfAnimationEvents() {
		UIController.singleton.EndOfEndGameAnimateOutActions();
		this.gameObject.SetActive (false);
	}
		


}

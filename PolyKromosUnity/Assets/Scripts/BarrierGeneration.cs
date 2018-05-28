using UnityEngine;
using System.Collections;

public class BarrierGeneration : MonoBehaviour {

	public GameObject singleDivisionBarrier;
	public GameObject doubleDivisionBarrier;
	public GameObject tripleDivisionBarrier;
	public GameObject quadrupleDivisionBarrier;

	public float gridX;
	public float gridY;
	public float spacing;
	public float zOffset;

	// Use this for initialization
	public void Start () {
		zOffset = PlayerController.singleton.zDistance;

		// For some reason I'm setting the barrier game objects manually if they can't be found in the scene - I'll just go ahead and assume this is necessary for some reason
		if (singleDivisionBarrier == null || doubleDivisionBarrier == null || tripleDivisionBarrier == null) {
			singleDivisionBarrier = GameObject.Find ("Barrier");
			doubleDivisionBarrier = GameObject.Find ("DoubleBarrier");
			tripleDivisionBarrier = GameObject.Find ("TrippleBarrier");
			quadrupleDivisionBarrier = GameObject.Find ("QuadrupleBarrier");
		}

		int yStartValue = 1;

		for (int y = yStartValue; y < gridY; y++) {
			for (int x = 0; x < gridX; x++) {
				Vector3 pos = new Vector3 (0, 0, y) * spacing;

				pos.z += (zOffset > 0) ? zOffset - 50f : zOffset;

				GameObject theBarrier = null;

				int randomNum = Random.Range (1, 5);
                // A single barrier should always start the same for simplicity sake, and should not be unpassable barrier so that the countdown will appear clearly (single barrier cannot be unpassable so no additional logic is needed)
                if ((randomNum == 1 && pos.z > 100) || pos.z < 26) {
					pos = new Vector3 (pos.x, pos.y + 0.325f, pos.z);
					theBarrier = Instantiate (singleDivisionBarrier, pos, Quaternion.identity);
                } else if ((randomNum == 2 && pos.z > 100) || pos.z < 51) {
					theBarrier = Instantiate (doubleDivisionBarrier, pos, Quaternion.identity);
                } else if ((randomNum == 3 && pos.z > 100) || pos.z < 76) {
					theBarrier = Instantiate (tripleDivisionBarrier, pos, Quaternion.identity);
                } else if ((randomNum == 4 && pos.z > 100) || pos.z < 101) {
					theBarrier = Instantiate (quadrupleDivisionBarrier, pos, Quaternion.identity);
				} 

				// Set the tag for the barrier so that it can be found later when mass destroying barriers 
				theBarrier.tag = "Barrier";
			}
		}
	}
}

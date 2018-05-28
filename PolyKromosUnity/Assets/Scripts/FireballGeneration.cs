using UnityEngine;
using System.Collections;

public class FireballGeneration : MonoBehaviour {

	private GameObject fireball;
	public GameObject fireballPrefab;

    public GameObject player;
    public static FireballGeneration singleton;

    //Singleton
    void Awake() {
        if (singleton != null) {
            Object.Destroy(singleton);
        } else {
            singleton = this;
        }
    }
	
	// Update is called once per frame
	public void GenerateFireball () {
		int rand = Random.Range (0, 5);

		if (rand == 1) {
			int randValueForXPosition = Random.Range (0, 4);
			float xPosition;

			switch (randValueForXPosition) {
			    case 0: 
				    xPosition = -1.875f;
				    break;
			    case 1:
				    xPosition = -0.625f;
				    break;
			    case 2: 
				    xPosition = 0.625f;
				    break;
                case 3:
                    xPosition = 1.875f;
                    break;
			    default:
				xPosition = 0.0f;
				    break;
			}

			Vector3 pos = new Vector3 (xPosition, 0.35f, player.transform.position.z + 100f);
			fireball = Instantiate (fireballPrefab, pos, Quaternion.identity);
			fireball.tag = "Fireball";
		}
	}

}

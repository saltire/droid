using UnityEngine;
using UnityEngine.SceneManagement;

public class Restart : MonoBehaviour {
	void Update() {
		if (Input.GetAxis("Restart") > 0) {
			SceneManager.LoadScene("game");
		}
	}
}

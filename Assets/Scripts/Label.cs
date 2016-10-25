using UnityEngine;
using System.Collections;

public class Label : MonoBehaviour {
    public string label;

    void Start () {
        GetComponent<TextMesh>().text = label;
    }
	
	void LateUpdate () {
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, 1, transform.rotation.eulerAngles.z);
    }
}

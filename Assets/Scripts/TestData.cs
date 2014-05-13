using UnityEngine;
using System.Collections;

public class TestData : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		DebugGUI.Instance.Message(DebugGUI.Sides.RIGHT, "Hello", Color.yellow, TextAnchor.UpperLeft, 20);
		DebugGUI.Instance.Message(DebugGUI.Sides.RIGHT, "Test", Color.black, TextAnchor.UpperLeft, 12);
		DebugGUI.Instance.Message(DebugGUI.Sides.RIGHT, "OK");
		DebugGUI.Instance.Message(DebugGUI.Sides.LEFT, "STOP", Color.red);
		DebugGUI.Instance.Message(DebugGUI.Sides.LEFT, "GO", Color.green);
		DebugGUI.Instance.Message(DebugGUI.Sides.RIGHT, "Hello2", Color.yellow, TextAnchor.MiddleCenter, 20);

		if (Input.GetKeyDown(KeyCode.Space)) {
			DebugGUI.Instance.Log(DebugGUI.Sides.LEFT, "Logged", Color.green);
			DebugGUI.Instance.Log(DebugGUI.Sides.LEFT, "Logged2", Color.gray);
			DebugGUI.Instance.Log(DebugGUI.Sides.RIGHT, "LoggedR");
		}
	}
}

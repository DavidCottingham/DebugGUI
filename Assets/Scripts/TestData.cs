using UnityEngine;
using System.Collections;

public class TestData : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		DebugGUI.Instance.Message(DebugGUI.MessageArea.TOP_RIGHT, "Hello", Color.yellow, TextAnchor.UpperLeft, 20);
		DebugGUI.Instance.Message(DebugGUI.MessageArea.TOP_RIGHT, "Test", Color.black, TextAnchor.UpperLeft, 12);
		DebugGUI.Instance.Message(DebugGUI.MessageArea.TOP_RIGHT, "OK");
		DebugGUI.Instance.Message(DebugGUI.MessageArea.TOP_LEFT, "STOP", Color.red);
		DebugGUI.Instance.Message(DebugGUI.MessageArea.TOP_LEFT, "GO", Color.green);
		DebugGUI.Instance.Message(DebugGUI.MessageArea.TOP_RIGHT, "Hello2", Color.yellow, TextAnchor.MiddleCenter, 20);

		if (Input.GetKeyDown(KeyCode.Space)) {
			DebugGUI.Instance.Log(DebugGUI.MessageArea.TOP_LEFT, "Logged", Color.green);
			DebugGUI.Instance.Log(DebugGUI.MessageArea.TOP_LEFT, "Logged2", Color.gray);
			DebugGUI.Instance.Log(DebugGUI.MessageArea.TOP_RIGHT, "LoggedR");
		}
	}
}

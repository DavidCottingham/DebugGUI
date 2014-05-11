using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Helper class to display and log Debug output to the screen. Use <see cref="Message()"/>or <see cref="Log()"/>to display or log a message.
/// </summary>
public class DebugGUI : MonoBehaviour {

	///<summary>
	/// The target output area or log side for the message you are sending
	/// </summary>
	public enum Sides { LEFT, RIGHT };

	///<summary>
	/// Coupling struct for the message, its corresponding side's area/log to be displayed on, and a custom style.
	/// </summary>
	private struct DebugMessage {
		public Sides side;
		public string message;
		public GUIStyle style;

		public DebugMessage(Sides side, string message, Color fontColor, TextAnchor anchor, int fontSize) {
			this.side = side;
			this.message = message;
			this.style = new GUIStyle(GUI.skin.label); //??
			style.fontSize = fontSize;
			style.alignment = anchor;
			style.normal.textColor = fontColor;
		}

		public DebugMessage(Sides side, string message, GUIStyle style) : this(side, message, style.normal.textColor, style.alignment, style.fontSize) {}
	}

	/// <summary>
	/// The default <see cref="label"/>  style can be overridden here. The label of the <see cref="defaultSkin"/> will get overridden too, if this is set.
	/// </summary>
	[SerializeField] private static GUIStyle defaultStyle;
	/// <summary>
	/// Custom skin to be used for all output.
	/// </summary>
	[SerializeField] private static GUISkin defaultSkin;

	private int defaultLogLength = 8;
	private int defaultLineLength = 250;
	private int defaultLineHeight = 22;
	private int defaultLineSpacing = 12;

	/// <summary>
	/// The maximum number of lines in the right side log.
	/// </summary>
	public static int maxLogLinesRight = 8;
	/// <summary>
	/// The Maximum number of lines in the left side log.
	/// </summary>
	public static int maxLogLinesLeft = 8;

	/// <summary>
	/// Display line maximum length.
	/// </summary>
	[SerializeField] private int lineLength = 250;
	/// <summary>
	/// Display line maximum height.
	/// </summary>
	[SerializeField] private int lineHeight = 22;
	/// <summary>
	/// Spacing between output lines.
	/// </summary>
	[SerializeField] private int lineSpacing = 12;

	private static List<DebugMessage> messageList = new List<DebugMessage>();
	private static Queue<DebugMessage> bottomRightMsgLog = new Queue<DebugMessage>();
	private static Queue<DebugMessage> bottomLeftMsgLog = new Queue<DebugMessage>();

    void Start() {
		if (defaultSkin != null) {
			GUI.skin = defaultSkin;
		} else {
			GUI.skin = new GUISkin(); //??
		}
		if (defaultStyle == null) {
			defaultStyle = new GUIStyle(GUI.skin.label);
		}
        StartCoroutine(ClearMessages());
    }

	void OnValidate() {
		if (maxLogLinesLeft < 1) { maxLogLinesLeft = defaultLogLength; }
		if (maxLogLinesRight < 1) { maxLogLinesRight = defaultLogLength; }
		if (lineHeight < 1) { lineHeight = defaultLineHeight; }
		if (lineLength < 1) { lineLength = defaultLineLength; }
		if (lineSpacing < 1) { lineSpacing = defaultLineSpacing; }
	}

	/// <summary>
	/// Output the message to the default location of top left corner with a default style.
	/// </summary>
	/// <seealso cref="Message(side, msgText)"/>
	/// <param name="msgText">Message text to be displayed.</param>
    public static void Message(string msgText) {
		Message(Sides.LEFT, msgText);
    }

	/// <summary>
	/// Output the message to the specified side of the top half of the screen with a default style.
	/// </summary>
	/// <param name="side">Output side</param>
	/// <param name="msgText">Message text to be displayed.</param>
	public static void Message(Sides side, string msgText) {
		Message(side, msgText, defaultStyle.normal.textColor, defaultStyle.alignment, defaultStyle.fontSize);
	}

	/// <summary>
	/// Message to the specified side with custom fontColor, alignment / anchor and fontSize.
	/// </summary>
	/// <param name="side">Side.</param>
	/// <param name="msgText">Message text.</param>
	/// <param name="fontColor">Font color.</param>
	/// <param name="alignment">alignment / anchor.</param>
	/// <param name="fontSize">Font size.</param>
	public static void Message(Sides side, string msgText, Color fontColor, TextAnchor alignment, int fontSize) {
		messageList.Add(new DebugMessage(side, msgText, defaultStyle));
	}

	/// <summary>
	/// Message to the specified side with custom fontColor and alignment / anchor.
	/// </summary>
	/// <param name="side">Side.</param>
	/// <param name="msgText">Message text.</param>
	/// <param name="fontColor">Font color.</param>
	/// <param name="alignment">alignment / anchor.</param>
	public static void Message(Sides side, string msgText, Color fontColor, TextAnchor alignment) {
		Message(side, msgText, fontColor, alignment, defaultStyle.fontSize);
	}

	/// <summary>
	/// Message to the specified side with custom fontColor.
	/// </summary>
	/// <param name="side">Side.</param>
	/// <param name="msgText">Message text.</param>
	/// <param name="fontColor">Font color.</param>
	public static void Message(Sides side, string msgText, Color fontColor) {
		Message(side, msgText, fontColor, defaultStyle.alignment, defaultStyle.fontSize);
	}

	/// <summary>
	/// Message to the specified side with custom fontColor and fontSize.
	/// </summary>
	/// <param name="side">Side.</param>
	/// <param name="msgText">Message text.</param>
	/// <param name="fontColor">Font color.</param>
	/// <param name="fontSize">Font size.</param>
	public static void Message(Sides side, string msgText, Color fontColor, int fontSize) {
		Message(side, msgText, fontColor, defaultStyle.alignment, fontSize);
	}

	/// <summary>
	/// Message to the specified side with custom alignment / anchor.
	/// </summary>
	/// <param name="side">Side.</param>
	/// <param name="msgText">Message text.</param>
	/// <param name="alignment">alignment / anchor.</param>
	public static void Message(Sides side, string msgText, TextAnchor alignment) {
		Message(side, msgText, defaultStyle.normal.textColor, alignment, defaultStyle.fontSize);
	}

	/// <summary>
	/// Message to the specified side with custom alignment / anchor and fontSize.
	/// </summary>
	/// <param name="side">Side.</param>
	/// <param name="msgText">Message text.</param>
	/// <param name="alignment">alignment / anchor.</param>
	/// <param name="fontSize">Font size.</param>
	public static void Message(Sides side, string msgText, TextAnchor alignment, int fontSize) {
		Message(side, msgText, defaultStyle.normal.textColor, alignment, fontSize);
	}

	/// <summary>
	/// Message to the specified side with custom fontSize.
	/// </summary>
	/// <param name="side">Side.</param>
	/// <param name="msgText">Message text.</param>
	/// <param name="fontSize">Font size.</param>
	public static void Message(Sides side, string msgText, int fontSize) {
		Message(side, msgText, defaultStyle.normal.textColor, defaultStyle.alignment, fontSize);
	}

	/// <summary>
	/// Log message to the specified side with custom fontColor, alignment / anchor and fontSize.
	/// </summary>
	/// <param name="side">Side.</param>
	/// <param name="msgText">Message text.</param>
	/// <param name="fontColor">Font color.</param>
	/// <param name="alignment">alignment / anchor.</param>
	/// <param name="fontSize">Font size.</param>
	public static void Log(Sides side, string msgText, Color fontColor, TextAnchor alignment, int fontSize) {
		if (side == Sides.RIGHT) {
			if (bottomRightMsgLog.Count < maxLogLinesRight) {
				bottomRightMsgLog.Enqueue(new DebugMessage(side, msgText, defaultStyle));
			} else {
				while (bottomRightMsgLog.Count >= maxLogLinesRight) {
					bottomRightMsgLog.Dequeue();
				}
				bottomRightMsgLog.Enqueue(new DebugMessage(side, msgText, defaultStyle));
			}
		} else {
			if (bottomLeftMsgLog.Count < maxLogLinesLeft) {
				bottomLeftMsgLog.Enqueue(new DebugMessage(side, msgText, defaultStyle));
			} else {
				while (bottomLeftMsgLog.Count >= maxLogLinesLeft) {
					bottomLeftMsgLog.Dequeue();
				}
				bottomLeftMsgLog.Enqueue(new DebugMessage(side, msgText, defaultStyle));
			}
		}
	}

	/// <summary>
	/// Log message to the specified side with default style.
	/// </summary>
	/// <param name="side">Side.</param>
	/// <param name="msgText">Message text.</param>
	public static void Log(Sides side, string msgText) {
		Log(side, msgText, defaultStyle.normal.textColor, defaultStyle.alignment, defaultStyle.fontSize);
	}

	/// <summary>
	/// Log message to the specified side with custom fontColor.
	/// </summary>
	/// <param name="side">Side.</param>
	/// <param name="msgText">Message text.</param>
	/// <param name="fontColor">Font color.</param>
	public static void Log(Sides side, string msgText, Color fontColor) {
		Log(side, msgText, fontColor, defaultStyle.alignment, defaultStyle.fontSize);
	}

	/// <summary>
	/// Log message to the specified side with custom fontColor and alignment / anchor.
	/// </summary>
	/// <param name="side">Side.</param>
	/// <param name="msgText">Message text.</param>
	/// <param name="fontColor">Font color.</param>
	/// <param name="alignment">alignment / anchor.</param>
	public static void Log(Sides side, string msgText, Color fontColor, TextAnchor alignment) {
		Log(side, msgText, fontColor, alignment, defaultStyle.fontSize);
	}

	/// <summary>
	/// Log message to the specified side with custom fontColor and fontSize.
	/// </summary>
	/// <param name="side">Side.</param>
	/// <param name="msgText">Message text.</param>
	/// <param name="fontColor">Font color.</param>
	/// <param name="fontSize">Font size.</param>
	public static void Log(Sides side, string msgText, Color fontColor, int fontSize) {
		Log(side, msgText, fontColor, defaultStyle.alignment, fontSize);
	}

	/// <summary>
	/// Log message to the specified side with custom alignment / anchor.
	/// </summary>
	/// <param name="side">Side.</param>
	/// <param name="msgText">Message text.</param>
	/// <param name="alignment">alignment / anchor.</param>
	public static void Log(Sides side, string msgText, TextAnchor alignment) {
		Log(side, msgText, defaultStyle.normal.textColor, alignment, defaultStyle.fontSize);
	}

	/// <summary>
	/// Log message to the specified side with custom alignment / anchor and fontSize.
	/// </summary>
	/// <param name="side">Side.</param>
	/// <param name="msgText">Message text.</param>
	/// <param name="alignment">alignment / anchor.</param>
	/// <param name="fontSize">Font size.</param>
	public static void Log(Sides side, string msgText, TextAnchor alignment, int fontSize) {
		Log(side, msgText, defaultStyle.normal.textColor, alignment, fontSize);
	}

	/// <summary>
	/// Log message to the specified side with custom fontSize.
	/// </summary>
	/// <param name="side">Side.</param>
	/// <param name="msgText">Message text.</param>
	/// <param name="fontSize">Font size.</param>
	public static void Log(Sides side, string msgText, int fontSize) {
		Log(side, msgText, defaultStyle.normal.textColor, defaultStyle.alignment, fontSize);
	}

    void OnGUI() {
        if (messageList != null && messageList.Count > 0) {
			int loopCount_TR = 0; //top right output's count
			int loopCount_TL = 0; //top left output's count
			Rect area = new Rect();
			foreach(DebugMessage msg in messageList) {
				if (msg.side == Sides.LEFT) {
					area = new Rect(0, loopCount_TL++ * lineSpacing, lineLength, lineHeight);
				} else if (msg.side == Sides.RIGHT) {
					area = new Rect(Screen.width - lineLength, loopCount_TR++ * lineSpacing, lineLength, lineHeight);
				}
				GUI.Label(area, msg.message, msg.style);
			}
        }

		if (bottomRightMsgLog != null && bottomRightMsgLog.Count > 0) {
			int loopCount = 0;
			Rect area = new Rect();

			foreach(DebugMessage msg in bottomRightMsgLog) {
				area = new Rect(Screen.width - lineLength, (Screen.height - (maxLogLinesLeft * lineHeight)) + (loopCount++ * lineHeight), lineLength, lineHeight);
				GUI.Label(area, msg.message, msg.style);
			}
		}

		if (bottomLeftMsgLog != null && bottomLeftMsgLog.Count > 0) {
			int loopCount = 0;
			Rect area = new Rect();
			
			foreach(DebugMessage msg in bottomLeftMsgLog) {
				area = new Rect(0, (Screen.height - (maxLogLinesLeft * lineHeight)) + (loopCount++ * lineHeight), lineLength, lineHeight);
				GUI.Label(area, msg.message, msg.style);
			}
		}
    }

    IEnumerator ClearMessages() {
		while(true) {
			yield return new WaitForEndOfFrame();
			messageList.Clear();
		}
    }
}
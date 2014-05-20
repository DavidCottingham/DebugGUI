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
	public enum MessageArea { TOP_LEFT = 0, TOP_RIGHT = 1, BOTTOM_LEFT = 2, BOTTOM_RIGHT = 3 };
	public enum AreaType { MESSAGE, LOG };

	///<summary>
	/// Grouping struct for the message, its corresponding side's area/log to be displayed on, and a custom style.
	/// </summary>
	private struct DebugMessage {
		public MessageArea area;
		public string message;
		public Color fontColor;
		public TextAnchor alignment;
		public int fontSize;

		public DebugMessage(MessageArea area, string message, Color fontColor, TextAnchor alignment, int fontSize) {
			this.area = area;
			this.message = message;
			this.fontSize = fontSize;
			this.alignment = alignment;
			this.fontColor = fontColor;
		}

		public DebugMessage(MessageArea area, string message, GUIStyle style) : this(area, message, style.normal.textColor, style.alignment, style.fontSize) {}
	}

	private static DebugGUI instance;
	public static DebugGUI Instance { 
		get {
			if (instance == null) {
				instance = FindObjectOfType(typeof(DebugGUI)) as DebugGUI;
			}
			return instance;
		} private set { }
	}

	/// <summary>
	/// The default <see cref="label"/> style can be overridden here. The label of the <see cref="defaultSkin"/> will get overridden too, if this is set.
	/// </summary>
	public GUIStyle sharedStyle;
	/// <summary>
	/// Custom skin to be used for all output.
	/// </summary>
	public GUISkin sharedSkin;

	//public Color defaultColor = Color.black;
	//public TextAnchor defaultAlignment;
	//public int defaultFontSize = 12;

	public TextAnchor rightSideAlign = TextAnchor.UpperRight;
	public TextAnchor leftSideAlign = TextAnchor.UpperLeft;

	private int defaultLogLength = 8;
	private int defaultLineLength = 250;
	private int defaultLineHeight = 22;
	private int defaultLineSpacing = 14;

	private GUIStyle msgStyle;
	private int loopCount_TR;
	private int loopCount_TL;
	private int loopCount;
	private Rect msgArea;

	/// <summary>
	/// The maximum number of lines in the right side log.
	/// </summary>
	public int maxLogLinesRight = 8;
	/// <summary>
	/// The Maximum number of lines in the left side log.
	/// </summary>
	public int maxLogLinesLeft = 8;

	/// <summary>
	/// Display line maximum length.
	/// </summary>
	public int lineLength = 250;
	/// <summary>
	/// Display line maximum height.
	/// </summary>
	public int lineHeight = 22;
	/// <summary>
	/// Spacing between output lines.
	/// </summary>
	public int lineSpacing = 14;

	//private List<DebugMessage> messageList = new List<DebugMessage>();
	//private Queue<DebugMessage> bottomRightMsgLog = new Queue<DebugMessage>();
	//private Queue<DebugMessage> bottomLeftMsgLog = new Queue<DebugMessage>();

	private OutputAreaSettings[] outputs = new OutputAreaSettings[4];

	private OutputAreaSettings topLeft = new OutputAreaSettings();
	private OutputAreaSettings topRight = new OutputAreaSettings();
	private OutputAreaSettings bottomLeft = new OutputAreaSettings();
	private OutputAreaSettings bottomRight = new OutputAreaSettings();

    void Start() {
        StartCoroutine(ClearMessages());
		outputs[(int) MessageArea.TOP_LEFT] = topLeft;
		outputs[(int) MessageArea.TOP_RIGHT] = topRight;
		outputs[(int) MessageArea.BOTTOM_LEFT] = bottomLeft;
		outputs[(int) MessageArea.BOTTOM_RIGHT] = bottomRight;
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
    public void Message(string msgText) {
		Message(MessageArea.TOP_LEFT, msgText);
    }

	/// <summary>
	/// Output the message to the specified side of the top half of the screen with a default style.
	/// </summary>
	/// <param name="side">Output side</param>
	/// <param name="msgText">Message text to be displayed.</param>
	public void Message(MessageArea area, string msgText) {
		Message(area, msgText, sharedStyle.normal.textColor, (area == MessageArea.TOP_LEFT ? leftSideAlign : rightSideAlign), sharedStyle.fontSize);
		//Message(side, msgText, defaultColor, defaultAlignment, defaultFontSize);
	}

	/// <summary>
	/// Message to the specified side with custom fontColor, alignment / anchor and fontSize.
	/// </summary>
	/// <param name="side">Side.</param>
	/// <param name="msgText">Message text.</param>
	/// <param name="fontColor">Font color.</param>
	/// <param name="alignment">alignment / anchor.</param>
	/// <param name="fontSize">Font size.</param>
	public void Message(MessageArea area, string msgText, Color fontColor, TextAnchor alignment, int fontSize) {
		messageList.Add(new DebugMessage(area, msgText, fontColor, alignment, fontSize));
	}

	/// <summary>
	/// Message to the specified side with custom fontColor and alignment / anchor.
	/// </summary>
	/// <param name="side">Side.</param>
	/// <param name="msgText">Message text.</param>
	/// <param name="fontColor">Font color.</param>
	/// <param name="alignment">alignment / anchor.</param>
	public void Message(MessageArea area, string msgText, Color fontColor, TextAnchor alignment) {
		//Message(side, msgText, fontColor, alignment, defaultFontSize);
		Message(area, msgText, fontColor, alignment, sharedStyle.fontSize);
	}

	/// <summary>
	/// Message to the specified side with custom fontColor.
	/// </summary>
	/// <param name="side">Side.</param>
	/// <param name="msgText">Message text.</param>
	/// <param name="fontColor">Font color.</param>
	public void Message(MessageArea area, string msgText, Color fontColor) {
		//Message(side, msgText, fontColor, defaultAlignment, defaultFontSize);
		Message(area, msgText, fontColor, (area == MessageArea.TOP_LEFT ? leftSideAlign : rightSideAlign), sharedStyle.fontSize);
	}

	/// <summary>
	/// Message to the specified side with custom fontColor and fontSize.
	/// </summary>
	/// <param name="side">Side.</param>
	/// <param name="msgText">Message text.</param>
	/// <param name="fontColor">Font color.</param>
	/// <param name="fontSize">Font size.</param>
	public void Message(MessageArea area, string msgText, Color fontColor, int fontSize) {
		//Message(side, msgText, fontColor, defaultAlignment, fontSize);
		Message(area, msgText, fontColor, (area == MessageArea.TOP_LEFT ? leftSideAlign : rightSideAlign), fontSize);
	}

	/// <summary>
	/// Message to the specified side with custom alignment / anchor.
	/// </summary>
	/// <param name="side">Side.</param>
	/// <param name="msgText">Message text.</param>
	/// <param name="alignment">alignment / anchor.</param>
	public void Message(MessageArea area, string msgText, TextAnchor alignment) {
		//Message(side, msgText, defaultColor, alignment, defaultFontSize);
		Message(area, msgText, sharedStyle.normal.textColor, alignment, sharedStyle.fontSize);
	}

	/// <summary>
	/// Message to the specified side with custom alignment / anchor and fontSize.
	/// </summary>
	/// <param name="side">Side.</param>
	/// <param name="msgText">Message text.</param>
	/// <param name="alignment">alignment / anchor.</param>
	/// <param name="fontSize">Font size.</param>
	public void Message(MessageArea area, string msgText, TextAnchor alignment, int fontSize) {
		//Message(side, msgText, defaultColor, alignment, fontSize);
		Message(area, msgText, sharedStyle.normal.textColor, alignment, fontSize);
	}

	/// <summary>
	/// Message to the specified side with custom fontSize.
	/// </summary>
	/// <param name="side">Side.</param>
	/// <param name="msgText">Message text.</param>
	/// <param name="fontSize">Font size.</param>
	public void Message(MessageArea area, string msgText, int fontSize) {
		//Message(side, msgText, defaultColor, defaultAlignment, fontSize);
		Message(area, msgText, sharedStyle.normal.textColor, (area == MessageArea.TOP_LEFT ? leftSideAlign : rightSideAlign), fontSize);
	}

	/// <summary>
	/// Log message to the specified side with custom fontColor, alignment / anchor and fontSize.
	/// </summary>
	/// <param name="side">Side.</param>
	/// <param name="msgText">Message text.</param>
	/// <param name="fontColor">Font color.</param>
	/// <param name="alignment">alignment / anchor.</param>
	/// <param name="fontSize">Font size.</param>
	public void Log(MessageArea area, string msgText, Color fontColor, TextAnchor alignment, int fontSize) {
		if (area == MessageArea.TOP_RIGHT) {
			if (bottomRightMsgLog.Count < maxLogLinesRight) {
				bottomRightMsgLog.Enqueue(new DebugMessage(area, msgText, fontColor, alignment, fontSize));
			} else {
				while (bottomRightMsgLog.Count >= maxLogLinesRight) {
					bottomRightMsgLog.Dequeue();
				}
				bottomRightMsgLog.Enqueue(new DebugMessage(area, msgText, fontColor, alignment, fontSize));
			}
		} else {
			if (bottomLeftMsgLog.Count < maxLogLinesLeft) {
				bottomLeftMsgLog.Enqueue(new DebugMessage(area, msgText, fontColor, alignment, fontSize));
			} else {
				while (bottomLeftMsgLog.Count >= maxLogLinesLeft) {
					bottomLeftMsgLog.Dequeue();
				}
				bottomLeftMsgLog.Enqueue(new DebugMessage(area, msgText, fontColor, alignment, fontSize));
			}
		}
	}

	/// <summary>
	/// Log message to the specified side with default style.
	/// </summary>
	/// <param name="side">Side.</param>
	/// <param name="msgText">Message text.</param>
	public void Log(MessageArea area, string msgText) {
		Log(area, msgText, sharedStyle.normal.textColor, (area == MessageArea.TOP_LEFT ? leftSideAlign : rightSideAlign), sharedStyle.fontSize);
	}

	/// <summary>
	/// Log message to the specified side with custom fontColor.
	/// </summary>
	/// <param name="side">Side.</param>
	/// <param name="msgText">Message text.</param>
	/// <param name="fontColor">Font color.</param>
	public void Log(MessageArea area, string msgText, Color fontColor) {
		Log(area, msgText, fontColor, (area == MessageArea.TOP_LEFT ? leftSideAlign : rightSideAlign), sharedStyle.fontSize);
	}

	/// <summary>
	/// Log message to the specified side with custom fontColor and alignment / anchor.
	/// </summary>
	/// <param name="side">Side.</param>
	/// <param name="msgText">Message text.</param>
	/// <param name="fontColor">Font color.</param>
	/// <param name="alignment">alignment / anchor.</param>
	public void Log(MessageArea area, string msgText, Color fontColor, TextAnchor alignment) {
		Log(area, msgText, fontColor, alignment, sharedStyle.fontSize);
	}

	/// <summary>
	/// Log message to the specified side with custom fontColor and fontSize.
	/// </summary>
	/// <param name="side">Side.</param>
	/// <param name="msgText">Message text.</param>
	/// <param name="fontColor">Font color.</param>
	/// <param name="fontSize">Font size.</param>
	public void Log(MessageArea area, string msgText, Color fontColor, int fontSize) {
		Log(area, msgText, fontColor, (area == MessageArea.TOP_LEFT ? leftSideAlign : rightSideAlign), fontSize);
	}

	/// <summary>
	/// Log message to the specified side with custom alignment / anchor.
	/// </summary>
	/// <param name="side">Side.</param>
	/// <param name="msgText">Message text.</param>
	/// <param name="alignment">alignment / anchor.</param>
	public void Log(MessageArea area, string msgText, TextAnchor alignment) {
		Log(area, msgText, sharedStyle.normal.textColor, alignment, sharedStyle.fontSize);
	}

	/// <summary>
	/// Log message to the specified side with custom alignment / anchor and fontSize.
	/// </summary>
	/// <param name="side">Side.</param>
	/// <param name="msgText">Message text.</param>
	/// <param name="alignment">alignment / anchor.</param>
	/// <param name="fontSize">Font size.</param>
	public void Log(MessageArea area, string msgText, TextAnchor alignment, int fontSize) {
		Log(area, msgText, sharedStyle.normal.textColor, alignment, fontSize);
	}

	/// <summary>
	/// Log message to the specified side with custom fontSize.
	/// </summary>
	/// <param name="side">Side.</param>
	/// <param name="msgText">Message text.</param>
	/// <param name="fontSize">Font size.</param>
	public void Log(MessageArea area, string msgText, int fontSize) {
		Log(area, msgText, sharedStyle.normal.textColor, (area == MessageArea.TOP_LEFT ? leftSideAlign : rightSideAlign), fontSize);
	}

    void OnGUI() {
		if (sharedSkin != null) {
			GUI.skin = sharedSkin;
			sharedStyle = sharedSkin.GetStyle("label");
		}

        if (messageList != null && messageList.Count > 0) {
			loopCount_TR = 0; //top right output's count
			loopCount_TL = 0; //top left output's count
			msgArea = new Rect();
			foreach(DebugMessage msg in messageList) {
				msgStyle = new GUIStyle();
				msgStyle.normal.textColor = msg.fontColor;
				msgStyle.fontSize = msg.fontSize;
				msgStyle.alignment = msg.alignment;

				if (msg.area == MessageArea.TOP_LEFT) {
					msgArea = new Rect(0, loopCount_TL++ * lineSpacing, lineLength, lineHeight);
				} else if (msg.area == MessageArea.TOP_RIGHT) {
					msgArea = new Rect(Screen.width - lineLength, loopCount_TR++ * lineSpacing, lineLength, lineHeight);
					msgStyle.alignment = rightSideAlign;
					if (msg.alignment != rightSideAlign) {
						msgStyle.alignment = msg.alignment;
					}
				}
				GUI.Label(msgArea, msg.message, msgStyle);
			}
        }

		if (bottomRightMsgLog != null && bottomRightMsgLog.Count > 0) {
			loopCount = 0;
			msgArea = new Rect();

			foreach(DebugMessage msg in bottomRightMsgLog) {
				msgStyle = new GUIStyle();
				msgStyle.normal.textColor = msg.fontColor;
				msgStyle.fontSize = msg.fontSize;

				msgStyle.alignment = rightSideAlign;
				msgStyle.alignment = msg.alignment;

				msgArea = new Rect(Screen.width - lineLength, (Screen.height - (maxLogLinesLeft * lineSpacing)) + (loopCount++ * lineSpacing), lineLength, lineHeight);
				GUI.Label(msgArea, msg.message, msgStyle);
			}
		}

		if (bottomLeftMsgLog != null && bottomLeftMsgLog.Count > 0) {
			loopCount = 0;
			msgArea = new Rect();
			
			foreach(DebugMessage msg in bottomLeftMsgLog) {
				msgStyle = new GUIStyle();
				msgStyle.normal.textColor = msg.fontColor;
				msgStyle.fontSize = msg.fontSize;
				msgStyle.alignment = leftSideAlign;
				msgStyle.alignment = msg.alignment;

				msgArea = new Rect(0, (Screen.height - (maxLogLinesLeft * lineSpacing)) + (loopCount++ * lineSpacing), lineLength, lineHeight);
				GUI.Label(msgArea, msg.message, msgStyle);
			}
		}
		//print(defaultStyle.normal.textColor.ToString());
    }

    IEnumerator ClearMessages() {
		while(true) {
			yield return new WaitForEndOfFrame();
			messageList.Clear();
		}
    }

	private class OutputAreaSettings {

		private ICollection messages;
		private MessageArea area; //needed in class?
		public MessageArea Area { get { return area; } }
		public GUIStyle msgStyle;

		/// <summary>
		/// The maximum number of lines in the right side log.
		/// </summary>
		public int maxLogLinesRight = 8;
		/// <summary>
		/// The Maximum number of lines in the left side log.
		/// </summary>
		public int maxLogLinesLeft = 8;
		
		/// <summary>
		/// Display line maximum length.
		/// </summary>
		public int lineLength = 250;
		/// <summary>
		/// Display line maximum height.
		/// </summary>
		public int lineHeight = 22;
		/// <summary>
		/// Spacing between output lines.
		/// </summary>
		public int lineSpacing = 14;

		public OutputAreaSettings(AreaType type) {
			switch (type) {
			case AreaType.LOG :
				messages = new Queue<DebugMessage>();
				break;
			case AreaType.MESSAGE :
				messages = new List<DebugMessage>();
				break;
			default :
				messages = new List<DebugMessage>();
				break;
			}
		}

		public OutputAreaSettings() : this(AreaType.MESSAGE) {}

		public void ChangeType(AreaType type) {
			if (type == AreaType.MESSAGE && messages.GetType(typeof(Queue))) {
				messages = new List<DebugMessage>(messages);
			} else if (type == AreaType.LOG && messages.GetType(typeof(System.Collections.Generic.List))) {
				messages = new Queue<DebugMessage>(messages);
			}
		}
	}
}
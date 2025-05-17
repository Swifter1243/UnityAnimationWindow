using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditorInternal.Enemeteen;
using UnityEngine;

[System.Serializable]
class AnimationToolWindow
{
	[SerializeField] private AnimationWindowState m_state;

	private Vector2 m_ScrollPosition = Vector2.zero;
	private Dictionary<string, AnimationTool> m_Tools;
	private string[] m_ToolsKeys;
	private int m_currentTool = 0;

	private GUIStyle s_MainTitleStyle => new GUIStyle
	{
		alignment = TextAnchor.MiddleCenter,
		fontStyle = FontStyle.Bold,
		normal =
		{
			textColor = Color.white,
			background = Texture2D.grayTexture
		},
		fixedHeight = 20
	};

	private GUIStyle s_ErrorTextStyle => new GUIStyle
	{
		alignment = TextAnchor.MiddleCenter,
		fontStyle = FontStyle.Bold,
		normal =
		{
			textColor = Color.red,
		},
		wordWrap = true,
	};

	public void Setup(AnimationWindowState state)
	{
		m_state = state;
		m_Tools = new Dictionary<string, AnimationTool>
		{
			["Rename Object"] = new RenameObjectTool()
		};
		m_ToolsKeys = m_Tools.Keys.ToArray();
	}

	public void OnGUI()
	{
		GUILayout.Label("Animation Tools", s_MainTitleStyle);
		m_ScrollPosition = GUILayout.BeginScrollView(m_ScrollPosition);
		GUILayout.Space(10);
		m_currentTool = EditorGUILayout.Popup(m_currentTool, m_ToolsKeys);
		GUILayout.FlexibleSpace();

		AnimationTool selectedTool = m_Tools[m_ToolsKeys[m_currentTool]];
		if (!selectedTool.ValidateReady(m_state, out string error))
		{
			GUILayout.BeginHorizontal();
			GUILayout.Space(20);
			GUILayout.Label(error, s_ErrorTextStyle);
			GUILayout.Space(20);
			GUILayout.EndHorizontal();
		}
		else if (GUILayout.Button("Run", GUILayout.Height(30))) // TODO: Validate run
		{
			selectedTool.Run(m_state);
		}
		GUILayout.Space(10);
		GUILayout.EndScrollView();
	}
}

using UnityEditor;
using UnityEditorInternal.Enemeteen;
using UnityEngine;
class RenameObjectTool : AnimationTool
{
	private string m_NewName = "";

	public override bool ValidateReady(AnimationWindowState state, out string errorMessage)
	{
		errorMessage = string.Empty;

		if (Selection.objects.Length == 0)
		{
			errorMessage = "Please select an object first";
			return false;
		}

		if (Selection.objects.Length > 1)
		{
			errorMessage = "Too many objects selected, please only select one.";
			return false;
		}

		if (m_NewName == string.Empty)
		{
			errorMessage = "Field 'New Name' cannot be empty.";
			return false;
		}

		return true;
	}

	public override void Run(AnimationWindowState state)
	{
		throw new System.NotImplementedException();
	}

	public override void OnGUI()
	{
		GUILayout.Space(10);
		m_NewName = EditorGUILayout.TextField("New Name", m_NewName);
	}
}

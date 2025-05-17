using System;
using UnityEditor;
using UnityEditorInternal.Enemeteen;
using UnityEngine;
class RenameObjectTool : AnimationTool
{
	private string m_NewName = "";

	public override bool ValidateReady(AnimationWindowState state, out string errorMessage)
	{
		errorMessage = string.Empty;

		if (Selection.gameObjects.Length == 0)
		{
			errorMessage = "Please select an object first";
			return false;
		}

		if (Selection.gameObjects.Length > 1)
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
		var clip = state.activeAnimationClip;
		var rootObject = state.activeRootGameObject;
		var currentObject = Selection.activeGameObject;
		string oldPath = AnimationUtility.CalculateTransformPath(currentObject.transform, rootObject.transform);
		currentObject.name = m_NewName;
		string newPath = AnimationUtility.CalculateTransformPath(currentObject.transform, rootObject.transform);

		var bindings = AnimationUtility.GetCurveBindings(clip);
		for (int i = 0; i < bindings.Length; i++)
		{
			var binding = bindings[i];

			int pathIndex = binding.path.IndexOf(oldPath, StringComparison.Ordinal);
			if (pathIndex == 0)
			{
				string newBindingPath = newPath + binding.path.Substring(oldPath.Length);
				binding.path = newBindingPath;

				AnimationCurve curve = AnimationUtility.GetEditorCurve(clip, bindings[i]);
				AnimationUtility.SetEditorCurve(clip, bindings[i], null);
				AnimationUtility.SetEditorCurve(clip, binding, curve);
			}
		}
	}

	public override void OnGUI()
	{
		GUILayout.Space(10);
		m_NewName = EditorGUILayout.TextField("New Name", m_NewName);
	}
}

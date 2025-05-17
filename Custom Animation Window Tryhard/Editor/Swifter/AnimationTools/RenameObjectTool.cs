using UnityEditor;
using UnityEditorInternal.Enemeteen;
class RenameObjectTool : AnimationTool
{
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

		return true;
	}

	public override void Run(AnimationWindowState state)
	{
		throw new System.NotImplementedException();
	}
}

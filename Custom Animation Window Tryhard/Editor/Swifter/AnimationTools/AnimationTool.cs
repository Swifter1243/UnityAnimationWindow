using UnityEditorInternal.Enemeteen;

abstract class AnimationTool
{
	public abstract bool ValidateReady(AnimationWindowState state, out string errorMessage);
	public abstract void Run(AnimationWindowState state);
	public abstract void OnGUI();
}

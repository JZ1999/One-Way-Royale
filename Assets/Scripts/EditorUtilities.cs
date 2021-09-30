using UnityEngine;

public class EditorUtilities : MonoBehaviour
{
	/// Add a context menu named "Do Something" in the inspector
	/// of the attached script.
	[ContextMenu("Erase PlayerPrefs")]
	public void DoSomething()
	{
		Debug.Log("PlayerPrefs eliminated");
		PlayerPrefs.DeleteAll();
	}
}
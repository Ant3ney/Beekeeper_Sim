using UnityEngine;

public static class GetDialogSystem
{
	public static void RunDialog(string identifier, DialogSettings dialogSettings) { 
		Debug.Log("Dialog Getter: " + identifier);
		GameObject dialogSystemObj = FindDialogObject();
		dialogSystemObj.SetActive(true);
		DialogSystem dialogSystem = dialogSystemObj.GetComponent<DialogSystem>();

		//GameObject.Find("DialogSystem").GetComponent<DialogSystem>();
		dialogSystem.RunDialog(identifier, dialogSettings);
	}

	 public static GameObject FindDialogObject() {
		GameObject dialogSystemObj = null;

		foreach (var obj in Resources.FindObjectsOfTypeAll<GameObject>())
		{
			if (obj.name == "DialogSystem")
			{
				return obj;
			}
		}

		return null;
	}
}

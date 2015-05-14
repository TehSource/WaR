using UnityEngine;
using System.Collections;

public class LoadingScreen : MonoBehaviour {

	public Texture2D texture;
	public int loadingScreenExtraDuration = 0;
	
	public static bool sceneReady = true;
	float loadingScreenExpiry = -1;

	// Use this for initialization
	void Start () {
		EventSystem.RegisterEvent("LOADING_SCENE_START", this);
		EventSystem.RegisterEvent("LOADING_SCENE_END", this);
		EventSystem.RegisterEvent("PLAYER_TELEPORTED", this);
	}
	
	// Update is called once per frame
	void Update () {
		if (loadingScreenExpiry < Time.time) {
			sceneReady = true;
		}
	}
	
	void OnGUI() 
	{
		if (!sceneReady) {
			GUI.depth = 1;
			Rect rect = new Rect(0, 0, Screen.width, Screen.height);
			GUI.DrawTexture(rect, texture);
		}
	}

	void OnLevelWasLoaded (int level) {
		//guiTexture.enabled = false;
		loadingScreenExpiry = Time.time + loadingScreenExtraDuration;
	}

	public void OnEvent(EventData eData) {
		if (eData.eventType == "LOADING_SCENE_START") {
			sceneReady = false;
			Logger.LogDebugMessage("Showing loading screen");
		}
		if (eData.eventType == "LOADING_SCENE_END") {
			//showLoadingScreen = false;
			guiTexture.enabled = false;
			Logger.LogDebugMessage("Hiding loading screen");
		}
		if (eData.eventType == "PLAYER_TELEPORTED") {
			Logger.LogDebugMessage("Got player teleport");
			sceneReady = false;
			loadingScreenExpiry = Time.time + loadingScreenExtraDuration;
		}
	}
}

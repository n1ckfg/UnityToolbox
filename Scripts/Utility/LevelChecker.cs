using UnityEngine;
using System.Collections;

public class LevelChecker : MonoBehaviour {

	public enum CurrentScene { LOADER, MAIN, HALLWAY, HAPPYBALL };
	public CurrentScene currentScene = CurrentScene.LOADER;

	void OnLevelWasLoaded(int level) {
		currentScene = (CurrentScene) level;
	}

	public void setLevel() {
		currentScene = (CurrentScene) Application.loadedLevel;
	}

}

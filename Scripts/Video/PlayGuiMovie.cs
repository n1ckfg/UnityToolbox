using UnityEngine;
using System.Collections;

public class PlayGuiMovie : MonoBehaviour {

	public bool loop = true;
    public bool playOnStart = true;

    [HideInInspector] MovieTexture mov;

    private GUITexture guiTex;

    private void Awake() {
        guiTex = GetComponent<GUITexture>();
    }

    private void Start () {
		mov = (MovieTexture) guiTex.texture;
		mov.loop=loop;

        if (playOnStart) mov.Play();	
	}
	
}

using UnityEngine;
using System.Collections;

public class TriggerPortal : MonoBehaviour {

	public float delay = 2f;
	public TriggerZone triggerZone;
    public Collider col;
    public AudioSource aud;

    private float markTriggerTime = 0f;
	private float markLookAtTime = 0f;
	private SceneLoader sceneLoader;
	private BasicController ctl;

	void Start() {
		sceneLoader = GameObject.FindGameObjectWithTag("Loader").GetComponent<SceneLoader>();
	}

	void Update() {
		RaycastHit hit;
        bool isLookedAt = ctl.isLookingAt == gameObject.name;

		if (isLookedAt && markLookAtTime == 0f) {
			markLookAtTime = Time.realtimeSinceStartup;
		}

		if (triggerZone.trigger && markTriggerTime == 0f) { 
			markTriggerTime = Time.realtimeSinceStartup; 
		}

		if (isLookedAt && triggerZone.trigger && Time.realtimeSinceStartup > markTriggerTime + delay && Time.realtimeSinceStartup > markLookAtTime + delay) {
			sceneLoader.sceneActivate();
		}
	}

}

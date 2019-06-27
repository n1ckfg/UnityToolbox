using UnityEngine;
using System.Collections;

public class ParticleControls : MonoBehaviour {

	private ParticleSystem particles;
	private AnimatorVel animatorVel;

	public float scaler = 1.0f;
	public GameObject target;

	void Awake(){
		animatorVel = target.GetComponent<AnimatorVel>();
	}

	void Start() {
		particles = (ParticleSystem) GetComponent("ParticleSystem");
	}
	
	void Update () {
		particles.startSpeed = animatorVel.vel * scaler;
		particles.startSize = animatorVel.vel * scaler / 10.0f;
		particles.emissionRate = animatorVel.vel * scaler * 10.0f;
	}
}
// 


using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class ParticleFlow : MonoBehaviour {
	public float m_Drift = 0.01f;

	private ParticleSystem m_System;
	private ParticleSystem.Particle[] m_Particles;

	private void LateUpdate() {
		InitializeIfNeeded();

		// GetParticles is allocation free because we reuse the m_Particles buffer between updates
		int numParticlesAlive = m_System.GetParticles(m_Particles);

		// Change only the particles that are alive
		for (int i = 0; i < numParticlesAlive; i++) {
			m_Particles[i].velocity += Vector3.up * m_Drift;
		}

		// Apply the particle changes to the particle system
		m_System.SetParticles(m_Particles, numParticlesAlive);
	}

	void InitializeIfNeeded() {
		if (m_System == null) {
			m_System = GetComponent<ParticleSystem> ();
		}

		if (m_Particles == null || m_Particles.Length < m_System.maxParticles) {
			m_Particles = new ParticleSystem.Particle[m_System.maxParticles]; 
		}
	}

}
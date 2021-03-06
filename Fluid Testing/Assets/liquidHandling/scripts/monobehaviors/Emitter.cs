﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LiquidHandling {
	[AddComponentMenu("Liquid Handling/Emitter")]
	public class Emitter : MonoBehaviour {

		// Public
		public Volume origin;
		public float emissionRate;
		public float emissionRandomForce;
		public GameObject liquidParticlePrefab;
		public bool stopped;
		public Liquid emission;

		// Private
		private Particle previousParticle;

		// For LiquidEmitterEditor
		public bool automatic;

		private void Awake() {

			StartCoroutine(emit());
		}

		private IEnumerator emit() {

			if(!stopped) {
				GameObject newParticle = Instantiate(liquidParticlePrefab, transform.position, Quaternion.identity);
				newParticle.GetComponent<Rigidbody>().AddRelativeForce(new Vector3(emissionRandomForce * Random.Range(-1f, 1f), 0, emissionRandomForce) * Random.Range(-1f, 1f));
				newParticle.GetComponent<Particle>().origin = this;
				newParticle.name = "liquid particle (" + name + ")";
				newParticle.GetComponent<Particle>().liquid = emission;
				if(previousParticle) {
					newParticle.GetComponent<Particle>().neighbor = previousParticle;
				}
				previousParticle = newParticle.GetComponent<Particle>();
				if(origin) {
					origin.addLiquid(-1);
				}
			}

			yield return new WaitForSeconds(1 / emissionRate);
			StartCoroutine(emit());
		}
	}
}

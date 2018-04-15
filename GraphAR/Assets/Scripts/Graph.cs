using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph : MonoBehaviour {

	public Transform pointPrefab;

	void Awake () {
		Vector3 scale = Vector3.one / 80f;
		Vector3 position;
		for (float x = -5f; x < 5f; x = x + 0.08f) {
			for (float z = 0f; z < 10f; z = z + 0.08f) {
				Transform point = Instantiate (pointPrefab);
				position.x = (x + 0.5f) / 5f - 1f;
				position.z = (z + 0.5f) / 5f - 1f;
				position.y = funcBumps(position.x, position.z);
				point.localPosition = position;
				point.localScale = scale;
			}
		}
	}

	float funcBumps(float x, float z) {
		float y = (Mathf.Sin(5f * x) * Mathf.Cos(5f * z)) / 5f;
		return y;
	}

}

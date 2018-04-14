using UnityEngine;

//@Source: http://catlikecoding.com/unity/tutorials/basics/building-a-graph/

public class Graph : MonoBehaviour {

	public Transform pointPrefab;

	void Awake () {
		Transform point = Instantiate(pointPrefab);
		point.localPosition = Vector3.right;

		//		Transform point = Instantiate(pointPrefab);
		point = Instantiate(pointPrefab);
		point.localPosition = Vector3.right * 2f;
	}
}

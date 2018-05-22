using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawAxis : MonoBehaviour {

	public Material material;
	public int size = 10;
	public int scale = 1;
	public bool posYonly = false;
	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}

	void OnPostRender() {
		RenderAxes();
	}

	void OnDrawGizmos() {
		RenderAxes();
	}

	void RenderAxes() {
		size = size * scale;
		GL.Begin(GL.LINES);
		material.SetPass(0);
		GL.Color(Color.black);
		GL.Vertex3(0,0,-size);
		GL.Vertex3(0,0,size);
		for (float i = -size; i <= size; i += 1f * scale) {
			GL.Vertex3(0,(float) 0.3,i);
			GL.Vertex3(0,(float)-0.3,i);
		}
		if (posYonly) {
			GL.Vertex3(0,0,0);
			GL.Vertex3(0,size,0);
			for (float i = 0; i <= size; i+= 1f * scale) {
				GL.Vertex3((float) 0.3,i,0);
				GL.Vertex3((float)-0.3,i,0);
			}
		} else {
			GL.Vertex3(0,-size,0);
			GL.Vertex3(0,size,0);
			for (float i = -size; i <= size; i+= 1f * scale) {
				GL.Vertex3((float) 0.3,i,0);
				GL.Vertex3((float)-0.3,i,0);
			}
		}
		GL.Vertex3(-size,0,0);
		GL.Vertex3(size,0,0);
		for (float i = -size; i <= size; i+= 1f * scale) {
			GL.Vertex3(i,(float) 0.3,0);
			GL.Vertex3(i,(float)-0.3,0);
		}
		GL.End();
	}
}

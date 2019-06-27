// http://answers.unity3d.com/questions/184442/drawing-lines-from-mouse-position.html

using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LineDrawTest : MonoBehaviour {

	public float zDepth = 1f;
	public float startWidth = 1.0f;
	public float threshold = 0.001f;

	List<Vector3> linePoints = new List<Vector3>();
	LineRenderer lineRenderer;
	float endWidth = 1.0f;
	int lineCount = 0;

	Vector3 lastPos = Vector3.one * float.MaxValue;


	void Awake() {
		lineRenderer = GetComponent<LineRenderer>();
		endWidth = startWidth;
	}

	void Update() {
		if (Input.GetMouseButton(0)) {
			Vector3 mousePos = Input.mousePosition;
			mousePos.z = zDepth;
			Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(mousePos);

			float dist = Vector3.Distance(lastPos, mouseWorld);
			if (dist <= threshold)
				return;

			lastPos = mouseWorld;
			if (linePoints == null)
				linePoints = new List<Vector3>();
			linePoints.Add(mouseWorld);
		}

		UpdateLine();
	}


	void UpdateLine() {
		lineRenderer.SetWidth(startWidth, endWidth);
		lineRenderer.SetVertexCount(linePoints.Count);

		for(int i = lineCount; i < linePoints.Count; i++) {
			lineRenderer.SetPosition(i, linePoints[i]);
		}
		lineCount = linePoints.Count;
	}

}
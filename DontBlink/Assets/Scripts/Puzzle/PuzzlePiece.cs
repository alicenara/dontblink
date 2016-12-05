using UnityEngine;
using System.Collections;

public abstract class PuzzlePiece {

	protected Transform transform;
	protected int rotation = 0;
	protected Material activeWireMaterial;
	protected Material inactiveWireMaterial;

	bool highlighted = false;
	float interpolation = 0.0f;
	float angle = 0;
	Vector3 unhighlightedPos;
	Vector3 highlightedPos;

	public PuzzlePiece(Transform transform, Material activeWireMaterial, Material inactiveWireMaterial) {
		this.transform = transform;
		this.activeWireMaterial = activeWireMaterial;
		this.inactiveWireMaterial = inactiveWireMaterial;
		highlightedPos = transform.position - transform.forward * 0.05f;
		unhighlightedPos = transform.position;
	}

	public void setHighlighted(bool highlighted) {
		this.highlighted = highlighted;
	}

	public void rotate() {
		rotation += 1;
		if (rotation > 3) {
			rotation -= 4;
			angle -= 360.0f;
		}
	}

	public void Update() {
		if (highlighted) {
			if (interpolation < 1.0f) {
				interpolation += Mathf.Min(10.0f * Time.deltaTime, 1.0f);
				transform.position = Vector3.Slerp(unhighlightedPos, highlightedPos, interpolation);
			}
		} else if (!highlighted) {
			if (interpolation > 0.0f) {
				interpolation -= Mathf.Max(10.0f * Time.deltaTime, 0.0f);
				transform.position = Vector3.Slerp(unhighlightedPos, highlightedPos, interpolation);
			}
		}
		if (angle < rotation * 90.0f) {
			float deltaAngle = 1000.0f * Time.deltaTime;
			if (angle + deltaAngle > rotation * 90.0f) {
				deltaAngle = rotation * 90.0f - angle;
			}
			angle += deltaAngle;
			transform.RotateAround (transform.position, transform.forward, deltaAngle);
		}
	}

	public abstract void setActivated (bool activated, Direction source);
	public abstract bool isValid (Direction source);
	public abstract Direction getNext (Direction source);
}

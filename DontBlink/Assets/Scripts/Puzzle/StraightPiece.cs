using System;
using UnityEngine;

public class StraightPiece: PuzzlePiece
{
	public StraightPiece(Transform transform, Material activeWireMaterial, Material inactiveWireMaterial):
	base(transform, activeWireMaterial, inactiveWireMaterial) {
	}
	public override void setActivated(bool activated, Direction source) {
		Transform wire = transform.Find ("wire");
		if (activated) {
			wire.GetComponent<Renderer>().sharedMaterial = activeWireMaterial;
		} else {
			wire.GetComponent<Renderer>().sharedMaterial = inactiveWireMaterial;
		}
	}

	public override bool isValid(Direction source) {
		if (rotation == 0 || rotation == 2) {
			return source == Direction.UP || source == Direction.DOWN;
		} else {
			return source == Direction.LEFT || source == Direction.RIGHT;
		}
	}

	public override Direction getNext(Direction source) {
		return source;
	}
}


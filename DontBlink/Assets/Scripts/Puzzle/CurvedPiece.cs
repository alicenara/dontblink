using System;
using UnityEngine;

public class CurvedPiece: PuzzlePiece
{
	public CurvedPiece(Transform transform, Material activeWireMaterial, Material inactiveWireMaterial):
	base(transform, activeWireMaterial, inactiveWireMaterial) {
	}
	public override void setActivated(bool activated, Direction source) {
		Transform wire = transform.Find ("wire_bend");
		if (activated) {
			wire.GetComponent<Renderer>().sharedMaterial = activeWireMaterial;
		} else {
			wire.GetComponent<Renderer>().sharedMaterial = inactiveWireMaterial;
		}
	}

	public override bool isValid(Direction source) {
		switch (rotation) {
		case 0:
			return source == Direction.UP || source == Direction.RIGHT;
		case 1:
			return source == Direction.LEFT || source == Direction.UP;
		case 2:
			return source == Direction.DOWN || source == Direction.LEFT;
		case 3:
			return source == Direction.RIGHT || source == Direction.DOWN;
		}
		return false;
	}

	public override Direction getNext(Direction source) {
		switch (source) {
		case Direction.DOWN:
			switch (rotation) {
			case 2:
				return Direction.RIGHT;
			case 3:
				return Direction.LEFT;
			}
			break;
		case Direction.LEFT:
			switch (rotation) {
			case 1:
				return Direction.DOWN;
			case 2:
				return Direction.UP;
			}
			break;
		case Direction.RIGHT:
			switch (rotation) {
			case 0:
				return Direction.DOWN;
			case 3:
				return Direction.UP;
			}
			break;
		case Direction.UP:
			switch (rotation) {
			case 0:
				return Direction.LEFT;
			case 1:
				return Direction.RIGHT;
			}
			break;
		}
		return Direction.NONE;
	}
}


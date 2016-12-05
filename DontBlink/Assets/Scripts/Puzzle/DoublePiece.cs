using System;
using UnityEngine;

public class DoublePiece: PuzzlePiece
{
	public DoublePiece(Transform transform, Material activeWireMaterial, Material inactiveWireMaterial):
	base(transform, activeWireMaterial, inactiveWireMaterial) {
	}
	public override void setActivated(bool activated, Direction source) {
		String name = "wire_bend1";
		switch (source) {
		case Direction.DOWN:
			if (rotation == 0 || rotation == 1) {
				name = "wire_bend2";
			}
			break;
		case Direction.UP:
			if (rotation == 2 || rotation == 3) {
				name = "wire_bend2";
			}
			break;
		case Direction.RIGHT:
			if (rotation == 1 || rotation == 2) {
				name = "wire_bend2";
			}
			break;
		case Direction.LEFT:
			if (rotation == 0 || rotation == 3) {
				name = "wire_bend2";
			}
			break;
		}
		Transform wire = transform.Find (name);
		if (activated) {
			wire.GetComponent<Renderer>().sharedMaterial = activeWireMaterial;
		} else {
			wire.GetComponent<Renderer>().sharedMaterial = inactiveWireMaterial;
		}
	}

	public override bool isValid(Direction source) {
		return true;
	}

	public override Direction getNext(Direction source) {
		if (rotation == 0 || rotation == 2) {
			switch (source) {
			case Direction.DOWN:
				return Direction.RIGHT;
			case Direction.RIGHT:
				return Direction.DOWN;
			case Direction.UP:
				return Direction.LEFT;
			case Direction.LEFT:
				return Direction.UP;
			}
		}
		else {
			switch (source) {
			case Direction.DOWN:
				return Direction.LEFT;
			case Direction.LEFT:
				return Direction.DOWN;
			case Direction.UP:
				return Direction.RIGHT;
			case Direction.RIGHT:
				return Direction.UP;
			}
		}
		return Direction.NONE;
	}
}


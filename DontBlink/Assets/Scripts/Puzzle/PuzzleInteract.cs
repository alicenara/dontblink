using UnityEngine;
using System.Collections;
using System;

public class PuzzleInteract : MonoBehaviour {
	public GameObject puzzleScreen;
	public Transform puzzleStraight;
	public Transform puzzleCurve;
	public Transform puzzleDouble;
	public Transform puzzleDoor;
	public Transform puzzleFinishWire;
	public Material activeWireMaterial;
	public Material inactiveWireMaterial;
	int w = 7;
	int h = 7;
	PuzzlePiece[,] puzzlePieces;
	bool puzzleMode = false;
	bool solved = false;
	float interpolation = 0.0f;
	int markerX = 3;
	int markerY = 3;
	int doorAngle = 0;
	Vector3 interpolationFrom;
	Vector3 interpolationTo;
	Collider interactor = null;
	ArrayList activeWires;

	// Use this for initialization
	void Start () {
		puzzlePieces = new PuzzlePiece[w, h];
		activeWires = new ArrayList ();
		Array values = Enum.GetValues(typeof(Piece));
		System.Random random = new System.Random ();
		for (int i = 0; i < w; i++) {
			for (int j = 0; j < h; j++) {
				instantiatePiece ((Piece)values.GetValue (random.Next (values.Length)), i, j);
			}
		}
		for (int i = 0; i < w; i++) {
			for (int j = 0; j < h; j++) {
				int rotations = random.Next (4);
				for (int r = 0; r < rotations; r++) {
					rotatePuzzlePiece (i, j);
				}
			}
		}
	}

	void instantiatePiece (Piece piece, int i, int j) {
		Vector3 origin = puzzleScreen.transform.position
			+ puzzleScreen.transform.up * 0.075f
			+ puzzleScreen.transform.forward * 0.30f
			- puzzleScreen.transform.right * 0.30f;
		Vector3 position = new Vector3 (i, 6 - j, 0) * 0.1f + origin;
		Transform transform;
		switch (piece) {
		case Piece.STRAIGHT:
			transform = (Transform) Instantiate (puzzleStraight, position, Quaternion.identity);
			puzzlePieces [i, j] = new StraightPiece (transform, activeWireMaterial, inactiveWireMaterial);
			break;
		case Piece.CURVE:
			transform = (Transform) Instantiate (puzzleCurve, position, Quaternion.identity);
			puzzlePieces [i, j] = new CurvedPiece (transform, activeWireMaterial, inactiveWireMaterial);
			break;
		case Piece.DOUBLE:
			transform = (Transform) Instantiate (puzzleDouble, position, Quaternion.identity);
			puzzlePieces [i, j] = new DoublePiece (transform, activeWireMaterial, inactiveWireMaterial);
			break;
		};
	}

	bool lookingAtPuzzle(Collider other) {
		return Vector3.Angle (other.transform.forward, puzzleScreen.transform.position - other.transform.position) < 20.0f;
	}
		
	void OnTriggerEnter(Collider other) {
		if (!puzzleMode) {
			if (other.tag == "Player" && lookingAtPuzzle(other)) {
				interactor = other;
			}
		}
	}

	void OnTriggerStay(Collider other) {
		if (!puzzleMode) {
			if (other.tag == "Player" && lookingAtPuzzle(other)) {
				interactor = other;
			}
		}
	}

	void moveMarkerLeft() {
		if (markerX > 0) {
			puzzlePieces [markerX, markerY].setHighlighted (false);
			markerX -= 1;
			puzzlePieces [markerX, markerY].setHighlighted (true);
		}
	}

	void moveMarkerRight() {
		if (markerX < w - 1) {
			puzzlePieces [markerX, markerY].setHighlighted (false);
			markerX += 1;
			puzzlePieces [markerX, markerY].setHighlighted (true);
		}
	}

	void moveMarkerUp() {
		if (markerY > 0) {
			puzzlePieces [markerX, markerY].setHighlighted (false);
			markerY -= 1;
			puzzlePieces [markerX, markerY].setHighlighted (true);
		}
	}

	void moveMarkerDown() {
		if (markerY < h - 1) {
			puzzlePieces [markerX, markerY].setHighlighted (false);
			markerY += 1;
			puzzlePieces [markerX, markerY].setHighlighted (true);
		}
	}

	void breakActiveWires(int i, int j) {
		Direction source = Direction.DOWN;
		int index;
		for (index = 0; index < activeWires.Count; index++) {
			PuzzlePieceIndex piece = (PuzzlePieceIndex) activeWires [index];
			if (piece.getI() == i && piece.getJ() == j) {
				break;
			}
			source = puzzlePieces [piece.getI (), piece.getJ ()].getNext (source);
		}
		for (int k = index; k < activeWires.Count; k++) {
			PuzzlePieceIndex piece = (PuzzlePieceIndex) activeWires[k];
			puzzlePieces [piece.getI(), piece.getJ()].setActivated (false, source);
			source = puzzlePieces [piece.getI (), piece.getJ ()].getNext (source);
		}
		activeWires.RemoveRange (index, activeWires.Count - index);
	}

	void updateActiveWires() {
		solved = false;
		Direction source = Direction.DOWN;
		if (activeWires.Count == 0) {
			if (puzzlePieces [0, 0].isValid (Direction.DOWN)) {
				puzzlePieces [0, 0].setActivated (true, Direction.DOWN);
				activeWires.Add (new PuzzlePieceIndex(0, 0));
			}
		}
		for (int i = 0; i < activeWires.Count; i++) {
			PuzzlePieceIndex x = (PuzzlePieceIndex) activeWires[i];
			puzzlePieces [x.getI(), x.getJ()].setActivated (true, source);
			source = puzzlePieces [x.getI (), x.getJ ()].getNext (source);
		}
		if (activeWires.Count > 0) {
			bool finished = false;
			while (!finished) {
				PuzzlePieceIndex x = (PuzzlePieceIndex)activeWires [activeWires.Count - 1];
				int i = x.getI ();
				int j = x.getJ ();
				if (source == Direction.NONE) {
					finished = true;
				}
				switch (source) {
				case Direction.DOWN:
					if (j < h - 1 && puzzlePieces [i, j + 1].isValid (source)) {
						puzzlePieces [i, j + 1].setActivated (true, source);
						activeWires.Add (new PuzzlePieceIndex (i, j + 1));
						source = puzzlePieces [i, j + 1].getNext (source);
					} else {
						finished = true;
					}
					break;
				case Direction.LEFT:
					if (i > 0 && puzzlePieces [i - 1, j].isValid (source)) {
						puzzlePieces [i - 1, j].setActivated (true, source);
						activeWires.Add (new PuzzlePieceIndex (i - 1, j));
						source = puzzlePieces [i - 1, j].getNext (source);
					} else {
						finished = true;
					}
					break;
				case Direction.RIGHT:
					if (i < w - 1 && puzzlePieces [i + 1, j].isValid (source)) {
						puzzlePieces [i + 1, j].setActivated (true, source);
						activeWires.Add (new PuzzlePieceIndex (i + 1, j));
						source = puzzlePieces [i + 1, j].getNext (source);
					} else {
						if (j == h - 1 && i == w - 1) {
							solved = true;
							puzzleFinishWire.GetComponent<Renderer>().sharedMaterial = activeWireMaterial;
						}
						finished = true;
					}
					break;
				case Direction.UP:
					if (j > 0 && puzzlePieces [i, j - 1].isValid (source)) {
						puzzlePieces [i, j - 1].setActivated (true, source);
						activeWires.Add (new PuzzlePieceIndex (i, j - 1));
						source = puzzlePieces [i, j - 1].getNext (source);
					} else {
						finished = true;
					}
					break;
				}
			}
		}
		if (!solved) {
			puzzleFinishWire.GetComponent<Renderer>().sharedMaterial = inactiveWireMaterial;
		}
	}

	void rotatePuzzlePiece(int i, int j) {
		breakActiveWires (i, j);
		puzzlePieces [i, j].rotate();
		updateActiveWires ();
	}

	private void updatePuzzlePieces() {
		for (int i = 0; i < w; i++) {
			for (int j = 0; j < h; j++) {
				puzzlePieces [i, j].Update ();
			}
		}
	}

	public bool isSolved() {
		return solved;
	}

	// Update is called once per frame
	void Update () {
		if (!puzzleMode) {
			if (doorAngle > 0) {
				doorAngle -= 20;
				puzzleDoor.RotateAround (puzzleDoor.transform.position - puzzleDoor.transform.up * 0.36f, puzzleDoor.transform.right, -20);
			}
			if (interactor != null && Input.GetKeyDown (KeyCode.E)) {
				puzzleMode = true;
				interpolation = 0.0f;
				interpolationFrom = interactor.transform.position;
				interpolationTo = transform.position;
				interpolationTo.y = interpolationFrom.y;
				puzzlePieces [markerX, markerY].setHighlighted (true);
			} else {
				interactor = null;
			}
		} else if (puzzleMode) {
			if (doorAngle < 160) {
				doorAngle += 20;
				puzzleDoor.RotateAround (puzzleDoor.transform.position - puzzleDoor.transform.up * 0.36f, puzzleDoor.transform.right, 20);
			}
			if (interpolation < 1.0f) {
				interactor.transform.position = Vector3.Slerp(interpolationFrom,
					interpolationTo,
					interpolation);
				interpolation += 5.0f * Time.deltaTime;
			} else {
				if (Input.GetKeyDown (KeyCode.E)) {
					puzzleMode = false;
					puzzlePieces [markerX, markerY].setHighlighted (false);
				} else if (Input.GetKeyDown (KeyCode.A)) {
					moveMarkerLeft ();
				} else if (Input.GetKeyDown (KeyCode.D)) {
					moveMarkerRight ();
				} else if (Input.GetKeyDown (KeyCode.W)) {
					moveMarkerUp ();
				} else if (Input.GetKeyDown (KeyCode.S)) {
					moveMarkerDown ();
				} else if (Input.GetKeyDown (KeyCode.Space)) {
					rotatePuzzlePiece (markerX, markerY);
				}
				interactor.transform.position = interpolationTo;
			}
		}
		updatePuzzlePieces ();
	}
}

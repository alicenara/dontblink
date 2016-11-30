using UnityEngine;
using System.Collections;

public class PuzzleInteract : MonoBehaviour {
	public GameObject puzzleScreen;
	public Transform puzzlePiece;
	Transform[,] puzzlePieces = new Transform[7, 7];
	bool puzzleMode = false;
	float interpolation = 0.0f;
	int markerX = 3;
	int markerY = 3;
	Vector3 interpolationFrom;
	Vector3 interpolationTo;
	Collider interactor = null;

	// Use this for initialization
	void Start () {
		Vector3 origin = puzzleScreen.transform.position
			+ puzzleScreen.transform.forward * 0.1f
			+ puzzleScreen.transform.right * 0.35f
			- puzzleScreen.transform.up * 0.35f;
		for (int i = 0; i < 7; i++) {
			for (int j = 0; j < 7; j++) {
				puzzlePieces[i, j] = (Transform) Instantiate (puzzlePiece, new Vector3 (i, 6 - j, 0) * 0.1f + origin, Quaternion.identity);
			}
		}
		puzzlePieces [markerX, markerY].transform.position -= puzzlePieces [markerX, markerY].transform.forward * 0.05f;
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
			puzzlePieces [markerX, markerY].transform.position += puzzlePieces [markerX, markerY].transform.forward * 0.05f;
			markerX -= 1;
			puzzlePieces [markerX, markerY].transform.position -= puzzlePieces [markerX, markerY].transform.forward * 0.05f;
		}
	}

	void moveMarkerRight() {
		if (markerX < 6) {
			puzzlePieces [markerX, markerY].transform.position += puzzlePieces [markerX, markerY].transform.forward * 0.05f;
			markerX += 1;
			puzzlePieces [markerX, markerY].transform.position -= puzzlePieces [markerX, markerY].transform.forward * 0.05f;
		}
	}

	void moveMarkerUp() {
		if (markerY > 0) {
			puzzlePieces [markerX, markerY].transform.position += puzzlePieces [markerX, markerY].transform.forward * 0.05f;
			markerY -= 1;
			puzzlePieces [markerX, markerY].transform.position -= puzzlePieces [markerX, markerY].transform.forward * 0.05f;
		}
	}

	void moveMarkerDown() {
		if (markerY < 6) {
			puzzlePieces [markerX, markerY].transform.position += puzzlePieces [markerX, markerY].transform.forward * 0.05f;
			markerY += 1;
			puzzlePieces [markerX, markerY].transform.position -= puzzlePieces [markerX, markerY].transform.forward * 0.05f;
		}
	}

	void rotatePuzzlePiece() {
		Transform piece = puzzlePieces [markerX, markerY];
		piece.RotateAround (piece.transform.position, piece.transform.forward, 90);
	}

	// Update is called once per frame
	void Update () {
		if (!puzzleMode) {
			if (interactor != null && Input.GetKeyDown (KeyCode.E)) {
				puzzleMode = true;
				interpolation = 0.0f;
				interpolationFrom = interactor.transform.position;
				interpolationTo = transform.position;
				interpolationTo.y = interpolationFrom.y;
			} else {
				interactor = null;
			}
		} else if (puzzleMode) {
			if (interpolation < 1.0f) {
				interactor.transform.position = Vector3.Slerp(interpolationFrom,
					interpolationTo,
					interpolation);
				interpolation += 10.0f * Time.deltaTime;
			} else {
				if (Input.GetKeyDown (KeyCode.E)) {
					puzzleMode = false;
				} else if (Input.GetKeyDown (KeyCode.A)) {
					moveMarkerLeft ();
				} else if (Input.GetKeyDown (KeyCode.D)) {
					moveMarkerRight ();
				} else if (Input.GetKeyDown (KeyCode.W)) {
					moveMarkerUp ();
				} else if (Input.GetKeyDown (KeyCode.S)) {
					moveMarkerDown ();
				} else if (Input.GetKeyDown (KeyCode.Space)) {
					rotatePuzzlePiece ();
				}
				interactor.transform.position = interpolationTo;
			}
		}
	}
}

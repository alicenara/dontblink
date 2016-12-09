using UnityEngine;
using System.Collections;
using System;

public class PuzzleInteract : MonoBehaviour {
	public Material activeWireMaterial;
	public Material inactiveWireMaterial;
	public Material solvedLightMaterial;
	public Material unsolvedLightMaterial;
	public TextAsset puzzleConfigurationFile;
	public AudioClip doorOpenClip;
	public AudioClip doorCloseClip;
	public AudioClip markerMoveClip;
	public AudioClip markerRotateClip;
	public AudioClip activatedSound;
	public Transform puzzleScreen;
	public Transform puzzleStraight;
	public Transform puzzleCurve;
	public Transform puzzleDouble;
	AudioSource doorAudioSource;
	AudioSource activatedAudioSource;
	Transform puzzleDoor;
	Transform puzzleFinishWire;
	Transform solvedLight;
	int w = 7;
	int h = 7;
	PuzzlePiece[,] puzzlePieces;
	bool puzzleMode = false;
	bool solved = false;
	float interpolation = 0.0f;
	float doorAngle = 0;
	int markerX = 3;
	int markerY = 3;
	Vector3 interpolationFrom;
	Vector3 interpolationTo;
	Collider interactor = null;
	ArrayList activeWires;

	// Use this for initialization
	void Start () {
		puzzleDoor = puzzleScreen.Find ("puzzle_frame/door");
		puzzleFinishWire = puzzleScreen.Find ("puzzle_frame/output_wire");
		solvedLight = puzzleScreen.Find ("puzzle_frame/light");
		AudioSource[] audioSources = puzzleScreen.GetComponents <AudioSource> ();
		doorAudioSource = audioSources[0];
		activatedAudioSource = audioSources[1];
		Debug.Log (doorAudioSource);
		puzzlePieces = new PuzzlePiece[w, h];
		activeWires = new ArrayList ();
		Array values = Enum.GetValues(typeof(Piece));
		System.Random random = new System.Random ();
		string puzzleConfiguration = puzzleConfigurationFile.text;
		int row = 0;
		int column = 0;
		for (int i = 0; i < puzzleConfiguration.Length; i++) {
			if (puzzleConfiguration [i] == '\n') {
				row++;
				column = 0;
			} else if (puzzleConfiguration [i] == '\r') {
				// Do nothing
			}else {
				switch (puzzleConfiguration [i]) {
				case '1':
					instantiatePiece (Piece.STRAIGHT, column, row);
					break;
				case '2':
					instantiatePiece (Piece.CURVE, column, row);
					break;
				case '3':
					instantiatePiece (Piece.DOUBLE, column, row);
					break;
				default:
					instantiatePiece ((Piece)values.GetValue (random.Next (values.Length)), column, row);
					break;
				}
				column++;
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
			+ puzzleScreen.transform.right * 0.30f;
		Vector3 position = origin - (puzzleScreen.transform.forward * i + puzzleScreen.transform.right * j) * 0.1f;
		Transform transform;
		switch (piece) {
		case Piece.STRAIGHT:
			transform = (Transform) Instantiate (puzzleStraight, position, this.transform.rotation);
			puzzlePieces [i, j] = new StraightPiece (transform, activeWireMaterial, inactiveWireMaterial);
			break;
		case Piece.CURVE:
			transform = (Transform) Instantiate (puzzleCurve, position, this.transform.rotation);
			puzzlePieces [i, j] = new CurvedPiece (transform, activeWireMaterial, inactiveWireMaterial);
			break;
		case Piece.DOUBLE:
			transform = (Transform) Instantiate (puzzleDouble, position, this.transform.rotation);
			puzzlePieces [i, j] = new DoublePiece (transform, activeWireMaterial, inactiveWireMaterial);
			break;
		};
	}

	bool lookingAtPuzzle(Collider other) {
		return Vector3.Angle (other.transform.forward, puzzleScreen.transform.position - other.transform.position) < 20.0f;
	}
		
	void OnTriggerEnter(Collider other) {
		if (!puzzleMode) {
			if ((other.tag == "Player" || other.tag == "Safe Player") && lookingAtPuzzle(other)) {
				interactor = other;
			}
		}
	}

	void OnTriggerStay(Collider other) {
		if (!puzzleMode) {
			if ((other.tag == "Player" || other.tag == "Safe Player") && lookingAtPuzzle(other)) {
				interactor = other;
			}
		}
	}

	void moveMarkerLeft() {
		if (markerX > 0) {
			puzzlePieces [markerX, markerY].setHighlighted (false);
			markerX -= 1;
			puzzlePieces [markerX, markerY].setHighlighted (true);
			doorAudioSource.PlayOneShot (markerMoveClip, 0.5f);
		}
	}

	void moveMarkerRight() {
		if (markerX < w - 1) {
			puzzlePieces [markerX, markerY].setHighlighted (false);
			markerX += 1;
			puzzlePieces [markerX, markerY].setHighlighted (true);
			doorAudioSource.PlayOneShot (markerMoveClip, 0.5f);
		}
	}

	void moveMarkerUp() {
		if (markerY > 0) {
			puzzlePieces [markerX, markerY].setHighlighted (false);
			markerY -= 1;
			puzzlePieces [markerX, markerY].setHighlighted (true);
			doorAudioSource.PlayOneShot (markerMoveClip, 0.5f);
		}
	}

	void moveMarkerDown() {
		if (markerY < h - 1) {
			puzzlePieces [markerX, markerY].setHighlighted (false);
			markerY += 1;
			puzzlePieces [markerX, markerY].setHighlighted (true);
			doorAudioSource.PlayOneShot (markerMoveClip, 0.5f);
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
							solvedLight.GetComponent<Renderer>().sharedMaterial = solvedLightMaterial;
							activatedAudioSource.Play();
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
			solvedLight.GetComponent<Renderer>().sharedMaterial = unsolvedLightMaterial;
			activatedAudioSource.Stop ();
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
			if (doorAngle > 0.0f) {
				float deltaAngle = 500.0f * Time.deltaTime;
				if (doorAngle - deltaAngle < 0) {
					deltaAngle = doorAngle;
				}
				doorAngle -= deltaAngle;
				puzzleDoor.RotateAround (puzzleDoor.transform.position - puzzleDoor.transform.up * 0.36f, puzzleDoor.transform.right, -deltaAngle);
			}
			if (interactor != null && Input.GetKeyDown (KeyCode.E)) {
				puzzleMode = true;
				interpolation = 0.0f;
				interpolationFrom = interactor.transform.position;
				interpolationTo = transform.position;
				interpolationTo.y = interpolationFrom.y;
				puzzlePieces [markerX, markerY].setHighlighted (true);
				doorAudioSource.PlayOneShot (doorOpenClip);
			} else {
				interactor = null;
			}
		} else {
			if (doorAngle < 160.0f) {
				float deltaAngle = 500.0f * Time.deltaTime;
				if (doorAngle + deltaAngle > 160.0f) {
					deltaAngle = 160.0f - doorAngle;
				}
				doorAngle += deltaAngle;
				puzzleDoor.RotateAround (puzzleDoor.transform.position - puzzleDoor.transform.up * 0.36f, puzzleDoor.transform.right, deltaAngle);
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
					doorAudioSource.PlayOneShot (doorCloseClip);
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
					doorAudioSource.PlayOneShot (markerRotateClip, 0.5f);
				}
				interactor.transform.position = interpolationTo;
			}
		}
		updatePuzzlePieces ();
	}
}

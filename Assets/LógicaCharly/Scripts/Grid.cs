using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Grid : MonoBehaviour
{
    // Struct PrefabFicha
    [System.Serializable]
    public struct PiecePrefab 
    {
        public PieceType type;
        public GameObject prefab;
    }

    // Enum de tipos de fichas
    public enum PieceType {
        EMPTY,
        NORMAL,
        COUNT
    }

    // Diccionario TipoFicha-PrefabFicha
    private Dictionary<PieceType, GameObject> piecePrefabDict;

    // Array de PrefabsFicha a renderizar
    public PiecePrefab[] piecePrefabs;

    // Matriz virtual de fichas
    public GamePiece[,] pieces;

    [Header("Arrays y Prefabs")]
    public GameObject backgroundPrefab;
    public GameObject[] prefabProfesores;
    private GamePiece pressedPiece;
    private GamePiece enteredPiece;
    
    [Header("Variables")]
    public int xDim;
    public int zDim;
    public float fillTime;
    public bool isFilled = false;
    public bool playingAnim = false;

    public ResultsController resultsController;
    public ResultsControllerContrarreloj resultsControllerContrarreloj;
    public GameManager gameManager;

    void Start()
    {
        piecePrefabDict = new Dictionary<PieceType, GameObject>();

        for (int i = 0; i < piecePrefabs.Length; i++) {
            if (!piecePrefabDict.ContainsKey(piecePrefabs[i].type)){
                piecePrefabDict.Add(piecePrefabs[i].type, piecePrefabs[i].prefab);
            }
        }

        for (int x = 0; x < xDim; x++) {
            for (int z = 0; z < zDim; z++) {
                GameObject background = (GameObject)Instantiate(backgroundPrefab, GetWorldPosition(x, z), Quaternion.identity);
                background.name = "BGTile(" + x + "," + z + ")";
                background.transform.parent = transform;
                
            }
        }

        pieces = new GamePiece[xDim, zDim];

        for (int x = 0; x < xDim; x++) {
            for (int z = 0; z < zDim; z++) {
                SpawnNewPiece(x, z, PieceType.EMPTY);
            }
        }
        gameManager = FindObjectOfType<GameManager>();

        StartCoroutine(Fill());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator Fill() 
    {
        bool needsRefill = true;

        while (needsRefill) {
            yield return new WaitForSeconds(fillTime);

            while (FillStep()) {
                yield return new WaitForSeconds(fillTime);
            }

            needsRefill = ClearAllValidMatches();
        }
        if (!isFilled) { isFilled = true; }
    }

    public bool FillStep()
    {
        bool movedPiece = false;

        for (int z = zDim-2; z >= 0; z--) {
            for (int x = 0; x < xDim; x++) {
                GamePiece piece = pieces[x,z];

                if (piece.IsMovable()) {
                    GamePiece pieceBelow = pieces[x, z+1];

                    if (pieceBelow.Type == PieceType.EMPTY) {
                        Destroy(pieceBelow.gameObject);
                        piece.MovableComponent.Move(x, z+1, fillTime);
                        pieces[x, z+1] = piece;
                        SpawnNewPiece(x, z, PieceType.EMPTY);
                        movedPiece = true;
                    }
                }
            }
        }

        for (int x = 0; x < xDim; x++) {
            GamePiece pieceBelow = pieces[x, 0];

            if (pieceBelow.Type == PieceType.EMPTY) {
                Destroy(pieceBelow.gameObject);
                GameObject newPiece = (GameObject)Instantiate(piecePrefabDict[PieceType.NORMAL], GetWorldPosition(x, -1), Quaternion.identity);
                newPiece.name = "Ficha";
                newPiece.transform.parent = transform;

                pieces[x, 0] = newPiece.GetComponent<GamePiece>();
                pieces[x, 0].Init(x, -1, this, PieceType.NORMAL);
                pieces[x, 0].MovableComponent.Move(x, 0, fillTime);
                pieces[x, 0].SubjectComponent.SetSubject((SubjectPiece.SubjectType)Random.Range(0, pieces[x, 0].SubjectComponent.NumSubjects));
                movedPiece = true;
            }
        }

        return movedPiece;
    }

    public Vector3 GetWorldPosition(int x, int z) 
    {
        return new Vector3(transform.position.x - xDim/2.0f + x, /*0*/ transform.position.y,
        transform.position.z + zDim/2.0f - z);
    }

    public GamePiece SpawnNewPiece(int x, int z, PieceType type) 
    {
        GameObject newPiece = (GameObject)Instantiate(piecePrefabDict[type], GetWorldPosition(x,z), Quaternion.identity);
        newPiece.transform.parent = transform;

        pieces[x,z] = newPiece.GetComponent<GamePiece>();
        pieces[x,z].Init(x, z, this, type);

        return pieces[x,z];
    }

    public bool IsAdjacent(GamePiece piece1, GamePiece piece2) 
    {
        return (piece1.X == piece2.X && (int)Mathf.Abs(piece1.Z - piece2.Z) == 1
        || piece1.Z == piece2.Z && (int)Mathf.Abs(piece1.X - piece2.X) == 1);
    }

    public void SwapPieces(GamePiece piece1, GamePiece piece2)
    {
        if (piece1.IsMovable() && piece2.IsMovable()) {
            pieces[piece1.X, piece1.Z] = piece2;
            pieces[piece2.X, piece2.Z] = piece1;

            if (GetMatch(piece1, piece2.X, piece2.Z) != null || GetMatch(piece2, piece1.X, piece1.Z) != null) {
                int piece1X = piece1.X;
                int piece1Z = piece1.Z;

                piece1.MovableComponent.Move(piece2.X, piece2.Z, fillTime);
                piece2.MovableComponent.Move(piece1X, piece1Z, fillTime);

                ClearAllValidMatches();

                StartCoroutine(Fill());
            }
            else {
                pieces[piece1.X, piece1.Z] = piece1;
                pieces[piece2.X, piece2.Z] = piece2;
            }
        }
    }

    public void PressPiece(GamePiece piece)
    {
        pressedPiece = piece;
        Debug.Log("Pressed a piece");
    }

    public void EnterPiece(GamePiece piece) 
    {
        enteredPiece = piece;
        Debug.Log("Entered a piece");
    }

    public void ReleasePiece() 
    {
        if (IsAdjacent(pressedPiece, enteredPiece)) {
            SwapPieces(pressedPiece, enteredPiece);
        }
    }

    public List<GamePiece> GetMatch(GamePiece piece, int newX, int newZ) 
    {
        if (piece.IsSubject()) {
            SubjectPiece.SubjectType subject = piece.SubjectComponent.Subject;
            List<GamePiece> horizontalPieces = new List<GamePiece>();
            List<GamePiece> verticalPieces = new List<GamePiece>();
            List<GamePiece> matchingPieces = new List<GamePiece>();

            // First check horizontal
            horizontalPieces.Add(piece);

            for (int dir = 0; dir <= 1; dir++) {
                for (int xOffset = 1; xOffset < xDim; xOffset++) {
                    int x;

                    if (dir == 0) { // Left 
                        x = newX - xOffset;
                    }
                    else { // Right
                        x = newX + xOffset;
                    }

                    if (x < 0 || x >= xDim) {
                        break;
                    }

                    if (pieces[x, newZ].IsSubject() && pieces[x, newZ].SubjectComponent.Subject == subject) {
                        horizontalPieces.Add(pieces[x, newZ]);
                    } else {
                        break;
                    }
                }
            }

            if (horizontalPieces.Count >= 3) {
                matchingPieces.AddRange(horizontalPieces);
            }

            // Check vertical if found match (L & T)
            if (horizontalPieces.Count >= 3) {
                for (int i = 0; i < horizontalPieces.Count; i++) {
                    for (int dir = 0; dir <= 1; dir++) {
                        for (int zOffset = 1; zOffset < zDim; zOffset++) {
                            int z; 

                            if (dir == 0) { // Up
                                z = newZ - zOffset;
                            } else { // Down
                                z = newZ + zOffset;
                            }   

                            if (z < 0 || z >= zDim) {
                                break;
                            }

                            if (pieces[horizontalPieces[i].X, z].IsSubject() && pieces[horizontalPieces[i].X, z].SubjectComponent.Subject == subject) {
                                verticalPieces.Add(pieces[horizontalPieces[i].X, z]);
                            } else {
                                break;
                            }
                        }
                    }

                    if (verticalPieces.Count < 2) {
                        verticalPieces.Clear();
                    } else {
                        matchingPieces.AddRange(verticalPieces);
                        break;
                    }
                }
            }

            if (matchingPieces.Count >= 3) {
                return matchingPieces;
            }

            // Now check vertical
            horizontalPieces.Clear();
            verticalPieces.Clear();
            verticalPieces.Add(piece);

            for (int dir = 0; dir <= 1; dir++) {
                for (int zOffset = 1; zOffset < zDim; zOffset++) {
                    int z;

                    if (dir == 0) { // Up 
                        z = newZ - zOffset;
                    }
                    else { // Down
                        z = newZ + zOffset;
                    }

                    if (z < 0 || z >= zDim) {
                        break;
                    }

                    if (pieces[newX, z].IsSubject() && pieces[newX, z].SubjectComponent.Subject == subject) {
                        verticalPieces.Add(pieces[newX, z]);
                    } else {
                        break;
                    }
                }
            }

            if (verticalPieces.Count >= 3) {
                matchingPieces.AddRange(verticalPieces);
            }

            // Check horizontal if found match (L & T)
            if (verticalPieces.Count >= 3) {
                for (int i = 0; i < verticalPieces.Count; i++) {
                    for (int dir = 0; dir <= 1; dir++) {
                        for (int xOffset = 1; xOffset < xDim; xOffset++) {
                            int x; 

                            if (dir == 0) { // Left
                                x = newX - xOffset;
                            } else { // Right
                                x = newX + xOffset;
                            }   

                            if (x < 0 || x >= xDim) {
                                break;
                            }

                            if (pieces[x, verticalPieces[i].Z].IsSubject() && pieces[x, verticalPieces[i].Z].SubjectComponent.Subject == subject) {
                                horizontalPieces.Add(pieces[x, verticalPieces[i].Z]);
                            } else {
                                break;
                            }
                        }
                    }

                    if (horizontalPieces.Count < 2) {
                        horizontalPieces.Clear();
                    } else {
                        matchingPieces.AddRange(horizontalPieces);

                        break;
                    }
                }
            }

            if (matchingPieces.Count >= 3) {
                return matchingPieces;
            }
        }

        return null;
    }

    public bool ClearAllValidMatches()
    {
        bool needsRefill = false;

        for (int z = 0; z < zDim; z++) {
            for (int x = 0; x < xDim; x++) {
                if (pieces[x, z].IsClearable()) {
                    List<GamePiece> match = GetMatch(pieces[x, z], x, z);

                    if (match != null) {
                        for (int i = 0; i < match.Count; i++) {
                            if (ClearPiece(match[i].X, match[i].Z)) {
                                needsRefill = true;
                            }
                        }
                    }
                }
            }
        }

        return needsRefill;
    }

    public bool ClearPiece(int x, int z)
    {
        if(SceneManager.GetActiveScene().name == "Contrarreloj" || SceneManager.GetActiveScene().name == "Multijugador") {
            if (pieces[x, z].IsClearable() && !pieces[x, z].ClearableComponent.IsBeingCleared) {
                pieces[x, z].ClearableComponent.Clear();
                SpawnNewPiece(x, z, PieceType.EMPTY);
                if(isFilled && SceneManager.GetActiveScene().name == "Contrarreloj") resultsControllerContrarreloj.SumarPuntos();
                
                if (gameManager != null && SceneManager.GetActiveScene().name == "Multijugador" && isFilled)
                {
                    // Supongamos que playerIndex es 1 para el jugador local y 2 para el jugador remoto.
                    int playerIndex = PhotonNetwork.IsMasterClient ? 1 : 2;
                    gameManager.IncreasePlayerScore(playerIndex, 8);
                }

                Debug.Log("Limpio en Contrarreloj/Multijugador");
                return true;
            } 
        } else {
            if (pieces[x, z].IsClearable() && !pieces[x, z].ClearableComponent.IsBeingCleared) {
                pieces[x, z].ClearableComponent.Clear();
                SpawnNewPiece(x, z, PieceType.EMPTY);
                if(isFilled) resultsController.SumarPuntos();

                if (Random.value <= 0.03f && isFilled && !playingAnim)
                {
                    ActivateRandomSpecialGameObject();
                }

                return true;
            }
        }
        
        return false;
    }

    private void ActivateRandomSpecialGameObject()
    {
        if (prefabProfesores.Length > 0)
        {
            playingAnim = true;
            int randomIndex = Random.Range(0, prefabProfesores.Length);
            prefabProfesores[randomIndex].SetActive(true);
            MovSpriteProfesor movSpriteProfesor = prefabProfesores[randomIndex].GetComponent<MovSpriteProfesor>();
            movSpriteProfesor.StartVariables();
            Debug.Log("Activated special game object: " + prefabProfesores[randomIndex].name);
        }
    }

    public void ClearBoard()
    {
        for (int x = 0; x < xDim ; x++) {
            for (int z = 0; z < zDim; z++) {
                if (pieces[x, z].IsClearable() && !pieces[x, z].ClearableComponent.IsBeingCleared) {
                    pieces[x, z].ClearableComponent.Clear();
                    SpawnNewPiece(x, z, PieceType.EMPTY);
                }
            }
        }
        isFilled = false;
        StartCoroutine(Fill());
    }
}

using System.Collections.Generic;
using Chess;
using DG.Tweening;
using UnityEngine;

public class ChessBoard : MonoBehaviour
{
    private const int BoardSize = 8;
    public float _tileSize = 1.0f;

    private ChessPiece[,] chessPieces;

    private ChessPiece currentlySelectedPiece;

    private List<Vector2Int> availableMoves = new();

    private PieceTeam currentTurn = PieceTeam.White;

    private List<GameObject> highlightObjects = new();

    private void Start()
    {
        GenerateBoard();
        SpawnAllPieces();
    }

    private void GenerateBoard()
    {
        for (int x = 0; x < BoardSize; x++)
        {
            for (int z = 0; z < BoardSize; z++)
            {
                GameObject tile = GameObject.CreatePrimitive(PrimitiveType.Cube);
                tile.transform.position = new Vector3(x * _tileSize, 0, z * _tileSize);
                tile.transform.parent = transform;
                tile.name = $"Tile_{x}_{z}";

                Renderer renderer = tile.GetComponent<Renderer>();
                renderer.material.color = (x + z) % 2 == 0 ? Color.black : Color.white;
            }
        }
    }

    private void SpawnAllPieces()
    {
        chessPieces = new ChessPiece[BoardSize, BoardSize];

        SpawnPiece(PieceType.Rook, PieceTeam.White, 0, 0);
        SpawnPiece(PieceType.Knight, PieceTeam.White, 1, 0);
        SpawnPiece(PieceType.Bishop, PieceTeam.White, 2, 0);
        SpawnPiece(PieceType.Queen, PieceTeam.White, 3, 0);
        SpawnPiece(PieceType.King, PieceTeam.White, 4, 0);
        SpawnPiece(PieceType.Bishop, PieceTeam.White, 5, 0);
        SpawnPiece(PieceType.Knight, PieceTeam.White, 6, 0);
        SpawnPiece(PieceType.Rook, PieceTeam.White, 7, 0);
        for (int i = 0; i < BoardSize; i++)
        {
            SpawnPiece(PieceType.Pawn, PieceTeam.White, i, 1);
        }

        SpawnPiece(PieceType.Rook, PieceTeam.Black, 0, 7);
        SpawnPiece(PieceType.Knight, PieceTeam.Black, 1, 7);
        SpawnPiece(PieceType.Bishop, PieceTeam.Black, 2, 7);
        SpawnPiece(PieceType.Queen, PieceTeam.Black, 3, 7);
        SpawnPiece(PieceType.King, PieceTeam.Black, 4, 7);
        SpawnPiece(PieceType.Bishop, PieceTeam.Black, 5, 7);
        SpawnPiece(PieceType.Knight, PieceTeam.Black, 6, 7);
        SpawnPiece(PieceType.Rook, PieceTeam.Black, 7, 7);
        for (int i = 0; i < BoardSize; i++)
        {
            SpawnPiece(PieceType.Pawn, PieceTeam.Black, i, 6);
        }
    }

    private void SpawnPiece(PieceType type, PieceTeam team, int x, int z)
    {
        GameObject pieceObject = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        pieceObject.transform.parent = transform;

        pieceObject.transform.position = new Vector3(x * _tileSize, 1.0f, z * _tileSize);
        pieceObject.transform.localScale = new Vector3(0.5f, 1f, 0.5f);
        pieceObject.name = $"{team}_{type}";

        ChessPiece pieceScript = null;
        switch (type)
        {
            case PieceType.King: pieceScript = pieceObject.AddComponent<King>(); break;
            case PieceType.Queen: pieceScript = pieceObject.AddComponent<Queen>(); break;
            case PieceType.Rook: pieceScript = pieceObject.AddComponent<Rook>(); break;
            case PieceType.Bishop: pieceScript = pieceObject.AddComponent<Bishop>(); break;
            case PieceType.Knight: pieceScript = pieceObject.AddComponent<Knight>(); break;
            case PieceType.Pawn: pieceScript = pieceObject.AddComponent<Pawn>(); break;
        }

        if (pieceScript != null)
        {
            pieceScript.Init(team, type);
            pieceScript.currentPosition = new Vector2Int(x, z);
            chessPieces[x, z] = pieceScript;
        }

        Renderer renderer = pieceObject.GetComponent<Renderer>();
        renderer.material.color = team == PieceTeam.White ? Color.white : Color.grey;

        pieceObject.layer = LayerMask.NameToLayer("Piece");
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100))
            {
                if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Piece"))
                {
                    ChessPiece clickedPiece = hit.transform.GetComponent<ChessPiece>();
                    if (clickedPiece != null)
                    {
                        bool captured = false;
                        if (currentlySelectedPiece != null && currentlySelectedPiece.team != clickedPiece.team)
                        {
                            if (currentlySelectedPiece.team == currentTurn)
                            {
                                foreach (Vector2Int move in availableMoves)
                                {
                                    if (move == clickedPiece.currentPosition)
                                    {
                                        MovePiece(currentlySelectedPiece.currentPosition.x, currentlySelectedPiece.currentPosition.y, clickedPiece.currentPosition.x, clickedPiece.currentPosition.y);
                                        currentlySelectedPiece = null;
                                        availableMoves.Clear();
                                        ClearHighlights();
                                        captured = true;
                                        break;
                                    }
                                }
                            }
                        }

                        if (!captured)
                        {
                            if (clickedPiece.team == currentTurn)
                            {
                                if (currentlySelectedPiece != clickedPiece)
                                {
                                    ClearHighlights();
                                    currentlySelectedPiece = clickedPiece;
                                    availableMoves = GetLegalMoves(currentlySelectedPiece);
                                    HighlightAllowedMoves(availableMoves);
                                    Debug.Log($"Selected {currentlySelectedPiece.team} {currentlySelectedPiece.type} at {currentlySelectedPiece.currentPosition}");
                                }
                            }
                            else
                            {
                                Debug.Log($"It is {currentTurn}'s turn.");
                            }
                        }
                    }
                }
                else if (currentlySelectedPiece != null)
                {
                    int targetX = Mathf.RoundToInt(hit.point.x / _tileSize);
                    int targetY = Mathf.RoundToInt(hit.point.z / _tileSize);

                    Vector2Int targetPos = new(targetX, targetY);
                    bool isValidMove = false;
                    foreach (Vector2Int move in availableMoves)
                    {
                        if (move == targetPos)
                        {
                            isValidMove = true;
                            break;
                        }
                    }

                    if (isValidMove)
                    {
                        MovePiece(currentlySelectedPiece.currentPosition.x, currentlySelectedPiece.currentPosition.y, targetX, targetY);
                        currentlySelectedPiece = null;
                        availableMoves.Clear();
                        ClearHighlights();
                    }
                    else
                    {
                        Debug.Log("Invalid move");
                        currentlySelectedPiece = null;
                        availableMoves.Clear();
                        ClearHighlights();
                    }
                }
            }
        }
    }

    public void MovePiece(int originalX, int originalY, int newX, int newY)
    {
        ChessPiece pieceToMove = chessPieces[originalX, originalY];

        if (pieceToMove == null)
        {
            Debug.LogError($"No piece found at {originalX}, {originalY}");
            return;
        }

        if (chessPieces[newX, newY] != null)
        {
            ChessPiece pieceToCapture = chessPieces[newX, newY];

            if (pieceToCapture.team != pieceToMove.team)
            {
                Debug.Log($"{pieceToMove.team} {pieceToMove.type} captures {pieceToCapture.team} {pieceToCapture.type} at {newX}, {newY}");
                Destroy(pieceToCapture.gameObject);
            }
            else
            {
                Debug.LogWarning($"Cannot capture own piece at {newX}, {newY}");
                return;
            }
        }

        chessPieces[newX, newY] = pieceToMove;
        chessPieces[originalX, originalY] = null;

        pieceToMove.currentPosition = new Vector2Int(newX, newY);

        pieceToMove.transform.DOMove(new Vector3(newX * _tileSize, 1.0f, newY * _tileSize), .25f);

        currentTurn = (currentTurn == PieceTeam.White) ? PieceTeam.Black : PieceTeam.White;
        Debug.Log($"Turn switched to {currentTurn}");

        if (CheckForCheckmate(currentTurn))
        {
            Debug.Log(IsKingInCheck(currentTurn) ? $"Checkmate! {((currentTurn == PieceTeam.White) ? PieceTeam.Black : PieceTeam.White)} wins!" : "Stalemate! Draw.");
        }
        else if (IsKingInCheck(currentTurn))
        {
            Debug.Log($"{currentTurn} is in Check!");
        }
    }

    private List<Vector2Int> GetLegalMoves(ChessPiece piece)
    {
        List<Vector2Int> moves = piece.GetAvailableMoves(chessPieces, BoardSize);
        List<Vector2Int> legalMoves = new List<Vector2Int>();

        foreach (Vector2Int move in moves)
        {
            ChessPiece targetPiece = chessPieces[move.x, move.y];
            Vector2Int originalPosition = piece.currentPosition;

            chessPieces[originalPosition.x, originalPosition.y] = null;
            chessPieces[move.x, move.y] = piece;
            piece.currentPosition = move;

            if (!IsKingInCheck(piece.team))
            {
                legalMoves.Add(move);
            }
            piece.currentPosition = originalPosition;
            chessPieces[originalPosition.x, originalPosition.y] = piece;
            chessPieces[move.x, move.y] = targetPiece;
        }

        return legalMoves;
    }

    private bool IsKingInCheck(PieceTeam team)
    {
        Vector2Int kingPos = Vector2Int.zero;
        bool kingFound = false;

        for (int x = 0; x < BoardSize; x++)
        {
            for (int y = 0; y < BoardSize; y++)
            {
                if (chessPieces[x, y] != null && chessPieces[x, y].type == PieceType.King && chessPieces[x, y].team == team)
                {
                    kingPos = new Vector2Int(x, y);
                    kingFound = true;
                    break;
                }
            }
            if (kingFound) break;
        }

        if (!kingFound) return false;

        for (int x = 0; x < BoardSize; x++)
        {
            for (int y = 0; y < BoardSize; y++)
            {
                if (chessPieces[x, y] != null && chessPieces[x, y].team != team)
                {
                    List<Vector2Int> enemyMoves = chessPieces[x, y].GetAvailableMoves(chessPieces, BoardSize);
                    if (enemyMoves.Contains(kingPos))
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }

    private bool CheckForCheckmate(PieceTeam team)
    {
        for (int x = 0; x < BoardSize; x++)
        {
            for (int y = 0; y < BoardSize; y++)
            {
                if (chessPieces[x, y] != null && chessPieces[x, y].team == team)
                {
                    List<Vector2Int> legalMoves = GetLegalMoves(chessPieces[x, y]);
                    if (legalMoves.Count > 0)
                    {
                        return false;
                    }
                }
            }
        }
        return true;
    }

    private void HighlightAllowedMoves(List<Vector2Int> moves)
    {
        foreach (Vector2Int move in moves)
        {
            GameObject highlight = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            highlight.transform.position = new Vector3(move.x * _tileSize, 0.5f, move.y * _tileSize);
            highlight.transform.localScale = Vector3.zero;

            Renderer renderer = highlight.GetComponent<Renderer>();
            renderer.material.color = new Color(0, 1, 0, 0.5f);

            highlight.transform.DOScale(new Vector3(0.5f, 0.5f, 0.5f), 0.2f).SetEase(Ease.OutBack);

            highlightObjects.Add(highlight);
        }
    }

    private void ClearHighlights()
    {
        foreach (GameObject highlight in highlightObjects)
        {
            Destroy(highlight);
        }
        highlightObjects.Clear();
    }
}
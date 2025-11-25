using System.Collections.Generic;
using UnityEngine;

namespace Chess
{
    public abstract class ChessPiece : MonoBehaviour
    {
        public PieceType type;
        public PieceTeam team;
        public Vector2Int currentPosition;

        public virtual void Init(PieceTeam team, PieceType type)
        {
            this.team = team;
            this.type = type;
            transform.rotation = Quaternion.identity;
        }

        public abstract List<Vector2Int> GetAvailableMoves(ChessPiece[,] board, int boardSize);

        protected bool IsWithinBounds(int x, int y, int boardSize)
        {
            return x >= 0 && x < boardSize && y >= 0 && y < boardSize;
        }
    }
}

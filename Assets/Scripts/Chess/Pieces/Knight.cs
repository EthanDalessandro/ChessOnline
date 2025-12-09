using System.Collections.Generic;
using UnityEngine;

namespace Chess
{
    public class Knight : ChessPiece
    {
        public override List<Vector2Int> GetAvailableMoves(ChessPiece[,] board, int boardSize)
        {
            List<Vector2Int> moves = new();

            Vector2Int[] offsets = new Vector2Int[]
            {
                new (1, 2), new (2, 1),
                new (2, -1), new (1, -2),
                new (-1, -2), new (-2, -1),
                new (-2, 1), new (-1, 2)
            };

            foreach (Vector2Int offset in offsets)
            {
                int nextX = currentPosition.x + offset.x;
                int nextY = currentPosition.y + offset.y;

                if (!IsWithinBounds(nextX, nextY, boardSize)) continue;
                if (!board[nextX, nextY] || board[nextX, nextY].team != team)
                {
                    moves.Add(new Vector2Int(nextX, nextY));
                }
            }

            return moves;
        }
    }
}

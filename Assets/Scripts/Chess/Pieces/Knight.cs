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
                new Vector2Int(1, 2), new Vector2Int(2, 1),
                new Vector2Int(2, -1), new Vector2Int(1, -2),
                new Vector2Int(-1, -2), new Vector2Int(-2, -1),
                new Vector2Int(-2, 1), new Vector2Int(-1, 2)
            };

            foreach (Vector2Int offset in offsets)
            {
                int nextX = currentPosition.x + offset.x;
                int nextY = currentPosition.y + offset.y;

                if (IsWithinBounds(nextX, nextY, boardSize))
                {
                    if (board[nextX, nextY] == null || board[nextX, nextY].team != team)
                    {
                        moves.Add(new Vector2Int(nextX, nextY));
                    }
                }
            }

            return moves;
        }
    }
}

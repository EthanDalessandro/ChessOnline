using System.Collections.Generic;
using UnityEngine;

namespace Chess
{
    public class King : ChessPiece
    {
        public override List<Vector2Int> GetAvailableMoves(ChessPiece[,] board, int boardSize)
        {
            List<Vector2Int> moves = new();

            Vector2Int[] directions = new Vector2Int[]
            {
                new Vector2Int(0, 1), new Vector2Int(1, 1), new Vector2Int(1, 0), new Vector2Int(1, -1),
                new Vector2Int(0, -1), new Vector2Int(-1, -1), new Vector2Int(-1, 0), new Vector2Int(-1, 1)
            };

            foreach (Vector2Int dir in directions)
            {
                int nextX = currentPosition.x + dir.x;
                int nextY = currentPosition.y + dir.y;

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

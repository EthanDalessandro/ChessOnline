using System.Collections.Generic;
using UnityEngine;

namespace Chess
{
    public class Bishop : ChessPiece
    {
        public override List<Vector2Int> GetAvailableMoves(ChessPiece[,] board, int boardSize)
        {
            List<Vector2Int> moves = new();

            Vector2Int[] directions = new Vector2Int[]
            {
                new Vector2Int(1, 1), new Vector2Int(1, -1),
                new Vector2Int(-1, -1), new Vector2Int(-1, 1)
            };

            foreach (Vector2Int dir in directions)
            {
                for (int i = 1; i < boardSize; i++)
                {
                    int nextX = currentPosition.x + dir.x * i;
                    int nextY = currentPosition.y + dir.y * i;

                    if (!IsWithinBounds(nextX, nextY, boardSize))
                        break;

                    if (board[nextX, nextY] == null)
                    {
                        moves.Add(new Vector2Int(nextX, nextY));
                    }
                    else
                    {
                        if (board[nextX, nextY].team != team)
                        {
                            moves.Add(new Vector2Int(nextX, nextY));
                        }
                        break;
                    }
                }
            }

            return moves;
        }
    }
}

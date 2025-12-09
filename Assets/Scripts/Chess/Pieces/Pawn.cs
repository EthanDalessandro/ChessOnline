using System.Collections.Generic;
using UnityEngine;

namespace Chess
{
    public class Pawn : ChessPiece
    {
        public override List<Vector2Int> GetAvailableMoves(ChessPiece[,] board, int boardSize)
        {
            List<Vector2Int> moves = new();

            int direction = (team == PieceTeam.White) ? 1 : -1;

            if (IsWithinBounds(currentPosition.x, currentPosition.y + direction, boardSize))
            {
                if (!board[currentPosition.x, currentPosition.y + direction])
                {
                    moves.Add(new Vector2Int(currentPosition.x, currentPosition.y + direction));

                    if ((team == PieceTeam.White && currentPosition.y == 1) || (team == PieceTeam.Black && currentPosition.y == 6))
                    {
                        if (IsWithinBounds(currentPosition.x, currentPosition.y + direction * 2, boardSize))
                        {
                            if (!board[currentPosition.x, currentPosition.y + direction * 2])
                            {
                                moves.Add(new Vector2Int(currentPosition.x, currentPosition.y + direction * 2));
                            }
                        }
                    }
                }
            }

            int[] captureOffsets = { -1, 1 };
            foreach (int offset in captureOffsets)
            {
                if (!IsWithinBounds(currentPosition.x + offset, currentPosition.y + direction, boardSize)) continue;
                ChessPiece target = board[currentPosition.x + offset, currentPosition.y + direction];
                if (target && target.team != team)
                {
                    moves.Add(new Vector2Int(currentPosition.x + offset, currentPosition.y + direction));
                }
            }

            return moves;
        }
    }
}

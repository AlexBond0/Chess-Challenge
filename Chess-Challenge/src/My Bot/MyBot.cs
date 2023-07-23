using ChessChallenge.API;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;

public class MyBot : IChessBot
{
    // Piece values: null, pawn, knight, bishop, rook, queen, king
    private static int[] pieceValues =      { 0, 1, 3, 3, 5, 9, 10 };
    private static int[] advanceWeight =    { 6, 6, 5, 5, 5, 4, 1 };

    private struct MoveScore {
        public Move move;
        public int score;   

        public MoveScore(Move move) : this()
        {
            this.move = move;
            this.score = extractScore(move);
        }

        private int extractScore(Move move)
        {
            if (move.IsCapture)
                return pieceValues[(int)move.CapturePieceType] * 10;

            int rankDelta = move.TargetSquare.Rank - move.StartSquare.Rank;
            if (rankDelta > 0)
                return advanceWeight[(int)move.MovePieceType] * 3;

            return 0;
        }
    }

    public Move Think(Board board, Timer timer)
    {
        Move[] moves = board.GetLegalMoves();

        Move[] move = moves
            .Select(m => new MoveScore(m))
            .OrderByDescending(i => i.score)
            .Select(m => m.move)
            .Concat(moves)
            .ToArray();

        return move[0];
    }
}
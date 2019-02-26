
public class LineScoreHandler
{
    const int comboBaseScore = 50;

    ScoreResult GerScore(int clearedLines, int comboLevel, int currentLevel)
    {
        return GetLineClear(clearedLines, currentLevel) + GetComboScore(comboLevel, currentLevel);
    }

    ScoreResult GetLineClear(int clearedLines, int currentLevel)
    {
        switch (clearedLines)
        { 
            case 1: return new ScoreResult(100 * currentLevel, 1);
            case 2: return new ScoreResult(300 * currentLevel, 3);
            case 3: return new ScoreResult(500 * currentLevel, 5);
            case 4: return new ScoreResult(800 * currentLevel, 8);
            default: return new ScoreResult(0, 0);
        }
    }

    ScoreResult GetComboScore(int comboLevel, int currentLevel)
    {
        return new ScoreResult(comboBaseScore * comboLevel * currentLevel, currentLevel / 2);
    }
}

public struct ScoreResult
{
    readonly int score;
    readonly int lineAward;

    public ScoreResult(int score, int lineAward)
    {
        this.score = score;
        this.lineAward = lineAward;
    }

    public static ScoreResult operator+ (ScoreResult A, ScoreResult B)
    {
        return new ScoreResult(A.score + B.score, A.lineAward + B.lineAward);
    }
}
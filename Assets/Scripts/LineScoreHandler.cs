
public static class LineScoreHandler
{
    const int comboBaseScore = 50;

    public static ScoreState GetScore(int clearedLines, ScoreState scoreState)
    {
        return GetLineClearScore(clearedLines, scoreState.Level) + GetComboScore(scoreState.ComboLevel, scoreState.Level);
    }

    static ScoreState GetLineClearScore(int clearedLines, int currentLevel)
    {
        switch (clearedLines)
        { 
            case 1: return new ScoreState(100 * currentLevel, 1);
            case 2: return new ScoreState(300 * currentLevel, 3);
            case 3: return new ScoreState(500 * currentLevel, 5);
            case 4: return new ScoreState(800 * currentLevel, 8);
            default: return new ScoreState(0, 0);
        }
    }

    static ScoreState GetComboScore(int comboLevel, int currentLevel)
    {
        return new ScoreState(comboBaseScore * comboLevel * currentLevel, currentLevel / 2);
    }
}

public struct ScoreState
{
    public int Score { get; private set; }
    public int ComboLevel { get; private set; }
    public int Level { get; private set; }
    public int ClearedLines { get; private set; }

    public ScoreState(int score, int clearedLines)
    {
        Score = score;
        this.ClearedLines = clearedLines;
        ComboLevel = 0;
        Level = 0;
        CalculateLevel();
    }

    public ScoreState(int score, int clearedLines, int comboLevel)
    {
        Score = score;
        this.ClearedLines = clearedLines;
        ComboLevel = comboLevel;
        Level = 0;
        CalculateLevel();
    }

    void CalculateLevel()
    {
        var newLevel = 0;
        var lines = ClearedLines;

        while (lines >= 0)
        {
            newLevel++;
            lines -= 5 * newLevel;
        }

        Level = newLevel;
    }

    public void ResetCombo()
    {
        ComboLevel = 0;
    }

    public void IncreaseCombo()
    {
        ComboLevel++;
    }

    /// <summary>
    /// Adds the score and linedCleared of two ScoreState, but keep the combo of the first one
    /// </summary>
    public static ScoreState operator+ (ScoreState A, ScoreState B)
    {
        return new ScoreState(A.Score + B.Score, A.ClearedLines+ B.ClearedLines, A.ComboLevel);
    }
}
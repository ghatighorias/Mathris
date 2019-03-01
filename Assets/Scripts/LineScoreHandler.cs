
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
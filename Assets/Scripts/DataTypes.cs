using UnityEngine;

public enum ActionType
{
    None,
    Rotate,
    MoveLeft,
    MoveRight,
    Down
}

public enum Move
{
    Up,
    Down,
    Left,
    Right
}

public enum Rotate
{
    ClockWise,
    CounterClockWise
}

public enum RaytraceHitResultType
{
    None,
    GridWall,
    GridBottom,
    Block
}

public enum MathItemType
{
    Zero,
    One,
    Two,
    Three,
    Four,
    Five,
    Six,
    Seven,
    Eight,
    Nine,
    Plus,
    Minus,
    Division,
    Multiply,
    Unknown
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
    public static ScoreState operator +(ScoreState A, ScoreState B)
    {
        return new ScoreState(A.Score + B.Score, A.ClearedLines + B.ClearedLines, A.ComboLevel);
    }
}
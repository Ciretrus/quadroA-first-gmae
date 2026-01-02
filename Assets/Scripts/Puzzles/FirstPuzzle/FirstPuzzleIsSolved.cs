using UnityEngine;

public class FirstPuzzleIsSolved : SolvedPuzzle
{
    private bool m_isSolved = false;

    public override bool isSolved()
    {
        return m_isSolved;
    }

}

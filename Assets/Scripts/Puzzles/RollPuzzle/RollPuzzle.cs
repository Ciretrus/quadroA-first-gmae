using UnityEngine;

namespace Puzzles
{
    public class RollPuzzle : BasePuzzle
    {
        [SerializeField] private ICondition[] m_rolls;

        public override void CheckCondition()
        {
            if (CheckRolls()) 
            {
                NotifySolved();
                Debug.Log("PuzzleSolved");
            }
        }
        private bool CheckRolls()
        {
            for (int i = 0; i < m_rolls.Length; i++) 
            {   
                if (!m_rolls[i].IsSolved) return false;
            }
            return true;
        }
    }
}

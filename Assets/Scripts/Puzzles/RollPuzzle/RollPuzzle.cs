using UnityEngine;
namespace Puzzles
{
    public class RollPuzzle : BasePuzzle
    {
        [SerializeField] private RollRotation[] m_rolls;

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
                if (!m_rolls[i].isRightNumber) return false;
            }
            return true;
        }
    }
}

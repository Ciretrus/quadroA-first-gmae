using UnityEngine;
using System.Collections;
namespace Puzzles
{
    public class RuneCheck: BasePuzzle
    {
        [SerializeField] private ICondition[] m_runes;

      
        public override void CheckCondition()
        {
            foreach (ICondition rune in m_runes)
            {
                if (rune.IsSolved == false)
                {
                    return;
                }
            }
            NotifySolved();

        }
    }
}

using UnityEngine;
using System.Collections;
namespace Puzzles
{
    public class RuneCheck: BasePuzzle
    {
        [SerializeField] private Object[] m_runes;

      
        public override void CheckCondition()
        {
            foreach (Object rune in m_runes)
            {
                if (TryGetComponent<ICondition>(out ICondition condition))
                {
                    if (condition.IsSolved == false)
                    {
                        return;
                    }
                }
            }
            NotifySolved();

        }
    }
}

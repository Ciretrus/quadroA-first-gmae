using UnityEngine;
namespace Puzzles
{
    public class RuneCheck: BasePuzzle
    {
        [SerializeField] private RuneSwitch[] m_runes;

        public override void CheckCondition()
        {
            foreach (RuneSwitch rune in m_runes)
            {
                if (rune.isActive == false)
                {
                    return;
                }
            }
            NotifySolved();
        }
    }
}

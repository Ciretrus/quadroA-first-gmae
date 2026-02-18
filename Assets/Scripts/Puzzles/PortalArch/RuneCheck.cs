using UnityEngine;

namespace Puzzles
{
    public class RuneCheck: BasePuzzle
    {
        [SerializeField] private GameObject[] m_runes;

        public override void CheckCondition()
        {
            foreach (GameObject rune in m_runes)
            {
                if (rune.TryGetComponent(out ICondition condition))
                {
                    if (condition.IsSolved == false)
                    {
                        return;
                    }
                }
                else 
                {
                    Debug.LogWarning($"No ICondition on {rune.name}");
                    return; 
                }
            }
            NotifySolved();
        }
    }
}

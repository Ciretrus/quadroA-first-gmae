using UnityEngine;
using System.Collections;
public class PortalActivation : MonoBehaviour
{
    [SerializeField] private RuneActivation[] m_runes;
    [SerializeField] private GameObject m_particle;
    private bool m_activated = false;

    void Update()
    {
        bool unlocked = true;
        foreach (RuneActivation rune in m_runes)
        {
            if (rune.isActive == false)
            {
                //print("not open");
                unlocked = false;
                break;
            }

        }
        if (unlocked && !m_activated)
        {
            //print("try open");
            m_activated = true;
            m_particle.SetActive(true);


        }

    }


    
}


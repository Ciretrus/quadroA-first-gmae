using System.Collections;
using UnityEngine;

public class LockCheck : MonoBehaviour
{
    [SerializeField] private RollRotation[] m_locks;
    [SerializeField] private Vector3 m_doorAngel = new Vector3(90f,0,0);
    [SerializeField] private float m_openingTime = 1f;
    private bool m_opened = false;

    void Update()
    {
        bool unlocked = true;
        foreach (RollRotation roll in m_locks)
        {
            if (roll.isRightNumber == false)
            {
                //print("not open");
                unlocked = false;
                break;
            }

        }
        if (unlocked && !m_opened)
        {
            print("try open");
            m_opened = true;
            StartCoroutine(OpenDoor());
        }

    }


    IEnumerator OpenDoor()
    {
        print("open");
        for (float i = 0; i < m_openingTime; i+=Time.deltaTime)
        {
            Vector3 angle = m_doorAngel * Time.deltaTime;
            transform.Rotate(angle);
            yield return null;
        }
        print("opened");

    }
}

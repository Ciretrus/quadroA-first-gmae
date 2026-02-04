using System.Collections;
using UnityEngine;

namespace Puzzles 
{
    public class DoorActivation : MonoBehaviour
    {
        [SerializeField] private Vector3 m_doorAngle = new Vector3(90f, 0, 0);
        [SerializeField] private BasePuzzle m_puzzle;
        [SerializeField] private float m_openingTime = 1f;

        private void OnEnable()
        {
            if (m_puzzle != null)
            {
                m_puzzle.m_onSolved += OpenDoor;
            }
        }

        private void OnDisable()
        {
            if (m_puzzle != null)
            {
                m_puzzle.m_onSolved -= OpenDoor;
            }
        }

        private void OpenDoor()
        {
            StartCoroutine(OpenDoorCoroutine());
        }

        private IEnumerator OpenDoorCoroutine()
        {
            print("open");
            for (float i = 0; i < m_openingTime; i += Time.deltaTime)
            {
                Vector3 angle = m_doorAngle * Time.deltaTime;
                transform.Rotate(angle);
                yield return null;
            }
            print("opened");
        }
    }
}
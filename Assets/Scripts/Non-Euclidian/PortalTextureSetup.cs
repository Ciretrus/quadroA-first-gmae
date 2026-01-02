using UnityEngine;

namespace NonEuclidian
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class PortalTextureSetup : MonoBehaviour
    {

        [SerializeField] private List<Camera> m_cameras = new List<Camera>();
        [SerializeField] private List<Material> m_materials = new List<Material>();

        void Start()
        {
            for (int i = 0; i < m_cameras.Count; i++)
            {
                if (m_cameras[i].targetTexture != null)
                {
                    m_cameras[i].targetTexture.Release();
                }
                m_cameras[i].targetTexture = new RenderTexture(Screen.width, Screen.height, 24);
                m_materials[i].mainTexture = m_cameras[i].targetTexture;
            }
        }
    }
}


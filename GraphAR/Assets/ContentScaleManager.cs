using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContentScaleManager : MonoBehaviour {

    public delegate void ContentScaleChanged(float newScale, float prevScale);
    public static ContentScaleChanged ContentScaleChangedEvent;

    [SerializeField]
    private float m_ContentScale = 1.0f;

    void Start()
    {
        if (ContentScaleChangedEvent != null)
            ContentScaleChangedEvent(m_ContentScale, 1.0f);
    }

    public float ContentScale
    {
        get { return m_ContentScale; }
        set
        {
            if (value != m_ContentScale)
            {
                float prevScale = m_ContentScale;
                m_ContentScale = Mathf.Clamp(value, 0.001f, 1000.0f);
                if (ContentScaleChangedEvent != null)
                {
                    ContentScaleChangedEvent(m_ContentScale, prevScale);
                }
            }
        }
    }
}

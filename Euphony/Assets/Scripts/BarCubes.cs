using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarCubes : MonoBehaviour {

    public int m_bar;
    public float m_startScale;
    public float m_scaleMultiplier;
    public bool m_bufferActive;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        //Draw with buffered smooth values...
        if (m_bufferActive)
        {
            transform.localScale = new Vector3(transform.localScale.x,
                                  (AudioPeer.m_boundaryBuffer[m_bar] * m_scaleMultiplier) + m_startScale, transform.localScale.z);
        }

        //or not.
        if (!m_bufferActive)
        {
            transform.localScale = new Vector3(transform.localScale.x,
                                  (AudioPeer.m_freqBoundaries[m_bar] * m_scaleMultiplier) + m_startScale, transform.localScale.z);
        }

        //transform.localPosition = new Vector3(transform.localPosition.x, transform.localScale.y / 2, transform.localPosition.z);
    }
}

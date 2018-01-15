using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarCubes : MonoBehaviour {

    public int m_bar;
    public float m_startScale;
    public float m_scaleMultiplier;
    public bool m_bufferActive;

    Material material;

	// Use this for initialization
	void Start ()
    {
        material = GetComponent<MeshRenderer>().materials[0];
	}
	
	// Update is called once per frame
	void Update () {

        //Draw with buffered smooth values...
        if (m_bufferActive)
        {
            transform.localScale = new Vector3(transform.localScale.x,
                                  (AudioManager.m_freqBoundsBuffer[m_bar] * m_scaleMultiplier) + m_startScale, transform.localScale.z);

            Color cubeColour = new Color(AudioManager.m_rangedBoundsBuffer[m_bar], AudioManager.m_rangedBoundsBuffer[m_bar], AudioManager.m_rangedBoundsBuffer[m_bar]);
            material.SetColor("_EmissionColor", cubeColour);
        }

        //or not.
        if (!m_bufferActive)
        {
            transform.localScale = new Vector3(transform.localScale.x,
                                  (AudioManager.m_freqBounds[m_bar] * m_scaleMultiplier) + m_startScale, transform.localScale.z);

            Color cubeColour = new Color(AudioManager.m_rangedBoundsBuffer[m_bar], AudioManager.m_rangedBoundsBuffer[m_bar], AudioManager.m_rangedBoundsBuffer[m_bar]);
            material.SetColor("_EmissionColor", cubeColour);
        }
    }
}

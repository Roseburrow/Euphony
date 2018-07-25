using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarCubes : MonoBehaviour {

    public int m_bar;
    public float m_startScale;
    public float m_scaleMultiplier;
    public bool m_bufferActive;
    public float colourOffset;

    Material material;

	// Use this for initialization
	void Start ()
    {
        material = GetComponent<MeshRenderer>().materials[0];
	}
	
	// Update is called once per frame
	void Update ()
    {
        //Draw with buffered smooth values...
        if (m_bufferActive)
        {
            transform.localScale = new Vector3(transform.localScale.x,
                                  (AudioBandBuffer.m_freqBoundsBuffer[m_bar] * m_scaleMultiplier) + m_startScale, transform.localScale.z);
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localScale.y / 2, transform.localPosition.z);

            Color cubeColour = new Color(RangedBandBuffer.m_rangedBoundsBuffer[m_bar] - colourOffset,
                                         RangedBandBuffer.m_rangedBoundsBuffer[m_bar] - colourOffset,
                                         RangedBandBuffer.m_rangedBoundsBuffer[m_bar] - colourOffset);
            material.SetColor("_EmissionColor", cubeColour);
        }

        //or not.
        if (!m_bufferActive)
        {
            transform.localScale = new Vector3(transform.localScale.x,
                                  (AudioBands.m_freqBounds[m_bar] * m_scaleMultiplier) + m_startScale, transform.localScale.z);
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localScale.y / 2, transform.localPosition.z);

            Color cubeColour = new Color(RangedBandBuffer.m_rangedBoundsBuffer[m_bar],
                                         RangedBandBuffer.m_rangedBoundsBuffer[m_bar],
                                         RangedBandBuffer.m_rangedBoundsBuffer[m_bar]);
            material.SetColor("_EmissionColor", cubeColour);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sampler : MonoBehaviour
{
    public static AudioSource m_AudioSource; //Unity class for storing audio files.
    public int m_SamplesTaken;

    public static float[] m_SamplesLeft;
    public static float[] m_SamplesRight;

    // Use this for initialization
    void Start ()
    {
        m_AudioSource = GetComponent<AudioSource>();
        m_SamplesLeft = new float[m_SamplesTaken];
        m_SamplesRight = new float[m_SamplesTaken];
    }
	
	// Update is called once per frame
	void Update ()
    {
        //This is essentially our audio stream...
        m_AudioSource.GetSpectrumData(m_SamplesLeft, 0, FFTWindow.BlackmanHarris);
        m_AudioSource.GetSpectrumData(m_SamplesRight, 1, FFTWindow.BlackmanHarris);
    }
}

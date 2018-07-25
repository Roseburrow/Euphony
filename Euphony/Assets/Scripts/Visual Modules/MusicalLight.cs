using UnityEngine;

public class MusicalLight : MonoBehaviour {

    Light m_light;

    [Header("Frequency Bar")]
    public int m_bar;

    [Space]

    [Header("Intensity")]
    public float m_MinIntensity;
    public float m_MaxIntensity;

    [Space]

    [Header("Range")]
    public float m_MinRange;
    public float m_MaxRange;
    

	// Use this for initialization
	void Start () 
	{
        m_light = GetComponent<Light>();
	}
	
	// Update is called once per frame
	void Update () 
	{
        m_light.intensity = (RangedBandBuffer.m_rangedBoundsBuffer[m_bar] * 
                            (m_MaxIntensity - m_MinIntensity)) + m_MinIntensity;
        m_light.range = (RangedBandBuffer.m_rangedBoundsBuffer[m_bar] *
                            (m_MaxRange- m_MinRange)) + m_MinRange;
                            

    }
}

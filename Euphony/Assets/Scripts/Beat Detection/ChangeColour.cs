using Assets.Scripts.Beat_Detection;
using UnityEngine;

public class ChangeColour : MonoBehaviour {

    public Color m_Colour;
    Color m_BeatColour;
    Material m_Material;
    public float smoothnessChange;

	// Use this for initialization
	void Start () 
	{
        m_Material = GetComponent<MeshRenderer>().materials[0];
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (BeatDetector.m_Beat)
        {
            m_BeatColour = m_Colour;
            m_Material.SetColor("_EmissionColor", m_BeatColour);
        }
        else
        {
            m_BeatColour = Color.Lerp(m_BeatColour, Color.black, smoothnessChange * Time.deltaTime); ;
            m_Material.SetColor("_EmissionColor", m_BeatColour);
        }
	}
}

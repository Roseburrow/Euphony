using UnityEngine;

public class BandedChangeColour : MonoBehaviour {

    public Color m_BeatColour;
    public Color storedColour;
    Material m_Material;
    public float smoothnessChange;
    public int bandNumber;

	// Use this for initialization
	void Start () 
	{
        m_Material = GetComponent<MeshRenderer>().materials[0];
	}
	
	// Update is called once per frame
	void Update () 
	{
        if (BandedBeatDetection.Bands[bandNumber].IsBeat())
        {
            m_BeatColour = storedColour;
            m_Material.SetColor("_EmissionColor", m_BeatColour);
        }
        else
        {
            m_BeatColour = Color.Lerp(m_BeatColour, Color.black, smoothnessChange * Time.deltaTime); ;
            m_Material.SetColor("_EmissionColor", m_BeatColour);
        }
	}

    private void LateUpdate()
    {
        
    }
}

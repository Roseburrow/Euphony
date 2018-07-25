using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTiles : MonoBehaviour {

    public GameObject panel;
    public GameObject detector;

    private Vector3 offset;
    private int panelsNum;
	// Use this for initialization
	void Start ()
    {
        Color panelColour = Color.clear;
        BandedBeatDetection bbd = detector.GetComponent<BandedBeatDetection>();
        panelsNum = bbd.m_NumberOfBands;

        offset = new Vector3(0, 0, 0);

        for (int i = 0; i < panelsNum; i++)
        {
            panel.GetComponent<BandedChangeColour>().bandNumber = i;

            if (i % 2 == 0)
            {
                panelColour = Color.blue;
            }
            else
            {
                panelColour = Color.yellow;
            }

            if (i % 8 == 0)
            {
                offset.z -= 1;
                offset.x = 0;
            }
            panel.GetComponent<BandedChangeColour>().storedColour = panelColour;
            Instantiate(panel, transform.position + offset, transform.rotation);
            offset.x += 1;
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}
}

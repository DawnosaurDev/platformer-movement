using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ColourSwitcher : MonoBehaviour
{
	/*
	This scripts was just for recording purposes you can ignore <3
	*/

	private int currentColour;
	public Color[] camColours;
	public Color[] groundColours;
	public Camera cam;
	public Tilemap tilemap;

	private void Update()
	{
		if(Input.GetKeyDown(KeyCode.X))
		{
			currentColour++;
			if(currentColour >= camColours.Length)
			{
				currentColour = 0;
			}

			cam.backgroundColor = camColours[currentColour];
			tilemap.color = groundColours[currentColour];
		}
	}
}

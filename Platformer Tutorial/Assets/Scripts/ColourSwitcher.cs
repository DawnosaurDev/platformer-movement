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
	private int currentPlayer;
	public GameObject[] players;
	public GameObject[] playerUI;
	private int currentLevel;
	public GameObject[] levels;
	public Vector3[] pos;

	public Camera cam;
	public Tilemap tilemap;

	private void Start()
	{
		tilemap = levels[currentLevel].GetComponent<Tilemap>();
	}

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

			currentPlayer++;
			if (currentPlayer >= players.Length)
			{
				currentPlayer = 0;
			}

			for (int i = 0; i < players.Length; i++)
			{
				players[i].SetActive(false);
				playerUI[i].SetActive(false);
			}

			players[currentPlayer].SetActive(true);
			playerUI[currentPlayer].SetActive(true);
		}

		if(Input.GetKeyDown(KeyCode.Z))
		{
			currentLevel++;
			if (currentLevel >= levels.Length)
			{
				currentLevel = 0;
			}

			for (int i = 0; i < levels.Length; i++)
			{
				levels[i].SetActive(false);
			}

			levels[currentLevel].SetActive(true);
			players[currentPlayer].transform.position = pos[currentLevel];

			tilemap = levels[currentLevel].GetComponent<Tilemap>();
			cam.backgroundColor = camColours[currentColour];
			tilemap.color = groundColours[currentColour];

		}
	}
}

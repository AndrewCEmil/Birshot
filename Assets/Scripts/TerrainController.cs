﻿using UnityEngine;
using System.Collections;

public class TerrainController : MonoBehaviour {

	private Terrain terrain;
	private TerrainData terrainData;
	private bool hasGenerated;
	// Use this for initialization
	void Start () {
		print (gameObject.ToString());
		terrain = gameObject.GetComponent<Terrain>();
		terrainData = terrain.terrainData;
		hasGenerated = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (!hasGenerated) {
			float hmWidth = terrainData.heightmapWidth;
			float hmHeight = terrainData.heightmapHeight;
			float[,] heights = terrainData.GetHeights (0, 0, (int)hmWidth, (int)hmHeight);
			float current = 0f;
			for (float i = 0; i < hmWidth; i++) {
				for (float j = 0; j < hmHeight; j++) {
					current = Mathf.PerlinNoise(i / hmWidth ,j / hmHeight);
					heights [(int)i, (int)j] = current;
				}
			}
			terrainData.SetHeights (0, 0, heights);
			hasGenerated = true;
		}
	}
}
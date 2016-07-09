using UnityEngine;
using System.Collections;

public class TerrainController : MonoBehaviour {

	public GameObject player;

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
		Vector3 max = new Vector3 (0,0,0);
		if (!hasGenerated) {
			float hmWidth = terrainData.heightmapWidth;
			float hmHeight = terrainData.heightmapHeight;
			float[,] heights = terrainData.GetHeights (0, 0, (int)hmWidth, (int)hmHeight);
			float current = 0f;
			for (float i = 0; i < hmWidth; i++) {
				for (float j = 0; j < hmHeight; j++) {
					current = Mathf.PerlinNoise(i / hmWidth ,j / hmHeight);
					heights [(int)i, (int)j] = current;
					if (current > max.y) {
						max.x = i;
						max.y = current;
						max.z = j;
					}
				}
			}
			terrainData.SetHeights (0, 0, heights);
			hasGenerated = true;
			//Vector3 newPos = new Vector3 (terrain.GetPosition ().x, heights[(int) terrain.GetPosition().x, (int) terrain.GetPosition().z], terrain.GetPosition ().z);
			//Vector3 newPos = new Vector3 (100, terrain.SampleHeight(new Vector3(100, 100, 100)), 100);
			max.y = terrain.SampleHeight(max);
			player.transform.position = max;
		}
	}
}

using UnityEngine;
using System.Collections;

public class TerrainController : MonoBehaviour {

	public GameObject player;

	private Terrain terrain;
	private TerrainData terrainData;
	private bool hasGenerated;
	private float hmWidth;
	private float hmHeight;
	private float mWidth;
	private float mHeight;
	// Use this for initialization
	void Start () {
		print (gameObject.ToString());
		terrain = gameObject.GetComponent<Terrain>();
		terrainData = terrain.terrainData;
		hasGenerated = false;
		hmWidth = terrainData.heightmapWidth;
		hmHeight = terrainData.heightmapHeight;
		mWidth = 250;
		mHeight = 250;
	}
	
	// Update is called once per frame
	void Update () {
		if (!hasGenerated) {

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
			//Vector3 newPos = new Vector3 (terrain.GetPosition ().x, heights[(int) terrain.GetPosition().x, (int) terrain.GetPosition().z], terrain.GetPosition ().z);
			//Vector3 newPos = new Vector3 (100, terrain.SampleHeight(new Vector3(100, 100, 100)), 100);
			player.transform.position = getMaxHeight();
		}
	}

	Vector3 getMaxHeight() {
		Vector3 maxPos = new Vector3 (0, 0, 0);
		Vector3 currentPos = new Vector3 (0, 0, 0);
		for (float i = 0; i < mWidth; i++) {
			for (float j = 0; j < mHeight; j++) {
				currentPos.x = i;
				currentPos.z = j;
				currentPos.y = terrain.SampleHeight (currentPos);
				if (currentPos.y > maxPos.y) {
					maxPos.x = currentPos.x;
					maxPos.y = currentPos.y;
					maxPos.z = currentPos.z;
				}
			}
		}
		return maxPos;
	}
}

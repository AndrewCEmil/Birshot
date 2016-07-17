using UnityEngine;
using System.Linq;
using System.Collections;

public class TerrainController : MonoBehaviour {

	public GameObject player;
	public GameObject target;

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
		mWidth = terrainData.size.x;
		mHeight = terrainData.size.z;
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
			player.transform.position = getMaxHeight();
			setTargets ();
			splat ();
		}
	}

	void setTargets() {
		GameObject newTarget;
		for (int i = 0; i < 30; i++) {
			Vector3 pos = new Vector3 ((float) Random.Range (0, mWidth), 0f, (float) Random.Range (0, mHeight));
			pos.y = terrain.SampleHeight (pos);
			newTarget = Instantiate (target);
			newTarget.transform.position = pos;
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

	//https://alastaira.wordpress.com/2013/11/14/procedural-terrain-splatmapping/
	void splat() {
		// Splatmap data is stored internally as a 3d array of floats, so declare a new empty array ready for your custom splatmap data:
		float[, ,] splatmapData = new float[terrainData.alphamapWidth, terrainData.alphamapHeight, terrainData.alphamapLayers];

		for (int y = 0; y < terrainData.alphamapHeight; y++) {
			for (int x = 0; x < terrainData.alphamapWidth; x++) {
				// Normalise x/y coordinates to range 0-1 
				float y_01 = (float)y/(float)terrainData.alphamapHeight;
				float x_01 = (float)x/(float)terrainData.alphamapWidth;

				// Sample the height at this location (note GetHeight expects int coordinates corresponding to locations in the heightmap array)
				float height = terrainData.GetHeight(Mathf.RoundToInt(y_01 * terrainData.heightmapHeight),Mathf.RoundToInt(x_01 * terrainData.heightmapWidth)) / terrainData.heightmapHeight;

				// Calculate the normal of the terrain (note this is in normalised coordinates relative to the overall terrain dimensions)
				Vector3 normal = terrainData.GetInterpolatedNormal(y_01,x_01);

				// Calculate the steepness of the terrain
				float steepness = terrainData.GetSteepness(y_01,x_01);

				// Setup an array to record the mix of texture weights at this point
				float[] splatWeights = new float[terrainData.alphamapLayers];

				// CHANGE THE RULES BELOW TO SET THE WEIGHTS OF EACH TEXTURE ON WHATEVER RULES YOU WANT


				// Water
				if (height < .4) {
					splatWeights [0] = 1;
				} else {
					splatWeights [0] = 0;
				}

				// Grass
				if (height >= .4 && height < .6) {
					splatWeights [1] = 1;
				} else {
					splatWeights [1] = 0;
				}

				// Ice
				if (height >= .6) {
					splatWeights [2] = 1;
				} else {
					splatWeights [2] = 0;
				}

				// Sum of all textures weights must add to 1, so calculate normalization factor from sum of weights
				float z = splatWeights.Sum();

				// Loop through each terrain texture
				for(int i = 0; i<terrainData.alphamapLayers; i++){

					// Normalize so that sum of all texture weights = 1
					splatWeights[i] /= z;

					// Assign this point to the splatmap array
					splatmapData[x, y, i] = splatWeights[i];
				}
			}
		}

	// Finally assign the new splatmap to the terrainData:
		terrainData.SetAlphamaps(0, 0, splatmapData);
	}
}

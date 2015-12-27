// MOVE SPHERE EITHER RANDOMLY OR ON A GRID AND CAPTURE LOCATION DATA

// 1. ATTACH THIS SCRIPT TO THE SPHERE OBJECT IN THE SCENE

using UnityEngine;
using System.Collections;
using System.IO;
using System;
using RandomU = UnityEngine.Random;



public class sphereScript2 : MonoBehaviour {

	// parameters to change
	public static float nearZ = 2;
	private static float incrementZ = .5f;
	private static int numZPlanes = 40;
	private int numStepsPerSide = 10;
	public static float farZ = nearZ + (numZPlanes * incrementZ);
	private string locFilename = @"/Users/mettinger/Data/sphereAndCube/sphereLocationData.txt";

	public static int resolutionX = 200;
	public static int resolutionY = 200;
	public static float fieldOfView = 80.0f;
	public static string depthFilename = @"/Users/mettinger/Data/sphereAndCube/depthMaps.txt";
	// end parameters to change

	// THESE PARAMETERS ARE FOR THE RANDOM SPHERE VERSION
	//private float randomRangeLimit = 2;
	//private float zLower = 4;
	//private float zUpper = 20;

	private int counter = 0;
	private int width = Screen.width;
	private int height = Screen.height;
	private string positionString;

	public static int dataFlag;
	private Vector3 position;
	private int numSphere;
	private Vector3[] positionArray;

	void makePositionArray() {
		int i = 0;
		for (float z = nearZ; z < farZ; z+= incrementZ) {
			float minX = - ( float ) Math.Tan((PostProcessDepthGrayscale.fieldOfView / 2.0) * (Math.PI/180.0 )) * z;
			float maxX = -minX;
			float minY = minX;
			float maxY = maxX;
			for (float x = minX; x <= maxX; x += (maxX - minX)/numStepsPerSide) {
				for (float y = minY; y <= maxY; y += (maxY - minY)/numStepsPerSide) {
					positionArray[i] = new Vector3(x,y,z);
					i += 1;
				}
			}
		}
	}

	void saveData(string rowOfData){
		using (StreamWriter outputFile = File.AppendText(locFilename) ) {
			outputFile.WriteLine(rowOfData);
		}
	}

	// Use this for initialization
	void Start () {

		numSphere = (numStepsPerSide + 1) * (numStepsPerSide + 1) * numZPlanes;
		positionArray = new Vector3[numSphere];
		makePositionArray ();
		dataFlag = 1;

		try {
			File.Delete (locFilename);
		}
		catch {
		}

		saveData(width.ToString () + "," + height.ToString () + "," + 0.0.ToString());
	}


	// Update is called once per frame
	void Update () {

		/*  THIS IS FOR RECORDING THE RANDOM SPHERE VERSION
		if (counter < PostProcessDepthGrayscale.numData) {
			// randomly move sphere
			Vector3 position = new Vector3(RandomU.Range(-randomRangeLimit, randomRangeLimit), 
			                               RandomU.Range(-randomRangeLimit, randomRangeLimit), 
			                               RandomU.Range(zLower, zUpper));
			transform.position = position;
			positionString = position.ToString("F4");
			saveData (positionString.Substring(1,positionString.Length - 2));
			counter ++;
		} */

		// THIS IS FOR RECORDING THE REGULARLY SPACED SPHERE VERSION
		if (counter == numSphere) {
			dataFlag = 0;
		}
		if (dataFlag == 1) {
			position = positionArray [counter];
			transform.position = position;
			positionString = position.ToString ("F4");
			saveData (positionString.Substring (1, positionString.Length - 2));
			counter += 1;
		}
	}
}

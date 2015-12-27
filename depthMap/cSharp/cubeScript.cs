using UnityEngine;
using System.Collections;
using System.IO;
using System;
using RandomU = UnityEngine.Random;

public class cubeScript : MonoBehaviour {
	
	private int counter = 0;
	public int cubeLimit;
	private string rowOfData;
	public float cubeZdistance;
	public float randomRangeLimit;
	public static int numSamplesPerSide=40;  // square for now
	private float[,] depth = new float[numSamplesPerSide, numSamplesPerSide];
	private Ray[,] rays = new Ray[numSamplesPerSide,numSamplesPerSide];
	
	// initialize the ray array for raycasts
	void initRays() {
		Ray ray;
		Vector3 rayTarget;
		float maxAngle = Mathf.Atan(randomRangeLimit/cubeZdistance);
		float theta1,theta2;
		float angleIncrement = (2 * maxAngle) / (numSamplesPerSide - 1);
		
		for (int i = 0; i < numSamplesPerSide; i++) {
			theta1 = -maxAngle + (i * angleIncrement);
			for (int j = 0; j < numSamplesPerSide; j++) {
				theta2 = -maxAngle + (j * angleIncrement);
				rayTarget = rayTargetGet(theta1,theta2);
				ray = new Ray (new Vector3(0.0f,0.0f,0.0f), rayTarget);
				rays[i,j] = ray;
			}
		}
	}
	
	// for fixed theta1, theta2 calculate the ray for raycast
	Vector3 rayTargetGet(float theta1, float theta2) {
		theta1 = theta1 * 180 / ( float)Math.PI;
		theta2 = theta2 * 180 / (float) Math.PI;
		Vector3 xzPlaneVector = new Vector3 ();
		xzPlaneVector = Quaternion.AngleAxis(theta1, Vector3.up) * Vector3.forward;
		Vector3 rotationAxis = Vector3.Cross (xzPlaneVector, Vector3.up).normalized;
		return Quaternion.AngleAxis(theta2, rotationAxis) * xzPlaneVector;
	}
	
	// get all the raycast data (once per update)
	void rayCastDataGet() {
		Ray ray;
		RaycastHit hit;
		rowOfData = "0"; // 0 label indicates cube
		for (int i = 0; i < numSamplesPerSide; i++) {
			for (int j = 0; j < numSamplesPerSide; j++) {
				ray = rays[i,j];
				if (Physics.Raycast(ray, out hit )) {
					depth[i, j] = hit.point.magnitude;
				}
				else {
					depth[i,j] = -1;
				}
				rowOfData += "," + Convert.ToString(depth[i,j]);
			}
		}
	}
	
	// write raycast info to file (with class label)
	void saveRayTraces(string rowOfData){
		using (StreamWriter outputFile = File.AppendText(@"/Users/mettinger/Desktop/sphereCubeData.txt") ) {
			outputFile.WriteLine(rowOfData);
		}
	}
	
	// Use this for initialization
	void Start () {
		initRays ();
	}
	
	// Update is called once per frame
	void Update () {
		if (counter < cubeLimit) {
			Debug.Log(counter.ToString());
			// randomly move  and rotate cube
			Vector3 position = new Vector3(RandomU.Range(-randomRangeLimit, randomRangeLimit), RandomU.Range(-randomRangeLimit, randomRangeLimit), cubeZdistance);
			transform.position = position;
			transform.rotation = RandomU.rotation;
			rayCastDataGet();
			saveRayTraces (rowOfData);
			counter ++;
		}
	}
}

// MOVE SPHERE EITHER RANDOMLY OR ON A GRID AND CAPTURE DEPTH DATA

//  1. ATTACH THIS SCRIPT TO THE CAMERA IN THE SCENE

using UnityEngine;
using System.Collections;
using System.IO;
using System;

//so that we can see changes we make without having to run the game

//[ExecuteInEditMode]
public class PostProcessDepthGrayscale : MonoBehaviour {

	// parameters set automatically DO NOT CHANGE
	private int resolutionX = sphereScript2.resolutionX;
	private int resolutionY = sphereScript2.resolutionY;
	public static float fieldOfView = sphereScript2.fieldOfView;
	private string filename = sphereScript2.depthFilename;
	private float nearClipPlane = sphereScript2.nearZ - 1.0f;
	private float farClipPlane = sphereScript2.farZ + 2.0f;

	// paramters for random sphere version
	//private int counter = 0;
	//public static int numData = 100000;

	public Material mat;

	private int width = Screen.width;
	private int height = Screen.height;

	void saveData(string rowOfData){
		using (StreamWriter outputFile = File.AppendText(filename) ) {
			outputFile.WriteLine(rowOfData);
		}
	}

	void Start () {
		GetComponent<Camera>().depthTextureMode = DepthTextureMode.Depth;
		GetComponent<Camera> ().fieldOfView = fieldOfView;
		GetComponent<Camera> ().nearClipPlane = nearClipPlane;
		GetComponent<Camera> ().farClipPlane = farClipPlane;
		Screen.SetResolution (resolutionX, resolutionY, false);

		try {
			File.Delete (filename);
		}
		catch {
		}
	}

	void OnRenderImage (RenderTexture source, RenderTexture destination){
		
		Graphics.Blit(source,destination,mat);
		//mat is the material which contains the shader
		//we are passing the destination RenderTexture to

		//  read the depth data from the shader into Texture2D
		Texture2D tex = new Texture2D(width, height);
		tex.ReadPixels(new Rect(0, 0, width, height), 0, 0);

		/*  THIS IS FOR RECORDING RANDOMLY CHOOSEN SPHERES
		// read the depth data from Texture2D into array and write to file
		if (counter < numData) {
			string[] blue = new string[width * height];
			
			for (int i = 0; i < height; i++) {
				for (int j=0; j < width; j++) {
					blue[(i * width) + j]  = tex.GetPixel (j, i).b.ToString ("F4");
				}
			}
			string output = string.Join(",", blue);
			saveData(output);
			counter++;
		}*/

		//  THIS IS FOR RECORDING DEPTH FOR REGULARLY SPACED SPHERES
		if (sphereScript2.dataFlag == 1) { 
			string[] blue = new string[width * height];
			
			for (int i = 0; i < height; i++) {
				for (int j=0; j < width; j++) {
					blue [(i * width) + j] = tex.GetPixel (j, i).b.ToString ("F4");
				}
			}
			string output = string.Join (",", blue);
			saveData (output);
		}
		
	}
	
}
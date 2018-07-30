using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace TextureSendReceive {
	[CustomEditor(typeof(WebcamManager))]
	public class WebcamEditor : Editor {
		string[] webcams = new string[0];
		int webcam = 0;

		public override void OnInspectorGUI() {
			DrawDefaultInspector();

			// Update list of available webcams
			webcams = new string[WebCamTexture.devices.Length];
			for (int i = 0; i < webcams.Length; i++) {
				webcams[i] = WebCamTexture.devices[i].name;
         	}

			// add GUI popup
			webcam = EditorGUILayout.Popup("Webcam", webcam, webcams);

			// Update property value in target class instance
			WebcamManager webcamManager = target as WebcamManager;
			webcamManager.webcamIndex = webcam;
     		
			 // Save the changes back to the object
     		EditorUtility.SetDirty(target);
		}
	}

	public class WebcamManager : MonoBehaviour {
    	[HideInInspector]
		public int webcamIndex = 0;
		public int width = 640;
		public int height = 480;
		public int fps = 30;

		[HideInInspector]
		public WebCamTexture texture;
		[HideInInspector]

		// Use this for initialization
		void Start () {
			// List connected cameras in console
         	for (int i = 0; i < WebCamTexture.devices.Length; i++) {
				print ("Webcam " + i + " available: " + WebCamTexture.devices[i].name);
         	}

			SetSelectedWebcam();
		}

		void SetSelectedWebcam() {
			// Stop previous webcam connection
			if(texture != null) texture.Stop();

			// assign and start selected webcam
        	texture = new WebCamTexture(WebCamTexture.devices[webcamIndex].name, width, height, fps);
        	texture.Play();
		}


        private void OnApplicationQuit() {
            if (texture != null && texture.isPlaying) {
                texture.Stop();
            }
        }		
	}
}
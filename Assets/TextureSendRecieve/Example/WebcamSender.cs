using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TextureSendReceive {
	[RequireComponent(typeof(WebcamManager), typeof(TextureSender))]
	public class WebcamSender : MonoBehaviour {
		WebcamManager webcam;
		TextureSender sender;
		Texture2D sendTexture;

		public RawImage image;

		
		bool ready = false;

		// Use this for initialization
		void Start () {
			webcam = GetComponent<WebcamManager>();
			sender = GetComponent<TextureSender>();

			sendTexture = new Texture2D(webcam.width, webcam.height);
		}
		
		// Update is called once per frame
		void Update () {
			if(!ready) {
				if(webcam.texture) {
					// Set preview image target
					image.texture = webcam.texture;

					// update sendTexture with pixels from webcamTexture
					sendTexture.SetPixels(webcam.texture.GetPixels());	
					
					// Set send texture
					sender.SetSourceTexture(sendTexture);

					// Mark as ready
					ready = true;
				}
			} else {
				// update sendTexture with pixels from webcamTexture
				sendTexture.SetPixels(webcam.texture.GetPixels());
			}
		}
	}
}
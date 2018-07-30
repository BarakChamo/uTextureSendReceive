using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

namespace TextureSendReceive {
	[RequireComponent(typeof(VideoPlayer), typeof(TextureSender))]
	public class VideoFileSender : MonoBehaviour {
		VideoPlayer videoPlayer;
		TextureSender sender;
		Texture2D sendTexture;
		RenderTexture videoTexture;

		public RawImage image;

		// Use this for initialization
		void Start () {
			videoPlayer = GetComponent<VideoPlayer>();
			sender = GetComponent<TextureSender>();

			sendTexture = new Texture2D((int)videoPlayer.clip.width, (int)videoPlayer.clip.height);
			videoTexture = new RenderTexture((int)videoPlayer.clip.width, (int)videoPlayer.clip.height,24);

			videoPlayer.renderMode = VideoRenderMode.RenderTexture;
			videoPlayer.targetTexture = videoTexture;

			videoPlayer.Prepare();
			videoPlayer.Play();
		}
		
		// Update is called once per frame
		void Update () {
			if(videoPlayer.isPlaying) {
				RenderTexture.active = videoTexture;
				sendTexture.ReadPixels(new Rect(0, 0, videoTexture.width, videoTexture.height), 0, 0, false);
				
				// Set preview image target
				image.texture = videoTexture;

				// update sendTexture with pixels from webcamTexture
				// sendTexture.SetPixels(sendTexture.GetPixels());	
				
				// Set send texture
				sender.SetSourceTexture(sendTexture);
			} else {
				videoPlayer.Play();
			}
		}
	}
}
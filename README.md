# uTextureSendReceive
###### Unity threaded network texture sender and receiver plugin for video and texture streaming

This is a small package that provides a threaded interface for sending and receiving any Unity texture over TCP/IP.
It can be used to stream video, webcams as well as any 2D or Render texture, like parts of your visuals, minimaps, etc.

### Features
1. Simple texture-based interface for sending images/video/textures/etc. over TCP/IP.
2. Threaded sending and receiving for improved performance of the rest of the project.
3. Configurable encoding and quality settings.
4. Example Webcam and Video file components and scenes (Game texture sending example coming soon).

### Installation
To install the package head to the [releases section](https://github.com/BarakChamo/uTextureSendReceive/releases) and download the latest release `.unitypackage`.

Follow [these instructions](https://docs.unity3d.com/Manual/AssetPackages.html) to import the custom package into your project.
The package should show up under `TextureSendReceive` in your project's `Assets folder`.

### Example scenes
Example usage scenes are provided in the `Example` folder.

All projects are set up with a sender and a receiver and a 20,000-count particle-system that runs smoothly when sending and receiving 720p video on an MSI laptop with a 260GhZ i7 and GTX970M running Windows 10. 

#### Webcam streaming example
The webcam example sends a receives a `WebCamTexure` and is included with a small utility to capture webcam textures and select the webcam you want to use.

#### Video file streaming example
The video file example sends a receives a RenderTexture from a `VideoPlayer`.

#### Camera texture streaming example
`Coming soon.`

### Setup and Usage
To use the 

#### TextureSender
#### TextureReceiver

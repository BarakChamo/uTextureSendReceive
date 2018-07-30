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


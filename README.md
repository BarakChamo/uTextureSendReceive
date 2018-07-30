# uTextureSendReceive
###### Unity threaded network texture sender and receiver plugin for video and texture streaming

This is a small package that provides a threaded interface for sending and receiving any Unity texture over TCP/IP.
It can be used to stream video, webcams as well as any 2D or Render texture, like parts of your visuals, minimaps, etc.

## Features
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

Check out the `WebcamSender` file to learn how to use `uTextureSendReceive` with a webcam and Unity's `WebCamTexture`.

#### Video file streaming example
The video file example sends a receives a RenderTexture from a `VideoPlayer`.

Check out the `VideoFileSender` file to learn how to use `uTextureSendReceive` with a `VideoPlayer`, `VideoClip`s and Unity's `RenderTexture` mode for video playback.

#### Camera texture streaming example
`Coming soon.`

## Setup and Usage
To begin using the package add a `TextureReceiver` or `TextureSender` component to your project.
Both components share some common properties, such as the `port` used for TCP/IP connection and the `Message byte length` that indicates the size of each video chunk sent and received.

All classes and properties are encapsulated under the `TextureSendReceive` namespace. 

#### TextureSender
The texture sender component accepts connection from receivers for streaming and, encodes textures and sends them over TCP/IP.
Once the `sendTexture`, an instance of `2DTexture`, is assigned with a `SetSourceTexture(Texture2D sendTexture)` the TextureSender
will send the contents of the texture once per frame.

##### Configuration
Begin by adding a `TextureSender` component to a new or existing `GameObject` and configure the `port` and `Encoding`. By default, the component is configured to encode textures as JPGs. It's recommended that you stick to JPGs unless strictly required as this cuts the encoding size by roughly half.

##### Scripting
Streaming a texture is done in code and only requires a single line for setting the streaming source texture.
The `SetSourceTexture(Texture2D sendTexutre)` accepts an instance or `2DTexture` and will continuously read from it once it's initialized.

```c#
using TextureSendReceive;

...

TextureSender sender;
Texture2D sendTexture;

void Start () {
  // Get sender instance
  sender = GetComponent<TextureSender>();
  
  // Initialize Texture2D, in this case with webcamTexture dimensions
  sendTexture = new Texture2D(webcam.width, webcam.height);
  
  // Set send texture
  sender.SetSourceTexture(sendTexture);
}
...
```

#### TextureReceiver
The texture sender component connects to remote senders and accepts texture frames over TCP/IP, currently there's no distinction between senders to handle multiple sources but that might change in the future.

Once the `receiveTexture`, an instance of `2DTexture`, is assigned with a `SetTargetTexture(Texture2D sendTexture)` the TextureReceiver
will write each received frame to the target texture. Note that the texture will be automatically resized to fit the received frame.

##### Configuration
Begin by adding a `TextureReceiver` component to a new or existing `GameObject` and configure the `ip`, `port` and `Encoding`. By default, the component is configured to send over `localhost` (`127.0.0.1`) and you can see a working setup of local streaming in the example scenes.

##### Scripting
Receiving a texture, like streaming, is done in code and only requires a single line for setting the target receiving texture.
The `SetTargetTexture(Texture2D sendTexutre)` accepts an instance or `2DTexture` and will continuously write to it as new frames are received.

```c#
using TextureSendReceive;

...

TextureReceiver receiver;
Texture2D targetTexture;

void Start () {
  // Get sender instance
  receiver = GetComponent<TextureReceiver>();

  // initialize new target texture
  targetTexture = new Texture2D(1, 1);
			
  // Set target texture
  receiver.SetTargetTexture(targetTexture);
}
...
```

## Usage tips
#### Sending and receiving multiple sources
You can initialize as many `TextureReceiver` and `TextureSender`s as you want and handle more than one receiving or sending source.
An example for multi-sending and multi-receiving will be added soon.

#### Switching sources and targets
The source and target textures can be swapped on the fly by making calls to `SetSourceTexture` and `SetTargetTexture` at runtime.

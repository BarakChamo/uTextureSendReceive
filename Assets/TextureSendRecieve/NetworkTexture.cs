using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace TextureSendReceive {
    public enum Encoding {
        JPG = 0,
        PNG = 1
    }
    public class _TextureSender : MonoBehaviour {
        Texture2D source;
        private TcpListener listner;
        private List<TcpClient> clients = new List<TcpClient>();
        private bool stop = false;

        public int port = 5000;
        public Encoding encoding = Encoding.JPG;


        [Header("Must be the same in sender and receiver")]
        public int messageByteLength = 24;

        private void Start() {
            Application.runInBackground = true;

            //Start coroutine
            StartCoroutine(initAndWaitForTexture());
        }

        public void SetSourceTexture(Texture2D t) {
            source = t;
        }


        //Converts the data size to byte array and put result to the fullBytes array
        void byteLengthToFrameByteArray(int byteLength, byte[] fullBytes) {
            //Clear old data
            Array.Clear(fullBytes, 0, fullBytes.Length);
            //Convert int to bytes
            byte[] bytesToSendCount = BitConverter.GetBytes(byteLength);
            //Copy result to fullBytes
            bytesToSendCount.CopyTo(fullBytes, 0);
        }

        IEnumerator initAndWaitForTexture() {
            while (source == null) {
                yield return null;
            }

            // Connect to the server
            listner = new TcpListener(IPAddress.Any, port);

            listner.Start();

            //Start sending coroutine
            StartCoroutine(senderCOR());
        }

        WaitForEndOfFrame endOfFrame = new WaitForEndOfFrame();
        IEnumerator senderCOR() {
            bool isConnected = false;
            TcpClient client = null;
            NetworkStream stream = null;

            // Wait for client to connect in another Thread 
            Loom.RunAsync(() => {
                while (!stop) {
                    // Wait for client connection
                    client = listner.AcceptTcpClient();
                    // We are connected
                    clients.Add(client);

                    isConnected = true;
                    stream = client.GetStream();
                }
            });

            //Wait until client has connected
            while (!isConnected) {
                yield return null;
            }

            bool readyToGetFrame = true;

            byte[] frameBytesLength = new byte[messageByteLength];

            while (!stop) {
                //Wait for End of frame
                yield return endOfFrame;
                byte[] imageBytes = EncodeImage();
                
                //Fill total byte length to send. Result is stored in frameBytesLength
                byteLengthToFrameByteArray(imageBytes.Length, frameBytesLength);

                //Set readyToGetFrame false
                readyToGetFrame = false;

                Loom.RunAsync(() => {
                    //Send total byte count first
                    stream.Write(frameBytesLength, 0, frameBytesLength.Length);

                    //Send the image bytes
                    stream.Write(imageBytes, 0, imageBytes.Length);
                    //Sent. Set readyToGetFrame true
                    readyToGetFrame = true;
                });

                //Wait until we are ready to get new frame(Until we are done sending data)
                while (!readyToGetFrame) {
                    yield return null;
                }
            }
        }

        private byte[] EncodeImage() {
            if(encoding == Encoding.PNG) return source.EncodeToPNG();
            return source.EncodeToJPG(); 
        }

        // stop everything
        private void OnApplicationQuit() {
            if (listner != null) {
                listner.Stop();
            }

            foreach (TcpClient c in clients)
                c.Close();
        }
    }

    public class _TextureReceiver : MonoBehaviour {
        public int port = 5000;
        public string IP = "127.0.0.1";
        TcpClient client;

        [HideInInspector]
        public Texture2D texture;

        private bool stop = false;

        [Header("Must be the same in sender and receiver")]
        public int messageByteLength = 24;

        // Use this for initialization
        void Start() {
            Application.runInBackground = true;

            client = new TcpClient();

            //Connect to server from another Thread
            Loom.RunAsync(() => {                
                // if on desktop
                // client.Connect(IPAddress.Loopback, port);
                client.Connect(IPAddress.Parse(IP), port);

                imageReceiver();
            });
        }
        void imageReceiver() {
            //While loop in another Thread is fine so we don't block main Unity Thread
            Loom.RunAsync(() => {
                while (!stop) {
                    //Read Image Count
                    int imageSize = readImageByteSize(messageByteLength);

                    //Read Image Bytes and Display it
                    readFrameByteArray(imageSize);
                }
            });
        }

        //Converts the byte array to the data size and returns the result
        int frameByteArrayToByteLength(byte[] frameBytesLength) {
            int byteLength = BitConverter.ToInt32(frameBytesLength, 0);
            return byteLength;
        }

        private int readImageByteSize(int size) {
            bool disconnected = false;

            NetworkStream serverStream = client.GetStream();
            byte[] imageBytesCount = new byte[size];
            var total = 0;
            do {
                var read = serverStream.Read(imageBytesCount, total, size - total);
                if (read == 0)
                {
                disconnected = true;
                break;
                }
                total += read;
            } while (total != size);

            int byteLength;

            if (disconnected) {
                byteLength = -1;
            } else {
                byteLength = frameByteArrayToByteLength(imageBytesCount);
            }

            return byteLength;
        }

        private void readFrameByteArray(int size) {
            bool disconnected = false;

            NetworkStream serverStream = client.GetStream();
            byte[] imageBytes = new byte[size];
            var total = 0;
            do {
                var read = serverStream.Read(imageBytes, total, size - total);
                if (read == 0)
                {
                disconnected = true;
                break;
                }
                total += read;
            } while (total != size);

            bool readyToReadAgain = false;

            //Display Image
            if (!disconnected) {
                //Display Image on the main Thread
                Loom.QueueOnMainThread(() => {
                    loadReceivedImage(imageBytes);
                    readyToReadAgain = true;
                });
            }

            //Wait until old Image is displayed
            while (!readyToReadAgain) {
                System.Threading.Thread.Sleep(1);
            }
        }


        void loadReceivedImage(byte[] receivedImageBytes) {
            if(texture) texture.LoadImage(receivedImageBytes);
        }

        public void SetTargetTexture (Texture2D t) {
            texture = t;
        }

        void OnApplicationQuit() {
            stop = true;

            if (client != null) {
            client.Close();
            }
        }
    }
}
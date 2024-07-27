using UnityEngine;
using UnityEngine.UI;
using System.IO;

using System.Collections;
using System;

public class CameraCapture : MonoBehaviour
{
    public RawImage rawImage;
    private WebCamTexture webCamTexture;
    private string filePath;
    Texture2D t;
   // public Text pathtext;


    public void PickImage()
    {
        

        NativeGallery.Permission permission = NativeGallery.GetImageFromGallery((path) =>
        {
            if (path != null)
            {
          
            Texture2D texture = NativeGallery.LoadImageAtPath(path, maxSize: 512);
                if (texture == null)
                {
                    Debug.LogError("Failed to load texture from " + path);
                    return;
                }

           
            rawImage.texture = texture;
                t = texture;
                saveimage();
            }
        }, "Select an image", "image/*");

        if (permission == NativeGallery.Permission.Denied)
        {
            Debug.LogError("Permission denied");
        }

        
    }

    public void saveimage()
    {
        MakeTextureReadable(t);
        SaveImageToLocalStorage(MakeTextureReadable(t), "savedImage.png");
    }



    public void SaveImageToLocalStorage(Texture2D texture, string filename = "savedImage.png")
    {
        
        // Check if texture is not null
        if (texture == null)
        {
            Debug.LogError("Texture is null. Cannot save.");
            return;
        }

        // Convert texture to byte array
        byte[] imageBytes = texture.EncodeToPNG();

        // Generate the path to save the image
        string path = Path.Combine(Application.persistentDataPath, filename);

        // Log the path for debugging purposes

        //pathtext.text = "Saving to path: " + path;
        Debug.Log("Saving to path: " + path);

       
        try
        {
            File.WriteAllBytes(path, imageBytes);
            Debug.Log("Image successfully saved!");
            //pathtext.text = "successfully saved: " + path;

        }
        catch (Exception e)
        {
            //pathtext.text= "Failed to save image: " + e.Message;
            Debug.LogError("Failed to save image: " + e.Message);
        }
    }

    public static Texture2D MakeTextureReadable(Texture2D originalTexture)
    {
        RenderTexture renderTex = RenderTexture.GetTemporary(
                    originalTexture.width,
                    originalTexture.height,
                    0,
                    RenderTextureFormat.Default,
                    RenderTextureReadWrite.Linear);

        Graphics.Blit(originalTexture, renderTex);
        RenderTexture previous = RenderTexture.active;
        RenderTexture.active = renderTex;
        Texture2D readableTexture = new Texture2D(originalTexture.width, originalTexture.height);
        readableTexture.ReadPixels(new Rect(0, 0, renderTex.width, renderTex.height), 0, 0);
        readableTexture.Apply();
        RenderTexture.active = previous;
        RenderTexture.ReleaseTemporary(renderTex);

        return readableTexture;
    }



}

//public void PickImage()
//{
//    NativeGallery.Permission permission = NativeGallery.GetImageFromGallery((path) =>
//    {
//        if (path != null)
//        {
//            // Load the image into a texture and use it in your game
//            Texture2D texture = NativeGallery.LoadImageAtPath(path, maxSize: 512);
//            if (texture == null)
//            {
//                Debug.LogError("Failed to load texture from " + path);
//                return;
//            }

//            // Assign the texture to your UI or object
//            rawImage.GetComponent<Renderer>().material.mainTexture = texture;
//        }
//    }, "Select an image", "image/*");

//    if (permission == NativeGallery.Permission.Denied)
//    {
//        Debug.LogError("Permission denied");
//    }
//}

//private void Start()
//{
//    // File save path set karna
//    filePath = Path.Combine(Application.persistentDataPath, "capturedImage.png");
//}

//public void OpenCamera()
//{



//    if (webCamTexture == null)
//    {
//        WebCamDevice[] devices = WebCamTexture.devices;
//        string cameraName = devices[0].name;
//        webCamTexture = new WebCamTexture(cameraName, 400, 300, 30);

//        rawImage.texture = webCamTexture;  // Yeh line important hai!

//        webCamTexture.Play();
//    }
//}

//public void TakePicture()
//{
//    if (webCamTexture != null && webCamTexture.isPlaying)
//    {
//        StartCoroutine(CaptureFrame());
//    }
//}

//private IEnumerator CaptureFrame()
//{
//    yield return new WaitForEndOfFrame();

//    Texture2D snap = new Texture2D(webCamTexture.width, webCamTexture.height);
//    snap.SetPixels(webCamTexture.GetPixels());
//    snap.Apply();

//    // Image ko RawImage UI component par set karna
//    rawImage.texture = snap;

//    // Image ko file system par save karna
//    File.WriteAllBytes(filePath, snap.EncodeToPNG());
//    Debug.Log("Saved to: " + filePath);

//    // Camera stop karna (agar aap chahte hain ki capture ke baad camera band ho jaye)
//    webCamTexture.Stop();
//    webCamTexture = null;
//}


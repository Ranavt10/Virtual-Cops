using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Video;
using UnityEngine.Networking;

[DefaultExecutionOrder(-99)]
public class VideoPlayerController : MonoBehaviour
{
    public string videoURL = "YOUR_VIDEO_URL";
    private string localVideoPath;
    private VideoPlayer videoPlayer;
    public string persistentPathForVideo;

    void Awake()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        localVideoPath = Path.Combine(Application.persistentDataPath, persistentPathForVideo);

        StartCoroutine(LoadVideo());
    }

    IEnumerator LoadVideo()
    {
        if (File.Exists(localVideoPath))
        {
            // Load the video from local storage
            videoPlayer.url = "file://" + localVideoPath;
            videoPlayer.Prepare();
            while (!videoPlayer.isPrepared)
            {
                yield return null;
            }
            videoPlayer.Play();
        }
        else
        {
            // Download the video
            using (UnityWebRequest www = UnityWebRequest.Get(videoURL))
            {
                yield return www.SendWebRequest();
                if (www.result == UnityWebRequest.Result.Success)
                {
                    File.WriteAllBytes(localVideoPath, www.downloadHandler.data);
                    videoPlayer.url = "file://" + localVideoPath;
                    videoPlayer.Prepare();
                    while (!videoPlayer.isPrepared)
                    {
                        yield return null;
                    }
                    videoPlayer.Play();
                }
                else
                {
                    Debug.LogError("Video download failed: " + www.error);
                }
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class OnGameLoaded : MonoBehaviour
{
     public GameObject _mainCamera;
    private VideoPlayer videoPlayer;

    private void Awake()
    {
        gameObject.SetActive(false);
        videoPlayer = _mainCamera.AddComponent<VideoPlayer>();
        videoPlayer.playOnAwake = false;
        videoPlayer.renderMode = VideoRenderMode.CameraNearPlane;
        videoPlayer.targetCameraAlpha = 1F;
        videoPlayer.url = "C:\\Users\\82334\\Desktop\\MiniGame\\Assets\\Movies\\show.mp4";
        videoPlayer.frame = 100;

        videoPlayer.isLooping = true;
        videoPlayer.loopPointReached += EndReached;
        videoPlayer.Play();
    }

    void EndReached(UnityEngine.Video.VideoPlayer vp)
    {
        vp.playbackSpeed = vp.playbackSpeed / 10.0F;
        gameObject.SetActive(true);
    }
}

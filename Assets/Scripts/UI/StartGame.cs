using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class StartGame : MonoBehaviour
{
    public GameObject _mainCamera;
    private VideoPlayer videoPlayer;
    public Button enterBtn;

    public GameObject canvas;

    private void Awake()
    {
        videoPlayer = _mainCamera.AddComponent<VideoPlayer>();
        videoPlayer.playOnAwake = false;
        videoPlayer.renderMode = VideoRenderMode.CameraNearPlane;
        videoPlayer.targetCameraAlpha = 1F;
        videoPlayer.url = "C:\\Users\\82334\\Desktop\\MiniGame\\Assets\\Movies\\show.mp4";
        videoPlayer.frame = 100;
        
        videoPlayer.isLooping = true;
        videoPlayer.loopPointReached += EndReached;
        enterBtn.onClick.AddListener(() =>
        {

            videoPlayer.Play();
            gameObject.SetActive(false);
            canvas.SetActive(false);
        });
    }

    void EndReached(UnityEngine.Video.VideoPlayer vp)
    {
        vp.playbackSpeed = vp.playbackSpeed / 10.0F;
        canvas.SetActive(true);
    }
}

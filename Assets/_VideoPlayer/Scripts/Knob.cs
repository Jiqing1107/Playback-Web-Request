using UnityEngine;

public class Knob : MonoBehaviour
{
    public GameObject videoPlayer;
    private MyVideoPlayer videoPlayerScript;

    void Start()
    {
        videoPlayerScript = videoPlayer.GetComponent<MyVideoPlayer>();
    }

    void OnMouseDown()
    {
        videoPlayerScript.KnobOnPressDown();
    }

    void OnMouseUp()
    {
        videoPlayerScript.KnobOnRelease();
    }

    void OnMouseDrag()
    {
        videoPlayerScript.KnobOnDrag();
    }

}

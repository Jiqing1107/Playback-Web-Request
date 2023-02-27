using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class PlaybackControllerAPI : MonoBehaviour {
    public string BASE_URL;
    public int clientId;

    public void Start() {
        if (BASE_URL == "") BASE_URL = "http://localhost:5001/Playback/";
        // For testing purposes
        PauseStream();
        SeekToFrame(100);
    }

    // Invoke via GUI button
    public void PauseStream() {
        StartCoroutine(SendClientPBStateRequest(clientId, 1));
    }

    // Invoke via GUI button
    public void PlayStream() {
        StartCoroutine(SendClientPBStateRequest(clientId, 0));
    }

    // Invoke via GUI slider event
    public void SeekToTime(float time) {
        StartCoroutine(SendSeekRequest(clientId, time));
    }

    // Invoke via GUI slider event
    public void SeekToFrame(long frame) {
        StartCoroutine(SendSeekFrameRequest(clientId, frame));
    }

    private IEnumerator SendClientPBStateRequest(int clientId, int state) {
        byte[] data = new byte[sizeof(int) * 2];
        Buffer.BlockCopy(BitConverter.GetBytes(clientId), 0, data, 0, sizeof(int));
        Buffer.BlockCopy(BitConverter.GetBytes(state), 0, data, sizeof(int), sizeof(int));
        string url = $"{BASE_URL}clientPBState?clientId={clientId}&state={state}";
        Debug.Log("The data looks like this:" + BitConverter.ToString(data));
        using (UnityWebRequest request = UnityWebRequest.Put(url, data)) {
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success) {
                Debug.LogError(request.error);
            } else { 
                Debug.Log("Client PB state set to: " + state);
            }
        }
    }

    private IEnumerator SendSeekRequest(int clientId, float time) {
        byte[] data = new byte[sizeof(int) + sizeof(float)];
        Buffer.BlockCopy(BitConverter.GetBytes(clientId), 0, data, 0, sizeof(int));
        Buffer.BlockCopy(BitConverter.GetBytes(time), 0, data, sizeof(int), sizeof(float));
        string url = $"{BASE_URL}seek?clientId={clientId}&time={time}";
        using (UnityWebRequest request = UnityWebRequest.Put(url, data)) {
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success) {
                Debug.LogError(request.error);
            } else {
                Debug.Log("Seeked to time: " + time);
            }
        }
    }

    private IEnumerator SendSeekFrameRequest(int clientId, long frame) {
        byte[] data = new byte[sizeof(int) * 2];
        Buffer.BlockCopy(BitConverter.GetBytes(clientId), 0, data, 0, sizeof(int));
        Buffer.BlockCopy(BitConverter.GetBytes(frame), 0, data, sizeof(int), sizeof(long));
        string url = $"{BASE_URL}seekframe?clientId={clientId}&frame={frame}";
        using (UnityWebRequest request = UnityWebRequest.Put(url, data)) {
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success) {
                Debug.LogError(request.error);
            } else {
                Debug.Log("Seeked to frame: " + frame);
            }
        }
    }
}

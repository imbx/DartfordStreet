using UnityEngine;

public class CameraDepthTexture : MonoBehaviour {
    private void Start() {
        Camera cam = GetComponent<Camera>();
        cam.depthTextureMode = cam.depthTextureMode | DepthTextureMode.Depth;
    }
}
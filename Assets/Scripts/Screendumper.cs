using System.IO;
using UnityEngine;
using UnityEngine.UIElements;

public class Screendumper : MonoBehaviour
{
    [SerializeField] Camera cam;

    public Texture2D Capture()
    {
        RenderTexture currentRT = RenderTexture.active;
        RenderTexture.active = cam.targetTexture;
        cam.Render();
        Texture2D image = new Texture2D(cam.targetTexture.width, cam.targetTexture.height);
        image.ReadPixels(new Rect(0, 0, cam.targetTexture.width, cam.targetTexture.height), 0, 0);
        image.Apply();
        RenderTexture.active = currentRT;

        return image;
    }

    public void SaveCapture(string filename)
    {
        var image = Capture();
        var bytes = image.EncodeToPNG();
        File.WriteAllBytes($"{ Application.dataPath }/Backgrounds/{ filename }", bytes);
    }
}

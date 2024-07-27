using System.Collections;
using UnityEngine;

public class FaceObjectToCamera : MonoBehaviour
{
    private Camera mainCamera;

    private void Start()
    {
        // Coroutine başlat
        StartCoroutine(FindMainCamera());
    }

    private void Update()
    {
        if (mainCamera != null)
        {
            transform.LookAt(mainCamera.transform);
        }
    }

    private IEnumerator FindMainCamera()
    {
        // Camera.main bulunana kadar bekle
        while (Camera.main == null)
        {
            yield return null; // Bir frame bekle
        }

        // Camera.main atandıktan sonra mainCamera değişkenine ata
        mainCamera = Camera.main;
    }
}

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class CameraAspectFitter
{
    private const float ReferenceAspect = 16f / 9f;
    private const float DesktopUsage = 0.85f;
    private const int MaximumWindowWidth = 1920;
    private const int MaximumWindowHeight = 1080;

    private static readonly Dictionary<int, float> BaseOrthographicSizes =
        new Dictionary<int, float>();

    private static int lastScreenWidth;
    private static int lastScreenHeight;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void Initialize()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneLoaded += OnSceneLoaded;

        Application.onBeforeRender -= RefreshIfResolutionChanged;
        Application.onBeforeRender += RefreshIfResolutionChanged;

        SetBestWindowedResolution();
        FitAllCameras();
    }

    private static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        FitAllCameras();
    }

    private static void SetBestWindowedResolution()
    {
        int desktopWidth = Display.main.systemWidth;
        int desktopHeight = Display.main.systemHeight;

        if (desktopWidth <= 0 || desktopHeight <= 0)
        {
            desktopWidth = Screen.currentResolution.width;
            desktopHeight = Screen.currentResolution.height;
        }

        int availableWidth = Mathf.FloorToInt(desktopWidth * DesktopUsage);
        int availableHeight = Mathf.FloorToInt(desktopHeight * DesktopUsage);

        int windowWidth = Mathf.Min(MaximumWindowWidth, availableWidth);
        int windowHeight = Mathf.RoundToInt(windowWidth / ReferenceAspect);

        if (windowHeight > availableHeight)
        {
            windowHeight = Mathf.Min(MaximumWindowHeight, availableHeight);
            windowWidth = Mathf.RoundToInt(windowHeight * ReferenceAspect);
        }

        windowWidth = Mathf.Max(640, windowWidth / 2 * 2);
        windowHeight = Mathf.Max(360, windowHeight / 2 * 2);

        Screen.SetResolution(windowWidth, windowHeight, FullScreenMode.Windowed);
    }

    private static void RefreshIfResolutionChanged()
    {
        if (Screen.width == lastScreenWidth && Screen.height == lastScreenHeight)
            return;

        FitAllCameras();
    }

    private static void FitAllCameras()
    {
        if (Screen.width <= 0 || Screen.height <= 0)
            return;

        lastScreenWidth = Screen.width;
        lastScreenHeight = Screen.height;

        float currentAspect = (float)Screen.width / Screen.height;
        float sizeMultiplier = Mathf.Max(1f, ReferenceAspect / currentAspect);
        Camera[] cameras = Object.FindObjectsOfType<Camera>(true);

        foreach (Camera camera in cameras)
        {
            if (!camera.orthographic)
                continue;

            int cameraId = camera.GetInstanceID();
            if (!BaseOrthographicSizes.TryGetValue(cameraId, out float baseSize))
            {
                baseSize = camera.orthographicSize;
                BaseOrthographicSizes[cameraId] = baseSize;
            }

            camera.orthographicSize = baseSize * sizeMultiplier;
        }
    }
}
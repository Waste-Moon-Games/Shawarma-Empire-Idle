using UnityEngine;

public static class GameBootstrap
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    public static void Bootstrap()
    {
        if (Object.FindObjectOfType<GameController>() != null) return;

        var root = new GameObject("GameController");
        root.AddComponent<GameController>();

        if (Camera.main == null) return;

        Camera.main.transform.position = new Vector3(0f, 0f, -10f);
        Camera.main.orthographic = true;
        Camera.main.orthographicSize = 5f;
    }
}

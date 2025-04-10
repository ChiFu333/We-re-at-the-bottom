using System;
using UnityEngine;
using UnityEngine.UI;
[DefaultExecutionOrder(-9999)]
public class Main : MonoBehaviour
{
    void Awake()
    {
        //Base Managers
        if(!R.isInited) R.Init();
        
        if (!G.SceneLoader) G.SceneLoader = CreateSimpleService<SceneLoader>();
        if (!G.AudioManager) G.AudioManager = CreateSimpleService<AudioManager>();
        
        //ManagersWithObjectsInScnee
        if (!G.LocSystem) G.LocSystem = CreateSimpleService<LocSystem>();
        if (!G.Canvas) G.Canvas = CreateCanvas();
        
        if (G.PausePanel == null) G.PausePanel = CreateSimpleService<PausePanel>();

        
    }
    private void Start()
    {
        //if (G.Player == null && G.SceneLoader.currentSceneName != "MainMenu") G.Player = CreatePlayer();
    }
    private void OnApplicationQuit()
    {
        R.isInited = false;
    }
    
    private GameObject CreateCanvas()
    {
        GameObject g = new GameObject("MainCanvas");
        Canvas c = g.AddComponent<Canvas>();
        
        CanvasScaler cs = g.AddComponent<CanvasScaler>();
        cs.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        cs.referenceResolution = new Vector2(1920, 1080);
        cs.matchWidthOrHeight = 1;
        
        g.AddComponent<GraphicRaycaster>();
        c.worldCamera = Camera.main;
        c.renderMode = RenderMode.ScreenSpaceCamera;
        c.sortingOrder = 100;
        return g;
    }
    
    public T CreateSimpleService<T>() where T : Component, IService
    {
        GameObject g = new GameObject(typeof(T).ToString());
        DontDestroyOnLoad(g);
        T t = g.AddComponent<T>();
        t.Init();
        return g.GetComponent<T>();
    }
}

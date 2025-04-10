using System.Collections.Generic;
using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class LocSystem : MonoBehaviour, IService
{
    public string language = LANG_EN;
    
    public const string LANG_EN = "en";
    public const string LANG_RU = "ru";

    private GameObject _LocCanvas;
    
    public static List<string> langs = new List<string>() { LANG_EN, LANG_RU };

    public void Init()
    {
        language = LANG_EN;
        _LocCanvas = new GameObject("LanguageCanvas");
        return;
        _LocCanvas.AddComponent<Canvas>().renderMode = RenderMode.ScreenSpaceCamera;
        _LocCanvas.GetComponent<Canvas>().worldCamera = Camera.main;
        _LocCanvas.GetComponent<Canvas>().sortingOrder = 100; 
        
        _LocCanvas.AddComponent<GraphicRaycaster>();
        
        CanvasScaler cs = _LocCanvas.AddComponent<CanvasScaler>();
        cs.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        cs.referenceResolution = new Vector2(1920, 1080);
        cs.matchWidthOrHeight = 1;
        
        GameObject languagePanel = Instantiate(Resources.Load<GameObject>("Services/" + "LanguageSelector"), _LocCanvas.transform, false);
    }

    public void UpdateTexts()
    {
        GameObject[] allObjects = FindObjectsByType<GameObject>(FindObjectsSortMode.None);

        foreach (GameObject obj in allObjects)
        {
            // Получаем компонент, который реализует интерфейс UITextSetter
            UITextSetter textSetter = obj.GetComponent<UITextSetter>();

            // Если компонент найден, вызываем метод UpdateUI()
            if (textSetter != null)
            {
                textSetter.UpdateUI();
            }
        }
    }
}
[Serializable] public class LocString
{
    public string en;
    public string ru;

    Dictionary<string, FieldInfo> field = new Dictionary<string, FieldInfo>();

    public string GetText()
    {
        if(field == null) field = new Dictionary<string, FieldInfo>();
        if (!field.ContainsKey(G.LocSystem.language))
        {
            Type type = this.GetType();
            FieldInfo fieldInfo = type.GetField(G.LocSystem.language);
            field.Add(G.LocSystem.language, fieldInfo);
        }

        return field[G.LocSystem.language].GetValue(this) as string;
    }

    public LocString(string a, string b)
    {
        en = a;
        ru = b;
    }

    public override string ToString()
    {
        return GetText();
    }
}
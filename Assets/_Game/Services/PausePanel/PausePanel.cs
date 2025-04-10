using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PausePanel : MonoBehaviour, IService
{
    public UIPanelScaler panel;
    
    private string _notInMainMenu = "MainMenu";

    public bool inMenu = false;
    public void Init()
    {
        GameObject can = new GameObject("MainMenuCanvas");
        DontDestroyOnLoad(can);
        
        Canvas c = can.AddComponent<Canvas>();
        
        CanvasScaler cs = can.AddComponent<CanvasScaler>();
        cs.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        cs.referenceResolution = new Vector2(1920, 1080);
        cs.matchWidthOrHeight = 1;
        
        can.AddComponent<GraphicRaycaster>();
        c.renderMode = RenderMode.ScreenSpaceCamera;
        c.sortingOrder = 500;
        
        GameObject g = Instantiate(Resources.Load<GameObject>("Services/" + "PauseMenu"), can.transform);
        panel = g.GetComponent<UIPanelScaler>();
    }

    public void Update()
    {
        if (G.SceneLoader.currentSceneName != _notInMainMenu && Input.GetKeyDown(KeyCode.Tab) && !panel.inAnim)
        {
            
            if (panel.gameObject.activeSelf)
            {
                panel.Close();
                inMenu = false;
            }
            else
            {
                panel.gameObject.SetActive(true);
                inMenu = true;
            }
        }
    }

    public void ChangeStartButton(bool b)
    {
        panel.transform.GetChild(0).GetChild(4).gameObject.SetActive(b);
    }
    public void ChangeGraphic(bool b)
    {
        panel.transform.GetChild(0).GetChild(5).gameObject.SetActive(b);
    }
}

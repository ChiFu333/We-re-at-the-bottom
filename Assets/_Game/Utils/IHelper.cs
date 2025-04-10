using Cysharp.Threading.Tasks;
using UnityEngine;
[CreateAssetMenu(fileName = "IHelper", menuName = "IHelper")]
public class IHelper : ScriptableObject
{
    public void SetRu()
    {
        G.LocSystem.language = LocSystem.LANG_RU;
        UpdateLan();    
    }

    public void SetEn()
    {
        G.LocSystem.language = LocSystem.LANG_EN;
        UpdateLan();
    }

    private void UpdateLan()
    {
        G.LocSystem.UpdateTexts();
    }

    public void LoadScene(string line)
    {
        _ = G.SceneLoader.Load(line);
    }

    public void ShowSettingPanel()
    {
        G.PausePanel.panel.gameObject.SetActive(true);
    }

    public async void FunnyQuit()
    {
        await FindFirstObjectByType<GameMain>().uiHandler.ThrowText(new LocString("Bye-bye!", "Пока-пока!"));
        await UniTask.Delay(100);
        Quit();
    }
    public void Quit()
    {
        Debug.Log("GameQuited");
        Application.Quit();
    }

    public void NextLevel()
    {
        _ = GameMain.inst.NextLevel();
    }

    public void ChangeStartButtonOnPause(bool b)
    {
        G.PausePanel.ChangeStartButton(b);
    }
    public void ChangeButtonToGraphic(bool b)
    {
        G.PausePanel.ChangeGraphic(b);
    }

    public void StartMusic(AudioClip clip)
    {
        G.AudioManager.PlayMusic(clip);
    }

    public void PlaySound(AudioClip clip)
    {
        G.AudioManager.PlaySound(clip);
    }

    public void ChangeGraphic(bool b)
    {
        ChangeAllGraphic.inst.ChangeGraphic(b);
    }
}

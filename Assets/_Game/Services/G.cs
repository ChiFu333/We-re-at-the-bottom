using UnityEngine;

public static class G 
{
    //Ссылки на переменные
    public static GameObject Canvas;
    //Сервисы
    public static LocSystem LocSystem;
    public static AudioManager AudioManager;
    public static SceneLoader SceneLoader;
    public static PausePanel PausePanel;
}

public interface IService
{
    public void Init();
}
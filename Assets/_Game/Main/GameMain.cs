using System;
using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.Serialization;

public class GameMain : MonoBehaviour
{
    public static GameMain inst;
    public UIHandler uiHandler;
    public Levels levels;
    [Header("Params")] 
    [SerializeField] private int _currentLevel;
    private int _maxLevel = 9;
    

    public bool isMovingLift { get; private set; } = false;

    private void Awake()
    {
        inst = this;
        uiHandler = GetComponent<UIHandler>();
        levels = GetComponent<Levels>();
    }

    void Start()
    {
        levels.SetLevel(_currentLevel);
    }

    public async UniTask NextLevel()
    {
        if (isMovingLift) return;
        isMovingLift = true;
        _currentLevel++;
        levels.SetLevel(_currentLevel);
        if (_currentLevel <= _maxLevel)
        {
            G.AudioManager.ChangeMusicPitch(uiHandler._lightData.musicIscas.Evaluate(1 - (float)_currentLevel/_maxLevel));
            uiHandler.GoToLightCount(1 - (float)_currentLevel/_maxLevel);
            await uiHandler.AnimateHeight();
        }
        else
        {
            G.AudioManager.ChangeMusicPitch(1);   
        }
        if(_currentLevel == _maxLevel) G.AudioManager.ChangeMusicPitch(1);  
        isMovingLift = false;
    }
}

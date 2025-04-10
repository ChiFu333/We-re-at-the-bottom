using System;
using UnityEngine;
using Febucci.UI;
using Cysharp.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using TMPro;
using UnityEngine.Events;
using UnityEngine.UI;


public class NovelaUIHandler : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _renderer;
    [SerializeField] private TextAnimatorPlayer _mainText;
    [SerializeField] private List<GameObject> _buttons;
    
    public bool _isTyping = false;
    private bool _forceStop = false;

    private void Update()
    {
        if (_isTyping && Input.GetMouseButtonDown(0))
        {
            CancelAnimation();
        }
    }

    public async UniTask WriteText(string text, VoiceSO voice)
    {
        _isTyping = true;
        _mainText.ShowText("");
        _mainText.transform.GetComponent<TMP_Text>().color = voice.color;
        _mainText.transform.GetComponent<TMP_Text>().fontSharedMaterial = voice.textMaterial;
        
        _mainText.ShowText(text);
        
        var soundCancellationToken = new CancellationTokenSource();
        var playSoundTask = PlayTypingSoundRepeatedly(voice.deltaSound, soundCancellationToken.Token, voice.voice);
        
        while (!_mainText.textAnimator.allLettersShown && !_forceStop)
        {
            if (Input.GetMouseButtonDown(0))
            {
                CancelAnimation();
            }
            await UniTask.Yield(); // Ждём следующего кадра
        }
        soundCancellationToken.Cancel();
        
        await UniTask.WaitUntil(() => Input.GetMouseButtonDown(0));
        await UniTask.Delay(100);
        _isTyping = false; 
        _forceStop = false;  
    }
    
    // Отмена анимации
    public void CancelAnimation()
    {
        _mainText.SkipTypewriter();
        _forceStop = true;
    }

    public void SetBackround(Sprite p)
    {
        _renderer.sprite = p;
    }

    public void RemoveAllChoices()
    {
        for (int i = 0; i < 3; i++)
        {
            _buttons[i].GetComponent<UIButtonScaling>().OnClick = new UnityEvent();
            _buttons[i].SetActive(false);
        }
    }
    public void SetChoices(List<Choice> choices)
    {
        int tempI = 0;
        for (int i = 0; i < choices.Count; i++)
        {
            if (choices[i] != null)
            {
                int t = i;
                _buttons[i].SetActive(true);
                _buttons[i].GetComponentInChildren<TMP_Text>().text = choices[i].choiceText.ToString();
                _buttons[i].GetComponent<UIButtonScaling>().OnClick.AddListener(() => choices[t].onSelect());    
            }
            else
            {
                _buttons[i].GetComponent<UIButtonScaling>().OnClick = new UnityEvent();
                _buttons[i].SetActive(false);
            }
        }

        for (int j = choices.Count; j < 3; j++)
        {
            _buttons[j].GetComponent<UIButtonScaling>().OnClick = new UnityEvent();
            _buttons[j].SetActive(false);
        }
    }
    private async UniTask PlayTypingSoundRepeatedly(int intervalMs, CancellationToken cancellationToken, AudioClip sample)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            G.AudioManager.PlayWithRandomPitch(sample, 0.15f);
            // Ждём указанный интервал
            await UniTask.Delay(intervalMs, cancellationToken: cancellationToken);
        }
    }
}

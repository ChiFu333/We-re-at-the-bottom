using System;
using UnityEngine;
using DG.Tweening;
using Cysharp.Threading.Tasks;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class UIHandler : MonoBehaviour
{
    [Header("Global objects")] 
    [SerializeField] private SpriteRenderer _brickBackround;
    [SerializeField] private TextThrower _mainText;

    [Header("LightStuff")] 
    [SerializeField] private float _lightRatio;
    [SerializeField] private float _vignetteRatio;
    [SerializeField] public LightCurveData _lightData;
    [SerializeField] private Light2D _baseLight;
    [SerializeField] private Volume volume;
    private Vignette vignette;
    private float currentLightCount = 1f;

    private void Awake()
    {
        if (volume.profile.TryGet(out vignette))
        {
            vignette.active = true; // Включаем виньетку, если она выключена
        }
        else
        {
            Debug.LogError("Vignette not found in Volume Profile!");
        }
    }


    public async UniTask ThrowText(LocString str)
    {
        await _mainText.ThrowText(str, R.tinyVoice);
    }

    public void ChangeLightsToValue(float i)
    {
        _baseLight.intensity = _lightData.lightCurve.Evaluate(i) * _lightRatio;
        vignette.intensity.value = _lightData.vingeteCurve.Evaluate(i) * _vignetteRatio;
    }

    public async UniTask AnimateHeight()
    {
        G.AudioManager.PlaySound(R.Audio.liftSound);
        DOTween.Kill(this);
        // Устанавливаем начальное значение
        _brickBackround.size = new Vector2(42, 20.6f);

        await DOTween.To(
            () => _brickBackround.size.y,
            y => {
                Vector2 newSize = _brickBackround.size;
                newSize.y = (float)y;
                _brickBackround.size = newSize;
            },
            (float)_brickBackround.size.y + 18.67f,
            2f
        )
        .SetEase(Ease.OutBack, 0.4f) // Можно изменить на другой Ease-тип
        .SetId(this)
        .OnComplete(() => _brickBackround.size = new Vector2(42, 20.6f))
        .AsyncWaitForCompletion();
    }

    public void GoToLightCount(float t)
    {
        DOTween.To(
            () => currentLightCount,                      
            ChangeLightsToValue,                       
            t,                             
            1f                                
        ).SetEase(Ease.Linear)
        .OnComplete(() => currentLightCount = t);
    }
}

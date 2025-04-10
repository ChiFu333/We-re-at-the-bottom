using System;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using  Cysharp.Threading.Tasks;

public class Door : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private bool isMini = false;
    [SerializeField] private bool addedShader = false;
    [SerializeField] private bool realEndDoor = false;
    void Update()
    {
        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (addedShader)
        {
            G.AudioManager.PlayWithRandomPitch(R.Audio.wrong,0.15f);
            return;
        }    
        NextLevelDoor();
    }
    private float shakeDuration = 0.4f; // Длительность тряски
    private float shakeStrength = 0.4f; // Сила тряски
    private int shakeVibrato = 10; // Частота тряски
    private float shakeRandomness = 90f; // Случайность тряски
    private float interval = 2f; // Интервал между трясками

    private Vector3 originalPosition;
    private Sequence shakeSequence;

    private void Start()
    {
        originalPosition = transform.localPosition;
        if(!isMini) ShowWithAnim();
        // Создаем последовательность для повторяющейся тряски
        shakeSequence = DOTween.Sequence();
        shakeSequence.AppendInterval(interval);
        shakeSequence.AppendCallback(DoShake); // Вызываем тряску
        shakeSequence.SetLoops(-1); // Бесконечный цикл
        
    }

    public async void ShowWithAnim()
    {
        transform.localScale = Vector3.zero;
        await UniTask.Delay(800);
        G.AudioManager.PlaySound(R.Audio.showSound);
        transform.DOScale(Vector3.one * 2, 1f).SetEase(Ease.OutElastic, 1.1f, 0.5f);
    }

    private void DoShake()
    {
        // Сбрасываем позицию перед тряской
        transform.localPosition = originalPosition;
        
        // Создаем тряску с помощью DOShakePosition
        transform.DOShakePosition(shakeDuration, shakeStrength, shakeVibrato, shakeRandomness, false, true)
            .OnComplete(() => transform.localPosition = originalPosition);
        if(!isMini) G.AudioManager.PlayWithRandomPitch(R.Audio.hitDoor, 0.1f);
        DOVirtual.DelayedCall(0.2f, () => 
        {
            if(!isMini) G.AudioManager.PlayWithRandomPitch(R.Audio.hitDoor, 0.1f);
            transform.DOShakePosition(shakeDuration, shakeStrength, shakeVibrato, shakeRandomness, false, true);
        });
    }

    private void OnDestroy()
    {
        // Очищаем твины при уничтожении объекта
        shakeSequence?.Kill();
    }

    private void OnDisable()
    {
        shakeSequence?.Kill();
    }

    private void NextLevelDoor()
    {
        if (GameMain.inst.isMovingLift) return;
        if (realEndDoor)
        {
            G.AudioManager.PlaySound(R.Audio.openDoor);
            G.SceneLoader.Load("VisualEnd");
            return;
        }
        G.AudioManager.PlaySound(R.Audio.doorTouched);
        _ = FindFirstObjectByType<GameMain>().NextLevel();
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            NextLevelDoor();
        }
    }
}

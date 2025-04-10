using UnityEngine;
using DG.Tweening;
using Cysharp.Threading.Tasks;

public class DoorBreaker : MonoBehaviour
{
    [Header("Door Parts")]
    public Transform leftDoorHalf;
    public Transform rightDoorHalf;
    public bool fallOut = false;
    [Header("Animation Settings")]
    private float separationDistance = 2f; // Дистанция разъезжания половинок
    private float shakeDuration = 0.5f; // Длительность тряски
    private float shakeStrength = 0.5f; // Сила тряски
    private int shakeVibrato = 10; // Интенсивность тряски
    private float fallDuration = 0.7f; // Длительность падения
    private float fallDistance = 10f; // Дистанция падения
    
    [Header("Effects")]
    public ParticleSystem crackEffect; // Эффект трещины/треска
    
    private Vector3 leftDoorStartPos;
    private Vector3 rightDoorStartPos;
    private Quaternion leftDoorStartRot;
    private Quaternion rightDoorStartRot;
    
    private void Start()
    {
        // Сохраняем начальные позиции и вращения
        leftDoorStartPos = leftDoorHalf.position;
        rightDoorStartPos = rightDoorHalf.position;
        leftDoorStartRot = leftDoorHalf.rotation;
        rightDoorStartRot = rightDoorHalf.rotation;
    }
    
    public async UniTask BreakDoor()
    {
        await UniTask.Delay(1000);
        // 1. Анимация разъединения с тряской
        Sequence breakSequence = DOTween.Sequence();
        
        // Добавляем тряску
        G.AudioManager.PlaySound(R.Audio.closedLoud);
        if (crackEffect != null) crackEffect.Play();
        
        leftDoorHalf.DOShakePosition(shakeDuration, shakeStrength, shakeVibrato);
        rightDoorHalf.DOShakePosition(shakeDuration, shakeStrength, shakeVibrato);
        
        // Разъединяем половинки
        leftDoorHalf.DOMoveX(leftDoorStartPos.x - separationDistance, 1f).SetEase(Ease.OutBack);
        await rightDoorHalf.DOMoveX(rightDoorStartPos.x + separationDistance, 1f).SetEase(Ease.OutBack).AsyncWaitForCompletion();
        
        
        // 2. Анимация падения
        Sequence fallSequence = DOTween.Sequence();
        
        // Падение левой половинки с вращением
        fallSequence.Join(leftDoorHalf.DOMoveY(leftDoorStartPos.y - fallDistance, 0.7f).SetEase(Ease.InQuad));
        fallSequence.Join(leftDoorHalf.DORotate(new Vector3(0, 0, 45f), fallDuration));
        
        // Падение правой половинки с вращением
        fallSequence.Join(rightDoorHalf.DOMoveY(rightDoorStartPos.y - fallDistance, 0.7f).SetEase(Ease.InQuad));
        fallSequence.Join(rightDoorHalf.DORotate(new Vector3(0, 0, -45f), fallDuration).OnComplete(() => fallOut = true));
    }
    
    public void ResetDoor()
    {
        // Сбрасываем анимации
        DOTween.Kill(leftDoorHalf);
        DOTween.Kill(rightDoorHalf);
        
        // Возвращаем двери в исходное положение
        leftDoorHalf.position = leftDoorStartPos;
        rightDoorHalf.position = rightDoorStartPos;
        leftDoorHalf.rotation = leftDoorStartRot;
        rightDoorHalf.rotation = rightDoorStartRot;
    }
}

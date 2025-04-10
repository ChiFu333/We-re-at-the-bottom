using UnityEngine;
using Cysharp.Threading.Tasks;
using DG.Tweening;

public class BackroundAppear : MonoBehaviour
{
    public async void OnEnable()
    {
        Color c = GetComponent<SpriteRenderer>().color;
        c.a = 0;
        GetComponent<SpriteRenderer>().color = c;
        await UniTask.Delay(1000);
        GetComponent<SpriteRenderer>().DOFade(1, 20f);
    }
}

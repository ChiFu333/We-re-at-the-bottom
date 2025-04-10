using UnityEngine;

[CreateAssetMenu(fileName = "NewAnimationCurve", menuName = "MyScriptableObject/Curve")]
public class LightCurveData : ScriptableObject
{
    public AnimationCurve lightCurve;
    public AnimationCurve vingeteCurve;
    public AnimationCurve musicIscas;

}
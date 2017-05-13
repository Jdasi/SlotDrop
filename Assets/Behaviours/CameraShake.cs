using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private static CameraShake cameraShake = null;
    public static CameraShake instance { get { return cameraShake; } }

    private Transform camTransform = null;
    public Vector3 originalLocalPosition = Vector3.zero;

    private bool isShaking = false;
    private bool isShakingPaused = false;
    private float shakeStrength = 0.7f;
    private float shakeDuration = 0.0f;
    private float shakeTime = 0.0f;
    [SerializeField] private AnimationCurve shakeDecayRate = null;

    private void Awake()
    {
        if (cameraShake)
        {
            DestroyImmediate(this);
        }
        else
        {
            cameraShake = this;
            camTransform = Camera.main.transform;
        }
    }

    private void Update()
    {
        if (!isShakingPaused)
        {
            if (isShaking)
            {
                shakeTime += Time.deltaTime;
                if (shakeTime < shakeDuration)
                {
                    camTransform.localPosition = originalLocalPosition + ((Random.insideUnitSphere * shakeStrength) * shakeDecayRate.Evaluate(shakeTime / shakeDuration));
                }
                else
                {
                    isShaking = false;
                    camTransform.localPosition = originalLocalPosition;
                }
            }
        }
    }

    public void PauseShake()
    {
        isShakingPaused = true;
    }

    public void ResumeShake()
    {
        isShakingPaused = false;
    }

    public void ShakeCamera(float _strength, float _duration)
    {
        if (!isShaking)
        {
            originalLocalPosition = camTransform.localPosition;
            shakeTime = 0.0f;
            shakeStrength = _strength;
            shakeDuration = _duration;
            isShaking = true;
        }
        else
        {
            shakeTime = 0.0f;
            shakeStrength = _strength;
            shakeDuration = _duration;
        }
    }
}

#if UNITY_EDITOR
[UnityEditor.CustomEditor(typeof(CameraShake))]
public class CameraShakeEditor : UnityEditor.Editor
{
    private float strength = 0.2f;
    private float duration = 0.2f;

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        UnityEditor.EditorGUILayout.Space();
        UnityEditor.EditorGUILayout.LabelField("Camera Shake Test", UnityEditor.EditorStyles.boldLabel);

        CameraShake myScript = (CameraShake)target;

        strength = UnityEditor.EditorGUILayout.Slider("Strength", strength, 0.1f, 5.0f);
        duration = UnityEditor.EditorGUILayout.Slider("Duration", duration, 0.1f, 10.0f);

        if (GUILayout.Button("Shake"))
        {
            myScript.ShakeCamera(strength, duration);
        }
    }
}
#endif


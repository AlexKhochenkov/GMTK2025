using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Wheel : MonoBehaviour
{
    [Header("Speed Settings")]
    [SerializeField] private float _startSpeed = 50f;
    [SerializeField] private float _maxSpeed = 500f;
    [SerializeField] private AnimationCurve _accelCurve = AnimationCurve.Linear(0, 0, 1, 1);
    [SerializeField] private float _accelTime = 3f;

    [Header("Rotation Direction")]
    [SerializeField] private bool clockwise = true;

    private Rigidbody2D _rb;
    private float _timer;
    private bool _isAccelerating = true;

    IEnumerator Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        yield return new WaitUntil(() => GameManager.Instance.GameStarted);
        _rb.angularVelocity = clockwise ? _startSpeed : -_startSpeed;
    }

    void Update()
    {
        if (!GameManager.Instance.GameStarted)
        {
            _rb.angularVelocity = 0;
            return;
        }
        if (!_isAccelerating) return;

        _timer += Time.deltaTime;

        float normalizedTime = Mathf.Clamp01(_timer / _accelTime);

        float curveValue = _accelCurve.Evaluate(normalizedTime);

        float currentSpeed = Mathf.Lerp(_startSpeed, _maxSpeed, curveValue);

        _rb.angularVelocity = clockwise ? currentSpeed : -currentSpeed;

        if (_timer >= _accelTime)
        {
            _isAccelerating = false;
            Debug.Log("Acceleration completed! Final speed: " + currentSpeed);
        }
    }

    void OnDrawGizmosSelected()
    {
        if (_accelCurve == null) return;

        GUIStyle style = new GUIStyle();
        style.normal.textColor = Color.white;

        for (float t = 0; t <= 1; t += 0.1f)
        {
            float speed = Mathf.Lerp(_startSpeed, _maxSpeed, _accelCurve.Evaluate(t));
            Vector3 labelPos = transform.position + Vector3.up * 0.5f + Vector3.right * t;
            Debug.DrawLine(
                transform.position + Vector3.right * t,
                transform.position + Vector3.right * t + Vector3.up * (speed / _maxSpeed),
                Color.green
            );
            //UnityEditor.Handles.Label(labelPos, $"{speed:F0}", style);
        }
    }
}

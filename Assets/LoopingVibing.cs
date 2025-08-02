using System.Collections;
using UnityEngine;

public class LoopingVibing : MonoBehaviour
{
    private RectTransform rectTransform;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private Coroutine coroutine;
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        //StartCoroutine(Sequence());
    }

    void OnEnable()
    {
        if (rectTransform is null)
        {
            rectTransform = GetComponent<RectTransform>();
        }
        coroutine = StartCoroutine(Sequence());
    }

    void OnDisable()
    {
        StopCoroutine(coroutine);
        coroutine = null;
    }

    private IEnumerator HandleIt()
    {
        var time = 0f;
        while (time < 100f)
        {
            time += Time.deltaTime;
            rectTransform.Rotate(Vector3.forward, Time.deltaTime);
        }

        while (time < 200f)
        {
            time += Time.deltaTime;
            rectTransform.Rotate(Vector3.forward, -1 * Time.deltaTime);
        }
        StartCoroutine(HandleIt());
        yield return null;
    }

    public IEnumerator Sequence()
    {
        var angle = 0.05f;
        yield return StartCoroutine(Turn(angle));
        yield return StartCoroutine(Turn(-angle));

        StartCoroutine(Sequence());
    }

    public IEnumerator Turn(float angle)
    {
        float time = 0f;

        while(time < 1f)
        {
            time += Time.deltaTime;

            rectTransform.Rotate(Vector3.forward, angle);
 
            yield return null;
        }
    }

}

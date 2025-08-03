using System.Collections;
using UnityEngine;

public class DeleteAfterStart : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    IEnumerator Start()
    {
        yield return new WaitUntil(() => GameManager.Instance.GameStarted);
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

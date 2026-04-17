using UnityEngine;
using System.Collections;

public class StartMessage : MonoBehaviour
{
    public CanvasGroup messageGroup;
    public float delay = 5f;
    public float displayTime = 7f;

    void Awake()
    {
        messageGroup.alpha = 0;
    }

    void Start()
    {
        StartCoroutine(ShowMessage());
    }

    IEnumerator ShowMessage()
    {
        yield return new WaitForSeconds(delay);

        messageGroup.alpha = 1; // aparece

        yield return new WaitForSeconds(displayTime);

        messageGroup.alpha = 0; // some
    }
}
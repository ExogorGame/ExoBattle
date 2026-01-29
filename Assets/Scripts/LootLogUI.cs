using UnityEngine;
using TMPro;
using System.Collections;

public class LootLogEntry : MonoBehaviour
{
    public TextMeshProUGUI text;
    public float lifetime = 2f;
    public float fadeTime = 0.5f;

    CanvasGroup canvasGroup;

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void SetText(string message)
    {
        text.text = message;
        StartCoroutine(FadeOut());
    }

    IEnumerator FadeOut()
    {
        yield return new WaitForSeconds(lifetime);

        float t = 0;
        while (t < fadeTime)
        {
            t += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(1, 0, t / fadeTime);
            yield return null;
        }

        Destroy(gameObject);
    }
}


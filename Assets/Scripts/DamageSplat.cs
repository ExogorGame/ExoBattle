using UnityEngine;
using TMPro;

public class DamageSplat : MonoBehaviour
{
    public TextMeshProUGUI damageText;
    public float floatUpDistance = 60f;
    public float lifetime = 1f;

    Vector2 startPos;

    public void Initialize(int damage, Color color)
    {
        if (damageText == null)
        {
            Debug.LogError("DamageText is not assigned!");
            return;
        }

        damageText.text = damage.ToString();   // Set the damage number
        damageText.color = color;
        damageText.gameObject.SetActive(true); // Make sure it’s visible

        startPos = GetComponent<RectTransform>().anchoredPosition;

        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        float t = Time.deltaTime / lifetime;
        GetComponent<RectTransform>().anchoredPosition += Vector2.up * floatUpDistance * t;
    }
}

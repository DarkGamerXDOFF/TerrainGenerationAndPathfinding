using TMPro;
using UnityEngine;
using System.Collections;

public class Message : MonoBehaviour
{
    [SerializeField] private float stayDuration = 0.5f;
    [SerializeField] private float fadeDuration = 1.5f;

    [SerializeField] private TMP_Text textMesh;
    
    private Color originalColor;

    void Start()
    {
        originalColor = textMesh.color;
        StartCoroutine(FadeOut());
    }

    IEnumerator FadeOut()
    {
        float elapsedTime = 0f;

        while (elapsedTime < stayDuration)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;

            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            textMesh.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            
            yield return null;
        }

        Destroy(gameObject);
    }
}

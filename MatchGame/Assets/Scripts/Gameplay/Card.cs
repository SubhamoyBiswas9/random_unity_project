using UnityEngine;
using System;
using System.Collections;
using System.Buffers.Text;

public class Card : MonoBehaviour
{
    [field: SerializeField] public SpriteRenderer front { get; private set; }
    [SerializeField] SpriteRenderer back;
    [SerializeField] SpriteRenderer baseSR;

    [SerializeField] float flipDuration = 0.25f;

    [SerializeField] float punchAmount = 0.2f;
    [SerializeField] float punchDuration = 0.2f;
    [SerializeField] float flashDuration = 0.1f;
    [SerializeField] float disappearDuration = 0.25f;

    private bool isFront;
    private bool isAnimating;

    private Vector3 baseScale;

    public Action<Card> OnClicked;

    public bool IsFront() => isFront;

    private void Start()
    {
        baseScale = transform.localScale;
    }

    public void Setup(Sprite frontSprite, Sprite baseSprite)
    {
        front.sprite = frontSprite;
        baseSR.sprite = baseSprite;
        ShowBackInstant();
    }

    public void OnTap()
    {
        if (isAnimating) return;
        OnClicked?.Invoke(this);
    }

    public void Flip(bool showFront)
    {
        if (isAnimating) return;
        StartCoroutine(FlipRoutine(showFront));
    }

    IEnumerator FlipRoutine(bool showFront)
    {
        isAnimating = true;

        Vector3 originalScale = transform.localScale;

        float t = 0;

        // --- First half (shrink X to 0) ---
        while (t < flipDuration)
        {
            t += Time.deltaTime;
            float progress = t / flipDuration;

            float scaleX = Mathf.Lerp(originalScale.x, 0, progress);
            transform.localScale = new Vector3(scaleX, originalScale.y, originalScale.z);

            yield return null;
        }

        // --- Swap sprite ---
        isFront = showFront;
        front.gameObject.SetActive(showFront);
        back.gameObject.SetActive(!showFront);

        t = 0;

        // --- Second half (expand back to original) ---
        while (t < flipDuration)
        {
            t += Time.deltaTime;
            float progress = t / flipDuration;

            float scaleX = Mathf.Lerp(0, originalScale.x, progress);
            transform.localScale = new Vector3(scaleX, originalScale.y, originalScale.z);

            yield return null;
        }

        // Ensure exact final value (avoids floating errors)
        transform.localScale = originalScale;

        isAnimating = false;
    }

    public void ShowBackInstant()
    {
        isFront = false;
        front.gameObject.SetActive(false);
        back.gameObject.SetActive(true);
    }

    public void PlayMatchAnimation(System.Action onComplete = null)
    {
        if (isAnimating) return;
        StartCoroutine(MatchSequence(onComplete));
    }

    IEnumerator MatchSequence(System.Action onComplete)
    {
        isAnimating = true;

        // Run punch + flash together
        StartCoroutine(Punch());
        StartCoroutine(Flash(front));

        yield return new WaitForSeconds(punchDuration);

        // Then disappear
        yield return StartCoroutine(ShrinkAndDisable());

        onComplete?.Invoke();
    }

    IEnumerator Punch()
    {
        float t = 0;

        while (t < punchDuration)
        {
            t += Time.deltaTime;
            float progress = t / punchDuration;

            // Smooth punch curve
            float scale = 1 + Mathf.Sin(progress * Mathf.PI) * punchAmount;

            transform.localScale = baseScale * scale;
            yield return null;
        }

        transform.localScale = baseScale;
    }

    IEnumerator Flash(SpriteRenderer sr)
    {
        Color original = sr.color;

        sr.color = Color.white;
        yield return new WaitForSeconds(flashDuration);

        sr.color = original;
    }

    IEnumerator ShrinkAndDisable()
    {
        float t = 0;

        while (t < disappearDuration)
        {
            t += Time.deltaTime;
            float progress = t / disappearDuration;

            float scale = Mathf.Lerp(1, 0, progress);
            transform.localScale = baseScale * scale;

            yield return null;
        }

        gameObject.SetActive(false);
    }

    public void ShowFrontInstant()
    {
        isFront = true;
        front.gameObject.SetActive(true);
        back.gameObject.SetActive(false);
    }
}
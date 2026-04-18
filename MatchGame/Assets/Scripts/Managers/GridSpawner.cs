using UnityEngine;
using System.Collections.Generic;

public class GridSpawner : MonoBehaviour
{
    public GridConfigSO config;
    public GameObject cardPrefab;
    public Transform parent;
    public Camera cam;

    public List<CardDataSO> cardPool;

    private void Start()
    {
        Spawn(FindObjectOfType<MatchHandler>());
    }

    public void Spawn(MatchHandler matchHandler)
    {
        int total = config.rows * config.cols;

        // --- Prepare card list (pairs) ---
        List<CardDataSO> selected = new();

        List<CardDataSO> shuffledPool = new(cardPool);
        Shuffle(shuffledPool);

        for (int i = 0; i < total / 2; i++)
        {
            var data = shuffledPool[i % shuffledPool.Count];
            selected.Add(data);
            selected.Add(data);
        }

        Shuffle(selected);

        // --- Screen size in world units ---
        float screenWidth = cam.orthographicSize * 2 * cam.aspect;
        float screenHeight = cam.orthographicSize * 2;

        // --- Available space after padding ---
        float usableWidth = screenWidth - (config.paddingX * 2);
        float usableHeight = screenHeight - (config.paddingY * 2);

        // --- Aspect ratio ---
        float aspect = selected[0].frontSprite.bounds.size.x /
                       selected[0].frontSprite.bounds.size.y;

        // --- Max size per card ---
        float maxWidth = (usableWidth - (config.cols - 1) * config.spacingX) / config.cols;
        float maxHeight = (usableHeight - (config.rows - 1) * config.spacingY) / config.rows;

        // --- Maintain aspect ratio ---
        float finalWidth = maxWidth;
        float finalHeight = finalWidth / aspect;

        if (finalHeight > maxHeight)
        {
            finalHeight = maxHeight;
            finalWidth = finalHeight * aspect;
        }

        // --- Grid total size ---
        float totalWidth = config.cols * finalWidth + (config.cols - 1) * config.spacingX;
        float totalHeight = config.rows * finalHeight + (config.rows - 1) * config.spacingY;

        // --- Start position (centered + padding aware) ---
        Vector3 startOffset = new Vector3(
            -usableWidth / 2 + (usableWidth - totalWidth) / 2 + finalWidth / 2,
             usableHeight / 2 - (usableHeight - totalHeight) / 2 - finalHeight / 2,
            0
        );

        // --- Spawn ---
        int index = 0;

        for (int r = 0; r < config.rows; r++)
        {
            for (int c = 0; c < config.cols; c++)
            {
                Vector3 pos = startOffset + new Vector3(
                    c * (finalWidth + config.spacingX),
                   -r * (finalHeight + config.spacingY),
                    0
                );

                GameObject go = Instantiate(cardPrefab, parent);
                go.transform.localPosition = pos;
                go.transform.localScale = Vector3.one;

                Card card = go.GetComponent<Card>();
                CardController controller = new(selected[index], card);

                // --- Maintain aspect ratio via scaling ---
                Sprite sprite = card.front.sprite;

                float spriteW = sprite.bounds.size.x;
                float spriteH = sprite.bounds.size.y;

                float scaleX = finalWidth / spriteW;
                float scaleY = finalHeight / spriteH;

                float scale = Mathf.Min(scaleX, scaleY);
                go.transform.localScale = Vector3.one * scale;

                matchHandler.RegisterCard(controller);

                index++;
            }
        }
    }

    void Shuffle(List<CardDataSO> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int rand = Random.Range(i, list.Count);
            (list[i], list[rand]) = (list[rand], list[i]);
        }
    }
}
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] LayerMask cardLayer;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            Collider2D cardCollider = Physics2D.OverlapPoint(mousePos);

            if (cardCollider != null)
            {
                cardCollider.transform.GetComponent<Card>().OnTap();
            }
        }
    }
}

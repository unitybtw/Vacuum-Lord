using UnityEngine;
using UnityEngine.EventSystems; // Dokunmayı algılamak için gerekli

public class MobileJoystick : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    [Header("Ayarlar")]
    public RectTransform background; // Dış daire
    public RectTransform handle;     // İç top
    
    [Range(0, 2f)] public float handleLimit = 1f; // Top ne kadar uzağa gitsin?

    // Bu değerleri Player okuyacak (-1 ile 1 arasında)
    public Vector2 InputVector { get; private set; } 

    private Vector2 joystickPosition = Vector2.zero;
    private Camera cam;

    void Start()
    {
        // Başlangıçta top ortada dursun
        handle.anchoredPosition = Vector2.zero;
        cam = null; // Canvas "Screen Space - Overlay" ise kamera gerekmez
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 direction = Vector2.zero;

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(background, eventData.position, cam, out direction))
        {
            // Boyutu hesapla
            direction.x = (direction.x / background.sizeDelta.x) * 2;
            direction.y = (direction.y / background.sizeDelta.y) * 2;

            // Yönü al ve sınırla (Normalize)
            InputVector = new Vector2(direction.x, direction.y);
            if (InputVector.magnitude > 1) InputVector = InputVector.normalized;

            // Topu hareket ettir (Görsel)
            handle.anchoredPosition = new Vector2(
                InputVector.x * (background.sizeDelta.x / 2) * handleLimit, 
                InputVector.y * (background.sizeDelta.y / 2) * handleLimit
            );
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData); // Dokunur dokunmaz algıla
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // Parmağı kaldırınca her şeyi sıfırla
        InputVector = Vector2.zero;
        handle.anchoredPosition = Vector2.zero;
    }
}
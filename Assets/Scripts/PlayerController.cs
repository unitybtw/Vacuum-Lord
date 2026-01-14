using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Ayarlar")]
    public float moveSpeed = 5f;
    public float rotationSpeed = 10f;

    [Header("Kontroller")]
    public MobileJoystick joystick; // Joystick scriptini buraya bağlayacağız

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate() // Fizik işlemleri FixedUpdate'te yapılır
    {
        // Joystick yoksa hata vermesin diye kontrol
        if (joystick == null) return;

        // Joystick'ten gelen veriyi al
        float xHareket = joystick.InputVector.x;
        float zHareket = joystick.InputVector.y;

        // Hareket vektörünü oluştur
        Vector3 movement = new Vector3(xHareket, 0, zHareket);

        // Eğer parmak hareket ediyorsa (Vektör sıfır değilse)
        if (movement.magnitude > 0.1f)
        {
            // 1. Hareket Ettir (Fizik motoru ile)
            // Karakterin mevcut pozisyonuna hareket yönünü ekliyoruz
            Vector3 yeniPozisyon = rb.position + movement * moveSpeed * Time.fixedDeltaTime;
            rb.MovePosition(yeniPozisyon);

            // 2. Yüzünü Döndür
            Quaternion hedefRotasyon = Quaternion.LookRotation(movement);
            rb.rotation = Quaternion.Lerp(rb.rotation, hedefRotasyon, rotationSpeed * Time.fixedDeltaTime);
        }
    }
}
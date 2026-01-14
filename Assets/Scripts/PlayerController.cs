using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Ayarlar")]
    public float moveSpeed = 5f;
    public float rotationSpeed = 10f;

    [Header("Kontroller")]
    public MobileJoystick joystick; 

    [Header("Animasyon")]
    // YENİ: Animatörü buraya bağlayacağız
    public Animator characterAnimator; 

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        
        // Eğer animatörü elle bağlamadıysan, kod otomatik bulmaya çalışsın (Kolaylık olsun)
        if (characterAnimator == null)
            characterAnimator = GetComponentInChildren<Animator>();
    }

    void FixedUpdate() 
    {
        if (joystick == null) return;

        // Joystick verisi
        float xHareket = joystick.InputVector.x;
        float zHareket = joystick.InputVector.y;

        Vector3 movement = new Vector3(xHareket, 0, zHareket);
        
        // Hareket ediyor mu? (Hız 0.1'den büyükse evet)
        bool isMoving = movement.magnitude > 0.1f;

        // --- YENİ KISIM: ANİMATÖRÜ GÜNCELLE ---
        if (characterAnimator != null)
        {
            // Animator penceresinde oluşturduğumuz "IsRunning" parametresini değiştiriyoruz
            characterAnimator.SetBool("IsRunning", isMoving);
        }
        // --------------------------------------

        // Fiziksel Hareket
        if (isMoving)
        {
            Vector3 yeniPozisyon = rb.position + movement * moveSpeed * Time.fixedDeltaTime;
            rb.MovePosition(yeniPozisyon);

            Quaternion hedefRotasyon = Quaternion.LookRotation(movement);
            rb.rotation = Quaternion.Lerp(rb.rotation, hedefRotasyon, rotationSpeed * Time.fixedDeltaTime);
        }
    }
}
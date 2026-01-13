using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Ayarlar")]
    public float moveSpeed = 5f; // Karakterin hızı
    public float rotationSpeed = 10f; // Dönüş hızı

    private Vector3 _moveVector;
    private Vector3 _lastInputPos;
    private bool _isTouching;

    void Update()
    {
        // --- 1. GİRİŞİ AL (Mouse veya Dokunmatik) ---
        if (Input.GetMouseButtonDown(0))
        {
            _isTouching = true;
            _lastInputPos = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            _isTouching = false;
            _moveVector = Vector3.zero; // Bırakınca dursun
        }

        // --- 2. HAREKETİ HESAPLA ---
        if (_isTouching)
        {
            Vector3 currentInputPos = Input.mousePosition;
            Vector3 direction = (currentInputPos - _lastInputPos).normalized;
            
            // Joystick hassasiyeti için ufak bir ölü bölge (deadzone)
            if (Vector3.Distance(currentInputPos, _lastInputPos) > 10f) 
            {
                // Yere paralel hareket (Y ekseni yok)
                _moveVector = new Vector3(direction.x, 0, direction.y);
                
                // --- 3. KARAKTERİ HAREKET ETTİR ---
                transform.position += _moveVector * moveSpeed * Time.deltaTime;

                // --- 4. KARAKTERİ DÖNDÜR ---
                Quaternion targetRotation = Quaternion.LookRotation(_moveVector);
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
            
            // Parmağın kaymasını takip et (Dynamic Joystick hissi)
            _lastInputPos = currentInputPos; 
        }
    }
}
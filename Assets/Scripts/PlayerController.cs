using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Ayarlar")]
    public float moveSpeed = 5f;
    public float rotationSpeed = 10f;

    private Vector3 _moveVector;
    private Vector3 _lastInputPos;
    private bool _isTouching;

    void Update()
    {
        // Dokunma algılama
        if (Input.GetMouseButtonDown(0))
        {
            _isTouching = true;
            _lastInputPos = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            _isTouching = false;
        }

        // Hareket hesaplama
        if (_isTouching)
        {
            Vector3 currentInputPos = Input.mousePosition;
            Vector3 direction = (currentInputPos - _lastInputPos).normalized;

            if (Vector3.Distance(currentInputPos, _lastInputPos) > 5f)
            {
                _moveVector = new Vector3(direction.x, 0, direction.y);
                transform.position += _moveVector * moveSpeed * Time.deltaTime;

                Quaternion targetRotation = Quaternion.LookRotation(_moveVector);
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
            _lastInputPos = currentInputPos;
        }
    }
}
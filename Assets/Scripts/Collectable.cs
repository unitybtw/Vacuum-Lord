using UnityEngine;

public class Collectable : MonoBehaviour
{
    public bool vakumlaniyorMu = false;

    [Header("Değer Ayarı")]
    public int fiyatCarpani = 1; // Normal küpler 1, Altın küpler 5 olacak
    
    // Altın efekti için (Opsiyonel): Kendi etrafında dönsün
    void Update()
    {
        if(fiyatCarpani > 1 && !vakumlaniyorMu)
        {
            transform.Rotate(Vector3.up * 50 * Time.deltaTime);
        }
    }
}
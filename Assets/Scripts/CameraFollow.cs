using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform hedef; // Takip edilecek obje (Player)
    public float takipHizi = 0.125f; // Ne kadar yumuşak takip etsin? (Düşük = Yumuşak)
    public Vector3 offset; // Oyuncuya göre konumu (Mesafesi)

    void LateUpdate() // LateUpdate, oyuncu hareket ettikten SONRA çalışır. Titremeyi önler.
    {
        if (hedef == null) return;

        // Hedef pozisyonu hesapla (Oyuncunun yeri + aradaki mesafe)
        Vector3 istenenPozisyon = hedef.position + offset;
        
        // Yumuşak geçiş yap (Lerp)
        Vector3 yumusakPozisyon = Vector3.Lerp(transform.position, istenenPozisyon, takipHizi);
        
        // Kamerayı taşı
        transform.position = yumusakPozisyon;

        // Kameranın açısını her zaman oyuncuya bakacak şekilde sabitle (İsteğe bağlı, ama bu açıda gerek yok)
        // transform.LookAt(hedef); 
    }
}
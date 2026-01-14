using UnityEngine;
using TMPro; // TextMeshPro için gerekli

public class FloatingText : MonoBehaviour
{
    public float hareketHizi = 3f; // Ne kadar hızlı yükselsin?
    public float yokOlmaSuresi = 1.5f; // Kaç saniye sonra kaybolsun?
    
    private TextMeshPro textMesh; // Yazının kendisi
    private Color baslangicRengi;

    void Awake()
    {
        textMesh = GetComponent<TextMeshPro>();
        baslangicRengi = textMesh.color;
    }

    // Bu fonksiyonu GameManager'dan çağıracağız
    public void Setup(int miktar)
    {
        textMesh.text = "+" + miktar + " $";
        
        // Belirlenen süre sonunda kendini yok etmeye kur
        Destroy(gameObject, yokOlmaSuresi);
    }

    void Update()
    {
        // 1. Yukarı Hareket
        transform.position += Vector3.up * hareketHizi * Time.deltaTime;

        // 2. Yavaşça Sönme (Fade Out) Efekti
        // Kalan süreye göre şeffaflığı (alpha) azaltıyoruz
        float kalanOmur = yokOlmaSuresi - Time.timeSinceLevelLoad; // Basit bir zaman hesabı
        // Not: Bu sönme hesabı biraz basitleştirilmiştir, maksat iş görsün.
        Color yeniRenk = textMesh.color;
        yeniRenk.a -= Time.deltaTime / yokOlmaSuresi; // Zamanla görünmez ol
        textMesh.color = yeniRenk;
    }
}
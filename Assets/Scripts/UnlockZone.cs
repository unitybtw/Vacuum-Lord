using UnityEngine;
using TMPro; // Yazı için şart

public class UnlockZone : MonoBehaviour
{
    [Header("Ayarlar")]
    public int toplamMaliyet = 500; // Açmak için kaç para lazım?
    public float odemeHizi = 0.1f;  // Ne kadar hızlı para çeksin?
    public GameObject engelDuvari;  // Yok olacak duvar/kapı

    [Header("UI")]
    public TextMeshPro kalanParaText; // Yerdeki "500$" yazısı

    private int _suAnkiOdenen = 0;
    private float _zamanlayici = 0;

    private void Start()
    {
        UpdateText();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Zamanlayıcıyı çalıştır
            _zamanlayici -= Time.deltaTime;

            // Yeterli sürede bir ve paramız varsa ödeme yap
            if (_zamanlayici <= 0 && GameManager.instance.toplamPara > 0 && _suAnkiOdenen < toplamMaliyet)
            {
                OdemeYap();
                _zamanlayici = odemeHizi; // Süreyi sıfırla
            }
        }
    }

    void OdemeYap()
    {
        // 1. Oyuncunun parasını azalt
        GameManager.instance.toplamPara -= 5; // Her seferinde 5$ düşsün (Hızlı aksın)
        // Eğer para eksiye düşerse düzelt
        if(GameManager.instance.toplamPara < 0) GameManager.instance.toplamPara = 0;
        
        // GameManager UI güncellemesini tetikle
        GameManager.instance.ParaEkle(0); 

        // 2. Ödenen miktarı artır
        _suAnkiOdenen += 5;

        // 3. Yazıyı güncelle
        UpdateText();

        // 4. Bitti mi kontrol et
        if (_suAnkiOdenen >= toplamMaliyet)
        {
            BolgeyiAc();
        }
    }

    void UpdateText()
    {
        int kalan = toplamMaliyet - _suAnkiOdenen;
        if (kalan < 0) kalan = 0;
        
        // Ekrana "Kalan: 450 $" yazar
        kalanParaText.text = kalan.ToString() + " $";
    }

    void BolgeyiAc()
    {
        // --- YENİ KISIM: ZAFER SESİ ÇAL ---
        if (AudioManager.instance != null)
        {
            AudioManager.instance.PlayWin();
        }

        // Duvarı yok et
        if(engelDuvari != null) Destroy(engelDuvari);
        
        // Bu ödeme noktasını da yok et (Artık işi bitti)
        Destroy(gameObject);
    }
}
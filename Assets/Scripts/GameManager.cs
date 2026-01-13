using UnityEngine;
using TMPro; // TextMeshPro kütüphanesi

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("UI Elemanları")]
    public TextMeshProUGUI paraText;         // Sol üstteki para yazısı
    public TextMeshProUGUI hizFiyatText;     // Hız butonu üzerindeki yazı (Opsiyonel)
    public TextMeshProUGUI kapasiteFiyatText;// Çanta butonu üzerindeki yazı (Opsiyonel)

    [Header("Ekonomi Ayarları")]
    public int toplamPara = 0;
    public int hizMaliyeti = 50;
    public int kapasiteMaliyeti = 100;

    [Header("Referanslar")]
    public PlayerController playerScript; // Hızı artırmak için oyuncuya erişmemiz lazım

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    private void Start()
    {
        UpdateUI(); // Oyun başlarken fiyatları ve parayı yaz
    }

    // Para ekleme fonksiyonu
    public void ParaEkle(int miktar)
    {
        toplamPara += miktar;
        UpdateUI();
    }

    // --- BUTON 1: HIZ UPGRADE ---
    public void HizaUpgradeYap()
    {
        if (toplamPara >= hizMaliyeti)
        {
            toplamPara -= hizMaliyeti;       // Parayı düş
            playerScript.moveSpeed += 1f;    // Oyuncuyu hızlandır
            hizMaliyeti += 50;               // Yeni fiyatı artır
            UpdateUI();                      // Ekranı güncelle
        }
    }

    // --- BUTON 2: KAPASİTE UPGRADE ---
    public void KapasiteUpgradeYap()
    {
        if (toplamPara >= kapasiteMaliyeti)
        {
            toplamPara -= kapasiteMaliyeti;          // Parayı düş
            StackManager.instance.maxKapasite += 5;  // Çantayı büyüt
            kapasiteMaliyeti += 100;                 // Yeni fiyatı artır
            UpdateUI();                              // Ekranı güncelle
        }
    }

    // Tüm yazıları güncelleyen yardımcı fonksiyon
    void UpdateUI()
    {
        if (paraText != null) 
            paraText.text = toplamPara.ToString() + " $";

        if (hizFiyatText != null) 
            hizFiyatText.text = "HIZ: " + hizMaliyeti + "$";

        if (kapasiteFiyatText != null) 
            kapasiteFiyatText.text = "ÇANTA: " + kapasiteMaliyeti + "$";
    }
}
using UnityEngine;
using TMPro; // TextMeshPro kütüphanesi

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("UI Elemanları")]
    public TextMeshProUGUI paraText;         
    public TextMeshProUGUI hizFiyatText;     
    public TextMeshProUGUI kapasiteFiyatText;

    [Header("Efektler")]
    public GameObject floatingTextPrefab; // --- YENİ EKLENDİ: Uçan yazı prefab'ı buraya gelecek ---

    [Header("Ekonomi Ayarları")]
    public int toplamPara = 0;
    public int hizMaliyeti = 50;
    public int kapasiteMaliyeti = 100;

    [Header("Referanslar")]
    public PlayerController playerScript; 

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    private void Start()
    {
        UpdateUI(); 
    }

    // Para ekleme fonksiyonu (GÜNCELLENDİ)
    public void ParaEkle(int miktar)
    {
        toplamPara += miktar;
        UpdateUI();

        // --- YENİ EKLENDİ: Uçan Yazı Oluşturma ---
        if (floatingTextPrefab != null && playerScript != null)
        {
            // Oyuncunun kafasının biraz üstünde pozisyon belirle (Y + 2.0f)
            Vector3 spawnPos = playerScript.transform.position + new Vector3(0, 2.5f, 0);
            
            // Yazıyı oluştur
            GameObject yazi = Instantiate(floatingTextPrefab, spawnPos, Quaternion.identity);
            
            // Yazıya miktarı gönder (Setup fonksiyonunu çağır)
            // Not: FloatingText scriptinin var olduğundan emin olmalıyız
            FloatingText textScript = yazi.GetComponent<FloatingText>();
            if (textScript != null)
            {
                textScript.Setup(miktar);
            }
        }
        // ------------------------------------------
    }

    public void HizaUpgradeYap()
    {
        if (toplamPara >= hizMaliyeti)
        {
            toplamPara -= hizMaliyeti;       
            playerScript.moveSpeed += 1f;    
            hizMaliyeti += 50;               
            UpdateUI();                      
        }
    }

    public void KapasiteUpgradeYap()
    {
        if (toplamPara >= kapasiteMaliyeti)
        {
            toplamPara -= kapasiteMaliyeti;          
            StackManager.instance.maxKapasite += 5;  
            kapasiteMaliyeti += 100;                 
            UpdateUI();                              
        }
    }

    void UpdateUI()
    {
        if (paraText != null) paraText.text = toplamPara.ToString() + " $";
        if (hizFiyatText != null) hizFiyatText.text = "HIZ: " + hizMaliyeti + "$";
        if (kapasiteFiyatText != null) kapasiteFiyatText.text = "ÇANTA: " + kapasiteMaliyeti + "$";
    }
}
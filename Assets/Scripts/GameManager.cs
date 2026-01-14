using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("UI Elemanları")]
    public TextMeshProUGUI paraText;         
    public TextMeshProUGUI hizFiyatText;     
    public TextMeshProUGUI kapasiteFiyatText;

    [Header("Efektler")]
    public GameObject floatingTextPrefab; 

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
        LoadGame(); // Oyun açılınca verileri yükle
        UpdateUI(); 
    }

    public void ParaEkle(int miktar)
    {
        toplamPara += miktar;
        SaveGame(); // Her para değişiminde kaydet
        UpdateUI();

        // Uçan Yazı Efekti
        if (floatingTextPrefab != null && playerScript != null && miktar > 0)
        {
            Vector3 spawnPos = playerScript.transform.position + new Vector3(0, 2.5f, 0);
            GameObject yazi = Instantiate(floatingTextPrefab, spawnPos, Quaternion.identity);
            
            FloatingText textScript = yazi.GetComponent<FloatingText>();
            if (textScript != null) textScript.Setup(miktar);
        }
    }

    public void HizaUpgradeYap()
    {
        if (toplamPara >= hizMaliyeti)
        {
            toplamPara -= hizMaliyeti;       
            playerScript.moveSpeed += 1f;    
            hizMaliyeti += 50;
            
            SaveGame(); // Kaydet
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
            
            SaveGame(); // Kaydet
            UpdateUI();                              
        }
    }

    void UpdateUI()
    {
        if (paraText != null) paraText.text = toplamPara.ToString() + " $";
        if (hizFiyatText != null) hizFiyatText.text = "HIZ: " + hizMaliyeti + "$";
        if (kapasiteFiyatText != null) kapasiteFiyatText.text = "ÇANTA: " + kapasiteMaliyeti + "$";
    }

    // --- SAVE & LOAD SİSTEMİ ---

    public void SaveGame()
    {
        PlayerPrefs.SetInt("Para", toplamPara);
        PlayerPrefs.SetInt("HizMaliyeti", hizMaliyeti);
        PlayerPrefs.SetInt("KapasiteMaliyeti", kapasiteMaliyeti);
        
        // Oyuncunun özelliklerini de kaydedelim
        PlayerPrefs.SetFloat("PlayerSpeed", playerScript.moveSpeed);
        PlayerPrefs.SetInt("PlayerCapacity", StackManager.instance.maxKapasite);
        
        PlayerPrefs.Save(); // Diske yaz
    }

    public void LoadGame()
    {
        // Eğer daha önce kayıt varsa yükle, yoksa varsayılanları kullan
        if (PlayerPrefs.HasKey("Para"))
        {
            toplamPara = PlayerPrefs.GetInt("Para");
            hizMaliyeti = PlayerPrefs.GetInt("HizMaliyeti");
            kapasiteMaliyeti = PlayerPrefs.GetInt("KapasiteMaliyeti");

            // Oyuncunun hızını ve kapasitesini geri yükle
            playerScript.moveSpeed = PlayerPrefs.GetFloat("PlayerSpeed");
            StackManager.instance.maxKapasite = PlayerPrefs.GetInt("PlayerCapacity");
        }
    }
    
    // Test amaçlı: Verileri sıfırlamak istersen bu kodu kullanabilirsin
    // private void Update() { if(Input.GetKeyDown(KeyCode.R)) { PlayerPrefs.DeleteAll(); Debug.Log("Resetlendi"); } }
}
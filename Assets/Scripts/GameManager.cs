using UnityEngine;
using TMPro;
using UnityEngine.UI; // Slider için gerekli

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("UI Elemanları")]
    public TextMeshProUGUI paraText;         
    public TextMeshProUGUI hizFiyatText;     
    public TextMeshProUGUI kapasiteFiyatText;
    
    // --- YENİ EKLENENLER (ÇANTA UI) ---
    public Slider kapasiteSlider;
    public TextMeshProUGUI kapasiteText;
    // ----------------------------------

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
        LoadGame();
        UpdateUI(); 
    }

    public void ParaEkle(int miktar)
    {
        toplamPara += miktar;
        SaveGame();
        UpdateUI();

        if (floatingTextPrefab != null && playerScript != null && miktar > 0)
        {
            Vector3 spawnPos = playerScript.transform.position + new Vector3(0, 2.5f, 0);
            GameObject yazi = Instantiate(floatingTextPrefab, spawnPos, Quaternion.identity);
            FloatingText textScript = yazi.GetComponent<FloatingText>();
            if (textScript != null) textScript.Setup("+" + miktar);
        }
    }

    public void HizaUpgradeYap()
    {
        if (toplamPara >= hizMaliyeti)
        {
            toplamPara -= hizMaliyeti;       
            playerScript.moveSpeed += 1f;    
            hizMaliyeti += 50;
            SaveGame();
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
            SaveGame();
            UpdateUI();                              
        }
    }

    // Bu fonksiyonu her yerden çağırabiliriz (Update içinde de çağıracağız)
    public void UpdateUI()
    {
        // Para Yazısı
        if (paraText != null) paraText.text = toplamPara.ToString() + " $";
        if (hizFiyatText != null) hizFiyatText.text = "HIZ: " + hizMaliyeti + "$";
        if (kapasiteFiyatText != null) kapasiteFiyatText.text = "ÇANTA: " + kapasiteMaliyeti + "$";

        // --- YENİ: ÇANTA UI GÜNCELLEME ---
        if (StackManager.instance != null)
        {
            int mevcut = StackManager.instance.tasinanObjeler.Count;
            int max = StackManager.instance.maxKapasite;

            // Slider doluluk oranı (0 ile 1 arası)
            if (kapasiteSlider != null) 
                kapasiteSlider.value = (float)mevcut / (float)max;

            // Yazı (Örn: 5 / 10)
            if (kapasiteText != null)
                kapasiteText.text = mevcut + " / " + max;
        }
    }

    private void Update()
    {
        // Çanta durumu sürekli değişebileceği için UI'ı sürekli güncel tutalım
        UpdateUI(); 
        
        if (Input.GetKeyDown(KeyCode.Delete)) { PlayerPrefs.DeleteAll(); Debug.Log("Sıfırlandı"); }
    }

    // Save & Load kodları aynı kalıyor...
    public void SaveGame()
    {
        PlayerPrefs.SetInt("Para", toplamPara);
        PlayerPrefs.SetInt("HizMaliyeti", hizMaliyeti);
        PlayerPrefs.SetInt("KapasiteMaliyeti", kapasiteMaliyeti);
        if(playerScript != null) PlayerPrefs.SetFloat("PlayerSpeed", playerScript.moveSpeed);
        if(StackManager.instance != null) PlayerPrefs.SetInt("PlayerCapacity", StackManager.instance.maxKapasite);
        PlayerPrefs.Save();
    }

    public void LoadGame()
    {
        if (PlayerPrefs.HasKey("Para"))
        {
            toplamPara = PlayerPrefs.GetInt("Para");
            hizMaliyeti = PlayerPrefs.GetInt("HizMaliyeti");
            kapasiteMaliyeti = PlayerPrefs.GetInt("KapasiteMaliyeti");
            if(playerScript != null) playerScript.moveSpeed = PlayerPrefs.GetFloat("PlayerSpeed", 5f);
            if(StackManager.instance != null) StackManager.instance.maxKapasite = PlayerPrefs.GetInt("PlayerCapacity", 10);
        }
    }
}
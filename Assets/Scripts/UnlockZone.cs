using UnityEngine;
using TMPro;

public class UnlockZone : MonoBehaviour
{
    [Header("Kimlik Ayarı")]
    public string zoneID = "Zone1"; 

    [Header("Ayarlar")]
    public int toplamMaliyet = 500;
    public float odemeHizi = 0.1f;
    public GameObject engelDuvari;

    [Header("Efektler")]
    public GameObject konfetiPrefab; // --- YENİ EKLENDİ ---

    [Header("UI")]
    public TextMeshPro kalanParaText;

    private int _suAnkiOdenen = 0;
    private float _zamanlayici = 0;
    private bool _acildiMi = false;

    private void Start()
    {
        if (PlayerPrefs.GetInt(zoneID) == 1)
        {
            _acildiMi = true;
            if(engelDuvari != null) Destroy(engelDuvari);
            Destroy(gameObject);
        }
        else
        {
            UpdateText();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (_acildiMi) return;

        if (other.CompareTag("Player"))
        {
            _zamanlayici -= Time.deltaTime;

            if (_zamanlayici <= 0 && GameManager.instance.toplamPara > 0 && _suAnkiOdenen < toplamMaliyet)
            {
                OdemeYap();
                _zamanlayici = odemeHizi;
            }
        }
    }

    void OdemeYap()
    {
        GameManager.instance.toplamPara -= 5;
        if(GameManager.instance.toplamPara < 0) GameManager.instance.toplamPara = 0;
        
        GameManager.instance.ParaEkle(0); 

        _suAnkiOdenen += 5;
        UpdateText();

        if (_suAnkiOdenen >= toplamMaliyet)
        {
            BolgeyiAc();
        }
    }

    void UpdateText()
    {
        int kalan = toplamMaliyet - _suAnkiOdenen;
        if (kalan < 0) kalan = 0;
        kalanParaText.text = kalan.ToString() + " $";
    }

    void BolgeyiAc()
    {
        _acildiMi = true;
        PlayerPrefs.SetInt(zoneID, 1);
        PlayerPrefs.Save();

        if (AudioManager.instance != null) AudioManager.instance.PlayWin();

        // --- YENİ KISIM: KONFETİ PATLAT ---
        if (konfetiPrefab != null)
        {
            // Efekti duvarın olduğu yerde oluştur
            // (Duvar yoksa kendi üzerimizde oluştururuz)
            Vector3 efektYeri = transform.position;
            if (engelDuvari != null) efektYeri = engelDuvari.transform.position;

            Instantiate(konfetiPrefab, efektYeri, Quaternion.identity);
        }
        // ----------------------------------

        if(engelDuvari != null) Destroy(engelDuvari);
        Destroy(gameObject);
    }
}
using UnityEngine;
using TMPro;

public class UnlockZone : MonoBehaviour
{
    [Header("Kimlik Ayarı")]
    public string zoneID = "Zone1"; // Her bölge için FARKLI bir isim verin (Örn: Zone2, Zone3)

    [Header("Ayarlar")]
    public int toplamMaliyet = 500;
    public float odemeHizi = 0.1f;
    public GameObject engelDuvari;

    [Header("UI")]
    public TextMeshPro kalanParaText;

    private int _suAnkiOdenen = 0;
    private float _zamanlayici = 0;
    private bool _acildiMi = false;

    private void Start()
    {
        // 1. Kayıt Kontrolü: Bu bölge daha önce açıldı mı?
        if (PlayerPrefs.GetInt(zoneID) == 1)
        {
            _acildiMi = true;
            // Duvarı ve kendimizi hemen yok et
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
        
        GameManager.instance.ParaEkle(0); // UI Güncellemesi için

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

        // KAYIT: Bu bölgenin açıldığını hafızaya yaz (1 = Açıldı)
        PlayerPrefs.SetInt(zoneID, 1);
        PlayerPrefs.Save();

        // Ses Çal
        if (AudioManager.instance != null) AudioManager.instance.PlayWin();

        if(engelDuvari != null) Destroy(engelDuvari);
        Destroy(gameObject);
    }
}
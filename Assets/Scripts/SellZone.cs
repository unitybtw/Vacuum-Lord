using UnityEngine;

public class SellZone : MonoBehaviour
{
    [Header("Ayarlar")]
    public float satisHizi = 0.1f; 
    public int bazFiyat = 15; // "birimFiyat" yerine "bazFiyat" dedik

    [Header("Efektler")]
    public GameObject floatingTextPrefab; // Uçan yazı prefabı buraya!

    private float _zamanlayici = 0f;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _zamanlayici -= Time.deltaTime;

            if (_zamanlayici <= 0f)
            {
                SatisYap();
                _zamanlayici = satisHizi;
            }
        }
    }

    void SatisYap()
    {
        // Sırtta eşya var mı?
        if (StackManager.instance.tasinanObjeler.Count > 0)
        {
            // En üstteki objeyi al
            int sonIndex = StackManager.instance.tasinanObjeler.Count - 1;
            GameObject satilacakObje = StackManager.instance.tasinanObjeler[sonIndex];

            // --- FİYAT HESAPLAMA (ÖNEMLİ KISIM) ---
            // Objede "Collectable" scripti var mı? Varsa çarpanına bak.
            Collectable copScript = satilacakObje.GetComponent<Collectable>();
            
            int odenecekTutar = bazFiyat; 
            bool altinMi = false;

            if (copScript != null)
            {
                // Normal küp için 1, Altın için 5 ile çarpar
                odenecekTutar = bazFiyat * copScript.fiyatCarpani;
                
                if (copScript.fiyatCarpani > 1) altinMi = true; // Altın olduğunu anladık
            }
            // --------------------------------------

            // Listeden çıkar ve yok et
            StackManager.instance.tasinanObjeler.RemoveAt(sonIndex);
            Destroy(satilacakObje);

            // Parayı ver
            GameManager.instance.ParaEkle(odenecekTutar);

            // 1. SES
            if (AudioManager.instance != null) AudioManager.instance.PlayCash();
            
            // 2. TİTREŞİM
            if (VibrationManager.instance != null) VibrationManager.instance.Titret();

            // 3. UÇAN YAZI (FLOATING TEXT)
            if (floatingTextPrefab != null)
            {
                Vector3 pozisyon = transform.position + Vector3.up * 2f; 
                GameObject yazi = Instantiate(floatingTextPrefab, pozisyon, Quaternion.identity);
                
                FloatingText textScript = yazi.GetComponent<FloatingText>();
                if (textScript != null)
                {
                    if (altinMi)
                    {
                        // Altınsa SARI renk
                        textScript.SetText("+" + odenecekTutar + "$", Color.yellow);
                        yazi.transform.localScale = Vector3.one * 1.5f; 
                    }
                    else
                    {
                        // Normalse YEŞİL renk
                        textScript.SetText("+" + odenecekTutar + "$", Color.green);
                    }
                }
            }
        }
    }
}
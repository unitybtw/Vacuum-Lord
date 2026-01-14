using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("Ses Dosyaları (Klipler)")]
    public AudioClip popSesi;     // Küpü vakumlama sesi
    public AudioClip paraSesi;    // Satış yapma sesi
    public AudioClip kilitAcmaSesi; // Yeni bölge açılma sesi (Zafer)

    [Header("Ses Kaynağı")]
    public AudioSource efektKaynagi; // Seslerin çıkacağı hoparlör

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    // --- 1. VAKUMLAMA SESİ (ASMR) ---
    public void PlayPop()
    {
        // Püf Nokta: Her seferinde tonu (pitch) biraz değiştirirsek 
        // ses "makineli tüfek" gibi değil, doğal ve tatlı gelir.
        efektKaynagi.pitch = Random.Range(0.8f, 1.2f); 
        efektKaynagi.PlayOneShot(popSesi);
    }

    // --- 2. PARA SESİ ---
    public void PlayCash()
    {
        efektKaynagi.pitch = 1f; // Para sesi hep aynı netlikte olsun
        efektKaynagi.PlayOneShot(paraSesi);
    }

    // --- 3. ZAFER SESİ ---
    public void PlayWin()
    {
        efektKaynagi.pitch = 1f;
        efektKaynagi.PlayOneShot(kilitAcmaSesi);
    }
}
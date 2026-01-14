using UnityEngine;

public class VibrationManager : MonoBehaviour
{
    public static VibrationManager instance;

    [Header("Ayarlar")]
    public bool titresimAcik = true; // İleride ayarlardan kapatmak istersen

    private void Awake()
    {
        // Singleton yapısı (Her yerden erişebilmek için)
        if (instance == null) instance = this;
    }

    // Basit Titreşim (Unity'nin standart titreşimi)
    public void Titret()
    {
        if (titresimAcik)
        {
            // Sadece telefonda çalışır, editörde hata vermez ama çalışmaz.
#if UNITY_ANDROID || UNITY_IOS
            Handheld.Vibrate();
#endif
        }
    }
}
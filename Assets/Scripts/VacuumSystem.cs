using UnityEngine;

public class VacuumSystem : MonoBehaviour
{
    [Header("Vakum Ayarları")]
    public float cekimHizi = 15f;    // Objelerin gelme hızı
    public Transform vakumNoktasi;   // Objelerin gireceği delik

    private void OnTriggerStay(Collider other)
    {
        // 1. Kapasite kontrolü: Çanta doluysa çekme
        if (StackManager.instance.tasinanObjeler.Count >= StackManager.instance.maxKapasite)
            return;

        Collectable cop = other.GetComponent<Collectable>();

        if (cop != null)
        {
            cop.vakumlaniyorMu = true;

            // Objeyi vakum noktasına doğru çek
            other.transform.position = Vector3.MoveTowards(other.transform.position, vakumNoktasi.position, cekimHizi * Time.deltaTime);

            // Wobble (Küçülme) efekti
            float mesafe = Vector3.Distance(other.transform.position, vakumNoktasi.position);
            if (mesafe < 0.5f)
            {
                other.transform.localScale = Vector3.Lerp(other.transform.localScale, Vector3.zero, 15f * Time.deltaTime);
            }

            // Obje deliğe girdiyse StackManager'a teslim et
            if (mesafe < 0.2f)
            {
                // Objenin scale'ini düzeltip öyle verelim (yoksa minicik kalır)
                other.transform.localScale = Vector3.one; 
                
                StackManager.instance.AddToStack(other.gameObject);

                // --- YENİ KISIM: SESİ ÇAL (POP) ---
                if (AudioManager.instance != null)
                {
                    AudioManager.instance.PlayPop();
                }
            }
        }
    }
}
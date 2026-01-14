using UnityEngine;

public class VacuumSystem : MonoBehaviour
{
    [Header("Vakum Ayarları")]
    public float cekimHizi = 15f;    
    public Transform vakumNoktasi;   

    [Header("Efektler")]
    public GameObject vakumEfektiPrefab;

    private void OnTriggerStay(Collider other)
    {
        if (StackManager.instance.tasinanObjeler.Count >= StackManager.instance.maxKapasite)
            return;

        Collectable cop = other.GetComponent<Collectable>();

        if (cop != null)
        {
            cop.vakumlaniyorMu = true;

            // Çekim hareketi
            other.transform.position = Vector3.MoveTowards(other.transform.position, vakumNoktasi.position, cekimHizi * Time.deltaTime);

            float mesafe = Vector3.Distance(other.transform.position, vakumNoktasi.position);
            
            // Küçülme efekti
            if (mesafe < 0.5f)
            {
                other.transform.localScale = Vector3.Lerp(other.transform.localScale, Vector3.zero, 15f * Time.deltaTime);
            }

            // İçeri girdi mi?
            if (mesafe < 0.2f)
            {
                other.transform.localScale = Vector3.one; 
                StackManager.instance.AddToStack(other.gameObject);

                // Ses Çal
                if (AudioManager.instance != null) AudioManager.instance.PlayPop();

                // Toz Efekti
                if (vakumEfektiPrefab != null)
                {
                    Instantiate(vakumEfektiPrefab, vakumNoktasi.position, Quaternion.identity);
                }

                // --- YENİ KISIM: TİTREŞİM ---
                if (VibrationManager.instance != null)
                {
                    VibrationManager.instance.Titret();
                }
                // ----------------------------
            }
        }
    }
}
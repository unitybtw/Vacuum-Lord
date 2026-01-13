using UnityEngine;

public class VacuumSystem : MonoBehaviour
{
    [Header("Vakum Ayarları")]
    public float cekimHizi = 5f; // Objelerin sana gelme hızı
    public Transform vakumNoktasi; // Objelerin gireceği delik (Sırt çantası veya hortum ucu)

    private void OnTriggerStay(Collider other)
    {
        // Eğer alanımıza giren şey bir "Collectable" ise...
        Collectable cop = other.GetComponent<Collectable>();

        if (cop != null)
        {
            cop.vakumlaniyorMu = true;

            // 1. Objeyi vakum noktasına doğru çek
            other.transform.position = Vector3.MoveTowards(other.transform.position, vakumNoktasi.position, cekimHizi * Time.deltaTime);

            // 2. Objeyi küçült (Wobble / Jöle Efekti)
            // Obje yaklaştıkça scale'i 0'a doğru küçülsün
            float mesafe = Vector3.Distance(other.transform.position, vakumNoktasi.position);
            
            if (mesafe < 0.5f) // Çok yaklaştıysa
            {
                other.transform.localScale = Vector3.Lerp(other.transform.localScale, Vector3.zero, 10f * Time.deltaTime);
            }

            // 3. Obje tam deliğe girdiyse YOK ET ve PARA VER
            if (mesafe < 0.2f)
            {
                // TODO: İleride buraya para artırma kodu ekleyeceğiz (GameManager.Instance.ParaEkle(cop.paraDegeri))
                Debug.Log(cop.name + " Vakumlandı! +" + cop.paraDegeri + " TL");
                Destroy(other.gameObject);
            }
        }
    }
}
using UnityEngine;
using System.Collections; // Coroutine (Zamanlayıcı) için gerekli

public class Spawner : MonoBehaviour
{
    [Header("Ayarlar")]
    public GameObject copPrefab; // Üretilecek çöp (Küp/Sphere)
    public float uretimHizi = 2f; // Kaç saniyede bir çıksın?
    public float alanGenisligi = 4f; // Ne kadar geniş bir alana dağılsın?
    public int maksCopSayisi = 20; // Sahnede en fazla kaç tane olsun? (Kasma yapmasın)

    private void Start()
    {
        // Oyun başlayınca üretime başla
        StartCoroutine(CopUret());
    }

    IEnumerator CopUret()
    {
        while (true) // Sonsuz döngü
        {
            // Sahnede kaç tane Collectable var say
            int mevcutCop = FindObjectsOfType<Collectable>().Length;

            // Eğer sayı sınırın altındaysa yeni üret
            if (mevcutCop < maksCopSayisi)
            {
                // Rastgele bir pozisyon belirle (Spawner'ın olduğu yerin etrafında)
                Vector3 rastgelePos = transform.position + new Vector3(
                    Random.Range(-alanGenisligi, alanGenisligi), 
                    0, 
                    Random.Range(-alanGenisligi, alanGenisligi)
                );

                // Çöpü oluştur
                Instantiate(copPrefab, rastgelePos, Quaternion.identity);
            }

            // Belirlenen süre kadar bekle
            yield return new WaitForSeconds(uretimHizi);
        }
    }
}
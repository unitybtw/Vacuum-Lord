using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour
{
    [Header("Prefablar")]
    public GameObject copPrefab;       // Normal (Mavi) Çöp
    public GameObject altinCopPrefab;  // YENİ: Altın (Sarı) Çöp

    [Header("Ayarlar")]
    public float uretimHizi = 1f;
    public float alanGenisligi = 8f;
    public int maksCopSayisi = 30;
    
    [Range(0, 100)] 
    public int altinSansi = 10; // %10 ihtimalle altın çıksın

    private void Start()
    {
        StartCoroutine(CopUret());
    }

    IEnumerator CopUret()
    {
        while (true)
        {
            // Sahnedeki toplam çöp sayısını bul (Hem normal hem altınları sayar)
            int mevcutCop = FindObjectsOfType<Collectable>().Length;

            if (mevcutCop < maksCopSayisi)
            {
                // Rastgele pozisyon belirle
                Vector3 rastgelePos = transform.position + new Vector3(
                    Random.Range(-alanGenisligi, alanGenisligi), 
                    1f, // Biraz havadan doğsun
                    Random.Range(-alanGenisligi, alanGenisligi)
                );

                // --- YENİ KISIM: HANGİSİNİ ÜRETELİM? ---
                GameObject secilenPrefab = copPrefab; // Varsayılan: Normal

                // 0 ile 100 arasında bir zar at. Eğer şansımızdan düşük gelirse ALTIN seç.
                // (Örn: Şans 10 ise, 0-9 arası gelirse altın olur)
                if (altinCopPrefab != null && Random.Range(0, 100) < altinSansi)
                {
                    secilenPrefab = altinCopPrefab;
                }
                // ---------------------------------------

                Instantiate(secilenPrefab, rastgelePos, Quaternion.identity);
            }
            
            // Bekle
            yield return new WaitForSeconds(uretimHizi);
        }
    }
}
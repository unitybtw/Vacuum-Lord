using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour
{
    [Header("Ayarlar")]
    public GameObject copPrefab;
    public float uretimHizi = 1f;
    public float alanGenisligi = 8f;
    public int maksCopSayisi = 30;

    private void Start()
    {
        StartCoroutine(CopUret());
    }

    IEnumerator CopUret()
    {
        while (true)
        {
            int mevcutCop = FindObjectsOfType<Collectable>().Length;

            if (mevcutCop < maksCopSayisi)
            {
                Vector3 rastgelePos = transform.position + new Vector3(
                    Random.Range(-alanGenisligi, alanGenisligi), 
                    1f, // Biraz havadan doğsun
                    Random.Range(-alanGenisligi, alanGenisligi)
                );

                Instantiate(copPrefab, rastgelePos, Quaternion.identity);
            }
            yield return new WaitForSeconds(uretimHizi);
        }
    }
}
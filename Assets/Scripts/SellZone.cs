using UnityEngine;

public class SellZone : MonoBehaviour
{
    [Header("Ayarlar")]
    public float satisHizi = 0.1f; // Ne kadar hızlı satacak?
    public int birimFiyat = 15;    // Tanesi kaç para?

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

            // Listeden çıkar ve yok et
            StackManager.instance.tasinanObjeler.RemoveAt(sonIndex);
            Destroy(satilacakObje);

            // Parayı ver
            GameManager.instance.ParaEkle(birimFiyat);
        }
    }
}
using UnityEngine;

public class SmartArrow : MonoBehaviour
{
    [Header("Hedefler")]
    public Transform target; // Satış Alanı (SellZone)
    public Transform player; // Oyuncu (Player)

    [Header("Görsellik")]
    public GameObject okModeli; // Okun kendisi (Gizleyip açmak için)
    public float yukseklik = 3f;
    public float donusHizi = 5f;

    void Update()
    {
        if (target == null || player == null || StackManager.instance == null) return;

        // Ok her zaman oyuncunun tepesinde dursun
        transform.position = player.position + Vector3.up * yukseklik;

        // --- KRİTİK KONTROL ---
        // Çanta dolu mu?
        bool cantaDolu = StackManager.instance.tasinanObjeler.Count >= StackManager.instance.maxKapasite;

        // Doluysa oku göster, boşsa gizle
        if (okModeli != null)
        {
            okModeli.SetActive(cantaDolu);
        }

        // Eğer ok görünüyorsa, hedefe doğru dönsün
        if (cantaDolu)
        {
            Vector3 hedefYonu = target.position - transform.position;
            hedefYonu.y = 0; // Yere bakmasın
            
            Quaternion hedefRotasyon = Quaternion.LookRotation(hedefYonu);
            transform.rotation = Quaternion.Slerp(transform.rotation, hedefRotasyon, donusHizi * Time.deltaTime);
        }
    }
}
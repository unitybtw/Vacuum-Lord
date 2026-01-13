using UnityEngine;
using System.Collections.Generic; // Listeleri kullanmak için şart

public class StackManager : MonoBehaviour
{
    public static StackManager instance; // Heryerden ulaşmak için

    [Header("Ayarlar")]
    public Transform stackStartPoint; // İstifin başlayacağı nokta (Sırtımız)
    public float itemAraligi = 0.5f; // Objeler üst üste binerken ne kadar boşluk olsun?
    
    // Sırtımızdaki objeleri tutan liste
    public List<GameObject> tasinanObjeler = new List<GameObject>();

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    // Bu fonksiyonu Vakum scriptinden çağıracağız
    public void AddToStack(GameObject yeniObje)
    {
        // 1. Fiziği ve Çarpışmayı Kapat (Artık bir eşya, fiziksel bir engel değil)
        Destroy(yeniObje.GetComponent<Rigidbody>());
        Destroy(yeniObje.GetComponent<Collider>());
        
        // 2. Scriptini kapat (Artık vakumlanamaz)
        Destroy(yeniObje.GetComponent<Collectable>());

        // 3. Objeyi bizim çocuğumuz yap (Biz nereye, o oraya)
        yeniObje.transform.SetParent(stackStartPoint);

        // 4. Konumunu Hesapla: Listedeki kaçıncı eleman? * Aralık
        float yPozisyonu = tasinanObjeler.Count * itemAraligi;
        
        // 5. Yerleştir (Animasyonsuz, direkt ışınla)
        yeniObje.transform.localPosition = new Vector3(0, yPozisyonu, 0);
        yeniObje.transform.localRotation = Quaternion.identity; // Düz dursun

        // 6. Listeye Ekle
        tasinanObjeler.Add(yeniObje);
    }
}
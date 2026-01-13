using UnityEngine;
using System.Collections.Generic;

public class StackManager : MonoBehaviour
{
    public static StackManager instance;

    [Header("Ayarlar")]
    public Transform stackStartPoint; // Sırtımızdaki nokta
    public float itemAraligi = 0.5f;  // Objeler arası mesafe
    public int maxKapasite = 5;       // Başlangıç kapasitesi

    // Sırtımızdaki objelerin listesi
    public List<GameObject> tasinanObjeler = new List<GameObject>();

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    public void AddToStack(GameObject yeniObje)
    {
        // Kapasite doluysa alma
        if (tasinanObjeler.Count >= maxKapasite) return;

        // 1. Fiziği ve eski scriptleri temizle
        Destroy(yeniObje.GetComponent<Rigidbody>());
        Destroy(yeniObje.GetComponent<Collider>());
        Destroy(yeniObje.GetComponent<Collectable>());

        // 2. Oyuncunun çocuğu yap ve listeye ekle
        yeniObje.transform.SetParent(stackStartPoint);
        tasinanObjeler.Add(yeniObje);

        // 3. Sırtına yerleştir
        float yPozisyonu = (tasinanObjeler.Count - 1) * itemAraligi;
        yeniObje.transform.localPosition = new Vector3(0, yPozisyonu, 0);
        yeniObje.transform.localRotation = Quaternion.identity;
    }
}
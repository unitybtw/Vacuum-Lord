using UnityEngine;
using TMPro; // UI yazı sistemi kütüphanesi

public class GameManager : MonoBehaviour
{
    public static GameManager instance; // Heryerden ulaşmak için anahtar

    public TextMeshProUGUI paraText; // Ekrana sürükleyeceğimiz yazı kutusu
    public int toplamPara = 0;

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    public void ParaEkle(int miktar)
    {
        toplamPara += miktar;
        // Yazıyı güncelle: "50 $" gibi
        paraText.text = toplamPara.ToString() + " $"; 
    }
}
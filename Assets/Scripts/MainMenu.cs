using UnityEngine;
using UnityEngine.SceneManagement; // Sahne geçişleri için şart

public class MainMenu : MonoBehaviour
{
    // Oyna butonuna basınca çalışacak
    public void OyunuBaslat()
    {
        // 1 numara, Build Settings'deki sahne sırasıdır.
        // Genelde 0: Menu, 1: Oyun olur.
        SceneManager.LoadScene(1); 
    }

    // Çıkış butonuna basınca çalışacak
    public void CikisYap()
    {
        Debug.Log("Oyundan Çıkıldı!"); // Editörde çıkmaz, telefonda çıkar.
        Application.Quit();
    }
}
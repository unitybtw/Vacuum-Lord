using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public GameObject pausePaneli; // Gizleyip açacağımız panel

    // Oyunu Durdurur
    public void Durdur()
    {
        pausePaneli.SetActive(true); // Paneli aç
        Time.timeScale = 0f; // ZAMANI DONDUR (Her şey durur)
    }

    // Oyuna Devam Eder
    public void DevamEt()
    {
        pausePaneli.SetActive(false); // Paneli kapat
        Time.timeScale = 1f; // Zamanı akıt
    }

    // Ana Menüye Döner
    public void MenuyeDon()
    {
        Time.timeScale = 1f; // Önce zamanı düzelt (Yoksa menü de donuk kalır!)
        SceneManager.LoadScene(0); // 0 numaralı sahne (Ana Menü)
    }
}
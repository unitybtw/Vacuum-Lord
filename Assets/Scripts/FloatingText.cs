using UnityEngine;
using TMPro;

public class FloatingText : MonoBehaviour
{
    public float yokOlmaSuresi = 1.5f;
    public float yukariHiz = 2f;
    public TextMeshPro textMesh; 

    private Color _baslangicRengi;
    private float _gecenSure;

    void Awake()
    {
        if (textMesh == null)
            textMesh = GetComponent<TextMeshPro>();
            
        if(textMesh != null)
             _baslangicRengi = textMesh.color;
    }

    void Start()
    {
        float xRandom = Random.Range(-0.5f, 0.5f);
        transform.position += new Vector3(xRandom, 0, 0);
    }

    void Update()
    {
        transform.Translate(Vector3.up * yukariHiz * Time.deltaTime);

        _gecenSure += Time.deltaTime;
        
        if (textMesh != null)
        {
            float alpha = Mathf.Lerp(1, 0, _gecenSure / yokOlmaSuresi);
            textMesh.color = new Color(_baslangicRengi.r, _baslangicRengi.g, _baslangicRengi.b, alpha);
        }

        if (_gecenSure >= yokOlmaSuresi)
        {
            Destroy(gameObject);
        }
    }

    // --- HATALARI ÇÖZEN KISIMLAR ---

    // 1. Ana Fonksiyon (SellZone bunu kullanıyor)
    public void SetText(string metin, Color renk)
    {
        if (textMesh == null) textMesh = GetComponent<TextMeshPro>();

        if (textMesh != null)
        {
            textMesh.text = metin;
            textMesh.color = renk;
            _baslangicRengi = renk;
        }
    }

    // 2. Setup (GameManager, renk ile çağırırsa burası çalışır)
    public void Setup(string metin, Color renk)
    {
        SetText(metin, renk);
    }

    // 3. Setup (GameManager, SADECE YAZI ile çağırırsa burası çalışır - Rengi Beyaz yapar)
    // SENİN ALDIĞIN HATAYI BU SATIR ÇÖZER
    public void Setup(string metin)
    {
        SetText(metin, Color.white); 
    }
}
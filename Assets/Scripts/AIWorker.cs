using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class AIWorker : MonoBehaviour
{
    [Header("Ayarlar")]
    public float toplamaMesafesi = 1.5f;
    public int maxKapasite = 5;
    public float beklemeSuresi = 0.5f;

    [Header("Görsellik")]
    public GameObject collectedCubePrefab; // Buraya "VisualCube" (Collidersız) atılacak!
    public Transform stackPoint;           
    public float stackOffset = 0.35f;      
    private List<GameObject> visualStack = new List<GameObject>(); 

    [Header("Hedefler")]
    public Transform satisNoktasi;

    // Durumlar
    private NavMeshAgent _agent;
    private Transform _hedefCop;
    private int _suankiYuk = 0;
    private float _beklemeSayaci = 0;

    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();

        if (satisNoktasi == null)
        {
            GameObject zone = GameObject.FindGameObjectWithTag("SellZone");
            if (zone != null) satisNoktasi = zone.transform;
        }
    }

    void Update()
    {
        if (_agent == null || satisNoktasi == null) return;

        // Çanta Dolu mu?
        if (_suankiYuk >= maxKapasite)
        {
            SatisaGit();
        }
        else
        {
            CopTopla();
        }
    }

    void CopTopla()
    {
        // --- DÜZELTME 1: HEDEF KONTROLÜ ---
        // Eğer hedeflediğim küp yok olduysa VEYA birinin sırtına girdiyse (Parent'ı olduysa)
        if (_hedefCop != null)
        {
            if (_hedefCop.parent != null) 
            {
                _hedefCop = null; // Hedefi unut
                return;           // Yeni hedef bulmaya git
            }
        }
        // ----------------------------------

        if (_hedefCop == null)
        {
            EnYakinCopuBul();
            return;
        }

        _agent.SetDestination(_hedefCop.position);

        // Hedef null değilse mesafeye bak
        if (_hedefCop != null)
        {
            float mesafe = Vector3.Distance(transform.position, _hedefCop.position);
            if (mesafe <= toplamaMesafesi)
            {
                _beklemeSayaci -= Time.deltaTime;
                if (_beklemeSayaci <= 0)
                {
                    // --- Çöpü Alma Anı ---
                    Destroy(_hedefCop.gameObject); 
                    _suankiYuk++;                  
                    KupGorseliEkle();              
                    
                    _beklemeSayaci = beklemeSuresi;
                }
            }
        }
    }

    void SatisaGit()
    {
        _agent.SetDestination(satisNoktasi.position);

        float mesafe = Vector3.Distance(transform.position, satisNoktasi.position);
        if (mesafe <= toplamaMesafesi)
        {
            _beklemeSayaci -= Time.deltaTime;
            if (_beklemeSayaci <= 0)
            {
                // --- Satış Anı ---
                int kazanilanPara = _suankiYuk * 15;
                GameManager.instance.ParaEkle(kazanilanPara);

                _suankiYuk = 0;             
                GorselleriTemizle();        
                _beklemeSayaci = beklemeSuresi;
            }
        }
    }

    // --- GÖRSEL İŞLEMLER ---
    void KupGorseliEkle()
    {
        if (collectedCubePrefab == null || stackPoint == null) return;

        GameObject yeniKup = Instantiate(collectedCubePrefab, stackPoint.position, Quaternion.identity, stackPoint);
        yeniKup.transform.localPosition = new Vector3(0, visualStack.Count * stackOffset, 0);
        visualStack.Add(yeniKup);
    }

    void GorselleriTemizle()
    {
        foreach (GameObject kup in visualStack)
        {
            Destroy(kup);
        }
        visualStack.Clear();
    }

    // --- DÜZELTME 2: PATRONA SAYGI KURALI ---
    void EnYakinCopuBul()
    {
        Collectable[] tumCopler = FindObjectsOfType<Collectable>();
        float enYakinMesafe = Mathf.Infinity;
        Transform enYakin = null;

        // Oyuncuyu bul (GameManager üzerinden)
        Transform playerTr = null;
        if(GameManager.instance != null && GameManager.instance.playerScript != null)
        {
            playerTr = GameManager.instance.playerScript.transform;
        }

        foreach (Collectable cop in tumCopler)
        {
            // Küp var mı? && Başkası çekiyor mu? && Yerde mi (Parent yok mu)?
            if (cop != null && !cop.vakumlaniyorMu && cop.transform.parent == null)
            {
                // EKSTRA KURAL: Eğer bu küp oyuncuya çok yakınsa (2.5 metre), BOT DOKUNMASIN.
                if (playerTr != null)
                {
                    float oyuncuyaMesafe = Vector3.Distance(cop.transform.position, playerTr.position);
                    if (oyuncuyaMesafe < 2.5f) continue; // Pas geç, bu patronun hakkı
                }

                float mesafe = Vector3.Distance(transform.position, cop.transform.position);
                if (mesafe < enYakinMesafe)
                {
                    enYakinMesafe = mesafe;
                    enYakin = cop.transform;
                }
            }
        }
        _hedefCop = enYakin;
    }
}
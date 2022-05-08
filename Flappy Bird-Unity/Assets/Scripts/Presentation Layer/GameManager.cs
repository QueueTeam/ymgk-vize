using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PCGSharp;
public class GameManager : MonoBehaviour
{
    [SerializeField]
    GameObject column;

    [SerializeField]
    GameObject pauseMenu;

    bool game_started = false;

    static Pcg p = new Pcg();

    void Start()
    {
        Time.timeScale = 0;
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && game_started==false)
        {
            Time.timeScale = 1;
            pauseMenu.SetActive(false);
            StartCoroutine(CreateColumn());
            game_started = true;
        }
    }


    IEnumerator CreateColumn()
    {
        // using PCGSharp class
        float random_pcg = p.NextFloat(-1.04f, 2.2f);
        yield return new WaitForSeconds(1.5f);
        GameObject new_column = Instantiate(column);
        //new_column.transform.position = new Vector3(2,Random.Range(-1.04f, 2.2f), 0);
        new_column.transform.position = new Vector3(2,random_pcg, 0);
        StartCoroutine(CreateColumn());
        
    }

}

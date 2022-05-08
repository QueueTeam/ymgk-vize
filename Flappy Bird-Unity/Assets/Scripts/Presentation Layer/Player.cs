using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.IO;
using Assets.Scripts.DAL;
using Assets.Scripts.Data_Access_Layer;
using System;

public class Player : MonoBehaviour
{
    [SerializeField]
    float jump;

    [SerializeField]
    TextMeshProUGUI scoreText;

    [SerializeField]
    GameObject gameOver;

    [SerializeField]
    AudioClip lose_clip; 

    [SerializeField]
    AudioClip jump_clip;

    Rigidbody2D rb;
    Animator anim;
    public int counter = 0;


    // Best Score
    [SerializeField]
    TextMeshProUGUI bestScore ;

    byte [] StringToByteArr (string data)
    {

        string[] sArr = data.Split(',');
        byte[] bArr = new byte[sArr.Length];

        for (int i = 0; i < sArr.Length; i++)
        {
            bArr[i] = byte.Parse(sArr[i]);
        }

        return bArr;
    }

    void DecryptInfos()
    {

        try
        {
            string [] data = Acess_File_Data.readFromFile(Application.persistentDataPath + "/queue.txt").Split('\n');
            string pas = data[1].Trim();
            byte[] iv = StringToByteArr(data[2].Trim());

            string dec = AES_Encryption.Decrypt_Click(data[0].Trim(), pas, iv);
            bestScore.text = dec;
        }

        catch (System.Exception e) 
        {
            Debug.LogError(e.Message);
            return;
        }
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        //Debug.Log(readFromFile(Application.persistentDataPath + "/queue.txt"));


        //// Decryption part
        DecryptInfos();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.velocity = new Vector2(0,jump);
            // playing jump clip
            GetComponent<AudioSource>().PlayOneShot(jump_clip);
        }
    }



    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "GameOver")
        {
            // stop the game
            Time.timeScale = 0;
            // loading gameover panel
            gameOver.SetActive(true);
            // playing lose clip
            GetComponent<AudioSource>().PlayOneShot(lose_clip);


            // 
            if (int.Parse(scoreText.text)>int.Parse(bestScore.text))
            {
                bestScore.text = scoreText.text;

                byte[] iv = new byte[16];
                for (int i = 0; i < iv.Length; i++)
                {
                    iv[i] = (byte)(UnityEngine.Random.Range(0, 255));
                    //AES_Encryption.pas.Append((char)('a' + Random.Range(0, 26)));
                    AES_Encryption.pas.Append(DateTime.Now.ToString());
                    // *** We can replace the pas value with DateTime value ***

                }

                string enc = AES_Encryption.Encrypt_Click(scoreText.text, AES_Encryption.pas.ToString(), iv);
                //ENC + IV + PASS

                File.Delete(Application.persistentDataPath + "/queue.txt");

                Acess_File_Data.writeOnFile(Application.persistentDataPath + "/queue.txt" , enc);
                Acess_File_Data.writeOnFile(Application.persistentDataPath + "/queue.txt", AES_Encryption.pas.ToString());
                Acess_File_Data.writeOnFile(Application.persistentDataPath + "/queue.txt", string.Join(",", iv));
                //Debug.Log(Application.persistentDataPath);

            }
        }
    }


    


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("passing"))
        {
            counter++;
            scoreText.text = counter.ToString();
            Destroy(collision.gameObject,2);
        }
    }


    // change the scene 
    public void ReloadGame()
    {
        SceneManager.LoadScene("SampleScene");
    }

}
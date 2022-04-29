using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.IO;
using Assets.Scripts.DAL;
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


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        //// Decryption part
        //try
        //{
        //    if ()
        //    {

        //    }
        //}
        //catch (System.Exception)
        //{

        //    throw;
        //}
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
            // plaing lose clip
            GetComponent<AudioSource>().PlayOneShot(lose_clip);


            // 
            if (int.Parse(scoreText.text)>int.Parse(bestScore.text))
            {
                bestScore.text = scoreText.text;

                byte[] iv = new byte[16];
                for (int i = 0; i < iv.Length; i++)
                {
                    iv[i] = (byte)(Random.Range(0, 255));
                    AES_Encryption.pas.Append((char)('a' + Random.Range(0, 26)));
                }
                Debug.Log(AES_Encryption.pas);

                string enc = AES_Encryption.Encrypt_Click(scoreText.text, AES_Encryption.pas.ToString(), iv);
                //ENC + IV + PASS
                writeOnFile(@"C:\Users\Yusuf\Documents\My Games\queue.txt", enc);
                writeOnFile(@"C:\Users\Yusuf\Documents\My Games\queue.txt", AES_Encryption.pas.ToString());
                writeOnFile(@"C:\Users\Yusuf\Documents\My Games\queue.txt", string.Join(",", iv));

            }


            //writeOnFile(@"C:\Users\Yusuf\Documents\My Games\queue.txt", bestScore.text);
            //readFromFile(@"C:\Users\Yusuf\Documents\My Games\queue.txt");
            //File.Delete(@"C:\Users\Yusuf\Documents\My Games\queue.txt");
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


    static void writeOnFile(string path,string data)
    {
        StreamWriter writing = new StreamWriter(path,true);
        writing.WriteLine(data);
        writing.Close();
    }

    static string readFromFile(string path)
    {
        StreamReader reading = new StreamReader(path);
        string fileData = reading.ReadLine();
        return fileData;

    }
}

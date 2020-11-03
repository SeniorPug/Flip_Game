using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class GameScript : MonoBehaviour
{
    public static GameObject cur_ID;
    public static int[] cardsArray = new int[6] { 0, 0, 0, 0, 0, 0 };
    public static int theme_id;
    public static int flipped_cards = 0;
    public static bool fresh_game = true;
    public class imgArray
    {
        public List<string> array { get; set; }
    }

    public static imgArray img;

    void CardArrayGenerator()
    {
        fresh_game = true;
        theme_id = Random.Range(0, img.array.Count);
        int img_num = 0;
        switch (theme_id)
        {
            case 0:
                img_num = 19;
                break;
            case 1:
                img_num = 7;
                break;
            default:
                break;
        }
        for (int i = 0; i < cardsArray.Length; i++)
        {
            if (cardsArray[i] == 0)
            {
                int rand_num = Random.Range(i, img_num);
                if (!cardsArray.Contains(rand_num))
                {
                    cardsArray[i] = rand_num;
                    bool check = false;
                    do
                    {
                        rand_num = Random.Range(i + 1, 6);
                        if (cardsArray[rand_num] == 0)
                        {
                            cardsArray[rand_num] = cardsArray[i];
                            check = true;
                        }
                    } while (!check);
                } else
                {
                    i--;
                }
            }
        }
    }

    void Start()
    {
        using (StreamReader reader = new StreamReader("Assets/Images.json"))
        {
            string json = reader.ReadToEnd();
            img = JsonConvert.DeserializeObject<imgArray>(json);
        }
        CardArrayGenerator();
    }

    void Update()
    {
        if (cardsArray[0] == 0 && cardsArray[1] == 0 && cardsArray[2] == 0 && cardsArray[3] == 0 && cardsArray[4] == 0 && cardsArray[5] == 0)
        {
            CardArrayGenerator();
        }
    }
}

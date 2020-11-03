using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CardScript : MonoBehaviour
{
    public int cardID = 0;
    public int imgID = 0;
    bool flipped = false;
    public GameObject back;
    public RawImage rImage;
    Animator anim;
    string url = "";

    public void FlipTrigger()
    {
        if (flipped)
        {
            back.SetActive(true);
            flipped = false;
            GetComponent<Button>().interactable = true;
        }
        else
        {
            back.SetActive(false);
            flipped = true;
            GameScript.flipped_cards++;
            StartCoroutine(CardCheck());
        }
    }

    public void Flip()
    {
        GetComponent<Button>().interactable = false;
        if (!flipped && GameScript.flipped_cards < 2)
        {
            anim.Play("Flip");
        }
    }

    void Start()
    {
        GetComponent<Button>().interactable = false;
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (GameScript.fresh_game)
        {
            StartCoroutine(IdleTrigger());
        }
    }

    IEnumerator CardCheck()
    {
        yield return new WaitForSecondsRealtime(1f);
        if (GameScript.cur_ID == null)
        {
            GameScript.cur_ID = gameObject;
        }
        else if (GameScript.cur_ID.GetComponent<CardScript>().imgID == imgID)
        {
            GameScript.cur_ID.GetComponent<Animator>().Play("Destroy");
            GameScript.cardsArray[GameScript.cur_ID.GetComponent<CardScript>().cardID - 1] = 0;
            GameScript.cardsArray[cardID - 1] = 0;
            GameScript.flipped_cards = 0;
            GameScript.cur_ID = null;
            anim.Play("Destroy");
        }
        else
        {
            GameScript.cur_ID.GetComponent<Animator>().Play("Flip");
            anim.Play("Flip");
            GameScript.cur_ID = null;
            GameScript.flipped_cards = 0;
        }
    }
    IEnumerator IdleTrigger()
    {
        imgID = GameScript.cardsArray[cardID - 1];
        url = GameScript.img.array[GameScript.theme_id] + (imgID + 1).ToString() + ".png";
        StartCoroutine(LoadImage());
        yield return new WaitForSecondsRealtime(1f);
        back.SetActive(true);
        flipped = false;
        yield return new WaitForEndOfFrame();
        anim.Play("Idle");
        GameScript.fresh_game = false;
        GetComponent<Button>().interactable = true;
    }

    IEnumerator LoadImage()
    {
        rImage.texture = null;
        WWW wwwLoader = new WWW(url);
        yield return wwwLoader;
        rImage.texture = wwwLoader.texture;
    }
}

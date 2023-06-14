using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_UI : MonoBehaviour
{
    public GameObject BulletNumUI;
    public GameObject StaminaBar;
    public GameObject ReloadTimerUI;
    public GameObject ItemNumUI;

    Player player;

    private void OnEnable()
    {
        Player.changeBullet += UpdateBullet;
        Player.changeStamina += updateStamina;
        Player.changeReloadTime += updateReloadTimer;
    }

    private void OnDisable()
    {
        Player.changeBullet -= UpdateBullet;
        Player.changeStamina -= updateStamina;
        Player.changeReloadTime -= updateReloadTimer;

    }

    // Start is called before the first frame update
    void Start()
    {
        player = transform.GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void UpdateBullet(int bullet)
    {
        if (!player.useReload) return;
        BulletNumUI.transform.Find("bulletText").gameObject.GetComponent<Text>().text = player.bulletNum.ToString();
    }

    void updateStamina(float stamina)
    {
        StaminaBar.GetComponent<Slider>().value = stamina;
    }
    void updateReloadTimer(float timer)
    {
        ReloadTimerUI.GetComponent<Slider>().value = timer;
    }


}

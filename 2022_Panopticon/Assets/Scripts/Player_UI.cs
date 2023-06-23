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
    public Image BombGauge;
    public GameObject PowerTimeUI;

    Player player;

    private void OnEnable()
    {
        Player.changeBullet += UpdateBullet;
        Player.changeStamina += updateStamina;
        Player.changeReloadTime += updateReloadTimer;
        Player.changeItemNum += updateBombNum;
        Player.changeBombGauge += updateBombGauge;
        Player.changePowerUpTime += updatePowerTimer;
    }

    private void OnDisable()
    {
        Player.changeBullet -= UpdateBullet;
        Player.changeStamina -= updateStamina;
        Player.changeReloadTime -= updateReloadTimer;
        Player.changeItemNum -= updateBombNum;
        Player.changeBombGauge -= updateBombGauge;
        Player.changePowerUpTime -= updatePowerTimer;
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

    void updateBombNum(int bomb)
    {
        ItemNumUI.transform.GetChild(0).GetComponent<Text>().text = bomb.ToString();
    }

    void updateBombGauge(float fill)
    {
        BombGauge.fillAmount = fill;
    } 

    void updatePowerTimer(float timer)
    {
        PowerTimeUI.GetComponent<Slider>().value = timer;

    }
}

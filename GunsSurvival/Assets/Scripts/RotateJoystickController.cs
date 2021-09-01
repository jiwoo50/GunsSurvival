using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RotateJoystickController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public static Vector2 leverPos;
    public static bool isTouch = false;
    public static bool canFire = false;

    public PlayerController player;
    public RectTransform joystick_BG;
    public RectTransform joystick_lever;

    float radius;

    void Start()
    {
        radius = joystick_BG.rect.width * 0.5f;
    }

    void Update()
    {
        if (isTouch) ControlPlayer();
    }

    public void OnDrag(PointerEventData eventData)
    {
        leverPos = eventData.position - (Vector2)joystick_BG.position;
        leverPos = Vector3.ClampMagnitude(leverPos, radius);
        joystick_lever.localPosition = leverPos;

        leverPos = leverPos.normalized;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isTouch = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isTouch = false;
        canFire = false;
        if (WeaponController.machineGun) StartCoroutine(MachineGaugeDecrease());
        if (WeaponController.shotGun) StartCoroutine(ShotgunGaugeDecrease());
        if (WeaponController.bazooka) StartCoroutine(BazookaGaugeDecrease());
        joystick_lever.localPosition = Vector3.zero;
    }

    public void ControlPlayer()
    {
        if (!PauseMenu.gamePaused && GameController.Instance.canShoot)
        {
            canFire = true;
            player.PlayerRotate(leverPos); //player rotates
        }
    }

    IEnumerator MachineGaugeDecrease()
    {
        MachinegunBullet.machineGaugeDecrease = false;
        yield return new WaitForSeconds(2.0f);
        MachinegunBullet.machineGaugeDecrease = true;
    }

    IEnumerator ShotgunGaugeDecrease()
    {
        ShotgunBullet.ShotgunGaugeDecrease = false;
        yield return new WaitForSeconds(2.0f);
        ShotgunBullet.ShotgunGaugeDecrease = true;
    }
    IEnumerator BazookaGaugeDecrease()
    {
        BazookaBomb.bazookaGaugeDecrease = false;
        yield return new WaitForSeconds(2.0f);
        BazookaBomb.bazookaGaugeDecrease = true;
    }
}

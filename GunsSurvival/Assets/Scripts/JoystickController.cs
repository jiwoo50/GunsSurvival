using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class JoystickController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public static Vector2 leverPos;
    public static bool isTouch = false;
    public static bool fireBullets = false;

    public PlayerController player;
    public RectTransform joystick_BG;
    public RectTransform joystick_lever;
    
    float radius;

    void Start()
    {
        radius = joystick_BG.rect.width * 0.5f;
    }

    public void OnDrag(PointerEventData eventData)
    { 
        leverPos = eventData.position - (Vector2)joystick_BG.position;
        leverPos = Vector3.ClampMagnitude(leverPos, radius);
        joystick_lever.localPosition = leverPos;

        leverPos = leverPos.normalized;
        ControlJoystick();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isTouch = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isTouch = false;
        fireBullets = false;
        joystick_lever.localPosition = Vector3.zero;
    }

    void ControlJoystick()
    {
        if (this.gameObject.CompareTag("moveJoystick") && !PauseMenu.gamePaused && GameController.Instance.canShoot) //player moves
        {
            player.PlayerMove(leverPos);
            fireBullets = false;
        }
        if (this.gameObject.CompareTag("rotateJoystick") && !PauseMenu.gamePaused && GameController.Instance.canShoot) //player rotates
        {
            player.PlayerRotate(leverPos);
            fireBullets = true;
        }
    }
}
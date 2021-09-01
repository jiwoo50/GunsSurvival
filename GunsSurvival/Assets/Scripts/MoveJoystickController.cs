using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MoveJoystickController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public static Vector2 leverPos;
    public static bool isTouch = false;
    public static bool canFire = false;

    public PlayerController player;
    public RectTransform joystick_BG;
    public RectTransform joystick_lever;
    
    float radius;
    float dist;

    void Start()
    {
        radius = joystick_BG.rect.width * 0.5f;
    }

    void FixedUpdate()
    {
        if(isTouch) ControlPlayer();
    }

    public void OnDrag(PointerEventData eventData)
    {
        leverPos = eventData.position - (Vector2)joystick_BG.position;
        leverPos = Vector3.ClampMagnitude(leverPos, radius);
        joystick_lever.localPosition = leverPos;
        dist = Vector3.Distance(joystick_BG.position, joystick_lever.position) / radius;        

        leverPos = leverPos.normalized;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isTouch = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isTouch = false;
        joystick_lever.localPosition = Vector3.zero;
    }

    public void ControlPlayer()
    {
        if (!PauseMenu.gamePaused && GameController.Instance.canShoot) player.PlayerMove(leverPos, dist); //player moves
        
    }
}
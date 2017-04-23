using UnityEngine;
using System.Collections;
using XInputDotNetPure;

enum CurrentRoom
{
    Middle,
    Engine,
    Freeze,
    Shield,
    Turret
}

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private PlayerIndex controller;
    public PlayerIndex Controller
    {
        get { return controller; }
        set { controller = value; }
    }

    [SerializeField]
    private CurrentRoom room;
    
    public Transform[] Rooms;

    [SerializeField]
    private EngineController engine;

    [SerializeField]
    private ShieldController shield;

    [SerializeField]
    private GunController gun;

    [SerializeField]
    private FreezeController freeze;

	// Use this for initialization
	void Start()
    {
        transform.position = Rooms[0].position;
        room = CurrentRoom.Middle;
	}
	
	// Update is called once per frame
	void FixedUpdate()
    {
        MoveRooms();
	}

    void MoveRooms()
    {
        GamePadState currentState = GamePad.GetState(Controller);
        if (currentState.Buttons.Y == ButtonState.Pressed)
        {
            
            if (Rooms[1].gameObject.GetComponent<RoomManager>().Empty)
            {
                LeaveRoom();
                Rooms[1].gameObject.GetComponent<RoomManager>().Empty = false;
                transform.position = Rooms[1].position;
                room = CurrentRoom.Engine;
                engine.Joystick = controller;
            }
        }
        else if (currentState.Buttons.B == ButtonState.Pressed)
        {
            if (Rooms[2].gameObject.GetComponent<RoomManager>().Empty)
            {
                LeaveRoom();
                Rooms[2].gameObject.GetComponent<RoomManager>().Empty = false;
                transform.position = Rooms[2].position;
                room = CurrentRoom.Shield;
                shield.Joystick = controller;
            }
        }
        else if (currentState.Buttons.X == ButtonState.Pressed)
        {
            if (Rooms[3].gameObject.GetComponent<RoomManager>().Empty)
            {
                LeaveRoom();
                Rooms[3].gameObject.GetComponent<RoomManager>().Empty = false;
                transform.position = Rooms[3].position;
                room = CurrentRoom.Turret;
                gun.Joystick = controller;
            }
        }
        else if(currentState.Buttons.A == ButtonState.Pressed)
        {
            if (Rooms[4].GetComponent<RoomManager>().Empty)
            {
                LeaveRoom();
                Rooms[4].gameObject.GetComponent<RoomManager>().Empty = false;
                transform.position = Rooms[4].position;
                room = CurrentRoom.Freeze;
                freeze.Joystick = controller;
            }
        }

    }

    void LeaveRoom()
    {
        switch(room)
        {
            case CurrentRoom.Engine:
                Rooms[1].gameObject.GetComponent<RoomManager>().Empty = true;
                engine.Joystick = PlayerIndex.Four;
                engine.DisableFlame();
                break;
            case CurrentRoom.Freeze:
                Rooms[4].gameObject.GetComponent<RoomManager>().Empty = true;
                freeze.Joystick = PlayerIndex.Four;
                break;
            case CurrentRoom.Turret:
                Rooms[3].gameObject.GetComponent<RoomManager>().Empty = true;
                gun.Joystick = PlayerIndex.Four;
                break;
            case CurrentRoom.Shield:
                Rooms[2].gameObject.GetComponent<RoomManager>().Empty = true;
                shield.Joystick = PlayerIndex.Four;
                break;
            default:
                break;
        }
    }
}

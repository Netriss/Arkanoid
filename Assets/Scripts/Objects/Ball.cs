using UnityEngine;
using System;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PositionResetter))]
public class Ball : MonoBehaviour
{
    ////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////
    
    #region exposed

    public Paddle Paddle;
    public Collider2D ExitZone;
    
    #endregion
    
    ////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////
    
    #region public methods

    public void Init(IGameStartedProvider gameStartedProvider, Action<bool> onBounce, Action onExit)
    {
        _gameStartedProvider = gameStartedProvider;
        _onBounce = onBounce;
        _onExit = onExit;
    }

    public void Reinit()
    {
        _positionInitializer.ResetPosition();
        rigidbody2D.velocity = Vector2.zero;
    }

    public void Launch()
    {
        Launch(_initialPaddlePart);
    }
    
    public void SetSpeed(float speed)
    {
        _speed = speed;
    }
    
    #endregion
    
    ////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////
    
    #region events
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        bool touchingPaddle = (collision.gameObject == Paddle.gameObject);
        if(touchingPaddle)
        {
            float paddlePart = 2 * (collision.contacts[0].point.x - Paddle.transform.position.x) / Paddle.Width;

            if(_gameStartedProvider.GameStarted)
            {
                Launch(paddlePart);
            }
            else if(Utils.IsEqual0(_initialPaddlePart))
            {
                _initialPaddlePart = paddlePart;
            }
        }
        
        if(_gameStartedProvider.GameStarted)
        {
            Brick brick = collision.gameObject.GetComponent<Brick>();
            if(brick == null)
            {
                // we need reduce score and play default sound only when ball collides walls or paddle
                Utils.InvokeAction(_onBounce, touchingPaddle);
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // check and fix velocity
        if(_gameStartedProvider.GameStarted)
        {
            Vector2 maxAngleTemplate = Utils.DegreesToVector2(Utils.Settings.MaxAngle);

            Vector2 velocity = rigidbody2D.velocity.normalized;
            if(Mathf.Abs(velocity.y) < maxAngleTemplate.y)
            {
                velocity.x = maxAngleTemplate.x * Mathf.Sign(velocity.x);
                velocity.y = maxAngleTemplate.y * Mathf.Sign(velocity.y);
                rigidbody2D.velocity = _speed * velocity;
            }

            Brick brick = collision.gameObject.GetComponent<Brick>();
            if(brick != null)
            {
                brick.Destroy();
            }
        }
    }
    
    private void OnTriggerExit2D(Collider2D col)
    {
        if(col == ExitZone)
        {
            Utils.InvokeAction(_onExit);
        }
    }
    
    #endregion
    
    ////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////
    
    #region MonoBehaviour

    private void Awake()
    {
        _positionInitializer = GetComponent<PositionResetter>();
    }

    private void Start()
    {
        _initialPaddlePart = 0.0f;
        _initialShift = transform.position.x - Paddle.gameObject.transform.position.x;
    }

    private void Update()
    {
        if(!_gameStartedProvider.GameStarted)
        {
            Vector3 pos = transform.position;
            pos.x = Paddle.gameObject.transform.position.x + _initialShift;
            transform.position = pos;
        }
    }
    
    #endregion
    
    ////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////
    
    #region private methods
    
    private void Launch(float paddlePart)
    {
        rigidbody2D.velocity = _speed * Utils.DegreesToVector2(paddlePart * Utils.Settings.MaxAngle);
    }
    
    #endregion
    
    ////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////
    
    #region private members

    private IGameStartedProvider _gameStartedProvider;
    private Action<bool> _onBounce;
    private Action _onExit;
    private float _speed;
    private PositionResetter _positionInitializer;
    private float _initialPaddlePart;
    private float _initialShift;
    
    #endregion
    
    ////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////
}
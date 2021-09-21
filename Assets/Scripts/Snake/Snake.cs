using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(TaileGenerator))]
[RequireComponent(typeof(SnakeInput))]

public class Snake : MonoBehaviour
{
    [SerializeField] private SnakeHead _head;
    [SerializeField] private int _tailSize;
    [SerializeField] private float _speed;
    [SerializeField] private float _tailSpringiness;

    private SnakeInput _input;
    private List<Segment> _tail;
    private TaileGenerator _taileGenerator;

    public event UnityAction<int> SizeUpdated;
    public event UnityAction SnakeSegmentsIsOver;

    private void Start()
    {
        _input = GetComponent<SnakeInput>();
        _taileGenerator = GetComponent<TaileGenerator>();
        _tail = _taileGenerator.Generate(_tailSize);
        SizeUpdated?.Invoke(_tail.Count);
    }

    private void OnEnable()
    {
        _head.BlockCollided += OnBlockCollided;
        _head.BonusCollected += OnBonusCollected;
    }

    private void OnDisable()
    {
        _head.BlockCollided -= OnBlockCollided;
        _head.BonusCollected -= OnBonusCollected;
    }

    private void FixedUpdate()
    {
        Move(_head.transform.position + _head.transform.up * _speed * Time.fixedDeltaTime);

        _head.transform.up = _input.GetDirectionToClick(_head.transform.position);
    }

    private void Move(Vector3 nextPosition)
    {
        Vector2 previousPosition = _head.transform.position;

        foreach (var segment in _tail)
        {
            Vector3 tempPosition = segment.transform.position;
            segment.transform.position = Vector2.Lerp(segment.transform.position, previousPosition, _tailSpringiness * Time.deltaTime);
            previousPosition = tempPosition;
        }

        _head.Move(nextPosition);
    }

    private void OnBlockCollided()
    {
        if (_tail.Count == 0)
        {
            _speed = 0;
            SnakeSegmentsIsOver?.Invoke();
            return;
        }

        Segment deletedSegment = _tail[_tail.Count - 1];
        _tail.Remove(deletedSegment);
        Destroy(deletedSegment.gameObject);
        SizeUpdated?.Invoke(_tail.Count);
    }

    private void OnBonusCollected(int bonusSize)
    {
        _tail.AddRange(_taileGenerator.Generate(bonusSize));
        SizeUpdated?.Invoke(_tail.Count);
    }
}

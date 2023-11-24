using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Facade

    public ItemGameManager GetItemGameManager => _itemGameManager;
    [SerializeField] private ItemGameManager _itemGameManager;

    #endregion
    
    #region Singleton
    static public GameManager Instance;

    private void Awake()
    {
        if (Instance != null) Destroy(this);

        Instance = this;
    }
    #endregion

    #region EVENT_QUEUE
    private Queue<ICommand> _events = new Queue<ICommand>();
    private List<IMemento> _reversibleEvents = new List<IMemento>();

    public void AddEvent(ICommand command) => _events.Enqueue(command);

    private void Update()
    {
        while (_events.Count > 0)
        {
            var command = _events.Dequeue();
            command.Do();

            if (command is IMemento mementoCmd)
            {
                if (mementoCmd.CanUndo)
                {
                    _reversibleEvents.Add(mementoCmd);
                }
            }
        }

        for (int i = _reversibleEvents.Count - 1; i >= 0; i--)
        {
            _reversibleEvents[i].TimeToUndo -= Time.deltaTime;

            if (_reversibleEvents[i].TimeToUndo <= 0)
            {
                _reversibleEvents[i].Undo();
                _reversibleEvents.Remove(_reversibleEvents[i]);
            }
        }
    }
    #endregion

    public void Resume()
    {
        Time.timeScale = 1;
    }

    public void Pause()
    {
        Time.timeScale = 0;
    }

    public void StartVictory()
    {
        // What happens when victory
        // animator.SetTrigger("Victory");
    }
}

[System.Serializable]
public class ItemGameManager
{
    [SerializeField] private BaseItem _bulletItem;
    [SerializeField] private BaseItem _healItem;

    public BaseItem SpawnItem()
    {
        BaseItem item=GameObject.Instantiate(Random.Range(0, 2) == 1 ? _bulletItem : _healItem);
        item.Amount = Random.Range(1, item.MaxStackeable);
        return item;
    }
}
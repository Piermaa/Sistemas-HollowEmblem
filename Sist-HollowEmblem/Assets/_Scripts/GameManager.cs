using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
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

    public void AddEvent(ICommand command) => _events.Enqueue(command);

    private void Update()
    {
        while (_events.Count > 0)
        {
            var command = _events.Dequeue();
            command.Do();
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

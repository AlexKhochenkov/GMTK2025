using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;

public class SpawnedObject : MonoBehaviour
{

    [SerializeField] private bool _hideAsDefault;
    [SerializeField] private List<State> _actions;
    [SerializeField] private GameObject _spawnObject;

    [Serializable]
    class State
    {

        public enum Action
        {
            Show,
            Hide,
            SpecialAction
        }
        public int iters;

        public Action action;

    }

    private int _current_iter = 0;

    int index_action = 0;

    void Start()
    {
        _spawnObject.SetActive(!_hideAsDefault);
    }

    public virtual void SpecialAction()
    {

    }

    public void AddIter()
    {
        if (index_action >= _actions.Capacity)
        {
            return;
        }

        _current_iter += 1;
        if (_current_iter >= _actions[index_action].iters)
            {
                switch (_actions[index_action].action)
                {
                    case State.Action.Show:
                        _spawnObject.SetActive(true);
                        break;
                    case State.Action.Hide:
                        _spawnObject.SetActive(false);
                        break;
                    case State.Action.SpecialAction:
                        SpecialAction();
                        break;
                }
                index_action += 1;
                _current_iter = 0;

            }
    }
}

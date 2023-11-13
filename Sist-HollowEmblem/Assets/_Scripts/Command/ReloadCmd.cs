using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReloadCmd : ICommand
{
    private PlayerShoot _playerShoot;
    public ReloadCmd(PlayerShoot playerShoot)
    {
        _playerShoot = playerShoot;
    }

    public void Do()
    {
       _playerShoot.Reload();
    }
}

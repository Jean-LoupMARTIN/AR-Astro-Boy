using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRef : MonoBehaviour
{
    public Player player;

    public void StartJumping() { player.StartJumping(); }
    public void EndJumping() { player.EndJumping(); }
    public void StartWaiting() { player.StartWaiting(); }
}

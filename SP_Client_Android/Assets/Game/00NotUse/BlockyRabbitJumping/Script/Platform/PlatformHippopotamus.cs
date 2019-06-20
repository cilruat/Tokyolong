using UnityEngine;
using System.Collections;

namespace BlockJumpRabbit
{
public class PlatformHippopotamus:Platform {

    public float timeToKillPlayer = 0.45f;
    private float timer = 0.0f;

    void OnTriggerStay2D(Collider2D other)
    {

        if (other.tag == "Player")
        {
            timer += Time.deltaTime;

            // Call enemy wake up. Sinal detect to player that enemy can attack!
            animPlatform.SetTrigger("wake_up");

            if (timer > timeToKillPlayer)
            {
                if (animPlatform)
                    // Call enemy kill player
                    animPlatform.SetTrigger("kill_player");

                int rand = Random.Range(3, 8);
                StartCoroutine(StartJumpRandomPostion(rand, other.GetComponent<PlayerController>()));

                timer = 0.0f;
            }
        }
    }

    // Jump to random postion when hippopotamus active
    IEnumerator StartJumpRandomPostion(int num,PlayerController player)
    {
        player.tag = "Untagged";
        int count = 0;

        // Wait player hit platform
        while (player.IsJump)
            yield return null;


        while (count < num)
        {
            if(count == num - 1)
                player.tag = "Player";
            player.Jump(PlayerController.TypeJump.NormalJump);

            while (player.IsJump)
                yield return null;

            count++;
        }
    }

    public override void Deactive()
    {
        timer = 0.0f;
        base.Deactive();
    }
}
}
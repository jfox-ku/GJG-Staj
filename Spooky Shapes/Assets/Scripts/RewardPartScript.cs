using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardPartScript : MonoBehaviour
{
    private RunPartScript runPart;
    public List<RewardFrameScript> frames;
    public PlayerScript player;

    // Start is called before the first frame update
    void Awake()
    {
        runPart = GetComponent<RunPartScript>();

    }

    public void SetUpItems() {
        foreach (RewardFrameScript frame in frames) {
            var item = runPart.set.getRandomItem();
            // Debug.Log("Setting item "+item.name+" at frame "+frame.name);
            item.SetUpItem();
            frame.SetItem(item);

        }
    }

    public void GiveItemPlayer(ItemScript it) {
        if (player == null) {
            var Player = GameObject.FindGameObjectWithTag("Player");
            player = Player.GetComponent<PlayerScript>();
        }
        

        player.addToInventory(it);
        foreach(RewardFrameScript frame in frames) {
            
            frame.gameObject.SetActive(false);
        }

    }

}

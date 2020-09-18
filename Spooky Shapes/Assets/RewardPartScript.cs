using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardPartScript : MonoBehaviour
{
    private RunPartScript runPart;
    public List<RewardFrameScript> frames;

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
        var Player = GameObject.FindGameObjectWithTag("Player");
        var PScript = Player.GetComponent<PlayerScript>();

        PScript.addToInventory(it);
        foreach(RewardFrameScript frame in frames) {
            frame.gameObject.SetActive(false);
        }

    }

}

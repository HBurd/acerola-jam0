using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteAnimator : MonoBehaviour
{
    [SerializeField]
    int num_frames;

    [SerializeField]
    double frame_time;

    new Renderer renderer;

    int last_frame_index = 0;

    [SerializeField]
    int[] anims;

    int selected_anim;

    void Start()
    {
        selected_anim = 1;
        renderer = GetComponent<Renderer>();
        renderer.material.SetFloat("_NumFrames", num_frames);
        renderer.material.SetFloat("_FrameIndex", 0);
    }

    int GetFramesInAnim()
    {
        int next_start = anims.Length == selected_anim + 1 ? anims.Length + 1 : anims[selected_anim + 1];
        return next_start - anims[selected_anim];
    }

    void Update()
    {
        int frame_offset = (int)((Time.timeAsDouble / frame_time) % GetFramesInAnim());
        int frame_index = frame_offset + anims[selected_anim];
        if (frame_index != last_frame_index)
        {
            last_frame_index = frame_index;
            renderer.material.SetFloat("_FrameIndex", (float)frame_index);
        }
    }

    public void SetAnim(int anim)
    {
        selected_anim = anim;
    }
}

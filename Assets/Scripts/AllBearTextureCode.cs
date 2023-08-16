using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllBearTextureCode : MonoBehaviour
{
    public Transform[] ears_group = new Transform[4];
    public Transform[] head_group = new Transform[4];
    public Transform[] body_group = new Transform[4];
    public Transform[] body1_group = new Transform[4];
    public Transform[] tail_group = new Transform[4];

    public SkinnedTexturePaint[] ears_texturePaint_group = new SkinnedTexturePaint[4];
    public SkinnedTexturePaint[] head_texturePaint_group = new SkinnedTexturePaint[4];
    public SkinnedTexturePaint[] body_texturePaint_group = new SkinnedTexturePaint[4];
    public SkinnedTexturePaint[] body1_texturePaint_group = new SkinnedTexturePaint[4];
    public SkinnedTexturePaint[] tail_texturePaint_group = new SkinnedTexturePaint[4];

    public void all_brush_change(Texture2D brush_texture, float brush_size)
    {
/*        ears_texturePaint_group[0].brushSize = brush_size * 3.0f;
        ears_texturePaint_group[0].brushTexture = brush_texture;
        head_texturePaint_group[0].brushSize = brush_size * 0.8f;
        head_texturePaint_group[0].brushTexture = brush_texture;
        body_texturePaint_group[0].brushSize = brush_size * 0.7f;
        body_texturePaint_group[0].brushTexture = brush_texture;
        body1_texturePaint_group[0].brushSize = brush_size * 3.3f;
        body1_texturePaint_group[0].brushTexture = brush_texture;
        tail_texturePaint_group[0].brushSize = brush_size * 2.0f;
        tail_texturePaint_group[0].brushTexture = brush_texture;*/

        for (int i = 0; i < 4; i++)
        {
            ears_texturePaint_group[i].brushSize = brush_size * 3.0f;
            ears_texturePaint_group[i].brushTexture = brush_texture;
            head_texturePaint_group[i].brushSize = brush_size * 0.8f;
            head_texturePaint_group[i].brushTexture = brush_texture;
            body_texturePaint_group[i].brushSize = brush_size * 0.7f;
            body_texturePaint_group[i].brushTexture = brush_texture;
            body1_texturePaint_group[i].brushSize = brush_size * 3.3f;
            body1_texturePaint_group[i].brushTexture = brush_texture;
            tail_texturePaint_group[i].brushSize = brush_size * 2.0f;
            tail_texturePaint_group[i].brushTexture = brush_texture;
        }
    }

    public void all_brush_change_L(Texture2D brush_texture, float brush_size)
    {
        /*        ears_texturePaint_group[0].brushSize_L = brush_size * 3.0f;
                ears_texturePaint_group[0].brushTexture_L = brush_texture;
                head_texturePaint_group[0].brushSize_L = brush_size * 0.8f;
                head_texturePaint_group[0].brushTexture_L = brush_texture;
                body_texturePaint_group[0].brushSize_L = brush_size * 0.6f;
                body_texturePaint_group[0].brushTexture_L = brush_texture;
                body1_texturePaint_group[0].brushSize_L = brush_size * 3.0f;
                body1_texturePaint_group[0].brushTexture_L = brush_texture;
                tail_texturePaint_group[0].brushSize_L = brush_size * 2.0f;
                tail_texturePaint_group[0].brushTexture_L = brush_texture;*/

        for (int i = 0; i < 4; i++)
        {
            ears_texturePaint_group[i].brushSize_L = brush_size * 3.0f;
            ears_texturePaint_group[i].brushTexture_L = brush_texture;
            head_texturePaint_group[i].brushSize_L = brush_size * 0.8f;
            head_texturePaint_group[i].brushTexture_L = brush_texture;
            body_texturePaint_group[i].brushSize_L = brush_size * 0.6f;
            body_texturePaint_group[i].brushTexture_L = brush_texture;
            body1_texturePaint_group[i].brushSize_L = brush_size * 3.0f;
            body1_texturePaint_group[i].brushTexture_L = brush_texture;
            tail_texturePaint_group[i].brushSize_L = brush_size * 2.0f;
            tail_texturePaint_group[i].brushTexture_L = brush_texture;
        }
    }

}

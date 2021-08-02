using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class PixelizatorController : MonoBehaviour
{
    [Header("Shaders")]
    public Shader _PixelShader;
    private Material PixelMat;

 
    [Header("Pixelizator")]
    public bool UsePixel;
    public int Rows;
    public int Columns;

    public Material _PixelMat
    {
        get
        {
            if (!PixelMat && _PixelShader)
            {
                PixelMat = new Material(_PixelShader);
                PixelMat.hideFlags = HideFlags.HideAndDontSave;
            }

            return PixelMat;
        }
    }
     
   

    public Camera _camera
    {
        get
        {
            if (!cam)
            {
                cam = GetComponent<Camera>();
                cam.depthTextureMode = DepthTextureMode.Depth;


            }
            return cam;
        }
    }
    private Camera cam;


    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {

        if (!UsePixel)
        {
            Graphics.Blit(source, destination);
            return;
        }

        _PixelMat.SetInt("_Columns", Columns);
        _PixelMat.SetInt("_Rows", Rows);

        _PixelMat.SetTexture("_TempTex", source);

        Graphics.Blit(source, destination, _PixelMat);

    }
}


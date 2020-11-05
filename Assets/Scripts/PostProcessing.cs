using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]

public class PostProcessing : MonoBehaviour
{

	public RenderTexture resolutionTexture;
    public Material[] postProcessingMats = new Material[0];

    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {

        if (postProcessingMats.Length == 0)
        {
            Graphics.Blit(src, dest);
        }
        else if (postProcessingMats.Length == 1)
        {
            if (postProcessingMats[0] != null)
            {
				postProcessingMats[0].SetTexture("_MainTex", src);
				//post processing material blitting
				if(resolutionTexture != null){
					Graphics.Blit(src, resolutionTexture, postProcessingMats[0]);
				}
            } else {
				Graphics.Blit(src, dest);
			}
			Graphics.Blit(resolutionTexture, dest);
        }
        else
        {
            //TODO: multiple chained rendertextures            
        }

    }
}

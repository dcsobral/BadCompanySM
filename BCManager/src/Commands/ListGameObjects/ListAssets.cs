using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using UMA;
using UnityEngine;

namespace BCM.Commands
{
  public class ListAssets : BCCommandAbstract
  {
    public override void Process()
    {
      string output = "\n";

      //OVERLAYS
      if (_options.ContainsKey("overlays"))
      {
        output += "Overlays:\n";
        OverlayDataAsset[] oda_array = OverlayLibrary.Instance.GetAllOverlayAssets();
        foreach (OverlayDataAsset oda in oda_array)
        {
          output += oda.name + _sep;
        }
        output += "\n";
      }

      //SLOTS
      if (_options.ContainsKey("slots"))
      {
        output += "Slots:\n";
        SlotDataAsset[] sda_array =  SlotLibrary.Instance.GetAllSlotAssets();
        foreach (SlotDataAsset sda in sda_array)
        {
          output += sda.slotName + _sep;
        }
        output += "\n";
      }

      //PARTICLES
      if (_options.ContainsKey("particles"))
      {
        output += "Particles:\n";
        Dictionary<string, Transform> effects = LoadParticleEffects();
        foreach (string key in effects.Keys)
        {
          output += key + _sep;
        }
        output += "\n";
      }

      //TEXTURES
      if (_options.ContainsKey("textures"))
      {
        output += "Textures:\n";
        var textures = BlockTextureData.list;
        foreach (BlockTextureData texture in textures)
        {
          if (texture != null)
          {
            output += texture.ID.ToString() + ":" + texture.LocalizedName + " (" + texture.Name + " - " + texture.TextureID.ToString() + ")" + _sep;
          }
        }
        output += "\n";
      }

      //MESHES
      if (_options.ContainsKey("meshes"))
      {
        //output += MeshDescription.meshes.Length;
        foreach (MeshDescription meshDesc in MeshDescription.meshes)
        {
          if (meshDesc != null)
          {
            output += "TextureAtlasClass:" + meshDesc.TextureAtlasClass + ",";
            output += "Name:" + meshDesc.Name + ",";
            output += "meshType:" + meshDesc.meshType.ToString() + ",";
            output += "bCastShadows:" + meshDesc.bCastShadows.ToString() + ",";
            output += "BlendMode:" + meshDesc.BlendMode.ToString() + ",";
            output += "Tag:" + meshDesc.Tag.ToString() + ",";
            output += "ColliderLayerName:" + meshDesc.ColliderLayerName + ",";

            output += "SecondaryShader:" + meshDesc.SecondaryShader + ",";
            output += "ShaderName:" + meshDesc.ShaderName + ",";
            output += "ShaderNameDistant:" + meshDesc.ShaderNameDistant + ",";
                        
            if (meshDesc.MetaData != null)
            {
              output += "MetaDataName:" + meshDesc.MetaData.name + ",";
              output += "MetaDataText:" + meshDesc.MetaData.text + ",";
            }
          }
          output += _sep;
        }
      }

      //ITEM ICONS
      if (_options.ContainsKey("itemicons"))
      {
        GameObject gameObject = GameObject.Find("/NGUI Root (2D)/ItemIconAtlas");
        if (gameObject == null)
        {
          SendOutput("Atlas object not found");
          return;
        }
        DynamicUIAtlas component = gameObject.GetComponent<DynamicUIAtlas>();

        if (component == null)
        {
          SendOutput("Atlas component not found");

          return;
        }

        string prebakedAtlas = component.PrebakedAtlas;
        List<UISpriteData> list;
        int num;
        int num2;
        if (!DynamicUIAtlasTools.ReadPrebakedAtlasDescriptor(prebakedAtlas, out list, out num, out num2))
        {
          SendOutput("Could not read dynamic atlas descriptor");

          return;
        }
        Texture2D texture2D;
        if (!DynamicUIAtlasTools.ReadPrebakedAtlasTexture(prebakedAtlas, out texture2D))
        {
          SendOutput("Could not read dynamic atlas texture");

          return;
        }

        for (int i = 0; i < list.Count; i++)
        {
          UISpriteData uISpriteData = list[i];
          output += uISpriteData.name + _sep;
        }
        Resources.UnloadAsset(texture2D);
      }


      ////UIAtlas
      //if (_options.ContainsKey("uiatlas") && false)//not correct game object?
      //{
      //  GameObject gameObject = GameObject.Find("UIAtlas_GUI_2");
      //  if (gameObject == null)
      //  {
      //    SendOutput("Atlas object not found");
      //    return;
      //  }
      //  DynamicUIAtlas component = gameObject.GetComponent<DynamicUIAtlas>();

      //  if (component == null)
      //  {
      //    SendOutput("Atlas component not found");

      //    return;
      //  }

      //  string prebakedAtlas = component.PrebakedAtlas;
      //  List<UISpriteData> list;
      //  int num;
      //  int num2;
      //  if (!DynamicUIAtlasTools.ReadPrebakedAtlasDescriptor(prebakedAtlas, out list, out num, out num2))
      //  {
      //    SendOutput("Could not read dynamic atlas descriptor");

      //    return;
      //  }
      //  Texture2D texture2D;
      //  if (!DynamicUIAtlasTools.ReadPrebakedAtlasTexture(prebakedAtlas, out texture2D))
      //  {
      //    SendOutput("Could not read dynamic atlas texture");

      //    return;
      //  }

      //  for (int i = 0; i < list.Count; i++)
      //  {
      //    UISpriteData uISpriteData = list[i];
      //    output += uISpriteData.name + _sep;
      //  }
      //  Resources.UnloadAsset(texture2D);
      //}

      //RESOURCES
      if (_options.ContainsKey("resources"))
      {
        var objs = Resources.LoadAll("");
        //var objs = Resources.LoadAll< Texture2D>("");
        //var objs = GameObject.FindObjectsOfType<UIAtlas>();
        output += "ResourceCount:" + objs.Length + ",";

        output += "Resources:[";

        foreach (var obj in objs)
        {
          output += obj.name + "(" + obj.GetType() + "),";
          //output += obj.ToString();
        }
        output.Substring(0, output.Length - 2);
        output += "]";
      }

      //UIATLAS
      //if (_options.ContainsKey("uiatlas"))
      //{
      //  output += "UIAtlas:\n";
      //  Dictionary<string, UISpriteData> _UIAtlas = LoadUIAtlas();
      //  foreach (UISpriteData sprite in _UIAtlas.Values)
      //  {
      //    if (sprite != null)
      //    {
      //      output += sprite.name + _sep;
      //    }
      //  }
      //  output += "\n";
      //}

      SendOutput(output);
    }

    public static Dictionary<string, Transform> LoadParticleEffects()
    {
      Dictionary<string, Transform> particleEffects = new Dictionary<string, Transform>();

      object[] effects = Resources.LoadAll("ParticleEffects", typeof(Transform));
      for (int i = 0; i < effects.Length; i++)
      {
        object effect = effects[i];
        string name = ((Transform)effect).gameObject.name;
        if (name.StartsWith(ParticleEffect.prefix))
        {
          name = name.Substring(ParticleEffect.prefix.Length);
          if (particleEffects.ContainsKey(name))
          {
            Log.Error(string.Format("Particle Effect {0} already exists! Skipping it!", name));
          }
          else
          {
            particleEffects.Add(name, (Transform)effect);
          }
        }
      }
      return particleEffects;
    }

    public static Dictionary<string, UISpriteData> LoadUIAtlas()
    {
      Dictionary<string, UISpriteData> _UIAtlas = new Dictionary<string, UISpriteData>();

      //todo: get path? /NGUI Root (2D)/
      object[] sprites = Resources.LoadAll("/NGUI Root (2D)/UIAtlas", typeof(TextureAtlas));

      for (int i = 0; i < sprites.Length; i++)
      {
        object sprite = sprites[i];
        string name = ((UISpriteData)sprite).name;
        if (_UIAtlas.ContainsKey(name))
        {
          Log.Error(string.Format("Sprite {0} already exists! Skipping it!", name));
        }
        else
        {
          _UIAtlas.Add(name, (UISpriteData)sprite);
        }
      }
      return _UIAtlas;
    }
  }
}

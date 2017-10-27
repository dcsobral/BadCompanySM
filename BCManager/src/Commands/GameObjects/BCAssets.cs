using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BCM.Commands
{
  public class BCAssets : BCCommandAbstract
  {
    public class BCMTexture
    {
      public int Id;
      public string Name;
      public string Local;
      public ushort TextureId;
    }

    public class BCMMesh
    {
      public string Name;
      public string AtlasClass;
      public string MeshType;
      public bool Shadows;
      public string BlendMode;
      public string Tag;
      public string Collider;
      public string SecShader;
      public string ShaderName;
      public string ShaderDistant;
      public string MetaName;
      public string MetaText;
    }

    public override void Process()
    {
      var data = new List<object>();
      switch (Params.Count)
      {
        case 0:
          SendJson(new[]
          {
            "overlays",
            "slots",
            "particles",
            "textures",
            "meshes",
            "itemicons",
            "resources"
          });

          return;

        case 1:
          switch (Params[0])
          {
            case "overlays":
              var odaArray = OverlayLibrary.Instance.GetAllOverlayAssets();
              data.Add(odaArray.Select(oda => oda.name).ToList());

              break;

            case "slots":
              var sdaArray = SlotLibrary.Instance.GetAllSlotAssets();
              data.Add(sdaArray.Select(sda => sda.slotName).ToList());

              break;

            case "particles":
              var effects = LoadParticleEffects();
              data.Add(effects.Keys.ToList());

              break;

            case "textures":
              var textures = GetTextures();
              data.Add(textures);

              break;

            case "meshes":
              var meshes = GetMeshes();
              data.Add(meshes);

              break;

            case "itemicons":

              var sprites = GetSprites();
              if (sprites == null) return;
              data.Add(sprites.Select(uISpriteData => uISpriteData.name).ToList());

              break;

            case "resources":
              var resources = GetResources(out int count);
              data.Add(new { Count = count.ToString(), Resources = resources });

              break;
          }

          break;

        default:
          SendOutput("Incorrect params");
          SendOutput(GetHelp());

          return;
      }

      SendJson(data);
    }

    private static List<UISpriteData> GetSprites()
    {
      var gameObject = GameObject.Find("/NGUI Root (2D)/ItemIconAtlas");
      if (gameObject == null)
      {
        SendOutput("Atlas object not found");

        return null;
      }

      var component = gameObject.GetComponent<DynamicUIAtlas>();
      if (component == null)
      {
        SendOutput("Atlas component not found");

        return null;
      }

      var prebakedAtlas = component.PrebakedAtlas;

      if (!DynamicUIAtlasTools.ReadPrebakedAtlasDescriptor(prebakedAtlas, out List<UISpriteData> sprites, out int _, out int _))
      {
        SendOutput("Could not read dynamic atlas descriptor");

        return null;
      }

      if (!DynamicUIAtlasTools.ReadPrebakedAtlasTexture(prebakedAtlas, out Texture2D texture2D))
      {
        SendOutput("Could not read dynamic atlas texture");

        return null;
      }

      Resources.UnloadAsset(texture2D);

      return sprites;
    }

    private static Dictionary<string, List<string>> GetResources(out int count)
    {
      var resources = new Dictionary<string, List<string>>();

      var resourcesAll = Resources.LoadAll("");
      count = resourcesAll.Length;
      foreach (var resource in resourcesAll)
      {
        if (resources.ContainsKey(resource.GetType().ToString()))
        {
          resources[resource.GetType().ToString()].Add(resource.name);
        }
        else
        {
          resources.Add(resource.GetType().ToString(), new List<string> { resource.name });
        }
      }
      Resources.UnloadUnusedAssets();

      return resources;
    }

    private static List<BCMMesh> GetMeshes()
    {
      var meshes = new List<BCMMesh>();
      foreach (var meshDesc in MeshDescription.meshes)
      {
        if (meshDesc == null) continue;

        var item = new BCMMesh
        {
          Name = meshDesc.Name,
          AtlasClass = meshDesc.TextureAtlasClass,
          MeshType = meshDesc.meshType.ToString(),
          Shadows = meshDesc.bCastShadows,
          BlendMode = meshDesc.BlendMode.ToString(),
          Tag = meshDesc.Tag,
          Collider = meshDesc.ColliderLayerName,
          SecShader = meshDesc.SecondaryShader,
          ShaderName = meshDesc.ShaderName,
          ShaderDistant = meshDesc.ShaderNameDistant,
          MetaName = meshDesc.MetaData?.name,
          MetaText = meshDesc.MetaData?.text
          //todo: convert xml to json for MetaText
        };
        meshes.Add(item);
      }

      return meshes;
    }

    private static List<BCMTexture> GetTextures()
    {
      var textures = new List<BCMTexture>();
      foreach (var texture in BlockTextureData.list)
      {
        if (texture == null) continue;

        textures.Add(new BCMTexture
        {
          Id = texture.ID,
          Name = texture.Name,
          Local = texture.LocalizedName,
          TextureId = texture.TextureID
        });
      }

      return textures;
    }

    public static Dictionary<string, Transform> LoadParticleEffects()
    {
      var particleEffects = new Dictionary<string, Transform>();

      foreach (var effect in Resources.LoadAll("ParticleEffects", typeof(Transform)))
      {
        var name = ((Transform)effect).gameObject.name;
        if (!name.StartsWith(ParticleEffect.prefix)) continue;

        name = name.Substring(ParticleEffect.prefix.Length);
        if (particleEffects.ContainsKey(name))
        {
          Log.Error($"Particle Effect {name} already exists! Skipping it!");
        }
        else
        {
          particleEffects.Add(name, (Transform)effect);
        }
      }

      return particleEffects;
    }
  }
}

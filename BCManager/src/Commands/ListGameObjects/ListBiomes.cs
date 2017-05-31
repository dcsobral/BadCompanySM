using System.Collections.Generic;
using UnityEngine;

namespace BCM.Commands
{
  public class ListBiomes : BCCommandAbstract
  {
    //todo: BiomeDefinition.Probabilities weatherProbabilities;
    public virtual Dictionary<string, string> jsonObject()
    {
      Dictionary<string, string> data = new Dictionary<string, string>();

      foreach (KeyValuePair<uint, BiomeDefinition> kvp in GameManager.Instance.World.Biomes.GetBiomeMap())
      {
        //Replacements
        List<string> replacements = new List<string>();
        foreach (KeyValuePair<int, int> replacement in kvp.Value.Replacements)
        {
          replacements.Add("{\"source\":\"" + replacement.Key.ToString() + "\",\"target\":\"" + replacement.Value.ToString() + "\"}");
        }
        var jsonReplacements = BCUtils.toJson(replacements);

        //Layers
        List<string> layers = new List<string>();
        foreach (BiomeLayer layer in kvp.Value.m_Layers)
        {
          //Layer Resources
          List<string> clusters = new List<string>();
          foreach (List<BiomeBlockDecoration> cluster in layer.m_Resources)
          {
            List<string> resources = new List<string>();
            foreach (BiomeBlockDecoration resource in cluster)
            {
              resources.Add("{\"blockValue\":\"" + resource.m_BlockValue.type.ToString() + "\",\"prob\":\"" + resource.m_Prob.ToString() + "\",\"genType\":\"" + resource.m_resourceGeneration.ToString() + "\"}");
            }
            var jsonResources = BCUtils.toJson(resources);
            clusters.Add(jsonResources);
          }
          var jsonClusters = BCUtils.toJson(clusters);

          layers.Add("{\"blockValue\":\"" + layer.m_Block.m_BlockValue.type.ToString() + "\",\"depth\":\"" + layer.m_Depth.ToString() + "\",\"resources\":"+ jsonClusters + "}");
        }
        var jsonLayers = BCUtils.toJson(layers);

        //DecoBlocks
        List<string> decoblocks = new List<string>();
        foreach (BiomeBlockDecoration decoblock in kvp.Value.m_DecoBlocks)
        {
          decoblocks.Add("{\"blockValue\":\"" + decoblock.m_BlockValue.type.ToString() + "\",\"prob\":\"" + decoblock.m_Prob.ToString() + "\",\"rotateMax\":\"" + decoblock.randomRotateMax.ToString() + "\"}");
        }
        var jsonDecoBlocks = BCUtils.toJson(decoblocks);

        //DecoPrefabs
        List<string> decoprefabs = new List<string>();
        foreach (BiomePrefabDecoration decoprefab in kvp.Value.m_DecoPrefabs)
        {
          decoprefabs.Add("{\"name\":\"" + decoprefab.m_sPrefabName + "\",\"prob\":\"" + (decoprefab.m_Prob.ToString("f5")).TrimEnd('0') + "\"}");
        }
        var jsonDecoPrefabs = BCUtils.toJson(decoprefabs);

        //SubBiomes
        List<string> subbiomes = new List<string>();
        foreach (BiomeDefinition subbiome in kvp.Value.subbiomes)
        {
          //sub layers
          List<string> sublayers = new List<string>();
          foreach (BiomeLayer layer in subbiome.m_Layers)
          {
            //Layer Resources
            List<string> clusters = new List<string>();
            foreach (List<BiomeBlockDecoration> cluster in layer.m_Resources)
            {
              List<string> resources = new List<string>();
              foreach (BiomeBlockDecoration resource in cluster)
              {
                resources.Add("{\"blockValue\":\"" + resource.m_BlockValue.type.ToString() + "\",\"prob\":\"" + resource.m_Prob.ToString() + "\",\"genType\":\"" + resource.m_resourceGeneration.ToString() + "\"}");
              }
              var jsonResources = BCUtils.toJson(resources);
              clusters.Add(jsonResources);
            }
            var jsonClusters = BCUtils.toJson(clusters);

            sublayers.Add("{\"blockValue\":\"" + layer.m_Block.m_BlockValue.type.ToString() + "\",\"depth\":\"" + layer.m_Depth.ToString() + "\",\"resources\":" + jsonClusters + "}");
          }
          var jsonSubBiomeLayers = BCUtils.toJson(sublayers);

          //sub deco blocks
          List<string> subdecoblocks = new List<string>();
          foreach (BiomeBlockDecoration decoblock in subbiome.m_DecoBlocks)
          {
            subdecoblocks.Add("{\"blockValue\":\"" + decoblock.m_BlockValue.type.ToString() + "\",\"prob\":\"" + decoblock.m_Prob.ToString() + "\",\"rotateMax\":\"" + decoblock.randomRotateMax.ToString() + "\"}");
          }
          var jsonSubBiomeDecoBlocks = BCUtils.toJson(subdecoblocks);

          //sub deco prefabs
          List<string> subdecoprefabs = new List<string>();
          foreach (BiomePrefabDecoration prefab in subbiome.m_DecoPrefabs)
          {
            subdecoprefabs.Add("{\"name\":\"" + prefab.m_sPrefabName + "\",\"prob\":\"" + prefab.m_Prob.ToString() + "\"}");
          }
          var jsonSubBiomeDecoPrefabs = BCUtils.toJson(subdecoprefabs);

          subbiomes.Add("{\"prob\":\"" + subbiome.prob.ToString() + "\",\"layers\":" + jsonSubBiomeLayers + ",\"decoblocks\":" + jsonDecoBlocks + ",\"decoprefabs\":" + jsonDecoPrefabs + "}");
        }
        var jsonSubBiomes = BCUtils.toJson(subbiomes);

        data.Add(kvp.Key.ToString(), "{\"id\":\"" + kvp.Value.m_Id.ToString() + "\",\"name\":\"" + kvp.Value.m_sBiomeName + "\",\"uiColor\":\"" + GetColor(kvp.Value.m_uiColor).ToString() + "\",\"spectrum\":\"" + kvp.Value.m_SpectrumName + "\",\"radiation\":\"" + kvp.Value.m_RadiationLevel.ToString() + "\",\"replacements\":" + jsonReplacements + ",\"layers\":" + jsonLayers + ",\"decoblocks\":" + jsonDecoBlocks + ",\"decoprefabs\":" + jsonDecoPrefabs + ",\"subbiomes\":" + jsonSubBiomes + "}");
      }

      return data;
    }

    public override void Process()
    {
      string output = "";
      if (_options.ContainsKey("json"))
      {
        output = BCUtils.toJson(jsonObject());
        SendOutput(output);
      }
      else
      {
        Dictionary<uint, BiomeDefinition> wb = GameManager.Instance.World.Biomes.GetBiomeMap();
        foreach (BiomeDefinition bd in wb.Values)
        {
          Color c = GetColor(bd.m_uiColor);
          int ri = (int)(c.r * 255f);
          int gi = (int)(c.g * 255f);
          int bi = (int)(c.b * 255f);
          int ai = (int)(c.a * 255f);
          if (_options.ContainsKey("hex"))
          {
            string r = ri.ToString("X").PadLeft(2, '0');
            string g = gi.ToString("X").PadLeft(2, '0');
            string b = bi.ToString("X").PadLeft(2, '0');
            output += bd.m_sBiomeName + ":" + r + g + b + _sep;
          }
          else
          {
            output += bd.m_sBiomeName + ":" + ri + "," + gi + "," + bi + "," + ai + _sep;
          }
        }
        SendOutput(output);
      }
    }

    private Color GetColor(uint par0001)
    {
      byte b = (byte)(par0001 >> 24);
      byte r = (byte)(par0001 >> 16);
      byte g = (byte)(par0001 >> 8);
      byte b2 = (byte)par0001;
      return new Color32(r, g, b2, 255);
    }

  }
}

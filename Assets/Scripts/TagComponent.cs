using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class TagComponent : MonoBehaviour
{
    public string Tag;
    private string _Tag;

    private bool isInit = false;
    static Dictionary<string, List<GameObject>> TagDictionary = new Dictionary<string, List<GameObject>>();

    public void Awake()
    {
        InitOnce();
    }

    public void InitOnce()
    {
        if (!string.IsNullOrEmpty(Tag) && !isInit)
        {
            isInit = true;
            if (!TagDictionary.ContainsKey(Tag))
            {
                TagDictionary[Tag] = new List<GameObject>();
            }

            if (!TagDictionary[Tag].Contains(this.gameObject))
            {
                TagDictionary[Tag].Add(this.gameObject);
            }
            _Tag = Tag;
        }
    }

    private void OnDestroy()
    {
        if (TagDictionary.ContainsKey(_Tag) && TagDictionary[_Tag].Contains(gameObject))
        {
            TagDictionary[_Tag].Remove(gameObject);
            if (TagDictionary[_Tag].Count == 0)
            {
                TagDictionary.Remove(_Tag);
            }
        }
    }

    public static GameObject GetObjectByTag(string tag)
    {
        if (TagDictionary.ContainsKey(tag) && TagDictionary[tag].Count > 0)
        {
            return TagDictionary[tag][TagDictionary[tag].Count - 1];
        }

        return null;
    }

    public static List<GameObject> GetObjectsByTag(string tag)
    {
        if (TagDictionary.ContainsKey(tag))
        {
            return TagDictionary[tag];
        }

        return null;
    }
}

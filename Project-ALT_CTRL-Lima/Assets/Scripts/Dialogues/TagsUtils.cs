
using System;
public static class TagsUtils
{
    private static readonly string[] TextMeshProTags = new string[]
    {
        "align",
        "allcaps",
        "alpha",
        "b",
        "color",
        "cspace",
        "font",
        "font-weight",
        "gradient",
        "i",
        "indent",
        "line-height",
        "line-indent",
        "link",
        "lowercase",
        "margin",
        "mark",
        "mspace",
        "nobr",
        "noparse",
        "page",
        "pos",
        "rotate",
        "s",
        "size",
        "smallcaps",
        "space",
        "sprite",
        "style",
        "sub",
        "sup",
        "u",
        "uppercase",
        "voffset",
        "width",
    };

    public static bool IsCustomTag(string tagName)
    {
        return Array.IndexOf(TextMeshProTags, tagName) < 0;
    }

    public static string ExtractTagName(string tag)
    {
        string[] tagData = _ExtractTagsData(tag);
        string tagName = tagData[0];
        tagName = tagName.Trim();
        tagName = tagName.ToLower();
        return tagName;
    }

    public static string ExtractTagArgs(string tag)
    {
        string[] tagData = _ExtractTagsData(tag);
        if (tagData.Length > 1)
        {
            return tagData[1];
        }

        return string.Empty;
    }

    private static string[] _ExtractTagsData(string tag)
    {
        tag = tag.Replace("<", "");
        tag = tag.Replace(">", "");
        tag = tag.Replace("/", "");
        string[] strTagData = tag.Split('=');
        return strTagData;
    }
}

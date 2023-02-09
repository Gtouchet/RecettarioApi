using RecettarioApi.Models.Database;
using System.ComponentModel;
using System.Reflection;

namespace RecettarioApi;

public class Utils
{
    public static List<string> ParseRecipeCategoriesToList(string value)
    {
        return value == ERecipeCategory.None.ToString() ? new List<string>() :
            value.Split("::")
                .Select(c => Utils.GetEnumDescription(Utils.ParseStringAs<ERecipeCategory>(c)))
                .ToList();
    }

    public static List<string> ParseRecipeStepsToList(string value)
    {
        if (value == null)
        {
            return null;
        }
        List<string> result = value.Split("::").ToList();
        for (int i = 0; i < result.Count; i += 1)
        {
            result[i] = $"{i + 1}. {result[i]}";
        }
        return result;
    }

    public static E ParseStringAs<E>(string value) where E : Enum
    {
        try
        {
            return (E)Enum.Parse(typeof(E), value, ignoreCase: true);
        }
        catch (Exception)
        {
            return default;
        }
    }

    public static string GetEnumDescription(Enum value)
    {
        FieldInfo fi = value.GetType().GetField(value.ToString());
        DescriptionAttribute[] attributes = fi.GetCustomAttributes(typeof(DescriptionAttribute), false) as DescriptionAttribute[];
        if (attributes != null && attributes.Any())
        {
            return attributes.First().Description;
        }
        return value.ToString();
    }
}

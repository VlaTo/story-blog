using System.Reflection;

namespace StoryBlog.Web.Microservices.Identity.WebApi.Core;

internal static class Permissions
{
    public static IEnumerable<FieldInfo> GetPermissionTypes(Type permissionType)
    {
        return permissionType
            .GetNestedTypes()
            .SelectMany(
                fieldInfo => fieldInfo.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
            );
    }
}
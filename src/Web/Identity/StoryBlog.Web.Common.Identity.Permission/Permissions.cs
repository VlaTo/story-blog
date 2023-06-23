using System.Reflection;

namespace StoryBlog.Web.Common.Identity.Permission;

public static class Permissions
{
    public static class Users
    {
        public const string View = "Permissions.Users.View";
        public const string Create = "Permissions.Users.Create";
        public const string Update = "Permissions.Users.Update";
        public const string Delete = "Permissions.Users.Delete";
    }
    
    public static class Roles
    {
        public const string View = "Permissions.Roles.View";
        public const string Create = "Permissions.Roles.Create";
        public const string Update = "Permissions.Roles.Update";
        public const string Delete = "Permissions.Roles.Delete";
    }
    
    public static class Blogs
    {
        public const string View = "Permissions.Blogs.View";
        public const string Create = "Permissions.Blogs.Create";
        public const string Update = "Permissions.Blogs.Update";
        public const string Delete = "Permissions.Blogs.Delete";
    }
    
    public static class Comments
    {
        public const string View = "Permissions.Comments.View";
        public const string Create = "Permissions.Comments.Create";
        public const string Update = "Permissions.Comments.Update";
        public const string Delete = "Permissions.Comments.Delete";
    }

    public static IReadOnlyList<string> GetRegisteredPermissions()
    {
        const BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy;
        var permissions = new List<string>();
        var selectMany = typeof(Permissions)
            .GetNestedTypes()
            .SelectMany(
                nestedType => nestedType.GetFields(bindingFlags)
            );

        foreach (var prop in selectMany)
        {
            var propertyValue = prop.GetValue(null);

            if (null != propertyValue)
            {
                permissions.Add(propertyValue.ToString()!);
            }
        }

        return permissions.AsReadOnly();
    }
}
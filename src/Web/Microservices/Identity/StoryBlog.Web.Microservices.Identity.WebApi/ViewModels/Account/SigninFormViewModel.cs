using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace StoryBlog.Web.Microservices.Identity.WebApi.ViewModels.Account;

public sealed class SigninFormViewModel
{
    [Required]
    [MaxLength(1024)]
    [MinLength(3)]
    [DataType(DataType.EmailAddress)]
    [Description("Email address")]
    [Display(Name = "Email1", Description = "Email address")]
    [Localizable(true)]
    public string Email
    {
        get;
        set;
    }

    [Required]
    [MinLength(3)]
    [DataType(DataType.Password)]
    [Description("Password")]
    [Display(Name = "Password1", Description = "Enter password")]
    public string Password
    {
        get;
        set;
    }

    [DefaultValue(false)]
    public bool RememberMe
    {
        get;
        set;
    }
}
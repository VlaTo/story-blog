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
    [Display(Name = "Email", Description = "Email address")]
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
    [Display(Name = "Password", Description = "Enter password")]
    [DefaultValue("User_guEst1")]
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
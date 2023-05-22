﻿using StoryBlog.Web.Microservices.Identity.Application.Validation.Contexts;

namespace StoryBlog.Web.Microservices.Identity.Application.Validation;

/// <summary>
/// The device code validator
/// </summary>
public interface IDeviceCodeValidator
{
    /// <summary>
    /// Validates the device code.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <returns></returns>
    Task ValidateAsync(DeviceCodeValidationContext context);
}
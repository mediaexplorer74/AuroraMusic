﻿// 
// Copyright SmartFormat Project maintainers and contributors.
// Licensed under the MIT license.

using System;
using SmartFormat.Core.Formatting;
using SmartFormat.Core.Parsing;

namespace SmartFormat.Extensions;

/// <summary>
/// An exception caused when localization had issues.
/// </summary>
[Serializable]
public class LocalizationFormattingException : FormattingException
{
    /// <summary>
    /// Creates a new instance of <see cref="LocalizationFormattingException"/>.
    /// </summary>
    /// <param name="errorItem">The <see cref="FormatItem"/> which caused the <see cref="Exception"/>.</param>
    /// <param name="formatException">The <see cref="Exception"/> that was caused by the <see cref="FormatItem"/>.</param>
    /// <param name="index">The index inside the format string, where the error occurred.</param>
    public LocalizationFormattingException(FormatItem? errorItem, Exception formatException, int index) : base(errorItem, formatException, index)
    {
    }

    /// <summary>
    /// Creates a new instance of <see cref="LocalizationFormattingException"/>.
    /// </summary>
    /// <param name="errorItem">The <see cref="FormatItem"/> which caused the <see cref="Exception"/>.</param>
    /// <param name="issue">The description of the error.</param>
    /// <param name="index">The index inside the format string, where the error occurred.</param>
    public LocalizationFormattingException(FormatItem? errorItem, string issue, int index) : base(errorItem, issue, index)
    {
    }

    ///<inheritdoc/>
    protected LocalizationFormattingException(
        string info,
        Exception context) : base(info, context) 
    { }
}
// Copyright (C) Sina Iravanian, Julian Verdurmen, axuno gGmbH and other contributors.
// Licensed under the MIT license.

using System;
using System.Globalization;
using System.Xml;

namespace YAXLib.Exceptions;

/// <summary>
/// Raised when the value provided for some property in the XML input, cannot be
/// assigned to the property.
/// This exception is raised during deserialization.
/// </summary>
public class YAXPropertyCannotBeAssignedTo : YAXDeserializationException
{

    Exception _innerException;

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="YAXPropertyCannotBeAssignedTo" /> class.
    /// </summary>
    /// <param name="propName">Name of the property.</param>
    public YAXPropertyCannotBeAssignedTo(string propName, Exception innerException) :
        this(propName, null, innerException)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="YAXPropertyCannotBeAssignedTo" /> class.
    /// </summary>
    /// <param name="propName">Name of the property.</param>
    /// <param name="lineInfo">IXmlLineInfo derived object, e.g. XElement, XAttribute containing line info</param>
    public YAXPropertyCannotBeAssignedTo(string propName, IXmlLineInfo? lineInfo, Exception innerException) :
        base(lineInfo)
    {
        PropertyName = propName;
        _innerException = innerException;
    }

    #endregion

    #region Properties

    /// <summary>
    /// Gets the name of the property.
    /// </summary>
    /// <value>The name of the property.</value>
    public string PropertyName { get; }

    /// <summary>
    /// Gets a message that describes the current exception.
    /// </summary>
    /// <value></value>
    /// <returns>
    /// The error message that explains the reason for the exception, or an empty string("").
    /// </returns>
    public override string Message => string.Format(CultureInfo.CurrentCulture,
        "Could not assign to the property '{0}'.{1}\n\n_innerException:{2}", PropertyName, LineInfoMessage, _innerException);

    #endregion
}

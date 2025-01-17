/* Yet Another Forum.NET
 * Copyright (C) 2003-2005 Bjørnar Henden
 * Copyright (C) 2006-2013 Jaben Cargman
 * Copyright (C) 2014-2023 Ingo Herbote
 * https://www.yetanotherforum.net/
 *
 * Licensed to the Apache Software Foundation (ASF) under one
 * or more contributor license agreements.  See the NOTICE file
 * distributed with this work for additional information
 * regarding copyright ownership.  The ASF licenses this file
 * to you under the Apache License, Version 2.0 (the
 * "License"); you may not use this file except in compliance
 * with the License.  You may obtain a copy of the License at

 * https://www.apache.org/licenses/LICENSE-2.0

 * Unless required by applicable law or agreed to in writing,
 * software distributed under the License is distributed on an
 * "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY
 * KIND, either express or implied.  See the License for the
 * specific language governing permissions and limitations
 * under the License.
 */
namespace YAF.Types.Extensions;

using Microsoft.Extensions.Logging;

using YAF.Types.Constants;
using YAF.Types.Models;

/// <summary>
///     The logger extensions.
/// </summary>
public static class LoggerExtensions
{
    /// <summary>
    /// The debug.
    /// </summary>
    /// <param name="logger">
    /// The logger.
    /// </param>
    /// <param name="message">
    /// The message.
    /// </param>
    public static void Debug(this ILogger logger, [NotNull] string message)
    {
        var logEntry = new EventLog { Type = EventLogTypes.Debug.ToInt() };

        using (logger.BeginScope(logEntry))
        {
            logger.Log(LogLevel.Debug, message);
        }
    }

    /// <summary>
    /// The error.
    /// </summary>
    /// <param name="logger">
    /// The logger.
    /// </param>
    /// <param name="ex">
    /// The ex.
    /// </param>
    /// <param name="message">
    /// The message.
    /// </param>
    public static void Error(this ILogger logger, Exception ex, [NotNull] string message)
    {
        var logEntry = new EventLog { Type = EventLogTypes.Error.ToInt(), Description = ex.ToString()};

        using (logger.BeginScope(logEntry))
        {
            logger.Log(LogLevel.Error, message);
        }
    }

    /// <summary>
    /// The info.
    /// </summary>
    /// <param name="logger">
    /// The logger.
    /// </param>
    /// <param name="message">
    /// The message.
    /// </param>
    public static void Info(this ILogger logger, [NotNull] string message)
    {
        var logEntry = new EventLog {Type = EventLogTypes.Information.ToInt()};

        using (logger.BeginScope(logEntry))
        {
            logger.Log(LogLevel.Information, message);
        }
    }

    /// <summary>
    /// Log user deleted.
    /// </summary>
    /// <param name="logger">
    /// The logger.
    /// </param>
    /// <param name="userId">
    /// The user Id.
    /// </param>
    /// <param name="description">
    /// The description.
    /// </param>
    public static void UserDeleted([NotNull] this ILogger logger, [CanBeNull] int? userId, [NotNull] string description)
    {
        CodeContracts.VerifyNotNull(logger);

        var logEntry = new EventLog { Type = EventLogTypes.UserDeleted.ToInt(), Source = "User Deleted", UserID = userId};

        using (logger.BeginScope(logEntry))
        {
            logger.Log(LogLevel.Information, description);
        }
    }

    /// <summary>
    /// Log spam message detected.
    /// </summary>
    /// <param name="logger">
    /// The logger.
    /// </param>
    /// <param name="userId">
    /// The user Id.
    /// </param>
    /// <param name="description">
    /// The description.
    /// </param>
    public static void SpamMessageDetected(
        [NotNull] this ILogger logger,
        [CanBeNull] int? userId,
        [NotNull] string description)
    {
        CodeContracts.VerifyNotNull(logger);

        var logEntry = new EventLog { Type = EventLogTypes.SpamMessageDetected.ToInt(), Source = "Spam Message Detected", UserID = userId };

        using (logger.BeginScope(logEntry))
        {
            logger.Log(LogLevel.Information, description);
        }
    }

    /// <summary>
    /// Log spam bot detected.
    /// </summary>
    /// <param name="logger">
    /// The logger.
    /// </param>
    /// <param name="userId">
    /// The user Id.
    /// </param>
    /// <param name="description">
    /// The description.
    /// </param>
    public static void SpamBotDetected(
        [NotNull] this ILogger logger,
        [CanBeNull] int? userId,
        [NotNull] string description)
    {
        CodeContracts.VerifyNotNull(logger);

        var logEntry = new EventLog { Type = EventLogTypes.SpamBotDetected.ToInt(), Source = "Bot Detected", UserID = userId };

        using (logger.BeginScope(logEntry))
        {
            logger.Log(LogLevel.Information, description);
        }
    }

    /// <summary>
    /// The log.
    /// </summary>
    /// <param name="logger">
    /// The logger.
    /// </param>
    /// <param name="message">
    /// The message.
    /// </param>
    /// <param name="eventType">
    /// The event type.
    /// </param>
    /// <param name="userId">
    /// The user Id.
    /// </param>
    /// <param name="source">
    /// The source.
    /// </param>
    /// <param name="exception">
    /// The exception.
    /// </param>
    public static void Log(
        [NotNull] this ILogger logger,
        string message,
        EventLogTypes eventType = EventLogTypes.Error,
        int? userId = null,
        string source = null,
        Exception exception = null)
    {
        CodeContracts.VerifyNotNull(logger);

        var logEntry = new EventLog { Type = eventType.ToInt(), Source = source, UserID = userId, Exception = exception};

        using (logger.BeginScope(logEntry))
        {
            logger.Log(LogLevel.Information, message);
        }
    }

    /// <summary>
    /// The log.
    /// </summary>
    /// <param name="logger">
    /// The logger.
    /// </param>
    /// <param name="userId">
    /// The user Id.
    /// </param>
    /// <param name="source">
    /// The source.
    /// </param>
    /// <param name="description">
    /// The description.
    /// </param>
    /// <param name="eventType">
    /// The event type.
    /// </param>
    public static void Log(
        [NotNull] this ILogger logger,
        [CanBeNull] int? userId,
        [CanBeNull] object source,
        [NotNull] string description,
        [NotNull] EventLogTypes eventType = EventLogTypes.Error)
    {
        CodeContracts.VerifyNotNull(logger);

        var sourceDescription = "unknown";

        if (source is Type)
        {
            sourceDescription = source.GetType().FullName;
        }
        else if (source != null)
        {
            sourceDescription = source.ToString();
        }

        var logEntry = new EventLog { Type = eventType.ToInt(), Source = sourceDescription, UserID = userId };

        using (logger.BeginScope(logEntry))
        {
            logger.Log(LogLevel.Error, description);
        }
    }

    /// <summary>
    /// The warn.
    /// </summary>
    /// <param name="logger">
    /// The logger.
    /// </param>
    /// <param name="format">
    /// The format.
    /// </param>
    /// <param name="args">
    /// The args.
    /// </param>
    public static void Warn(this ILogger logger, [NotNull] string format, [NotNull] params object[] args)
    {
        var logEntry = new EventLog { Type = EventLogTypes.Warning.ToInt() };

        using (logger.BeginScope(logEntry))
        {
            logger.Log(LogLevel.Warning, string.Format(format, args));
        }
    }
}
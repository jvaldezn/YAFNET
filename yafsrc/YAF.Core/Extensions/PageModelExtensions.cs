﻿/* Yet Another Forum.NET
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

namespace YAF.Core.Extensions;

using System;

using Microsoft.AspNetCore.Mvc.RazorPages;

using YAF.Types.Attributes;
using YAF.Types.Objects;

/// <summary>
/// The page model extensions.
/// </summary>
public static class PageModelExtensions
{
    /// <summary>
    /// Registers the Java Script block.
    /// </summary>
    /// <param name="pageModel">The page model.</param>
    /// <param name="scriptBlock">The script block.</param>
    /// <returns>PageModel.</returns>
    public static PageModel RegisterJsBlock(this PageModel pageModel, [NotNull] string scriptBlock)
    {
        var tempData = pageModel.TempData;

        tempData["_jsBlock"] = scriptBlock;

        return pageModel;
    }

    /// <summary>
    /// The toast message.
    /// </summary>
    /// <param name="pageModel">
    /// The page model.
    /// </param>
    /// <param name="type">
    /// The type.
    /// </param>
    /// <param name="body">
    /// The body.
    /// </param>
    /// <returns>
    /// The <see cref="PageModel"/>.
    /// </returns>
    public static PageModel ToastMessage(this PageModel pageModel, [NotNull] string type, [NotNull] string body)
    {
        var tempData = pageModel.TempData;

        tempData["_alert.type"] = type;
        tempData["_alert.body"] = body;

        return pageModel;
    }

    public static PageModel SessionToastMessage(this PageModel pageModel, [NotNull] MessageTypes type, [NotNull] string body)
    {
        var message = new MessageNotification {Message = body, MessageType = type};

        BoardContext.Current.Get<IDataCache>().Set("ToastMessageSession", message, TimeSpan.FromMinutes(2));

        return pageModel;
    }

    /// <summary>
    /// Show BootBox Confirm Modal
    /// </summary>
    /// <param name="pageModel">
    /// The page model.
    /// </param>
    /// <param name="title">
    /// The title.
    /// </param>
    /// <param name="text">
    /// The text.
    /// </param>
    /// <param name="yes">
    /// The yes.
    /// </param>
    /// <param name="no">
    /// The no.
    /// </param>
    /// <param name="link">
    /// The link.
    /// </param>
    /// <returns>
    /// The <see cref="PageModel"/>.
    /// </returns>
    public static PageModel ConfirmModal(
        this PageModel pageModel,
        [NotNull] string title,
        [NotNull] string text,
        [NotNull] string yes,
        [NotNull] string no,
        [NotNull] string link)
    {
        var tempData = pageModel.TempData;

        tempData["_confirm.title"] = title;
        tempData["_confirm.text"] = text;
        tempData["_confirm.yes"] = yes;
        tempData["_confirm.no"] = no;
        tempData["_confirm.link"] = link;

        return pageModel;
    }
}
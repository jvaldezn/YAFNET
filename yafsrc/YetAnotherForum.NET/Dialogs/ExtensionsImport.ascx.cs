/* Yet Another Forum.NET
 * Copyright (C) 2003-2005 Bjørnar Henden
 * Copyright (C) 2006-2013 Jaben Cargman
 * Copyright (C) 2014-2019 Ingo Herbote
 * http://www.yetanotherforum.net/
 * 
 * Licensed to the Apache Software Foundation (ASF) under one
 * or more contributor license agreements.  See the NOTICE file
 * distributed with this work for additional information
 * regarding copyright ownership.  The ASF licenses this file
 * to you under the Apache License, Version 2.0 (the
 * "License"); you may not use this file except in compliance
 * with the License.  You may obtain a copy of the License at

 * http://www.apache.org/licenses/LICENSE-2.0

 * Unless required by applicable law or agreed to in writing,
 * software distributed under the License is distributed on an
 * "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY
 * KIND, either express or implied.  See the License for the
 * specific language governing permissions and limitations
 * under the License.
 */

namespace YAF.Dialogs
{
    #region Using

    using System;

    using YAF.Core;
    using YAF.Core.Services.Import;
    using YAF.Types;
    using YAF.Types.Constants;
    using YAF.Types.Extensions;
    using YAF.Types.Interfaces;

    #endregion

    /// <summary>
    /// The Admin Extensions Import Dialog.
    /// </summary>
    public partial class ExtensionsImport : BaseUserControl
    {
        #region Methods

        /// <summary>
        /// Try to Import from selected File
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Import_OnClick([NotNull] object sender, [NotNull] EventArgs e)
        {
            // import selected file (if it's the proper format)...
            if (!this.importFile.PostedFile.ContentType.StartsWith("text"))
            {
                this.PageContext.AddLoadMessage(
                    string.Format(
                        this.GetText("ADMIN_EXTENSIONS_IMPORT", "IMPORT_FAILED"), "Invalid upload format specified: " + this.importFile.PostedFile.ContentType));
            }

            try
            {
                var importedCount = DataImport.FileExtensionImport(
                    this.PageContext.PageBoardID, this.importFile.PostedFile.InputStream);

                this.PageContext.AddLoadMessage(
                    importedCount > 0
                        ? string.Format(this.GetText("ADMIN_EXTENSIONS_IMPORT", "IMPORT_SUCESS"), importedCount)
                        : this.GetText("ADMIN_EXTENSIONS_IMPORT", "IMPORT_NOTHING"),
                    importedCount > 0 ? MessageTypes.success : MessageTypes.warning);
            }
            catch (Exception x)
            {
                this.PageContext.AddLoadMessage(
                    string.Format(this.GetText("ADMIN_EXTENSIONS_IMPORT", "IMPORT_FAILED"), x.Message), MessageTypes.danger);
            }
        }

        #endregion
    }
}
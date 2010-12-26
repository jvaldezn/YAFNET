﻿/* Yet Another Forum.NET
 * Copyright (C) 2006-2010 Jaben Cargman
 * http://www.yetanotherforum.net/
 * 
 * This program is free software; you can redistribute it and/or
 * modify it under the terms of the GNU General Public License
 * as published by the Free Software Foundation; either version 2
 * of the License, or (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program; if not, write to the Free Software
 * Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA  02111-1307, USA.
 */
namespace YAF.Classes.Core
{
  #region Using

  using System;

  using YAF.Classes.Utils;

  #endregion

    /// <summary>
    /// The Theme Handler Interface
    /// </summary>
    public interface IThemeHandler
    {
        /// <summary>
        /// The before init.
        /// </summary>
        event EventHandler<EventArgs> BeforeInit;

        /// <summary>
        /// The after init.
        /// </summary>
        event EventHandler<EventArgs> AfterInit;

        /// <summary>
        ///   Gets or sets Theme.
        /// </summary>
        YafTheme Theme { get; set; }

        /// <summary>
        /// Init Theme
        /// </summary>
        void InitTheme();
    }

  /// <summary>
  /// The theme handler.
  /// </summary>
    public class ThemeHandler : IThemeHandler
  {
    #region Constants and Fields

    /// <summary>
    ///   The _init theme.
    /// </summary>
    private bool _initTheme;

    /// <summary>
    ///   The _theme.
    /// </summary>
    private YafTheme _theme;

    #endregion

    #region Events

    /// <summary>
    ///   The after init.
    /// </summary>
    public event EventHandler<EventArgs> AfterInit;

    /// <summary>
    ///   The before init.
    /// </summary>
    public event EventHandler<EventArgs> BeforeInit;

    #endregion

    #region Properties

    /// <summary>
    ///   Gets or sets Theme.
    /// </summary>
    public YafTheme Theme
    {
      get
      {
        if (!this._initTheme)
        {
          this.InitTheme();
        }

        return this._theme;
      }

      set
      {
        this._theme = value;
        this._initTheme = value != null;
      }
    }

    #endregion

    #region Methods

    /// <summary>
    /// Sets the theme class up for usage
    /// </summary>
    /// <exception cref="CantLoadThemeException"><c>CantLoadThemeException</c>.</exception>
    public void InitTheme()
    {
        if (this._initTheme)
        {
            return;
        }

        if (this.BeforeInit != null)
        {
            this.BeforeInit(this, new EventArgs());
        }

        string themeFile;

        if (YafContext.Current.Page != null && YafContext.Current.Page["ThemeFile"] != DBNull.Value &&
            YafContext.Current.BoardSettings.AllowUserTheme)
        {
            // use user-selected theme
            themeFile = YafContext.Current.Page["ThemeFile"].ToString();
        }
        else if (YafContext.Current.Page != null && YafContext.Current.Page["ForumTheme"] != DBNull.Value)
        {
            themeFile = YafContext.Current.Page["ForumTheme"].ToString();
        }
        else
        {
            themeFile = YafContext.Current.BoardSettings.Theme;
        }

        if (!YafTheme.IsValidTheme(themeFile))
        {
            themeFile = StaticDataHelper.Themes().Rows[0][1].ToString();
        }

        // create the theme class
        this.Theme = new YafTheme(themeFile);

        // make sure it's valid again...
        if (!YafTheme.IsValidTheme(this.Theme.ThemeFile))
        {
            // can't load a theme... throw an exception.
            throw new CantLoadThemeException(
                "Unable to find a theme to load. Last attempted to load \"{0}\" but failed.".FormatWith(themeFile));
        }

        if (this.AfterInit != null)
        {
            this.AfterInit(this, new EventArgs());
        }
    }

    #endregion
  }
}
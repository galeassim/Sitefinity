﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SectionIO
{
    public interface ICacheFlusher
    {
        /// <summary>
        /// This method build the banexpression that needs to be passed into "API_Proxy_State_Post()" in order to clear cached dynamic pages in Section.IO
        /// </summary>
        /// <param name="pageUrl">Passes site url</param>
        void FlushByUrl(string pageUrl);

        /// <summary>
        /// This method builds the banexpression that needs to be passed into "API_Proxy_State_Post()"
        /// </summary>
        /// <param name="asset">Asset type that was passed into the method from SitesEdit.aspx button click</param>
        /// <param name="siteBaseUriFormat">The format of the uri <example>"://www.example.com/http_imagesjxtnetau/jxt-solutions/"</example></param
        void FlushAssetType(AssetClass assetToFlush, string siteBaseUriFormat);
    }

    public enum AssetClass
    {
        js,
        css,
        all
    }
}


﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JXTNext.Sitefinity.Widgets.Social.Mvc.Logics
{
    public interface IProcessSocialMediaData
    {
        (string, bool) ProcessData(string data);
    }
}
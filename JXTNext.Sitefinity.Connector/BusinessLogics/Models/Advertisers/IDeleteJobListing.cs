﻿using JXTNext.Sitefinity.Connector.BusinessLogics.Models.Job;
using System;
using System.Collections.Generic;
using System.Text;

namespace JXTNext.Sitefinity.Connector.BusinessLogics.Models.Advertisers
{
    public interface IDeleteJobListingRequest
    {
        int JobID { get; set; }        
    }

    public interface IDeleteJobListingResponse
    {
    }
}

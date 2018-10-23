﻿using JXTNext.Sitefinity.Services.Intefaces.Models.JobApplication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JXTNext.Sitefinity.Services.Intefaces
{
    public interface IJobApplicationService
    {
        string GetOverrideEmail(ref JobApplicationStatus status, ApplicantInfo applicantInfo, bool isSocialMedia = false);
        bool UploadFiles(List<JobApplicationAttachmentUploadItem> attachments);
        string GetHtmlEmailContent(string emailTemplateId, string emailTemplateProviderName, string itemType);
    }
}

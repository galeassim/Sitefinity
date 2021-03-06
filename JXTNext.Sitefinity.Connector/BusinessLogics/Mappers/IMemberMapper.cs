﻿using JXTNext.Sitefinity.Connector.BusinessLogics.Models.Member;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JXTNext.Sitefinity.Connector.BusinessLogics.Mappers
{
    public interface IMemberMapper
    {
        IntegrationMapperType mapperType { get; }
        dynamic Register_ConvertToAPIEntity<T>(T registerDetails);

        dynamic Application_ConvertToAPIEntity<T>(T applyDetails);

        List<T> MemberSavedJob_ConvertToLocalEntity<T>(dynamic data) where T : class;
        List<T> MemberAppliedJob_ConvertToLocalEntity<T>(dynamic data) where T : class;

        T Member_ConvertToLocalEntity<T>(dynamic data) where T : class;
        dynamic Member_ConvertToAPIEntity(MemberModel model);
    }
}

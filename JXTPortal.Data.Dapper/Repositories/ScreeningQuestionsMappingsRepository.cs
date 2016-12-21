﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JXTPortal.Data.Dapper.Factories;
using System.Data;
using Dapper;
using JXTPortal.Data.Dapper.Entities.ScreeningQuestions;

namespace JXTPortal.Data.Dapper.Repositories
{
    public interface IScreeningQuestionsMappingsRepository : IBaseEntityOperation<ScreeningQuestionsMappingsEntity>
    {
        List<ScreeningQuestionsMappingsEntity> SelectByScreeningQuestionsTemplateId(int jobId);
    }

    public class ScreeningQuestionsMappingsRepository : BaseEntityOperation<ScreeningQuestionsMappingsEntity>, IScreeningQuestionsMappingsRepository
    {
        public ScreeningQuestionsMappingsRepository(IConnectionFactory connectionFactory, string connectionStringName)
            : base(connectionFactory, connectionStringName)
        {
            TableName = "ScreeningQuestionsMappings";
            ColumnNames = new List<string> { "QuestionType", "Mandatory", "ScreeningQuestionsTemplateId", "Visible", "ScreeningQuestionId" };
            IdColumnName = "ScreeningQuestionsMappingId";
        }

        public List<ScreeningQuestionsMappingsEntity> SelectByScreeningQuestionsTemplateId(int screeningQuestionsTemplateId)
        {
            using (IDbConnection dbConnection = _connectionFactory.Create(_connectionStringName))
            {
                dbConnection.Open();
                string columns = IdColumnName + ", " + string.Join(", ", ColumnNames);
                string whereClause = string.Format("ScreeningQuestionsTemplateId = {0}", screeningQuestionsTemplateId);
                var query = string.Format("SELECT {0} FROM dbo.{1} WHERE {2}", columns, TableName, whereClause);
                var entity = dbConnection.Query<ScreeningQuestionsMappingsEntity>(query, new { ScreeningQuestionsTemplateId = screeningQuestionsTemplateId }).ToList();
                return entity as List<ScreeningQuestionsMappingsEntity>;
            }
        }
    }
}

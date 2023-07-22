using DBCommandHelper;
using System;
using System.Data;
using System.Data.SqlClient;
using Xunit;

namespace DbCommandHelper
{
    public class UnitDbCommandHelperTests
    {
        private readonly IDbCommand _dbCommand = new SqlCommand();

        [Fact()]
        public void CreateParamWithNameAndType()
        {
            _dbCommand.CreateParameter("paramName", DbType.String);

            Assert.True(!DbParameterExistsAndHasValue("paramName"));

            Assert.True(DbParameterTypeEquals("paramName", DbType.String));
        }

        [Fact()]
        public void CreateParamWithNameAndValue()
        {
            _dbCommand.CreateParameter("paramName", "test");

            Assert.True(DbParameterValueEquals("paramName", "test"));

            Assert.True(DbParameterTypeEquals("paramName", DbType.String));
        }

        [Fact()]
        public void CreateParamWithNameTypeAndValue()
        {
            _dbCommand.CreateParameter("paramName", DbType.String, "test");

            Assert.True(DbParameterValueEquals("paramName", "test"));

            Assert.True(DbParameterTypeEquals("paramName", DbType.String));
        }

        [Fact()]
        public void ClearParameters()
        {
            _dbCommand.CreateParameter("paramName", "test");

            _dbCommand.Parameters.ClearValues();

            object paramValue = ((IDataParameter)_dbCommand.Parameters["paramName"]).Value;

            Assert.Equal(DBNull.Value, paramValue);
        }

        private bool DbParameterTypeEquals(string paramName, DbType type)
        {
            IDataParameter param = (IDataParameter)_dbCommand.Parameters[paramName];

            return param.DbType.Equals(type);
        }

        private bool DbParameterExistsAndHasValue(string paraName)
        {
            IDataParameter param = (IDataParameter)_dbCommand.Parameters[paraName];

            return param.Value != null;
        }

        private bool DbParameterValueEquals(string paraName, string paramValue)
        {
            IDataParameter param = (IDataParameter)_dbCommand.Parameters[paraName];

            return param.Value.Equals(paramValue);
        }
    }
}

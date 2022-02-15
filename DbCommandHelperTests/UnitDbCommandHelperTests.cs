using DBCommandHelper;
using System;
using System.Data;
using System.Data.SqlClient;
using Xunit;

namespace DbCommand
{
    public class UnitDbCommandHelperTests
    {
        private readonly IDbCommand dbCommand;

        public UnitDbCommandHelperTests()
        {
            dbCommand = new SqlCommand();
        }

        [Fact]
        public void CreateParamWithNameTypeAndValue()
        {
            dbCommand.CreateParamWithNameTypeAndValue("paramName1", DbType.String, "stringValue");
            IDataParameter param = dbCommand.Parameters["paramName1"] as IDataParameter;
            bool paramExistsAndHasValue = dbCommand.Parameters.Contains("paramName1") && param.Value.ToString() == "stringValue";

            Assert.True(paramExistsAndHasValue);
        }

        [Fact]
        public void CreateParamWithName()
        {
            dbCommand.CreateParamWithNameAndType("paramName2", DbType.String);
            IDataParameter param = dbCommand.Parameters["paramName2"] as IDataParameter;
            bool paramExistsAndHasNullValue = dbCommand.Parameters.Contains("paramName2") && param.Value == null;

            Assert.True(paramExistsAndHasNullValue);
        }

        [Fact]
        public void CreateParamWithNameAndValue()
        {
            dbCommand.CreateParamWithNameAndValue("paramName3", "stringValue");
            IDataParameter param = dbCommand.Parameters["paramName3"] as IDataParameter;
            bool paramExistsAndHasValue = dbCommand.Parameters.Contains("paramName3") && param.Value.ToString() == "stringValue";

            Assert.True(paramExistsAndHasValue);
        }

        [Fact]
        public void SetParamValueShouldThrowAnException()
        {
            Assert.Throws<ArgumentNullException>(() => { dbCommand.SetParamValue("", ""); });
        }

        [Fact]
        public void SetParamWithNameAndValueShouldThrowAnException()
        {
            Assert.Throws<ArgumentNullException>(() => { dbCommand.CreateParamWithNameAndValue("", ""); });
        }

        [Fact]
        public void SetParamWithNameValueAndTypeShouldThrowAnException()
        {
            Assert.Throws<ArgumentNullException>(() => { dbCommand.CreateParamWithNameTypeAndValue("", DbType.String, ""); });
        }

    }
}

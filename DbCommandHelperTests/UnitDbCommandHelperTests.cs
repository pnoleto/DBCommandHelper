using System.Data;
using System.Data.SqlClient;
using Xunit;
using DBCommandHelper;
using System;

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

            bool paramExists = dbCommand.Parameters.Contains("paramName1");

            Assert.True(paramExists);
        }

        [Fact]
        public void CreateParamWithName()
        {
            dbCommand.CreateParamWithNameAndType("paramName2", DbType.String);

            bool paramExists = dbCommand.Parameters.Contains("paramName2");

            Assert.True(paramExists);
        }

        [Fact]
        public void CreateParamWithNameAndValue()
        {
            dbCommand.CreateParamWithNameAndValue("paramName3", "stringValue");

            bool paramExists = dbCommand.Parameters.Contains("paramName3");

            Assert.True(paramExists);
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

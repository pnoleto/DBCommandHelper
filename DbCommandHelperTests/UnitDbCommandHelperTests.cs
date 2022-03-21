using DBCommandHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Xunit;

namespace DbCommand
{
    public class UnitDbCommandHelperTests
    {
        private readonly IDbCommand _dbCommand;

        public UnitDbCommandHelperTests()
        {
            _dbCommand = new SqlCommand();
        }

        [Fact]
        public void CleanAllDbCommandParameterValues()
        {
            _dbCommand.CreateParamWithNameTypeAndValue("paramName1", DbType.String, "stringValue");
            _dbCommand.CreateParamWithNameTypeAndValue("paramName2", DbType.String, "stringValue");
            _dbCommand.CreateParamWithNameTypeAndValue("paramName3", DbType.String, "stringValue");

            _dbCommand.CleanAllParamValues();

            IList<IDataParameter> paramList = new List<IDataParameter>();

            foreach (IDataParameter item in _dbCommand.Parameters) paramList.Add(item);

            bool paramsHasValue = paramList.Where(y => !string.IsNullOrEmpty(y.Value.ToString())).Any();

            Assert.False(paramList.Any() && paramsHasValue);
        }

        [Fact]
        public void CreateParamWithNameTypeAndValue()
        {
            _dbCommand.CreateParamWithNameTypeAndValue("paramName1", DbType.String, "stringValue");
            IDataParameter param = (IDataParameter)_dbCommand.Parameters["paramName1"];

            Assert.True(GetParamExistsAnHasValue(param));
        }

        [Fact]
        public void CreateParamWithName()
        {
            _dbCommand.CreateParamWithNameAndType("paramName2", DbType.String);
            IDataParameter param = (IDataParameter)_dbCommand.Parameters["paramName2"];

            Assert.True(GetParamExistsAndHasNullValue(param));
        }

        [Fact]
        public void CreateParamWithNameAndValue()
        {
            _dbCommand.CreateParamWithNameAndValue("paramName3", "stringValue");
            IDataParameter param = (IDataParameter)_dbCommand.Parameters["paramName3"];

            Assert.True(GetParamExistsAndHasValue(param));
        }

        [Fact]
        public void SetParamValueShouldThrowAnException()
        {
            Assert.Throws<ArgumentNullException>(() => { SetEmptyParamString(); });
        }

        [Fact]
        public void SetParamWithNameAndValueShouldThrowAnException()
        {
            Assert.Throws<ArgumentNullException>(() => { CreateParamWithEmptyString(); });
        }

        [Fact]
        public void SetParamWithNameValueAndTypeShouldThrowAnException()
        {
            Assert.Throws<ArgumentNullException>(() => { CreateParammWithNameTypeAndValue(); });
        }

        private void CreateParammWithNameTypeAndValue()
        {
            _dbCommand.CreateParamWithNameTypeAndValue("", DbType.String, "");
        }

        private void CreateParamWithEmptyString()
        {
            _dbCommand.CreateParamWithNameAndValue("", "");
        }
        private bool GetParamExistsAndHasNullValue(IDataParameter param)
        {
            return _dbCommand.Parameters.Contains("paramName2") && param.Value == null;
        }

        private bool GetParamExistsAnHasValue(IDataParameter param)
        {
            return _dbCommand.Parameters.Contains("paramName1") && param.Value.ToString() == "stringValue";
        }

        private bool GetParamExistsAndHasValue(IDataParameter param)
        {
            return _dbCommand.Parameters.Contains("paramName3") && param.Value.ToString() == "stringValue";
        }
        private void SetEmptyParamString()
        {
            _dbCommand.SetParamValue("", "");
        }
    }
}

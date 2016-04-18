using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Contest.Core.Converters;
using Contest.Core.DataStore.Sql.BusinessObjectFactory;
using Contest.Core.DataStore.Sql.UnitTest.Entities;
using Contest.Core.Repository;
using Moq;
using NUnit.Framework;

namespace Contest.Core.DataStore.Sql.UnitTest.BusinessObjectFactory
{
    [TestFixture]
    public class SqlSerializerTest
    {
        private Mock<IConverter> Converter { get; set; }
        private Mock<IDataReader> Data { get; set; }

        [SetUp]
        public void Init()
        {
            Converter = new Mock<IConverter>();
            Data = new Mock<IDataReader>();
        }

        [TestCase]
        public void Create_FromNullDataReader_ShouldReturnDefaultValue()
        {
            var serializer = CreateSqlSerializer<object>();

            Assert.IsNull(serializer.Create(null));
        }

        [TestCase]
        public void Create_DefaultConstructorLessEntity_ShouldThrowException()
        {
            var serializer = CreateSqlSerializer<DefaultConstructorLessEntity>();
            var data = new Mock<IDataReader>();

            Assert.Throws<NotSupportedException>(() => serializer.Create(data.Object));
        }

        [TestCase]
        public void Create_BasicEntity_ShouldReturnBasicEntity()
        {
            var serializer = CreateSqlSerializer<BasicEntity>();
            var data = new Mock<IDataReader>();

            var obj = serializer.Create(data.Object);

            Assert.IsInstanceOf<BasicEntity>(obj);
        }

        [TestCase]
        public void Create_DataDoesNotConstainsValuePrimaryKeyField_BasicEntityPropertyShouldBeNotFill()
        {
            var serializer = CreateSqlSerializer<BasicEntity>();
            
            var obj = serializer.Create(Data.Object);

            Assert.AreEqual(Guid.Empty, obj.Id);
        }

        [TestCase]
        public void Create_DataDoesNotConstainsValueSqlField_BasicEntityPropertyShouldBeNotFill()
        {
            var serializer = CreateSqlSerializer<BasicEntity>();
            
            var obj = serializer.Create(Data.Object);

            Assert.AreEqual(null, obj.Name);
        }

        [TestCase]
        public void Create_DataConstainsValueForPrimaryKeyField_BasicEntityPropertyShouldBeFill()
        {
            var expectedValue = Guid.NewGuid();
            var serializer = CreateSqlSerializer<BasicEntity>();
            Data.Setup(_ => _["Id"]).Returns(expectedValue.ToString());
            Converter.Setup(_ => _.Convert(typeof(Guid), expectedValue.ToString(), It.IsAny<object[]>())).Returns(expectedValue);
            
            var obj = serializer.Create(Data.Object);

            Assert.AreEqual(expectedValue, obj.Id);
        }

        [TestCase]
        public void Create_DataConstainsValueForSqlField_BasicEntityPropertyShouldBeFill()
        {
            const string expectedValue = "Xxxx";
            var serializer = CreateSqlSerializer<BasicEntity>();
            Data.Setup(_ => _["Name"]).Returns(expectedValue);
            Converter.Setup(_ => _.Convert(typeof(string), expectedValue, It.IsAny<object[]>())).Returns(expectedValue);

            var obj = serializer.Create(Data.Object);

            Assert.AreEqual(expectedValue, obj.Name);
        }

        [TestCase]
        public void Create_DataConstainsValueForPropertyWithoutSqlAttribute_BasicEntityPropertyShouldNotBeFill()
        {
            const string expectedValue = "Xxxx";
            var serializer = CreateSqlSerializer<BasicEntity>();
            Data.Setup(_ => _["NoSerializedField"]).Returns(expectedValue);
            Converter.Setup(_ => _.Convert(typeof(string), expectedValue, It.IsAny<object[]>())).Returns(expectedValue);

            var obj = serializer.Create(Data.Object);

            Assert.IsNull(obj.NoSerializedField);
        }

        [TestCase]
        public void Create_DataConstainsValueForReadOnlyProperty_ShouldThrowException()
        {
            const string expectedValue = "Xxxx";
            var serializer = CreateSqlSerializer<BasicEntity>();
            Data.Setup(_ => _["ReadOnlyProperty"]).Returns(expectedValue);
            Converter.Setup(_ => _.Convert(typeof(string), expectedValue, It.IsAny<object[]>())).Returns(expectedValue);

            Assert.Throws<NotSupportedException>(() => serializer.Create(Data.Object));
        }

        [TestCase]
        public void GetEnumPivotValue_DataDoesNotContainsSqlField_ShouldThrowException()
        {
            var serializer = CreateSqlSerializer<EnumPivotEntity>();

            Assert.Throws<NotSupportedException>(() => serializer.GetEnumPivotValue(typeof(EnumPivot), Data.Object));
        }

        [TestCase]
        public void GetEnumPivotValue_EntityHasNoEnumCorresponding_ShouldThrowException()
        {
            var serializer = CreateSqlSerializer<EnumPivotEntity>();

            Assert.Throws<NotSupportedException>(() => serializer.GetEnumPivotValue(typeof(Enum), Data.Object));
        }

        [TestCase]
        public void GetEnumPivotValue_ValidEntityAndDatareader_ShouldReturnEnumPivotValue()
        {
            const string expectedValue = "Xxxx";
            var serializer = CreateSqlSerializer<EnumPivotEntity>();
            Data.Setup(_ => _["Pivot"]).Returns(expectedValue);

            Assert.AreEqual(expectedValue, serializer.GetEnumPivotValue(typeof(EnumPivot), Data.Object));
        }

        [TestCase]
        public void GetRealObjectType_NoSpecifiq_ShouldReturnSameType()
        {
            var expectedType = typeof (BasicEntity);
            var serializer = CreateSqlSerializer<BasicEntity>();

            Assert.AreEqual(expectedType, serializer.GetRealObjectType(expectedType, Data.Object));
        }

        [TestCase]
        public void GetRealObjectType_DynamicType_ShouldReturnProjectedType()
        {
            var serializer = CreateSqlSerializer<DynamicEntity>();
            Data.Setup(_ => _["Pivot"]).Returns("BasicAssociatedClass");
            SetupConverterForPivotEnum();

            Assert.AreEqual(typeof(BasicEntity), serializer.GetRealObjectType(typeof(DynamicEntity), Data.Object));
        }

        [TestCase]
        public void GetRealObjectType_GenericType_ShouldReturnProjectedType()
        {
            var serializer = CreateSqlSerializer<GenericEntity<BasicEntity>>();
            Data.Setup(_ => _["Pivot"]).Returns("BasicAssociatedClass");
            SetupConverterForPivotEnum();

            Assert.AreEqual(typeof(GenericEntity<BasicEntity>), serializer.GetRealObjectType(typeof(GenericEntity<>), Data.Object));
        }

        [TestCase]
        public void GetRealObjectType_DynamicGenericType_ShouldReturnProjectedType()
        {
            var serializer = CreateSqlSerializer<DynamicEntity>();
            Data.SetupSequence(_ => _["Pivot"]).Returns("GenericAssociatedClass")
                                               .Returns("GenericAssociatedClass")
                                               .Returns("BasicAssociatedClass")
                                               .Returns("BasicAssociatedClass");
            SetupConverterForPivotEnum();

            Assert.AreEqual(typeof(GenericEntity<BasicEntity>), serializer.GetRealObjectType(typeof(DynamicEntity), Data.Object));
        }

        [TestCase]
        public void GetRealObjectType_GenericTypeDynamic_ShouldReturnProjectedType()
        {
            var serializer = CreateSqlSerializer<GenericEntity<BasicEntity>>();
            Data.SetupSequence(_ => _["Pivot"]).Returns("DynamicAssociatedClass")
                                               .Returns("BasicAssociatedClass");
            SetupConverterForPivotEnum();

            Assert.AreEqual(typeof(GenericEntity<BasicEntity>), serializer.GetRealObjectType(typeof(GenericEntity<>), Data.Object));
        }
        
        [TestCase]
        public void FillOneToManyReferences_NullEntity_ShouldThrowException()
        {
            var serializer = CreateSqlSerializer<OneToManyEntity>();

            Assert.Throws<TargetException>(() => serializer.FillOneToManyReference(null, null));
        }
        
        [TestCase]
        public void FillOneToManyReferences_OneToManyReferenceProperties_ShouldBeFill()
        {
            var obj = new OneToManyEntity();
            var expectedReference = new ManyToOneEntity();
            var unitOfWorks = new Mock<IUnitOfWorks>();
            unitOfWorks.Setup(_ => _.Find(typeof(ManyToOneEntity), It.IsAny<LambdaExpression>()))
                       .Returns(new List<ManyToOneEntity> { expectedReference });
            var serializer = CreateSqlSerializer<OneToManyEntity>();

            serializer.FillOneToManyReference(unitOfWorks.Object, obj);

            Assert.AreEqual(expectedReference, obj.EntityList[0]);
        }

        private SqlSerializer<T, T> CreateSqlSerializer<T>()
            where T : class
        {
            return CreateSqlSerializer<T, T>();
        }

        private SqlSerializer<T, TI> CreateSqlSerializer<T, TI>()
            where T : class, TI
            where TI : class
        {
            return new SqlSerializer<T, TI>(Converter.Object);
        }

        private void SetupConverterForPivotEnum()
        {
            Converter.Setup(_ => _.Convert(typeof(EnumPivot), "DynamicAssociatedClass", It.IsAny<object[]>())).Returns(EnumPivot.DynamicAssociatedClass);
            Converter.Setup(_ => _.Convert(typeof(EnumPivot), "GenericAssociatedClass", It.IsAny<object[]>())).Returns(EnumPivot.GenericAssociatedClass);
            Converter.Setup(_ => _.Convert(typeof(EnumPivot), "BasicAssociatedClass", It.IsAny<object[]>())).Returns(EnumPivot.BasicAssociatedClass);
        }
    }
}

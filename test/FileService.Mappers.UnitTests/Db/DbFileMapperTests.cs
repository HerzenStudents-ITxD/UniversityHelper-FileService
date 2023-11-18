using UniversityHelper.FileService.Mappers.Db;
using UniversityHelper.FileService.Mappers.Db.Interfaces;
using UniversityHelper.FileService.Models.Db;
using UniversityHelper.FileService.Models.Dto.Requests;
using UniversityHelper.Core.Extensions;
using UniversityHelper.UnitTestCore;
using Microsoft.AspNetCore.Http;
using Moq.AutoMock;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace UniversityHelper.FileService.Mappers.UnitTests.Db
{
    public class DbFileMapperTests
    {
        /*private AutoMocker _autoMocker;
        private IDbFileMapper _dbFileMapper;
        private IDictionary<object, object> _items;
        private DbFile _dbFile;
        private AddFileRequest _fileRequest;
        private Guid _userId;

        [SetUp]
        public void SetUp()
        {
            _autoMocker = new();

            _userId = Guid.NewGuid();
            _items = new Dictionary<object, object>();
            _items.Add("UserId", _userId);

            _dbFileMapper = _autoMocker.CreateInstance<DbFileMapper>();

            _autoMocker
               .Setup<IHttpContextAccessor, IDictionary<object, object>>(x => x.HttpContext.Items)
               .Returns(_items);

            _fileRequest = new AddFileRequest
            {
                Content = "RGlnaXRhbCBPZmA5Y2U=",
                Extension = ".txt",
                Name = "UniversityHelperTestFile"
            };

            _dbFile = new DbFile
            {
                Id = Guid.NewGuid(),
                Content = "RGlnaXRhbCBPZmA5Y2U=",
                Extension = ".txt",
                Name = "UniversityHelperTestFile"
            };
        }*/

        /*[Test]
        public void ShouldThrowArgumentNullExceptionWhenRequestMappingObjectIsNull()
        {
            _fileRequest = null;

            Assert.Throws<ArgumentNullException>(() => _dbFileMapper.Map(_fileRequest));
        }

        [Test]
        public void ShouldReturnDbFileWhenMappingFileRequest()
        {
            var newFile = _dbFileMapper.Map(_fileRequest);

            var expectedFile = new DbFile
            {
                Id = newFile.Id,
                Content = _fileRequest.Content,
                Extension = _fileRequest.Extension,
                Name = _fileRequest.Name,
                IsActive = true,
                CreatedAtUtc = newFile.CreatedAtUtc,
                CreatedBy = _userId
            };

            SerializerAssert.AreEqual(expectedFile, newFile);
        }*/
    }
}

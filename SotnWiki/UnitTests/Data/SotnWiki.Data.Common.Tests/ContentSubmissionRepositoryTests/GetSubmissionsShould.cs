﻿using Moq;
using NUnit.Framework;
using SotnWiki.Data.Common.Repositories;
using SotnWiki.Data.Common.Tests.Mocks;
using SotnWiki.Models;
using System;
using System.Collections.Generic;

namespace SotnWiki.Data.Common.Tests.ContentSubmissionRepositoryTests
{
    [TestFixture]
    public class GetSubmissionsShould
    {
        [Test]
        public void ThrowArgumentNullExceptionWhenTitleIsNull()
        {
            //Arrange
            var mockedDbContext = new Mock<ISotnWikiDbContext>();
            string expectedExceptionMessage = "title";
            var submissions = new List<PageContentSubmission>
            {
                new PageContentSubmission() { Id = Guid.NewGuid(), Content = "asdasd" },
                new PageContentSubmission() { Id = Guid.NewGuid(), Content = "asdasd" }
            };
            var mockedSet = QueryableDbSetMock.GetQueryableMockDbSet<PageContentSubmission>(submissions);
            mockedDbContext.Setup(c => c.Set<PageContentSubmission>()).Returns(mockedSet);
            mockedDbContext.Setup(c => c.PageContentSubmissions).Returns(mockedSet);
            var repositoryUnderTest = new ContentSubmissionRepository(mockedDbContext.Object);

            //Act & Assert
            var exc = Assert.Throws<ArgumentNullException>(() => { repositoryUnderTest.GetSubmissions(null); });

            //Assert
            StringAssert.Contains(expectedExceptionMessage, exc.Message);
        }

        [Test]
        public void ThrowArgumentNullExceptionWhenTitleIsEmptyString()
        {
            //Arrange
            var mockedDbContext = new Mock<ISotnWikiDbContext>();
            string expectedExceptionMessage = "title";
            var submissions = new List<PageContentSubmission>
            {
                new PageContentSubmission() { Id = Guid.NewGuid(), Content = "asdasd" },
                new PageContentSubmission() { Id = Guid.NewGuid(), Content = "asdasd" }
            };
            var mockedSet = QueryableDbSetMock.GetQueryableMockDbSet<PageContentSubmission>(submissions);
            mockedDbContext.Setup(c => c.Set<PageContentSubmission>()).Returns(mockedSet);
            mockedDbContext.Setup(c => c.PageContentSubmissions).Returns(mockedSet);
            var repositoryUnderTest = new ContentSubmissionRepository(mockedDbContext.Object);

            //Act & Assert
            var exc = Assert.Throws<ArgumentException>(() => { repositoryUnderTest.GetSubmissions(""); });

            //Assert
            StringAssert.Contains(expectedExceptionMessage, exc.Message);
        }

        [Test]
        public void ReturnIEnumerableObjectOfTypePageContentSubmission()
        {
            //Arrange
            var mockedDbContext = new Mock<ISotnWikiDbContext>();
            var page = new Page() { Id = Guid.NewGuid(), Title = "page", Content = "cntnt", IsPublished = true };
            var submissions = new List<PageContentSubmission>
            {
                new PageContentSubmission() { Id = Guid.NewGuid(), Content = "asdasd", PageEdit=page },
                new PageContentSubmission() { Id = Guid.NewGuid(), Content = "asdasd", PageEdit=page }
            };
            var mockedSet = QueryableDbSetMock.GetQueryableMockDbSet<PageContentSubmission>(submissions);
            mockedDbContext.Setup(c => c.Set<PageContentSubmission>()).Returns(mockedSet);
            mockedDbContext.Setup(c => c.PageContentSubmissions).Returns(mockedSet);
            var repositoryUnderTest = new ContentSubmissionRepository(mockedDbContext.Object);

            //Act & Assert
            var result = repositoryUnderTest.GetSubmissions("page");

            //Assert
            Assert.IsInstanceOf<IEnumerable<PageContentSubmission>>(result);
        }
    }
}
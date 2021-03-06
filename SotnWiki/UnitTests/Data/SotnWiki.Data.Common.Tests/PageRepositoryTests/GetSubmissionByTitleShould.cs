﻿using Moq;
using NUnit.Framework;
using SotnWiki.Data.Repositories;
using SotnWiki.Data.Common.Tests.Mocks;
using SotnWiki.Models;
using System;
using System.Collections.Generic;

namespace SotnWiki.Data.Common.Tests.PageEfRepositoryTests
{
    [TestFixture]
    public class GetSubmissionByTitleShould
    {
        [Test]
        public void ThrowArgumentNullException_WhenTitleIsNull()
        {
            //Arrange
            var mockedDbContext = new Mock<ISotnWikiDbContext>();
            string expectedExceptionMessage = "title";
            var pages = new List<Page>
            {
                new Page() {Id = Guid.NewGuid(), Title = "page", Content = "cntnt"},
                new Page() {Id = Guid.NewGuid(), Title = "page", Content = "cntnt"}
            };
            var mockedPageSet = QueryableDbSetMock.GetQueryableMockDbSet<Page>(pages);
            mockedDbContext.Setup(c => c.Set<Page>()).Returns(mockedPageSet);
            mockedDbContext.Setup(c => c.Pages).Returns(mockedPageSet);
            var repositoryUnderTest = new PageEfRepository(mockedDbContext.Object);

            //Act & Assert
            var exc = Assert.Throws<ArgumentNullException>(() => { repositoryUnderTest.GetSubmissionByTitle(null); });

            //Assert
            StringAssert.Contains(expectedExceptionMessage, exc.Message);
        }

        [Test]
        public void ThrowArgumentNullException_WhenTitleIsEmptyString()
        {
            //Arrange
            var mockedDbContext = new Mock<ISotnWikiDbContext>();
            string expectedExceptionMessage = "title";
            var pages = new List<Page>
            {
                new Page() {Id = Guid.NewGuid(), Title = "page", Content = "cntnt"},
                new Page() {Id = Guid.NewGuid(), Title = "page", Content = "cntnt"}
            };
            var mockedPageSet = QueryableDbSetMock.GetQueryableMockDbSet<Page>(pages);
            mockedDbContext.Setup(c => c.Set<Page>()).Returns(mockedPageSet);
            mockedDbContext.Setup(c => c.Pages).Returns(mockedPageSet);
            var repositoryUnderTest = new PageEfRepository(mockedDbContext.Object);

            //Act & Assert
            var exc = Assert.Throws<ArgumentException>(() => { repositoryUnderTest.GetSubmissionByTitle(""); });

            //Assert
            StringAssert.Contains(expectedExceptionMessage, exc.Message);
        }
    }
}

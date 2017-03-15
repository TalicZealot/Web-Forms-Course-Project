﻿using Moq;
using NUnit.Framework;
using SotnWiki.Data.Common.Repositories;
using SotnWiki.Data.Common.Tests.Mocks;
using SotnWiki.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SotnWiki.Data.Common.Tests.PageRepositoryTests
{
    [TestFixture]
    public class FindPagesShould
    {
        [Test]
        public void ReturnIEnumerableOfTypePage()
        {
            //Arrange
            var mockedDbContext = new Mock<ISotnWikiDbContext>();
            var pages = new List<Page>
            {
                new Page() {Id = Guid.NewGuid(), Title = "page", Content = "cntnt", IsPublished = true},
                new Page() {Id = Guid.NewGuid(), Title = "pagea", Content = "cntnta", IsPublished = true}
            };
            var mockedPageSet = QueryableDbSetMock.GetQueryableMockDbSet<Page>(pages);
            mockedDbContext.Setup(c => c.Set<Page>()).Returns(mockedPageSet);
            mockedDbContext.Setup(c => c.Pages).Returns(mockedPageSet);
            var repositoryUnderTest = new PageRepository(mockedDbContext.Object);

            //Act & Assert
            var result = repositoryUnderTest.FindPages("pag");

            //Assert
            Assert.IsInstanceOf<IEnumerable<Page>>(result);
        }

        [Test]
        public void ReturnCorrectResult()
        {
            //Arrange
            var mockedDbContext = new Mock<ISotnWikiDbContext>();
            var expectedId = Guid.NewGuid();
            var pages = new List<Page>
            {
                new Page() {Id = expectedId, Title = "mayhem", Content = "cntnt", IsPublished = true},
                new Page() {Id = Guid.NewGuid(), Title = "pagea", Content = "cntnta", IsPublished = true}
            };
            var mockedPageSet = QueryableDbSetMock.GetQueryableMockDbSet<Page>(pages);
            mockedDbContext.Setup(c => c.Set<Page>()).Returns(mockedPageSet);
            mockedDbContext.Setup(c => c.Pages).Returns(mockedPageSet);
            var repositoryUnderTest = new PageRepository(mockedDbContext.Object);

            //Act & Assert
            var result = repositoryUnderTest.FindPages("mayhem").ToList();

            //Assert
            Assert.AreEqual(expectedId, result[0].Id);
        }
    }
}
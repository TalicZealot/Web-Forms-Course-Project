﻿using Moq;
using NUnit.Framework;
using SotnWiki.DataServices.Contracts;
using SotnWiki.MvcClient.Controllers;
using System;

namespace SotnWiki.MvcClient.Tests.EditControllerTests
{
    [TestFixture]
    public class ConstructorShould
    {
        [Test]
        public void ThrowArgumentNullException_WhenPageServiceIsNull()
        {
            //Arrange
            var mockedContentSubmissionService = new Mock<IContentSubmissionService>();
            string expectedExceptionMessage = "pageService";

            //Act
            var exc = Assert.Throws<ArgumentNullException>(() => { new EditController(null, mockedContentSubmissionService.Object); });

            //Assert
            StringAssert.Contains(expectedExceptionMessage, exc.Message);
        }

        [Test]
        public void ThrowArgumentNullException_WhenContentSubmissionServiceIsNull()
        {
            //Arrange
            var mockedPageService = new Mock<IPageService>();
            string expectedExceptionMessage = "contentSubmissionService";

            //Act
            var exc = Assert.Throws<ArgumentNullException>(() => { new EditController(mockedPageService.Object, null); });

            //Assert
            StringAssert.Contains(expectedExceptionMessage, exc.Message);
        }

        [Test]
        public void ReturnsAnInstance_WhenParametersAreNotNull()
        {
            //Arrange
            var mockedPageService = new Mock<IPageService>();
            var mockedContentSubmissionService = new Mock<IContentSubmissionService>();

            //Act
            var result = new EditController(mockedPageService.Object, mockedContentSubmissionService.Object);

            //Assert
            Assert.IsNotNull(result);
        }
    }
}

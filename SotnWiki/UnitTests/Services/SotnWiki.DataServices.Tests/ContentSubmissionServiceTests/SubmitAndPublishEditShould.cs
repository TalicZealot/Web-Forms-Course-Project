﻿using Moq;
using NUnit.Framework;
using SotnWiki.Data.Common;
using SotnWiki.Data.Common.Contracts;
using SotnWiki.DataServices.Contracts;
using SotnWiki.Models;
using System;
using System.Collections.Generic;

namespace SotnWiki.DataServices.Tests.ContentSubmissionServiceTests
{
    [TestFixture]
    public class SubmitAndPublishEditShould
    {
        [Test]
        public void ThrowArgumentNullExceptionWhenTitleArgumentIsNull()
        {
            //Arrange
            var mockedPageService = new Mock<IPageService>();
            var mockedPageContentSubmissionRepository = new Mock<IContentSubmissionRepository>();
            var mockedPageRepository = new Mock<IPageRepository>();
            Func<IUnitOfWork> mockedUnitOfWorkFactory = () => { return new Mock<IUnitOfWork>().Object; };
            string expectedExceptionMessage = "title";
            var submissionServiceUnderTest = new ContentSubmissionService(mockedPageContentSubmissionRepository.Object, mockedPageRepository.Object, mockedUnitOfWorkFactory, mockedPageService.Object);

            //Act
            var exc = Assert.Throws<ArgumentNullException>(() => submissionServiceUnderTest.SubmitAndPublishEdit("asdasd", null));

            //Assert
            StringAssert.Contains(expectedExceptionMessage, exc.Message);
        }

        [Test]
        public void ThrowArgumentExceptionWhenTitleArgumentIsEmptyString()
        {
            //Arrange
            var mockedPageService = new Mock<IPageService>();
            var mockedPageContentSubmissionRepository = new Mock<IContentSubmissionRepository>();
            var mockedPageRepository = new Mock<IPageRepository>();
            Func<IUnitOfWork> mockedUnitOfWorkFactory = () => { return new Mock<IUnitOfWork>().Object; };
            string expectedExceptionMessage = "title";
            var submissionServiceUnderTest = new ContentSubmissionService(mockedPageContentSubmissionRepository.Object, mockedPageRepository.Object, mockedUnitOfWorkFactory, mockedPageService.Object);

            //Act
            var exc = Assert.Throws<ArgumentException>(() => submissionServiceUnderTest.SubmitAndPublishEdit("asdasd", ""));

            //Assert
            StringAssert.Contains(expectedExceptionMessage, exc.Message);
        }

        [Test]
        public void ThrowArgumentNullExceptionWhenContentArgumentIsNull()
        {
            //Arrange
            var mockedPageService = new Mock<IPageService>();
            var mockedPageContentSubmissionRepository = new Mock<IContentSubmissionRepository>();
            var mockedPageRepository = new Mock<IPageRepository>();
            Func<IUnitOfWork> mockedUnitOfWorkFactory = () => { return new Mock<IUnitOfWork>().Object; };
            string expectedExceptionMessage = "content";
            var submissionServiceUnderTest = new ContentSubmissionService(mockedPageContentSubmissionRepository.Object, mockedPageRepository.Object, mockedUnitOfWorkFactory, mockedPageService.Object);

            //Act
            var exc = Assert.Throws<ArgumentNullException>(() => submissionServiceUnderTest.SubmitAndPublishEdit(null, "asdasd"));

            //Assert
            StringAssert.Contains(expectedExceptionMessage, exc.Message);
        }

        [Test]
        public void ThrowArgumentExceptionWhenContentArgumentIsEmptyString()
        {
            //Arrange
            var mockedPageService = new Mock<IPageService>();
            var mockedPageContentSubmissionRepository = new Mock<IContentSubmissionRepository>();
            var mockedPageRepository = new Mock<IPageRepository>();
            Func<IUnitOfWork> mockedUnitOfWorkFactory = () => { return new Mock<IUnitOfWork>().Object; };
            string expectedExceptionMessage = "content";
            var submissionServiceUnderTest = new ContentSubmissionService(mockedPageContentSubmissionRepository.Object, mockedPageRepository.Object, mockedUnitOfWorkFactory, mockedPageService.Object);

            //Act
            var exc = Assert.Throws<ArgumentException>(() => submissionServiceUnderTest.SubmitAndPublishEdit("", "asdasd"));

            //Assert
            StringAssert.Contains(expectedExceptionMessage, exc.Message);
        }

        [Test]
        public void ThrowNullReferenceExceptionWhenPageIsNotFound()
        {
            //Arrange
            var mockedPageService = new Mock<IPageService>();
            var mockedUnitOfWork = new Mock<IUnitOfWork>();
            Func<IUnitOfWork> mockedUnitOfWorkFactory = () => { return mockedUnitOfWork.Object; };
            var mockedPageContentSubmissionRepository = new Mock<IContentSubmissionRepository>();
            var mockedPageRepository = new Mock<IPageRepository>();
            var edits = new List<PageContentSubmission>();
            var submissionServiceUnderTest = new ContentSubmissionService(mockedPageContentSubmissionRepository.Object, mockedPageRepository.Object, mockedUnitOfWorkFactory, mockedPageService.Object);
            mockedPageContentSubmissionRepository.Setup(x => x.Add(It.IsAny<PageContentSubmission>())).Callback<PageContentSubmission>(
                x => {
                    edits.Add(x);
                });
            string expectedContent = "test content";
            var expectedExceptionMessage = "Page not found!";

            //Act
            var exc = Assert.Throws<NullReferenceException>(() => submissionServiceUnderTest.SubmitAndPublishEdit(expectedContent, "test_title"));

            //Assert
            StringAssert.Contains(expectedExceptionMessage, exc.Message);
        }

        [Test]
        public void CallCommitMethodOfUnitOfWork()
        {
            //Arrange
            var mockedPageService = new Mock<IPageService>();
            var mockedUnitOfWork = new Mock<IUnitOfWork>();
            Func<IUnitOfWork> mockedUnitOfWorkFactory = () => { return mockedUnitOfWork.Object; };
            var mockedPageContentSubmissionRepository = new Mock<IContentSubmissionRepository>();
            var mockedPageRepository = new Mock<IPageRepository>();
            var edits = new List<PageContentSubmission>();
            var submissionServiceUnderTest = new ContentSubmissionService(mockedPageContentSubmissionRepository.Object, mockedPageRepository.Object, mockedUnitOfWorkFactory, mockedPageService.Object);
            mockedPageContentSubmissionRepository.Setup(x => x.Add(It.IsAny<PageContentSubmission>())).Callback<PageContentSubmission>(
                x => {
                    edits.Add(x);
                });
            string expectedContent = "test content";
            var page = new Page()
            {
                Content = "aa",
                LastEdit = null,
            };
            mockedPageRepository.Setup(r => r.GetPageEntityByTitle(It.IsAny<string>())).Returns(page);

            //Act
            submissionServiceUnderTest.SubmitAndPublishEdit(expectedContent, "test_title");
            var edit = edits[0];

            //Assert
            mockedUnitOfWork.Verify(m => m.Commit(), Times.Once());
        }

        [Test]
        public void AddPageContentSubmissionToRepositoryWithCorrectContent()
        {
            //Arrange
            var mockedPageService = new Mock<IPageService>();
            var mockedPageContentSubmissionRepository = new Mock<IContentSubmissionRepository>();
            var mockedPageRepository = new Mock<IPageRepository>();
            Func<IUnitOfWork> mockedUnitOfWorkFactory = () => { return new Mock<IUnitOfWork>().Object; };
            var edits = new List<PageContentSubmission>();
            var submissionServiceUnderTest = new ContentSubmissionService(mockedPageContentSubmissionRepository.Object, mockedPageRepository.Object, mockedUnitOfWorkFactory, mockedPageService.Object);
            mockedPageContentSubmissionRepository.Setup(x => x.Add(It.IsAny<PageContentSubmission>())).Callback<PageContentSubmission>(
                x => {
                    edits.Add(x);
                });
            string expectedContent = "aa";
            var page = new Page()
            {
                Content = "aa",
                LastEdit = null,
            };
            mockedPageRepository.Setup(r => r.GetPageEntityByTitle(It.IsAny<string>())).Returns(page);

            //Act
            submissionServiceUnderTest.SubmitAndPublishEdit(expectedContent, "test_title");
            var edit = edits[0];
            
            //Assert
            Assert.AreEqual(expectedContent, edit.Content);
        }

        [Test]
        public void SetsNewContentSubmissiponToHistory()
        {
            //Arrange
            var mockedPageService = new Mock<IPageService>();
            var mockedPageContentSubmissionRepository = new Mock<IContentSubmissionRepository>();
            var mockedPageRepository = new Mock<IPageRepository>();
            Func<IUnitOfWork> mockedUnitOfWorkFactory = () => { return new Mock<IUnitOfWork>().Object; };
            var edits = new List<PageContentSubmission>();
            var submissionServiceUnderTest = new ContentSubmissionService(mockedPageContentSubmissionRepository.Object, mockedPageRepository.Object, mockedUnitOfWorkFactory, mockedPageService.Object);
            mockedPageContentSubmissionRepository.Setup(x => x.Add(It.IsAny<PageContentSubmission>())).Callback<PageContentSubmission>(
                x => {
                    edits.Add(x);
                });
            string expectedContent = "aa";
            var page = new Page()
            {
                Content = "aa",
                LastEdit = null,
            };
            mockedPageRepository.Setup(r => r.GetPageEntityByTitle(It.IsAny<string>())).Returns(page);

            //Act
            submissionServiceUnderTest.SubmitAndPublishEdit(expectedContent, "test_title");
            var edit = edits[0];

            //Assert
            Assert.AreEqual(page, edit.PageHistory);
        }

        [Test]
        public void SetsPageContentToContentArgument()
        {
            //Arrange
            var mockedPageService = new Mock<IPageService>();
            var mockedPageContentSubmissionRepository = new Mock<IContentSubmissionRepository>();
            var mockedPageRepository = new Mock<IPageRepository>();
            Func<IUnitOfWork> mockedUnitOfWorkFactory = () => { return new Mock<IUnitOfWork>().Object; };
            var edits = new List<PageContentSubmission>();
            var submissionServiceUnderTest = new ContentSubmissionService(mockedPageContentSubmissionRepository.Object, mockedPageRepository.Object, mockedUnitOfWorkFactory, mockedPageService.Object);
            mockedPageContentSubmissionRepository.Setup(x => x.Add(It.IsAny<PageContentSubmission>())).Callback<PageContentSubmission>(
                x => {
                    edits.Add(x);
                });
            string expectedContent = "zzzz";
            var page = new Page()
            {
                Content = "aa",
                LastEdit = null,
            };
            mockedPageRepository.Setup(r => r.GetPageEntityByTitle(It.IsAny<string>())).Returns(page);

            //Act
            submissionServiceUnderTest.SubmitAndPublishEdit(expectedContent, "test_title");
            var edit = edits[0];

            //Assert
            Assert.AreEqual(expectedContent, page.Content);
        }
    }
}
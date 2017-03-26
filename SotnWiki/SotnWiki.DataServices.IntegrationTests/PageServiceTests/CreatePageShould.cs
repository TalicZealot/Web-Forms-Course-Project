﻿using Ninject;
using NUnit.Framework;
using SotnWiki.Data;
using SotnWiki.DataServices.Contracts;
using SotnWiki.DTOs.PageViewsDTOs;
using SotnWiki.Models;
using SotnWiki.MvcClient.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SotnWiki.DataServices.IntegrationTests.PageServiceTests
{
    [TestFixture]
    public class CreatePageShould
    {
        private string defaultPageContent =
@"h1. This page is empty

There is currently no content in this page. Use the edit page button to edit and submit new content. Please adhere to the site templates.


This wiki uses TxStyle markup, for reference visit  ""https://txstyle.org/"":https://txstyle.org/.";

        private List<Character> characters = new List<Character>()
            {
                new Character () { Name = "Site", Id = 0},
                new Character () { Name = "Alucard", Id = 1}
            };

        private List<Page> pages = new List<Page>();

        private static IKernel kernel;

        [SetUp]
        public void Init()
        {
            kernel = NinjectWebCommon.CreateKernel();

            ISotnWikiDbContext dbContext = kernel.Get<ISotnWikiDbContext>();

            dbContext.Characters.Add(characters[0]);
            dbContext.Characters.Add(characters[1]);
            dbContext.SaveChanges();

            var siteEntity = dbContext.Characters.Find(0);
            var alucardEntity = dbContext.Characters.Find(1);
            pages = new List<Page>()
            {
                new Page () { Id = Guid.NewGuid(), IsPublished = false, Title = "Main Page", CreatedOn = DateTime.Now, GeneralCharacter = siteEntity , Content = defaultPageContent},
                new Page () { Id = Guid.NewGuid(), IsPublished = false, Title = "Alucard", CreatedOn = DateTime.Now, GeneralCharacter = alucardEntity , Content = defaultPageContent}
            };

            dbContext.Pages.Add(pages[0]);
            dbContext.Pages.Add(pages[1]);
            dbContext.SaveChanges();
        }

        [TearDown]
        public void Dispose()
        {
            ISotnWikiDbContext dbContext = kernel.Get<ISotnWikiDbContext>();
            dbContext.Database.ExecuteSqlCommand("DELETE FROM [Pages]");
            dbContext.Database.ExecuteSqlCommand("DELETE FROM [Characters]");
            dbContext.Database.ExecuteSqlCommand("DBCC CHECKIDENT('Characters',RESEED,0);");
            dbContext.SaveChanges();
        }

        [Test]
        public void ReturnNull_WhenPageSubmissionIsNotFound()
        {
            //Arrange
            IPageService pageService = kernel.Get<IPageService>();
            var pageContent = @"h1. This page is here to test stuff blabla bla bla bla bla bly contains no NUnit 3.0 tests: D:\Programming\web\Frontslide-Wiki\SotnWiki\Services\SotnWiki.DataServices\bin\Debug\SotnWiki.DataServices.dll
Assembly contains no NUnit 3.0 tests: D:\Programming\web\Frontslide-Wiki\SotnWiki\SotnWiki.MvcClient\bin\SotnWiki.MvcClient.dll
======================================================================
================================================================================================================================================================  ";

            ////Act
            pageService.CreatePage(1, "Category", "Alucard Any Percent", pageContent, true);
            ISotnWikiDbContext dbContext = kernel.Get<ISotnWikiDbContext>();

            //Assert
            var count = dbContext.Pages.Count();

            Assert.AreEqual(3, count);
        }
    }
}

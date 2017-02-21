﻿using Bytes2you.Validation;
using SotnWiki.Data.Common;
using SotnWiki.DataServices.Contracts;
using SotnWiki.Models;
using System;
using System.Collections.Generic;

namespace SotnWiki.DataServices
{
    public class ContentSubmissionService : IContentSubmissionService
    {
        private readonly IRepository<PageContentSubmission> pageContentSubmissionRepository;
        private readonly IRepository<Page> pageRepository;
        private readonly Func<IUnitOfWork> unitOfWorkFactory;
        private readonly IPageService pageService;

        public ContentSubmissionService(IRepository<PageContentSubmission> pageContentSubmissionRepository, IRepository<Page> pageRepository, Func<IUnitOfWork> unitOfWorkFactory, IPageService pageService)
        {
            Guard.WhenArgument(pageService, nameof(IPageService)).IsNull().Throw();
            Guard.WhenArgument(pageContentSubmissionRepository, nameof(IRepository<PageContentSubmission>)).IsNull().Throw();
            Guard.WhenArgument(pageRepository, nameof(IRepository<Page>)).IsNull().Throw();
            Guard.WhenArgument(unitOfWorkFactory, nameof(Func<IUnitOfWork>)).IsNull().Throw();

            this.pageService = pageService;
            this.pageContentSubmissionRepository = pageContentSubmissionRepository;
            this.pageRepository = pageRepository;
            this.unitOfWorkFactory = unitOfWorkFactory;
        }

        public void SubmitEdit(string content, string title)
        {
            Guard.WhenArgument(title, "title").IsNullOrEmpty().Throw();

            var page = this.pageService.GetPageByTitle(title);
            var submission = new PageContentSubmission() { Content = content, PageEdit = page };

            using (var unitOfWork = this.unitOfWorkFactory())
            {
                this.pageContentSubmissionRepository.Add(submission);
                unitOfWork.Commit();
            }
        }

        public void PublishEdit(string title, string content, Guid id)
        {
            Guard.WhenArgument(title, "title").IsNullOrEmpty().Throw();

            var pageContentSubmission = this.GetPageContentSubmissionById(id);
            pageContentSubmission.PageHistory = pageContentSubmission.PageEdit;
            pageContentSubmission.PageEdit = null;
            var page = this.pageService.GetPageByTitle(title);
            pageContentSubmission.Content = page.Content;
            page.Content = content;
            page.LastEdit = DateTime.Now;
            using (var unitOfWork = this.unitOfWorkFactory())
            {
                this.pageRepository.Update(page);
                this.pageContentSubmissionRepository.Update(pageContentSubmission);
                unitOfWork.Commit();
            }
        }

        public void DismissEdit(Guid id)
        {
            var pageContentSubmission = this.GetPageContentSubmissionById(id);

            using (var unitOfWork = this.unitOfWorkFactory())
            {
                this.pageContentSubmissionRepository.Delete(pageContentSubmission);
                unitOfWork.Commit();
            }
        }

        public PageContentSubmission GetPageContentSubmissionById(Guid id)
        {
            return this.pageContentSubmissionRepository.GetById(id);
        }

        public IEnumerable<PageContentSubmission> GetSubmissions(string title)
        {
            Guard.WhenArgument(title, "title").IsNullOrEmpty().Throw();

            var page = this.pageService.GetPageByTitle(title);
            return page.Pending;
        }
    }
}

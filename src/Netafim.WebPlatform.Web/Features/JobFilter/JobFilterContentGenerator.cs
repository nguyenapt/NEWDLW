using Castle.Core.Internal;
using Dlw.EpiBase.Content.Infrastructure.Data.ContentGenerator;
using EPiServer;
using EPiServer.Core;
using EPiServer.DataAccess;
using EPiServer.Logging;
using EPiServer.Security;
using EPiServer.Web;
using Netafim.WebPlatform.Web.Core.Extensions;
using Netafim.WebPlatform.Web.Core.Templates;
using Netafim.WebPlatform.Web.Features.DepartmentOverview;
using Netafim.WebPlatform.Web.Features.Home;
using Netafim.WebPlatform.Web.Features.JobFilter.CustomProperties;
using Netafim.WebPlatform.Web.Features.Settings;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Netafim.WebPlatform.Web.Features.JobFilter
{
    public class JobFilterContentGenerator : IContentGenerator
    {
        private const string DataDemoFolder = @"~/Features/JobFilter/Data/Demo/{0}";

        private readonly IContentRepository _contentRepository;
        private readonly IUrlSegmentCreator _urlSegmentCreator;
        private readonly ContentAssetHelper _contentAssetHelper;

        private ILogger _logger = LogManager.GetLogger();
        public JobFilterContentGenerator(IContentRepository contentRepository,
            IUrlSegmentCreator urlSegmentCreator,
            ContentAssetHelper contentAssetHelper
            )
        {
            _contentRepository = contentRepository;
            _urlSegmentCreator = urlSegmentCreator;
            _contentAssetHelper = contentAssetHelper;
        }

        public void Generate(ContentContext context)
        {
            EnsureComponent(context);
        }

        private void EnsureComponent(ContentContext context)
        {
            var homepage = _contentRepository.Get<HomePage>(context.Homepage);
            if (homepage == null) { return; }

            EnsureJobData(context);
        }

        private SettingsPage EnsureSettingsPage()
        {
            var settingsPage = _contentRepository.GetChildren<SettingsPage>(ContentReference.RootPage).SingleOrDefault();
            if (settingsPage != null) return settingsPage;

            var page = _contentRepository.GetDefault<SettingsPage>(ContentReference.RootPage);
            if (_contentRepository.Save(page, SaveAction.Publish, AccessLevel.NoAccess) != null) { return page; };

            return null;
        }

        private void EnsureJobData(ContentContext context)
        {
            var settingsPage = EnsureSettingsPage();
            if (settingsPage == null) return;

            EnsureJobSettings(settingsPage);

            var updatedSettingspage = _contentRepository.GetChildren<SettingsPage>(ContentReference.RootPage).SingleOrDefault();
            EnsureJobDetailData(updatedSettingspage.JobDepartments, updatedSettingspage.JobPositions, updatedSettingspage.JobLocations, context);

            EnsureJobFilter(context);
        }

        private void EnsureJobDetailData(IList<JobDepartment> jobDepartments, IList<JobPosition> jobPositions, IList<JobLocation> jobLocations, ContentContext context)
        {
            var totalItems = Math.Min(jobDepartments.Count, jobPositions.Count);

            for (int i = 0; i < totalItems; i++)
            {
                var jobDetailPage = InitPage<JobDetails.JobDetailsPage>(context.Homepage, jobPositions.ElementAt(i).JobName);
                jobDetailPage.Postingdate = new DateTime(2017, 4, 5);
                jobDetailPage.EndDate = new DateTime(2017, 6, 17);
                jobDetailPage.JobSchedule = "Full-time";
                jobDetailPage.Department = jobDepartments.ElementAt(i).DepartmentName;
                jobDetailPage.Position = jobPositions.ElementAt(i).JobName;
                Save(jobDetailPage);
            }
        }
        private void EnsureJobFilter(ContentContext context)
        {
            var jobFilterBlock = InitBlock<JobFilterBlock>(_contentAssetHelper.GetOrCreateAssetFolder(context.Homepage), "Job Filter");
            jobFilterBlock.AnchorId = this._urlSegmentCreator.Create((IContent)jobFilterBlock).Replace(" ", "");
            var jobFilterRef = Save((IContent)jobFilterBlock);
            var overViewPage = InitPage<GenericContainerPage>(context.Homepage, "Job overview");
            Save(overViewPage);
            overViewPage.Content = new ContentArea();
            overViewPage.Content.Items.Add(new ContentAreaItem()
            {
                ContentLink = jobFilterRef
            });

            overViewPage.Content.Items.Add(new ContentAreaItem() { ContentLink = GenerateDepartmentOverViewBlock(overViewPage.ContentLink, jobFilterBlock) });

            Save(overViewPage);
        }

        private void EnsureJobSettings(SettingsPage settingsPage)
        {
            var needUpdateDepartments = false;
            var needUpdatePositions = false;
            var needUpdateLocations = false;
            IList<JobDepartment> departments = EnsureDepartmentSetting(settingsPage, out needUpdateDepartments);
            //IList<JobLocation> locations = EnsureLocationSetting(settingsPage, out needUpdateLocations);
            IList<JobPosition> positions = EnsurePositionSetting(settingsPage, out needUpdatePositions);
            
            if (!needUpdatePositions && !needUpdateLocations && !needUpdateDepartments) return;

            var editableSettingsPage = settingsPage.CreateWritableClone() as SettingsPage;
            editableSettingsPage.JobDepartments = departments;
            editableSettingsPage.JobPositions = positions;
            //editableSettingsPage.JobLocations = locations;

            Save(editableSettingsPage);
        }
        private IList<JobPosition> EnsurePositionSetting(SettingsPage settingsPage, out bool needUpdate)
        {
            needUpdate = false;
            if (!settingsPage.JobPositions.IsNullOrEmpty()) return settingsPage.JobPositions;

            needUpdate = true;
            var result = new List<JobPosition>();
            var positions = new[] { "S.A", "PM", "Sale Manager", "Pre-sale", "Accountant", "PMO assitant" };
            foreach (var position in positions)
            {
                result.Add(new JobPosition()
                {
                    JobName = position
                });
            }
            return result;
        }

        private IList<JobLocation> EnsureLocationSetting(SettingsPage settingsPage, out bool needUpdate)
        {
            needUpdate = false;
            if (!settingsPage.JobLocations.IsNullOrEmpty()) return settingsPage.JobLocations;

            needUpdate = true;
            var result = new List<JobLocation>
            {
                new JobLocation("Paris", (new RegionInfo("fr-fr")).EnglishName),
                new JobLocation("Ha Noi", (new RegionInfo("vi-VN")).EnglishName),
                new JobLocation("Sai Gon", (new RegionInfo("vi-VN")).EnglishName),
                new JobLocation("NewYork", (new RegionInfo("en-US")).EnglishName),
                new JobLocation("California", (new RegionInfo("en-US")).EnglishName)
            };

            return result;
        }

        private IList<JobDepartment> EnsureDepartmentSetting(SettingsPage settingsPage, out bool needUpdate)
        {
            needUpdate = false;
            if (!settingsPage.JobDepartments.IsNullOrEmpty()) return settingsPage.JobDepartments;

            needUpdate = true;
            var result = new List<JobDepartment>();
            var departments = new[] { "Accountant", "Sale & Marketing", "Development", "PMO", "Developer" };
            foreach (var position in departments)
            {
                result.Add(new JobDepartment()
                {
                    DepartmentName = position
                });
            }
            return result;
        }

        private T InitBlock<T>(ContentAssetFolder folder, string name) where T : BlockData
        {
            var block = _contentRepository.GetDefault<T>(folder.ContentLink);
            ((IContent)block).Name = name;
            return block;
        }

        private T InitPage<T>(ContentReference parent, string title) where T : GenericContainerPage
        {
            var page = _contentRepository.GetDefault<T>(parent);
            page.PageName = title;
            page.Title = title;
            page.URLSegment = _urlSegmentCreator.Create(page);
            return page;
        }
        private ContentReference Save<T>(T page) where T : IContent
        {
            return _contentRepository.Save(page, SaveAction.Publish, AccessLevel.NoAccess);
        }

        private ContentReference GenerateDepartmentOverViewBlock(ContentReference containerPage, JobFilterBlock filterBlock)
        {
            var assertFolder = this._contentAssetHelper.GetOrCreateAssetFolder(containerPage);

            var overviewContainer = this.InitBlock<DepartmentOverviewContainerBlock>(assertFolder, "Our teams");
            overviewContainer.Title = overviewContainer.Title  = overviewContainer.Watermark = "Meet our teams";
            overviewContainer.FilterComponentAnchorId = filterBlock.AnchorId;

            var settingPage = EnsureSettingsPage();

            var containerReference = Save(((IContent)overviewContainer));

            overviewContainer.Departments = overviewContainer.Departments ?? new ContentArea();

            foreach(var department in settingPage.JobDepartments)
            {
                overviewContainer.Departments.Items.Add(new ContentAreaItem() { ContentLink = GenerateDepartmentOverviewItemBlock(containerReference, department) });
            }

            return Save(((IContent)overviewContainer));
        }

        private ContentReference GenerateDepartmentOverviewItemBlock(ContentReference departmentOverviewContainer, JobDepartment department)
        {
            var assertFolder = this._contentAssetHelper.GetOrCreateAssetFolder(departmentOverviewContainer);

            var overview = this.InitBlock<DepartmentOverviewBlock>(assertFolder, $"Department {department.DepartmentName}");
            overview.Title = $"Department {department.DepartmentName}";
            overview.Department = department.DepartmentName;
            overview.Description = new XhtmlString(@"Working side by side with growers and help them overcoming their challenges with my agronomic knowledge, it fulfills me");
            overview.Thumbnail = ((IContent)overview).CreateBlob(string.Format(DataDemoFolder, "thumbnail.jpg"));
            overview.Icon = ((IContent)overview).CreateBlob(string.Format(DataDemoFolder, "icon.png"));

            return Save(((IContent)overview));
        }
    }
}
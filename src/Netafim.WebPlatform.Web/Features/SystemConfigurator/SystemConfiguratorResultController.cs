using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DbLocalizationProvider;
using Dlw.EpiBase.Content.Cms;
using EPiServer;
using EPiServer.Core;
using EPiServer.Forms.Core;
using EPiServer.Forms.Core.Data;
using EPiServer.Forms.Core.Data.Internal;
using EPiServer.Forms.Core.Models;
using EPiServer.Web.Mvc;
using Netafim.WebPlatform.Web.Core.Exceptions;
using Netafim.WebPlatform.Web.Core.Extensions;
using Netafim.WebPlatform.Web.Features.SystemConfigurator.Repositories;
using Netafim.WebPlatform.Web.Features.SystemConfigurator.Services;
using Netafim.WebPlatform.Web.Features.SystemConfigurator.Services.Impl;
using Netafim.WebPlatform.Web.Features.SystemConfigurator.ViewModels;

namespace Netafim.WebPlatform.Web.Features.SystemConfigurator
{

    public class SystemConfiguratorResultController : PartialContentController<SystemConfiguratorResultBlock>
    {
        private readonly ISystemConfiguratorService _systemConfiguratorService;
        private readonly IUserContext _userContext;
        private readonly ISystemConfiguratorRepository _systemConfiguratorRepository;
        private readonly LocalizationProvider _localiaztionProvider;
        private readonly ISystemConfiguratorSettings _systemConfiguratorSettings;
        private readonly SubmissionStorageFactory _submissionStorageFactory;
        private readonly IFormRepository _formRepository;
        private readonly IContentRepository _contentRepository;
        private readonly IFormDataRepository _formDataRepository;

        public SystemConfiguratorResultController(ISystemConfiguratorService systemConfiguratorService,
            IUserContext userContext,
            ISystemConfiguratorRepository systemConfiguratorRepository,
            LocalizationProvider localizationProvider, 
            ISystemConfiguratorSettings systemConfiguratorSettings,
            SubmissionStorageFactory submissionStorageFactory, 
            IFormRepository formRepository, 
            IContentRepository contentRepository, 
            IFormDataRepository formDataRepository)
        {
            _systemConfiguratorService = systemConfiguratorService;
            _userContext = userContext;
            _systemConfiguratorRepository = systemConfiguratorRepository;
            _localiaztionProvider = localizationProvider;
            _systemConfiguratorSettings = systemConfiguratorSettings;
            _submissionStorageFactory = submissionStorageFactory;
            _formRepository = formRepository;
            _contentRepository = contentRepository;
            _formDataRepository = formDataRepository;
        }

        public override ActionResult Index(SystemConfiguratorResultBlock currentContent)
        {
            // flow
            // fetch all data from previous steps
            // from querystring
            // custom logic
            // let mvc bind to viewmodel
            // trigger configurator
            // render result

            // map data from querystring to systemConfiguratorData
            var queryContext = ActionContext.Create<ParametersActionContext>(Request.QueryString);

            UpdateLead(_systemConfiguratorSettings.LeadForm, new Guid(queryContext.SubmissionId), queryContext.ClientEmail);

            var systemConfiguratorData = new SystemConfiguratorData()
            {
                CropId = queryContext.Crop,
                FiltrationTypeId = queryContext.Filtration,
                MaxAllowedIrrigationTimePerDay = queryContext.MaxIrrigation,
                PlotArea = queryContext.PlotSize,
                RegionId = queryContext.Region,
                RowSpacing = queryContext.RowSpacing,
                WaterSourceId = queryContext.WaterSource,
                WeeklyIrrigationInterval = queryContext.IrrigationCycle
            };

            // Create setting
            var region = this._systemConfiguratorRepository.GetRegion(queryContext.Region);
            var crop = this._systemConfiguratorRepository.GetCrop(queryContext.Crop);
            var waterSource = this._systemConfiguratorRepository.GetWaterSource(queryContext.WaterSource);
            var filtration = this._systemConfiguratorRepository.GetFiltrationType(queryContext.Filtration);

            var generalGroup = new ParameterGroupSettingsViewModel(Translate(Labels.GeneralGroup))
            {
                new ParameterSettingViewModel() { Name = Translate(Labels.RegionTitle), Value = region != null ? region.Name : string.Empty },
                new ParameterSettingViewModel() { Name = Translate(Labels.CropTitle), Value = crop != null ? crop.Name : string.Empty }
            };

            var plotSizeGroup = new ParameterGroupSettingsViewModel(Translate(Labels.PlotGroup))
            {
                new ParameterSettingViewModel() { Name = Translate(Labels.PlotSizeTitle), Value = queryContext.PlotSize.ToString() },
                new ParameterSettingViewModel() { Name = Translate(Labels.RowSpacingTitle), Value = queryContext.RowSpacing.ToString() }
            };

            var irrigationGroup = new ParameterGroupSettingsViewModel(Translate(Labels.IrrigationGroup))
            {
                new ParameterSettingViewModel() { Name = Translate(Labels.WaterSourceTitle), Value = waterSource != null ? waterSource.Name : string.Empty },
                new ParameterSettingViewModel() { Name = Translate(Labels.MaxIrrigationTitle), Value = queryContext.MaxIrrigation.ToString() },
                new ParameterSettingViewModel() { Name = Translate(Labels.IrrigationCycleTitle), Value = queryContext.IrrigationCycle.ToString() },
                new ParameterSettingViewModel() { Name = Translate(Labels.FiltrationTitle), Value = filtration != null ? filtration.Name : string.Empty }
            };

            var settings = new[] { generalGroup, plotSizeGroup, irrigationGroup };

            // map result to viewmodel
            // Product -> ProductViewModel
            // Dealer -> DealerViewModel
            // load dealer through contentreference, fetch info from page and push to viewmodel
            // Contact -> ContactViewModel
            // Hotspot = from solution page
            // Tip = from solution page

            var viewModel = new SystemConfiguratorResultViewModel()
            {
                //Name = name from querystring
                //Result = result view model
                // no action url or null
                Settings = settings.ToList(),
                ClientName = queryContext.ClientName,
                ClientEmail = queryContext.ClientEmail,
                SubmissionId = queryContext.SubmissionId
            };

            try
            {
                viewModel.Result = _systemConfiguratorService.Process(systemConfiguratorData, _userContext.CurrentLanguage);
            }
            catch (BusinessException e)
            {
                viewModel.Messages.Add(Translate(e.Code));
            }

            return PartialView(currentContent.GetDefaultFullViewName(), viewModel);
        }

        private void UpdateLead(ContentReference leadFormReference, Guid submissionId, string email)
        {
            var leadForm = _contentRepository.Get<EPiServer.Forms.Implementation.Elements.FormContainerBlock>(leadFormReference, _userContext.CurrentLanguage);
            var leadFormIdentity = new FormIdentity(leadForm.Form.FormGuid, _userContext.CurrentLanguage.ToString());
            var friendlyInfo = _formRepository.GetDataFriendlyNameInfos(leadFormIdentity).ToList();

            if (!friendlyInfo[0].FriendlyName.ToLowerInvariant().Equals("name")) throw new Exception($"Field 'name' is missing from the configured lead form with ID {leadFormReference.ID}.");
            if (!friendlyInfo[1].FriendlyName.ToLowerInvariant().Equals("email")) throw new Exception($"Field 'email' is missing from the configured lead form with ID {leadFormReference.ID}.");
            if (!friendlyInfo[2].FriendlyName.ToLowerInvariant().Equals("url")) throw new Exception($"Field 'url' is missing from the configured lead form with ID {leadFormReference.ID}.");

            //look up existing submission and only continue the update if e-mail address matches with the e-mail of the querystring.
            var existingSubmission = _formDataRepository.GetSubmissionData(leadFormIdentity, new []{submissionId.ToString()}).First();
            object existingEmail;
            existingSubmission.Data.TryGetValue(friendlyInfo[1].ElementId, out existingEmail);

            if(existingEmail == null) throw new Exception($"E-mail field is not present for submission with ID {submissionId}");
            if(!existingEmail.ToString().ToLowerInvariant().Equals(email.ToLowerInvariant())) throw new Exception($"Unauthorized lead update. E-mail address ${email} does not match with existing ${existingEmail}.");

            Dictionary<string, object> leadSubmissionData = new Dictionary<string, object>
            {
                {friendlyInfo[2].ElementId, Request.Url},
            };

            var leadSubmission = new Submission
            {
                Data = leadSubmissionData
            };

            var submissionStorage = _submissionStorageFactory.GetStorage(leadFormIdentity);
            submissionStorage.UpdateToStorage(submissionId, leadFormIdentity, leadSubmission);
        }

        private string Translate(string key) => this._localiaztionProvider.GetString(key, _userContext.CurrentLanguage);
    }
}
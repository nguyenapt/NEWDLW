using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Dlw.EpiBase.Content.Cms;
using EPiServer;
using EPiServer.Core;
using EPiServer.Forms.Core;
using EPiServer.Forms.Core.Data.Internal;
using EPiServer.Forms.Core.Models;
using Netafim.WebPlatform.Web.Core.Extensions;
using Netafim.WebPlatform.Web.Features.SystemConfigurator.Repositories;
using Netafim.WebPlatform.Web.Features.SystemConfigurator.ViewModels;
using LocalizationProvider = DbLocalizationProvider.LocalizationProvider;

namespace Netafim.WebPlatform.Web.Features.SystemConfigurator
{

    public class SystemConfiguratorParametersController : PipelineBlockBaseController<SystemConfiguratorParametersBlock, ParametersActionContext>
    {
        private readonly ISystemConfiguratorRepository _configuratorRepository;
        private readonly IUserContext _userContext;
        private readonly LocalizationProvider _localizationProvider;
        private readonly SubmissionStorageFactory _submissionStorageFactory;
        private readonly IFormRepository _formRepository;
        private readonly IContentRepository _contentRepository;
        private readonly ISystemConfiguratorSettings _systemConfiguratorSettings;


        public SystemConfiguratorParametersController(IContentLoader contentLoader,
            IEnumerable<IActionHandler> actionHandlers,
            ISystemConfiguratorRepository configuratorRepository,
            IUserContext userContext,
            LocalizationProvider localizationProvider,
            SubmissionStorageFactory submissionStorageFactory,
            IFormRepository formRepository,
            IContentRepository contentRepository, 
            ISystemConfiguratorSettings systemConfiguratorSettings)
            : base(contentLoader, actionHandlers)
        {
            _configuratorRepository = configuratorRepository;
            _userContext = userContext;
            _localizationProvider = localizationProvider;
            _submissionStorageFactory = submissionStorageFactory;
            _formRepository = formRepository;
            _contentRepository = contentRepository;
            _systemConfiguratorSettings = systemConfiguratorSettings;
        }

        public override ActionResult Index(SystemConfiguratorParametersBlock currentContent)
        {
            // flow
            // load all data for parameters
            // return view to render form with fixed fields
            // viewmodel with fixed set op properties (parameters) have validation attributes
            // push data to querystring
            var startActionContext = ActionContext.Create<StarterActionContext>(Request.QueryString);

            var submissionId = CreateLead(_systemConfiguratorSettings.LeadForm, startActionContext.ClientName, startActionContext.ClientEmail);

            // General
            ParameterGroupViewModel groupsGeneral = CreateGeneralParameter();

            // Plot
            ParameterGroupViewModel plotGroup = CreatePlotParameter();

            // Irrigation
            ParameterGroupViewModel irrigationGroup = CreateIrrigationParameter();

            var viewModel = new SystemConfiguratorParametersViewModel(currentContent)
            {
                ParameterGroups = new[] { groupsGeneral, plotGroup, irrigationGroup },
                ActionUrl = $"/{_userContext.CurrentLanguage}{Url.Action(nameof(TakeAction))}",
                ClientName = startActionContext.ClientName,
                ClientEmail = startActionContext.ClientEmail,
                SubmissionId = submissionId
            };

            return PartialView(currentContent.GetDefaultFullViewName(), viewModel);
        }

        private ParameterGroupViewModel CreateGeneralParameter()
        {
            var regions = _configuratorRepository.GetAllRegions(_userContext.CurrentLanguage);
            var crops = _configuratorRepository.GetAllCrops(_userContext.CurrentLanguage);

            regions = regions != null && regions.Any() ? regions : Enumerable.Empty<Domain.Region>();

            var regionParameters = new ParameterViewModel(regions.Select(r => new ConfiguratorParameterViewModel() { Id = r.Id, Name = r.Name }))
            {
                Title = _localizationProvider.GetString(() => Labels.RegionTitle) + "*",
                Tooltip = _localizationProvider.GetString(() => Labels.RegionToolTip),
                ValidationError = _localizationProvider.GetString(() => Labels.RegionValidationError),
                ViewMode = ViewMode.Dropdown,
                Name = nameof(ParametersActionContext.Region)
            };

            crops = crops != null && crops.Any() ? crops : Enumerable.Empty<Domain.Crop>();

            var cropParameters = new ParameterViewModel(crops.Select(c => new ConfiguratorParameterViewModel() { Id = c.Id, Name = c.Name }))
            {
                Title = _localizationProvider.GetString(() => Labels.CropTitle) + "*",
                Tooltip = _localizationProvider.GetString(() => Labels.CropTooltip),
                ValidationError = _localizationProvider.GetString(() => Labels.CropValidationError),
                ViewMode = ViewMode.Dropdown,
                Name = nameof(ParametersActionContext.Crop)
            };

            return new ParameterGroupViewModel
            {
                Parameters = new[] { regionParameters, cropParameters },
                Title = _localizationProvider.GetString(() => Labels.GeneralGroup)
            };
        }

        private ParameterGroupViewModel CreatePlotParameter()
        {
            var plotSizesParameters = new ParameterViewModel(new[] { new NumbericParameterViewModel() { Id = 0, Name = _localizationProvider.GetString(Labels.PlotSizeUnit, _userContext.CurrentLanguage), Max = 10000, Min = 1, Step = 1 } })
            {
                Title = _localizationProvider.GetString(() => Labels.PlotSizeTitle),
                Tooltip = _localizationProvider.GetString(() => Labels.PlotSizeTooltip),
                ValidationError = _localizationProvider.GetString(() => Labels.PlotSizeValidationError),
                Name = nameof(ParametersActionContext.PlotSize),
                ViewMode = ViewMode.Numberic
            };

            var rowSpacingParameters = new ParameterViewModel(new[] { new NumbericParameterViewModel() { Id = 0, Name = _localizationProvider.GetString(Labels.RowSpacingUnit, _userContext.CurrentLanguage), Max = 10000, Min = 1, Step = 1 } })
            {
                Title = _localizationProvider.GetString(() => Labels.RowSpacingTitle) + "*",
                Tooltip = _localizationProvider.GetString(() => Labels.RowSpacingTooltip),
                ValidationError = _localizationProvider.GetString(() => Labels.RowSpacingValidationError),
                Name = nameof(ParametersActionContext.RowSpacing),
                ViewMode = ViewMode.Numberic
            };

            var plotGroup = new ParameterGroupViewModel()
            {
                Title = _localizationProvider.GetString(() => Labels.PlotGroup),
                Parameters = new List<ParameterViewModel>() { plotSizesParameters, rowSpacingParameters }
            };
            return plotGroup;
        }

        private ParameterGroupViewModel CreateIrrigationParameter()
        {
            var filtrationTypes = _configuratorRepository.GetAllFiltrationTypes(_userContext.CurrentLanguage);
            var waterSources = _configuratorRepository.GetAllWaterSources(_userContext.CurrentLanguage);

            waterSources = waterSources != null && waterSources.Any() ? waterSources : Enumerable.Empty<Domain.WaterSource>();

            var waterSourceParameter = new ParameterViewModel(waterSources.Select(w => new ConfiguratorParameterViewModel() { Id = w.Id, Name = w.Name }))
            {
                Title = _localizationProvider.GetString(() => Labels.WaterSourceTitle) + "*",
                Tooltip = _localizationProvider.GetString(() => Labels.WaterSourceTooltip),
                ValidationError = _localizationProvider.GetString(() => Labels.WaterSourceValidationError),
                ViewMode = ViewMode.Selection,
                Name = nameof(ParametersActionContext.WaterSource)
            };

            var maxIrrigation = new ParameterViewModel(new[] { new NumbericParameterViewModel() { Id = 1, Name = _localizationProvider.GetString(Labels.MaxIrrigationUnit, _userContext.CurrentLanguage), Max = 24, Min = 1, Step = 1 } })
            {
                Title = _localizationProvider.GetString(() => Labels.MaxIrrigationTitle) + "*",
                Tooltip = _localizationProvider.GetString(() => Labels.MaxIrrigationTooltip),
                ValidationError = _localizationProvider.GetString(() => Labels.MaxIrrigationValidationError),
                ViewMode = ViewMode.Numberic,
                Name = nameof(ParametersActionContext.MaxIrrigation)
            };

            var irrigationCycle = new ParameterViewModel(new[] { new NumbericParameterViewModel() { Id = 1, Name = _localizationProvider.GetString(Labels.IrrigationCycleUnit, _userContext.CurrentLanguage), Max = 7, Min = 1, Step = 1 } })
            {
                Title = _localizationProvider.GetString(() => Labels.IrrigationCycleTitle) + "*",
                Tooltip = _localizationProvider.GetString(() => Labels.IrrigationCycleTooltip),
                ValidationError = _localizationProvider.GetString(() => Labels.IrrigationCycleValidationError),
                ViewMode = ViewMode.Numberic,
                Name = nameof(ParametersActionContext.IrrigationCycle)
            };

            filtrationTypes = filtrationTypes != null && filtrationTypes.Any() ? filtrationTypes : Enumerable.Empty<Domain.FiltrationType>();

            var filtration = new ParameterViewModel(filtrationTypes.Select(f => new ConfiguratorParameterViewModel() { Id = f.Id, Name = f.Name }))
            {
                Title = _localizationProvider.GetString(() => Labels.FiltrationTitle) + "*",
                Tooltip = _localizationProvider.GetString(() => Labels.FiltrationTooltip),
                ValidationError = _localizationProvider.GetString(() => Labels.FiltrationValidationError),
                ViewMode = ViewMode.Selection,
                Name = nameof(ParametersActionContext.Filtration)
            };

            var irrigationGroup = new ParameterGroupViewModel()
            {
                Title = _localizationProvider.GetString(() => Labels.IrrigationGroup),
                Parameters = new List<ParameterViewModel>() { waterSourceParameter, maxIrrigation, irrigationCycle, filtration }
            };
            return irrigationGroup;
        }

        private string CreateLead(ContentReference leadFormReference, string name, string email)
        {
            var leadForm = _contentRepository.Get<EPiServer.Forms.Implementation.Elements.FormContainerBlock>(leadFormReference, _userContext.CurrentLanguage);
            var leadFormIdentity = new FormIdentity(leadForm.Form.FormGuid, _userContext.CurrentLanguage.ToString());
            var friendlyInfo = _formRepository.GetDataFriendlyNameInfos(leadFormIdentity).ToList();

            if (!friendlyInfo[0].FriendlyName.ToLowerInvariant().Equals("name")) throw new Exception($"Field 'name' is missing from the configured lead form with ID {leadFormReference.ID}.");
            if (!friendlyInfo[1].FriendlyName.ToLowerInvariant().Equals("email")) throw new Exception($"Field 'email' is missing from the configured lead form with ID {leadFormReference.ID}.");
            if (!friendlyInfo[2].FriendlyName.ToLowerInvariant().Equals("url")) throw new Exception($"Field 'url' is missing from the configured lead form with ID {leadFormReference.ID}.");

            Dictionary<string, object> leadSubmissionData = new Dictionary<string, object>
                {
                    {friendlyInfo[0].ElementId, name},
                    {friendlyInfo[1].ElementId, email},
                    {friendlyInfo[2].ElementId, string.Empty},
                    {"SYSTEMCOLUMN_Language", _userContext.CurrentLanguage.ToString() },
                    {"SYSTEMCOLUMN_HostedPage", leadFormReference.ID},
                    {"SYSTEMCOLUMN_SubmitTime", DateTime.Now},
                    {"SYSTEMCOLUMN_SubmitUser", "admin"},
                    {"SYSTEMCOLUMN_FinalizedSubmission", "True"}
                };

            var leadSubmission = new Submission
            {
                Data = leadSubmissionData
            };

            var submissionStorage = _submissionStorageFactory.GetStorage(leadFormIdentity);
            var submissionId = submissionStorage.SaveToStorage(leadFormIdentity, leadSubmission).ToString();

            return submissionId;
        }
    }
}

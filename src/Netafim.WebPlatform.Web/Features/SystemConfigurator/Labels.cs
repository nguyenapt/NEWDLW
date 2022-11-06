using DbLocalizationProvider;

namespace Netafim.WebPlatform.Web.Features.SystemConfigurator
{
    [LocalizedResource]
    public class Labels
    {
        public static string Name => "Name";

        public static string Email => "E-mail address";

        public static string NamePlaceHolder => "Your name";

        public static string EmailPlaceHolder => "example@domain.com";

        public static string NameValidationError => "Invalid name";

        public static string EmailValidationError => "Invalid e-mail address";

        public static string LinkText => "GET STARTED";

        public static string Tooltip => "We will send our proposed irrigation solution to your e-mail address so you can keep it.";

        public static string LinkViewExample => "VIEW EXAMPLE";

        public static string GenerateLink => "GENERATE";

        public static string NextStep => "NEXT STEP";

        public static string PrevStep => "PREVIOUS STEP";
        
        public static string RegionTitle => "Regions";

        public static string RegionToolTip => "Region tooltips";

        public static string RegionValidationError => "You must to select region";

        public static string CropTitle => "Crops";

        public static string CropTooltip => "Crop tooltips";

        public static string CropValidationError => "You must to select crop";

        public static string GeneralGroup => "GENERAL";

        public static string ValidationInputMessage => "Please verify the input parameters";


        // Plot
        public static string PlotSizeTitle => "Plot size";

        public static string PlotSizeTooltip => "Plot size tooltips";

        public static string PlotSizeValidationError => "Please input valid plot size";

        public static string PlotSizeUnit => "Hectars";

        // row spacing
        public static string RowSpacingTitle => "Row spacing";

        public static string RowSpacingTooltip => "Row spacing tooltips";

        public static string RowSpacingValidationError => "Please input valid row spacing";

        public static string RowSpacingUnit => "Rows / meter";

        public static string PlotGroup => "PLOT";

        // Irrigation group

        public static string WaterSourceTitle => "Water source";

        public static string WaterSourceTooltip => "Water source tooltips";

        public static string WaterSourceValidationError => "Select Water source";

        public static string WaterDistanceTitle => "Water resource distance*";

        public static string WaterDistanceTooltip => "Water distance tooltips";

        public static string WaterDistanceValidationError => "Please enter invalid water distance";

        public static string WaterDistanceUnit => "Meters";

        // Max irrigation
        public static string MaxIrrigationTitle => "Max irrigation time per day";

        public static string MaxIrrigationTooltip => "Max irrigation time per day";

        public static string MaxIrrigationValidationError => "Please enter valid Max irrigation time per day";

        public static string MaxIrrigationUnit => "Hours / day";

        // Irrigation
        public static string IrrigationCycleTitle => "Irrigation cycles per week";

        public static string IrrigationCycleTooltip => "Irrigation cycles per week tooltip";

        public static string IrrigationCycleValidationError => "Please enter valid Irrigation cycles per week";

        public static string IrrigationCycleUnit => "Cycles / week";

        public static string IrrigationBackground => "Irrigation Solution";

        public static string IrrigationVerticalText => "Irrigation Solution";

        // Filtration
        public static string FiltrationTitle => "Filtration";

        public static string FiltrationTooltip => "Filtration tooltip";

        public static string FiltrationValidationError => "Please enter valid Filtration";

        public static string IrrigationGroup => "IRRIGATION";

        // Result page
        public static string ChangeSettings => "CHANGE SETTINGS";

        public static string ClientSettingFormat => "{0}'s settings";

        public static string RequestMoreInfo => "Request more info";

        public static string DownLoad => "Download";

        public static string Share => "Share";

        public static string Print => "Print";

        public static string SolutionUserSummaryFormat => "The irrigation solutions for {0}'s almond orchard";

        public static string ProductMoreInfo => "More info";

        public static string FeatureAndBenifit => "Features &amp; benefits";

        public static string SocialSharing => "Social Sharing";

        public static string ContactUs => "Contact us";

        public static string FindMoreDealer => "FIND MORE DEALERS";

        public static string FlowRate => "Flow rate";

        public static string ResultPageBackground => "Your settings";

        public static string ResultPageVerticalText => "Your settings";

        // Dealer
        public static string DealerBackground => "Request an offer";

        public static string DealerVerticalText => "Request an offer";

        // Contact
        public static string ContactBackground => "Netafim contacts";

        public static string ContactVerticalText => "Your settings";
    }
}
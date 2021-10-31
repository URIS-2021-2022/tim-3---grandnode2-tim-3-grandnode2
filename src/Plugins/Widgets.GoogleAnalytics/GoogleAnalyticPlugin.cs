using Grand.Business.Common.Extensions;
using Grand.Business.Common.Interfaces.Configuration;
using Grand.Business.Common.Interfaces.Localization;
using Grand.Infrastructure.Plugins;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Widgets.GoogleAnalytics
{
    /// <summary>
    /// Live person provider
    /// </summary>
    public class GoogleAnalyticPlugin : BasePlugin, IPlugin
    {
        private readonly ITranslationService _translationService;
        private readonly ILanguageService _languageService;
        private readonly ISettingService _settingService;

        public GoogleAnalyticPlugin(ITranslationService translationService, ILanguageService languageService, ISettingService settingService)
        {
            _translationService = translationService;
            _settingService = settingService;
            _languageService = languageService;
        }

        /// <summary>
        /// Gets a configuration page URL
        /// </summary>
        public override string ConfigurationUrl()
        {
            return GoogleAnalyticDefaults.ConfigurationUrl;
        }

        /// <summary>
        /// Gets widget zones where this widget should be rendered
        /// </summary>
        /// <returns>Widget zones</returns>
        public IList<string> GetWidgetZones()
        {
            return new List<string>
            {
                "body_end_html_tag_before", "clean_body_end_html_tag_before"
            };
        }

        /// <summary>
        /// Install plugin
        /// </summary>
        public override async Task Install()
        {
            var settings = new GoogleAnalyticsEcommerceSettings
            {
                GoogleId = "UA-0000000-0",
                TrackingScript = @"<!-- Google code for Analytics tracking -->
                    <script>
                    var _gaq = _gaq || [];
                    _gaq.push(['_setAccount', '{GOOGLEID}']);
                    _gaq.push(['_trackPageview']);
                    {ECOMMERCE}
                    (function() {
                        var ga = document.createElement('script'); ga.type = 'text/javascript'; ga.async = true;
                        ga.src = ('https:' == document.location.protocol ? 'https://ssl' : 'http://www') + '.google-analytics.com/ga.js';
                        var s = document.getElementsByTagName('script')[0]; s.parentNode.insertBefore(ga, s);
                    })();
                    </script>",
                EcommerceScript = @"_gaq.push(['_addTrans', '{ORDERID}', '{SITE}', '{TOTAL}', '{TAX}', '{SHIP}', '{CITY}', '{STATEPROVINCE}', '{COUNTRY}']);
                    {DETAILS} 
                    _gaq.push(['_trackTrans']); ",
                EcommerceDetailScript = @"_gaq.push(['_addItem', '{ORDERID}', '{PRODUCTSKU}', '{PRODUCTNAME}', '{CATEGORYNAME}', '{UNITPRICE}', '{QUANTITY}' ]); ",
                ConsentName = "Google Analytics",
                ConsentDescription = "Allows us to analyse the statistics of visits to our website."
            };
            await _settingService.SaveSetting(settings);

            await this.AddOrUpdatePluginTranslateResource(_translationService, _languageService, "Widgets.GoogleAnalytics.FriendlyName", "Google Analytics");
            await this.AddOrUpdatePluginTranslateResource(_translationService, _languageService, "Widgets.GoogleAnalytics.GoogleId", "ID");
            await this.AddOrUpdatePluginTranslateResource(_translationService, _languageService, "Widgets.GoogleAnalytics.GoogleId.Hint", "Enter Google Analytics ID.");
            await this.AddOrUpdatePluginTranslateResource(_translationService, _languageService, "Widgets.GoogleAnalytics.TrackingScript", "Tracking code with {ECOMMERCE} line");
            await this.AddOrUpdatePluginTranslateResource(_translationService, _languageService, "Widgets.GoogleAnalytics.TrackingScript.Hint", "Paste the tracking code generated by Google Analytics here. {GOOGLEID} and {ECOMMERCE} will be dynamically replaced.");
            await this.AddOrUpdatePluginTranslateResource(_translationService, _languageService, "Widgets.GoogleAnalytics.EcommerceScript", "Tracking code for {ECOMMERCE} part, with {DETAILS} line");
            await this.AddOrUpdatePluginTranslateResource(_translationService, _languageService, "Widgets.GoogleAnalytics.EcommerceScript.Hint", "Paste the tracking code generated by Google analytics here. {ORDERID}, {SITE}, {TOTAL}, {TAX}, {SHIP}, {CITY}, {STATEPROVINCE}, {COUNTRY}, {DETAILS} will be dynamically replaced.");
            await this.AddOrUpdatePluginTranslateResource(_translationService, _languageService, "Widgets.GoogleAnalytics.EcommerceDetailScript", "Tracking code for {DETAILS} part");
            await this.AddOrUpdatePluginTranslateResource(_translationService, _languageService, "Widgets.GoogleAnalytics.EcommerceDetailScript.Hint", "Paste the tracking code generated by Google analytics here. {ORDERID}, {PRODUCTSKU}, {PRODUCTNAME}, {CATEGORYNAME}, {UNITPRICE}, {QUANTITY} will be dynamically replaced.");
            await this.AddOrUpdatePluginTranslateResource(_translationService, _languageService, "Widgets.GoogleAnalytics.IncludingTax", "Include tax");
            await this.AddOrUpdatePluginTranslateResource(_translationService, _languageService, "Widgets.GoogleAnalytics.IncludingTax.Hint", "Check to include tax when generating tracking code for {ECOMMERCE} part.");
            await this.AddOrUpdatePluginTranslateResource(_translationService, _languageService, "Widgets.GoogleAnalytics.AllowToDisableConsentCookie", "Allow disabling consent cookie");
            await this.AddOrUpdatePluginTranslateResource(_translationService, _languageService, "Widgets.GoogleAnalytics.AllowToDisableConsentCookie.Hint", "Get or set the value to disable consent cookie");
            await this.AddOrUpdatePluginTranslateResource(_translationService, _languageService, "Widgets.GoogleAnalytics.ConsentDefaultState", "Consent default state");
            await this.AddOrUpdatePluginTranslateResource(_translationService, _languageService, "Widgets.GoogleAnalytics.ConsentDefaultState.Hint", "Get or set the value to consent default state");
            await this.AddOrUpdatePluginTranslateResource(_translationService, _languageService, "Widgets.GoogleAnalytics.ConsentName", "Consent cookie name");
            await this.AddOrUpdatePluginTranslateResource(_translationService, _languageService, "Widgets.GoogleAnalytics.ConsentName.Hint", "Get or set the value to consent cookie name");
            await this.AddOrUpdatePluginTranslateResource(_translationService, _languageService, "Widgets.GoogleAnalytics.ConsentDescription", "Consent cookie description");
            await this.AddOrUpdatePluginTranslateResource(_translationService, _languageService, "Widgets.GoogleAnalytics.ConsentDescription.Hint", "Get or set the value to consent cookie description");

            await base.Install();
        }

        /// <summary>
        /// Uninstall plugin
        /// </summary>
        public override async Task Uninstall()
        {
            //settings
            await _settingService.DeleteSetting<GoogleAnalyticsEcommerceSettings>();

            //locales
            await this.DeletePluginTranslationResource(_translationService, _languageService, "Widgets.GoogleAnalytics.GoogleId");
            await this.DeletePluginTranslationResource(_translationService, _languageService, "Widgets.GoogleAnalytics.GoogleId.Hint");
            await this.DeletePluginTranslationResource(_translationService, _languageService, "Widgets.GoogleAnalytics.TrackingScript");
            await this.DeletePluginTranslationResource(_translationService, _languageService, "Widgets.GoogleAnalytics.TrackingScript.Hint");
            await this.DeletePluginTranslationResource(_translationService, _languageService, "Widgets.GoogleAnalytics.EcommerceScript");
            await this.DeletePluginTranslationResource(_translationService, _languageService, "Widgets.GoogleAnalytics.EcommerceScript.Hint");
            await this.DeletePluginTranslationResource(_translationService, _languageService, "Widgets.GoogleAnalytics.EcommerceDetailScript");
            await this.DeletePluginTranslationResource(_translationService, _languageService, "Widgets.GoogleAnalytics.EcommerceDetailScript.Hint");
            await this.DeletePluginTranslationResource(_translationService, _languageService, "Widgets.GoogleAnalytics.IncludingTax");
            await this.DeletePluginTranslationResource(_translationService, _languageService, "Widgets.GoogleAnalytics.IncludingTax.Hint");
            await this.DeletePluginTranslationResource(_translationService, _languageService, "Widgets.GoogleAnalytics.AllowToDisableConsentCookie");
            await this.DeletePluginTranslationResource(_translationService, _languageService, "Widgets.GoogleAnalytics.AllowToDisableConsentCookie.Hint");
            await this.DeletePluginTranslationResource(_translationService, _languageService, "Widgets.GoogleAnalytics.ConsentDefaultState");
            await this.DeletePluginTranslationResource(_translationService, _languageService, "Widgets.GoogleAnalytics.ConsentDefaultState.Hint");
            await this.DeletePluginTranslationResource(_translationService, _languageService, "Widgets.GoogleAnalytics.ConsentName");
            await this.DeletePluginTranslationResource(_translationService, _languageService, "Widgets.GoogleAnalytics.ConsentName.Hint");
            await this.DeletePluginTranslationResource(_translationService, _languageService, "Widgets.GoogleAnalytics.ConsentDescription");
            await this.DeletePluginTranslationResource(_translationService, _languageService, "Widgets.GoogleAnalytics.ConsentDescription.Hint");

            await base.Uninstall();
        }

    }
}

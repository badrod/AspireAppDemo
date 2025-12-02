using Microsoft.FeatureManagement;
using WebApi.Models;

namespace WebApi.Services
{
    public class ToggleAIService(OpenAIService openAI, GoogleAIService google, IFeatureManager toggle) : IAIService
    {

        private readonly OpenAIService _openAI = openAI;
        private readonly GoogleAIService _google = google;
        private readonly IFeatureManager _toggle = toggle;

        public async Task<Invoice> ExtractInvoice(InvoiceFile input)
        {
            if (await _toggle.IsEnabledAsync("UseGoogleAI"))
            {
                return await _google.ExtractInvoice(input);
            }
            else//default to OpenAI
            {
                return await _openAI.ExtractInvoice(input);
            }
        }
    }

}

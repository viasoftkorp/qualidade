using System.Threading.Tasks;
using Viasoft.Core.Reporting.Model;
using Viasoft.Core.Reporting.Store;
using Viasoft.Data.Seeder.Abstractions;
using Viasoft.Qualidade.RNC.Gateway.Host.Extensions;
using Viasoft.Qualidade.RNC.Gateway.Host.Relatorios;

namespace Viasoft.Qualidade.RNC.Gateway.Host.Seeders.Relatorios
{
    public class CriarRelatorioNaoConformidadeSeeder : ISeedData
    {
        private readonly IReportingStore _reportingStore;

        public CriarRelatorioNaoConformidadeSeeder(IReportingStore reportingStore)
        {
            _reportingStore = reportingStore;
        }

        public async Task SeedDataAsync()
        {
            var fileTemplate = GetFileByteArray();

            var reportDefaultCreateOrUpdateInput = new ReportDefaultCreateOrUpdateInput
            {
                ReportId = RelatorioPadraoConsts.ReportId,
                ReportingEngine = ReportingEngines.Stimulsoft,
                Domain = ReportDomains.QualityAssurance,
                Template = fileTemplate,
                ReportingType = ReportingType.Report,
                Extension = "pdf",
                Area = "Qualidade",
                AppId = "QUAL06_W",
                Description = "Relatório de impressão de uma não conformidade"
            };

            await _reportingStore.CreateOrUpdateDefaultAsync(reportDefaultCreateOrUpdateInput);
        }

        private byte[] GetFileByteArray()
        {
            var resourseName = "Viasoft.Qualidade.RNC.Gateway.Host.Relatorios.RelatorioPadraoNaoConformidade.mrt"; // nome do arquivo do relatorio

            byte[] file;
            using (var fileStream = GetType().Assembly.GetManifestResourceStream(resourseName))
            {
                file = fileStream?.ReadAllBytes();
            }

            return file;
        }
    }
}
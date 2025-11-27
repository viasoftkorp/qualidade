namespace Viasoft.Qualidade.RNC.Gateway.Domain.Authorizations.Policies;

public class Policies 
{
    // Nao Conformidades
    public const string CreateNaoConformidade = "NaoConformidades.Create";
    public const string ReadNaoConformidade = "NaoConformidades.Read";
    public const string UpdateNaoConformidade = "NaoConformidades.Update";
    public const string DeleteNaoConformidade = "NaoConformidades.Delete";
    public const string ConcluirNaoConformidade = "NaoConformidades.Concluir";
    public const string EstornarConclusaoNaoConformidade = "NaoConformidades.EstornarConclusao";
    
    // Natureza
    public const string CreateNatureza = "Settings.Naturezas.Create";
    public const string ReadNatureza = "Settings.Naturezas.Read";
    public const string UpdateNatureza = "Settings.Naturezas.Update";
    public const string DeleteNatureza = "Settings.Naturezas.Delete";
    
    // Causa
    public const string CreateCausa = "Settings.Causas.Create";
    public const string ReadCausa = "Settings.Causas.Read";
    public const string UpdateCausa = "Settings.Causas.Update";
    public const string DeleteCausa = "Settings.Causas.Delete";

    // Solucao
    public const string CreateSolucao = "Settings.Solucoes.Create";
    public const string ReadSolucao = "Settings.Solucoes.Read";
    public const string UpdateSolucao = "Settings.Solucoes.Update";
    public const string DeleteSolucao = "Settings.Solucoes.Delete";

    // Defeito
    public const string CreateDefeito = "Settings.Defeitos.Create";
    public const string ReadDefeito = "Settings.Defeitos.Read";
    public const string UpdateDefeito = "Settings.Defeitos.Update";
    public const string DeleteDefeito = "Settings.Defeitos.Delete";

    // Acao Preventiva
    public const string CreateAcaoPreventiva = "Settings.AcoesPreventivas.Create";
    public const string ReadAcaoPreventiva = "Settings.AcoesPreventivas.Read";
    public const string UpdateAcaoPreventiva = "Settings.AcoesPreventivas.Update";
    public const string DeleteAcaoPreventiva = "Settings.AcoesPreventivas.Delete";
    
    //Configuracoes Gerais
    public const string AtualizarConfiguracoesGerais = "Settings.Geral.Update";
}
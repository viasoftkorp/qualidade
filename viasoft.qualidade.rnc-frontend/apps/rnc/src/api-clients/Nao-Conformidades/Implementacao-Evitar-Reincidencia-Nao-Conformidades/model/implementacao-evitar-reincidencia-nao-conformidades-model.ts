export class ImplementacaoEvitarReincidenciaNaoConformidadesModel {
  public id: string;
  public idNaoConformidade: string;
  public idDefeitoNaoConformidade: string;
  public descricao: string;
  public idResponsavel: string;
  public dataAnalise: Date;
  public dataPrevistaImplantacao: Date;
  public idAuditor: string;
  public dataVerificacao: Date;
  public novaData: Date;
  public acaoImplementada: boolean;
}

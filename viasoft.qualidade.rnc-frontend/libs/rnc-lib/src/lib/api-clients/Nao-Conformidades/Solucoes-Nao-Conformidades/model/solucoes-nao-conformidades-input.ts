export interface SolucoesNaoConformidadesInput{
  id: string
  idNaoConformidade: string
  idDefeitoNaoConformidade: string
  idSolucao: string
  detalhamento: string
  solucaoImediata: boolean
  dataAnalise: Date
  dataPrevistaImplantacao: Date
  idResponsavel: string
  custoEstimado: number
  novaData: Date
  dataVerificacao: Date
  idAuditor: string
}

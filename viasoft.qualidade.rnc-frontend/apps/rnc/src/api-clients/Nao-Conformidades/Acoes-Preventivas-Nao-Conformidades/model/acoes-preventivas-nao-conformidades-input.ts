export interface AcoesPreventivasNaoConformidadesInput {
    id: string
    idNaoConformidade: string
    idDefeitoNaoConformidade: string
    idAcaoPreventiva: string
    acao: string
    detalhamento: string
    idResponsavel: string
    dataAnalise: Date
    dataPrevistaImplantacao: Date
    idAuditor: string
    implementada: boolean
    dataVerificacao: Date
    novaData: Date
}

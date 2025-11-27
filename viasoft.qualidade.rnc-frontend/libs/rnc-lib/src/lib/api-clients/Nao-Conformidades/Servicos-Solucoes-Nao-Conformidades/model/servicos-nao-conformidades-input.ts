export interface ServicosNaoConformidadesInput{
    idNaoConformidade: string
    id: string
    idProduto?: string
    idRecurso?: string
    operacaoEngenharia: string
    horas?: number
    minutos?: number
    quantidade: number
    detalhamento: string
}

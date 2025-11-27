export interface ServicosNaoConformidadesOutput{
  idNaoConformidade: string
  id: string
  idProduto: string
  codigo: string
  descricao: string
  idRecurso?: string
  descricaoRecurso: string
  operacaoEngenharia: string
  horas?: number
  minutos?: number
  quantidade: number
  detalhamento: string
  controlarApontamento: boolean
}

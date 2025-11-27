export interface EstoqueLocalPedidoVendaAlocacaoDTO {
    id: string;
    numeroPedido: number;
    ordemFabricacao: number;
    quantidadeTotalPedido: number;
    quantidadeAlocadaLoteLocal: number;
    quantidadeAprovada: number;
    quantidadeReprovada: number;
    quantidadeRetrabalhada: number;
    descricaoProduto: string;
    descricaoLocalReprovado: string;
    descricaoLocalRetrabalho: string;
    descricaoLocalAprovado: string;
    codigoLocalReprovado: number;
    codigoLocalRetrabalho: number;
    codigoLocalAprovado: number;
}

export interface GetAllEstoqueLocalPedidoVendaAlocacaoDTO {
    items: EstoqueLocalPedidoVendaAlocacaoDTO[];
    totalCount: number;
}
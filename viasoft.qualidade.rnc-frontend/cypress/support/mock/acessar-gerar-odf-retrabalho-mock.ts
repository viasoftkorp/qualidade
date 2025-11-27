import { EstoquePedidoVendaEstoqueLocalViewOutput } from "@viasoft/rnc/api-clients/Estoque-Pedido-Venda-Estoque-Locais/model/estoque-pedido-venda-estoque-local-view-output";
import { getAllEstoqueLocaisRequest } from "../requests/legacy-logistica/estoque-local-requests";
import { getAllEstoquePedidoVendaEstoqueLocaisViewRequest } from "../requests/legacy-logistica/estoque-pedido-venda-estoque-local-requests";
import { canGenerateOrdemRetrabalhoRequest } from "../requests/nao-conformidade/ordem-retrabalho-nao-conformidades/ordem-retrabalho-nao-conformidades";
import { IPagedResultOutputDto } from "@viasoft/common";
import { EstoqueLocalOutput } from "@viasoft/rnc/api-clients/Estoque-Locais/model/estoque-local-output";
import { getAllLocaisRequest } from "../requests/legacy-logistica/locais-requests";

export class AcessarGerarOdfRetrabalhoMock {
  public canGenerateOrdemRetrabalhoRequest = canGenerateOrdemRetrabalhoRequest()
  public getAllEstoqueLocaisRequest: CypressRequestV2<IPagedResultOutputDto<EstoqueLocalOutput>>
  | CypressRequestV2<IPagedResultOutputDto<EstoquePedidoVendaEstoqueLocalViewOutput>> = getAllEstoqueLocaisRequest();
  public getLocaisRequest = getAllLocaisRequest();
}

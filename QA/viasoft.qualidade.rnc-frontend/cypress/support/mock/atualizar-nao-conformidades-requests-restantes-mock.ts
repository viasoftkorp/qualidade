import { GerarOrdemRetrabalhoValidationResult } from "@viasoft/rnc/api-clients/Nao-Conformidades/Ordem-Retrabalho/model/gerar-ordem-retrabalho-validation-result";
import { getUserByIdRequest } from "../requests/authentication/users-requests";
import { getClienteById } from "../requests/erp-person/erp-person-requests";
import { getNotasFiscaisSaidaById } from "../requests/legacy-faturamento/notas-fiscais-saida-requests";
import { getProductByIdRequest } from "../requests/logistics-products/products-requests";
import { getAllDefeitosNaoConformidadeRequest } from "../requests/nao-conformidade/defeito-nao-conformidades/defeito-nao-conformidade.requests";
import { getOperacaoRetrabalhoRequest } from "../requests/nao-conformidade/operacao-retrabalho-nao-conformidades/operacao-retrabalho-nao-conformidades-requests";
import { getOrdemRetrabalhoRequest } from "../requests/nao-conformidade/ordem-retrabalho-nao-conformidades/ordem-retrabalho-nao-conformidades";
import { getNaturezaRequestForId } from "../requests/naturezas/naturezas.request";
import { getListOrdemProducao } from "../requests/producao/ordens-producao-requests";
import { IPagedResultOutputDto } from "@viasoft/common";
import { OrdemProducaoOutput } from "@viasoft/rnc/api-clients/Ordem-Producao/model";

export class AtualizarNaoConformidadeRequestsRestantesMock {
  public getNaturezaById = getNaturezaRequestForId();
  public getAllProductById = getProductByIdRequest()
  public defeitosNaoConformidadesList = getAllDefeitosNaoConformidadeRequest();
  public getClienteById = getClienteById();
  public getNotasFiscaisSaidaById = getNotasFiscaisSaidaById();
  public getUserById = getUserByIdRequest();
  public getOperacaoRetrabalhoRequest = getOperacaoRetrabalhoRequest();
  public getOrdemRetrabalhoRequest = getOrdemRetrabalhoRequest();
  public getListOrdemProducao: CypressRequestV2<IPagedResultOutputDto<OrdemProducaoOutput>> | undefined= getListOrdemProducao()
}

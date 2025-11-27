import { getUserByIdRequest } from "../requests/authentication/users-requests";
import { getClienteById } from "../requests/erp-person/erp-person-requests";
import { getProductByIdRequest } from "../requests/logistics-products/products-requests";
import { getAllDefeitosNaoConformidadeRequest } from "../requests/nao-conformidade/defeito-nao-conformidades/defeito-nao-conformidade.requests";
import { getNaoConformidadeRequestForId } from "../requests/nao-conformidade/nao-conformidades.requests";
import { getNaturezaRequestForId } from "../requests/naturezas/naturezas.request";
export class NaoConformidadesEditorMock {
  public naoConformidadeById = getNaoConformidadeRequestForId();
  public getUserById = getUserByIdRequest();
  public getNaturezaById = getNaturezaRequestForId();
  public getAllProductById = getProductByIdRequest()
  public defeitosNaoConformidadesList = getAllDefeitosNaoConformidadeRequest();
  public getClienteById = getClienteById();
}

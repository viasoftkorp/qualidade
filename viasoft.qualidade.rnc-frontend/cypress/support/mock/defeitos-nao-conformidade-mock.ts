import { getAllAcoesPreventivasNaoConformidadeRequest } from "../requests/nao-conformidade/acao-preventiva-nao-conformidades/acao-preventiva-nao-conformidade.requests";
import { getAllCausasNaoConformidadeRequest } from "../requests/nao-conformidade/causa-nao-conformidades/causa-nao-conformidade.requests";
import { getAllSolucoesNaoConformidadeRequest } from "../requests/nao-conformidade/solucao-nao-conformidades/solucao-nao-conformidade.requests";

export class GridsDependentesRequest {
  public getCausasNaoConformidadesList = getAllCausasNaoConformidadeRequest();
  public getSolucoesNaoConformidadesList = getAllSolucoesNaoConformidadeRequest();
  public getAcoesPreventivasNaoConformidadesList = getAllAcoesPreventivasNaoConformidadeRequest();
}

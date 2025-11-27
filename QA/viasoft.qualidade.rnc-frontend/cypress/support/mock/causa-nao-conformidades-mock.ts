import { createNewCausasNaoConformidadeRequest, getAllCausasNaoConformidadeRequest, getCausasList, getCausasNaoConformidadeRequestForId } from "../requests/nao-conformidade/causa-nao-conformidades/causa-nao-conformidade.requests";

export class CausaNaoConformidadesMock {
  public getCausasList = getCausasList();
  public getCausasNaoConformidadesList = getAllCausasNaoConformidadeRequest();
}

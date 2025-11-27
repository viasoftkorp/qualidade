import { getAllCausasNaoConformidadeRequest } from "../requests/nao-conformidade/causa-nao-conformidades/causa-nao-conformidade.requests";

export class SelectDefeitosGridMock {
  public getAllCausasNaoConformidade = getAllCausasNaoConformidadeRequest();
}

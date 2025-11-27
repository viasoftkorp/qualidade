import { createNewNaoConformidadeRequest, getNaoConformidadeRequestForId } from "../requests/nao-conformidade/nao-conformidades.requests";

export class CreateNaoConformidadeMock {
  public createNaoConformidade = createNewNaoConformidadeRequest();
}

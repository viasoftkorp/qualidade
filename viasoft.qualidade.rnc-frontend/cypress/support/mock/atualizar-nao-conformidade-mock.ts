import { ReclamacoesNaoConformidadesInput } from "@viasoft/rnc/api-clients/Nao-Conformidades";
import { updateNaoConformidadeRequest } from "../requests/nao-conformidade/nao-conformidades.requests";

export class AtualizarNaoConformidadeMock {
  updateNaoConformidade = updateNaoConformidadeRequest();
  createReclamacaoNaoConformidade:CypressRequestV2<ReclamacoesNaoConformidadesInput> | undefined;
}

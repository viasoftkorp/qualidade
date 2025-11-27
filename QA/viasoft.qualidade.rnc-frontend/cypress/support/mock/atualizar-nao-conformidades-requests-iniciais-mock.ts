import { getConfiguracoesGeraisRequest } from "../requests/configuracao/configuracao-requests";
import { getNaoConformidadeRequestForId } from "../requests/nao-conformidade/nao-conformidades.requests";
import { canGenerateOrdemRetrabalhoRequest } from "../requests/nao-conformidade/ordem-retrabalho-nao-conformidades/ordem-retrabalho-nao-conformidades";

export class AtualizarNaoConformidadeRequestsIniciaisMock {
  public naoConformidadeById = getNaoConformidadeRequestForId();
  public getConfiguracoesGeraisRequest = getConfiguracoesGeraisRequest();

}

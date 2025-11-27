import { AtualizarNaoConformidadeRequestsIniciaisMock } from "./atualizar-nao-conformidades-requests-iniciais-mock";
import { AtualizarNaoConformidadeRequestsRestantesMock } from "./atualizar-nao-conformidades-requests-restantes-mock";
import { NaoConformidadesList } from "./nao-conformidades-list-mock";

export class NavigateToUpdateNaoConformidadeUserActionMock {
  public naoConformidadesList = new NaoConformidadesList();
  public atualizarNaoConformidadeRequestsIniciaisMock = new AtualizarNaoConformidadeRequestsIniciaisMock()
  public atualizarNaoConformidadeRequestsRestantesMock = new AtualizarNaoConformidadeRequestsRestantesMock()
}

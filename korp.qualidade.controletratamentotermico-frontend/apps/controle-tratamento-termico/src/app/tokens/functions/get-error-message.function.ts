import { HttpErrorResponse } from '@angular/common/http';

const replaceAll = (property: string, value: string, formula: string) => {
  const regex = new RegExp(property, 'g');
  return formula.replace(regex, value);
};

export function formatErrorMessage(message: string): string {
  if (message) {
    // Tentar extrair a mensagem do campo "message" do objeto JSON
    try {
      // Verificar se a mensagem contém o padrão esperado
      const match = message.match(/Resposta: {[\s\S]*"message": "([\s\S]*?)"/);
      if (match && match[1]) {
        // Extrair apenas o conteúdo do campo "message"
        let formattedMessage = match[1];
        // Substituir as sequências de escape por seus caracteres reais
        formattedMessage = formattedMessage.replace(/\\r\\n/g, "\r\n");
        // Substituir diferentes tipos de quebras de linha por <br/>
        formattedMessage = formattedMessage.replace(/\r\n|\r|\n/g, "<br/>");
        formattedMessage = "<p>" + formattedMessage + "</p>";
        return formattedMessage;
      }
    } catch (e) {
      return message || 'ControleTratamentoTermico.ErroDesconhecido';
    }

    // Comportamento padrão (caso a extração falhe)
    let formattedMessage = message;
    // Substituir diferentes tipos de quebras de linha por <br/>
    formattedMessage = formattedMessage.replace(/\r\n|\r|\n/g, "<br/>");
    formattedMessage = "<p>" + formattedMessage + "</p>";
    return formattedMessage;
  }
  return message || 'ControleTratamentoTermico.ErroDesconhecido';
}

export function getErrorMessage(httpError: HttpErrorResponse): string {
  if (httpError?.error?.Message) {
    if ((httpError?.error?.Message as string).includes("Atenção não foi possível realizar a movimentação do estoque, pois o saldo movimentado ultrapassa o saldo disponível menos os saldos já reservados por outros módulos do sistema.") ||
      (httpError?.error?.Message as string).includes("Atenção não foi possível realizar a movimentação do estoque, pois a quantidade da movimentação diverge do saldo dos pacotes.")) {
      let html = httpError?.error?.Message as string;
      html = replaceAll("\r\n", "<br/>", html);
      html = "<p>" + html + "</p>";
      return html;
    } else {
      return httpError?.error?.Message;
    }
  }
  return httpError?.error?.Message || 'ControleTratamentoTermico.ErroDesconhecido';
}

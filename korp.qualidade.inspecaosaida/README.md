# Como utilizar esse template

Esse readme contem duas partes. a parte 1 explica como fazer a criação de um novo serviço, já a parte 2 explica o passo
a passo para se atualizar um serviço para o novo versionamento.

---

## 1. Criação de serviço

1. Copie todos os arquivos desse template para a pasta de seu repositório, certifique-se que seu repositório não esteja
   na pasta 'C:\Users\seu-usuário\go\src' ou 'C:\go\src'.

2. Rode o seguinte comando `` go mod init bitbucket.org/viasoftkorp/<nome-serviço> ``

3. Se seu projeto estiver usando a IDE GoLand é recomendado fazer esses sub-passos, caso o contrário rode o
   comando: ``go env -w GOPRIVATE=bitbucket.org/viasoftkorp``

    - Abra o tutorial de configuração de ambiente e siga os passos indicados, este tutorial está no repositório korp.sdk

4. Rode o comando `` go mod tidy ``

5. No arquivo main.go, defina o nome do seu serviço na variável service_info.ServiceName. Ele deve ser o **nome do
   repositório**

6. Crie a key/value consul de seu projeto com o mesmo ServiceName, sua configuração consul deve seguir o seguinte
   padrão:

```
{
  "ConnectionStrings": {
      "DefaultConnection": "sqlserver://ViasoftServices:korp!4518@localhost?database=%s&app name=ServiceName"
   },
 "Sentry": {
   "CanInitSentry": "false",
    "Dsn": ""
  },
  "ServicePort": "<SERVICE_PORT>"
}
```

- Detalhe importante:

    - Você pode dinamicamente mudar o database colocando um `%s` ao invés do nome dele dentro da DefaultConnection. Para
      isso funcionar corretamente certifique-se de passar um tenantModel com um databaseName nele.

7. Criar compose

    - No repositório `Korp-IAC`, criar ou alterar algum compose existente em `Oracle/Process` adicionando o seu serviço.

    - Para criar seu compose, voce pode se basear
      em <https://bitbucket.org/viasoftkorp/korp-iac/src/master/Oracle/Process/compose-solidworks.yml>

8. Pesquise por `TODO` no projeto e realize as devidas alterações

9. Configurar Jenkinsfile

    - Caso seu projeto utilize versionamento(caso padrão), use o arquivo `Jenkinsfile`, caso o contrário, use o
      arquivo `jenkinsfile_without_version` e renomeie ele para `Jenkinsfile`. **O 'J' DEVE SER MAIÚSCULO**.

    - Altere os parâmetros das duas primeiras linhas:
        - `serviceName` para o nome do seu repositório(com letras maiúsculas e pontos).
        - `composeName` para o nome do seu compose.
    - **OBS:**
        - Caso seu projeto tenha algum parâmetro que deve ser injetado em build, ele deve ser adicionado na func
          build( ) do jenkinsfile
        - Caso seu projeto não rode no servidor `Process - Oracle`, a primeira variável na chamada da função
          reloadService() (`env.IP_PROCESS_PROD`) deve ser alterada. Para saber qual valor colocar, consulte a equipe de
          DevOps
10. Configurar Dockerfile:

    - Altere a propriedade `<ServiceName>` para o nome do seu serviço, conforme BitBucket (Ex:
      Viasoft.ProjectTemplates.Golang)
    - Altere a propriedade `<ServicePort>` para a porta do seu serviço, a mesma que será usada no compose e no consul (
      Ex: 9840)

11. Execute o seu projeto.

### Antes de publicar o seu serviço, informe para a equipe de DevOps:

- As propriedades do consul do seu projeto
- As rotas, e porta
- Propriedades do Vault(se houver)

---

## 2. Atualização de versionamento

### Jenkinsfile

1. Verifique se a build no seu jenkinsfile contém alguma variável de ambiente específica do seu projeto.
    - Caso a build do seu projeto contenha algo
      como `-X 'bitbucket.org/viasoftkorp/<repoName>/<repoFile>.<variable>=${<someValue>}'`, ele contem variáveis de
      ambiente.
    - Perceba que todas a builds contem variáveis de ambiente do SDK, como por
      exemplo: `-X 'bitbucket.org/viasoftkorp/korp.sdk/service_info.Version=${deployVersion}'`, que não é considerada
      uma variável do projeto.
    - Caso todas as variáveis setas na build sejam do SDK, voce não precisa se preocupar.
    - Caso haja alguma variável de ambiente, copie elas para um arquivo externo. Cuidado para copiar **`-X '`**
      e **`'`** no início e no fim da variável.

2. Verifique a variável de IP na jenkinsfile do seu serviço, ela é o primeiro parâmero passado na chamada da função
   reloadService( ). Caso seu valor seja `env.IP_PROCESS_PROD`, voce não precisa fazer mais nada. Caso o contrário copie
   o seu valor que ele deverá ser substituído mais tarde.

3. Copie o valor da variável composeName da Jenkinsfile atual.

4. Configurar Jenkinsfile

    - Caso seu projeto utilize versionamento(caso padrão), use o arquivo `Jenkinsfile`, caso o contrário, use o
      arquivo `jenkinsfile_without_version` e renomeie ele para `Jenkinsfile`. **O 'J' DEVE SER MAIÚSCULO**.

    - Altere os parâmetros das duas primeiras linhas:
        - `serviceName` para o nome do seu repositório(com letras maiúsculas e pontos).
        - `composeName` para o nome do compose copiado no passo 3.

    - Caso seu projeto contenha parâmetros injetados em build (passo 1), adicione eles dentro da função build()

        - As variáveis que voce copiou devem ser injetados nas duas builds: `GOOS=window...` e `go build -ldflags...`.
        - Para isso copie e cole as variáveis no final da linha, mas não antes de `-o ${binaryName}`
            - Note que entre todas as variáveis há `' -X '`, e no final, antes de `-o`, há `''` **isso deve ser
              mantido**

    - Caso o IP do servidor do seu serviço não seja `env.IP_PROCESS_PROD` (passo 2), substitua o primeiro parâmetro da
      chamada da função reloadService() (`env.IP_PROCESS_PROD`), para o valor copiado na etapa 2.

---

### Dockerfile

Abra sua Docker file e edite:

1. Service name:

- O nome do seu serviço deve estar **idêntico** ao repositório, com letras maiúsculas e pontos
- As propriedades que voce tera que alterar provavelmente serão:
    - `COPY ./<seviceName> .`
    - `RUN chmod +x ./<serviceName>`
    - `CMD ["./<serviceName>"]`
- Abaixo há um exemplo do antes e depois da alteração

    - ANTES
      ```
      FROM golang:1.16
  
      USER root
      WORKDIR /app
  
      ENV GIN_MODE=release \
          CGO_ENABLED=1 \
          GLOBAL_CONFIG_PATH=/app/data/globalconfig.json
  
      COPY ./ViasoftKorpAuditTrail .
  
      RUN chmod +x ./ViasoftKorpAuditTrail
  
      EXPOSE 9981
  
      CMD ["./ViasoftKorpAuditTrail"]
      ```

    - DEPOIS
      ```
      FROM golang:1.16
  
      USER root
      WORKDIR /app
  
      ENV GIN_MODE=release \
          CGO_ENABLED=1 \
          GLOBAL_CONFIG_PATH=/app/data/globalconfig.json
  
      COPY ./Viasoft.AuditTrail.Client .
  
      RUN chmod +x ./Viasoft.AuditTrail.Client
  
      EXPOSE 9981
  
      CMD ["./Viasoft.AuditTrail.Client"]
      ```

---

### Docker-Compose

- alterações dentro do container:
    - Alterar `container_name` para o nome do repositório (com letras maiúsculas e pontos).
    - Adicionar `- secret-manager:/app/SecretManager/` dentro de `volumes:`.
    - Alterar o nome do serviço para o nome do repositório, tudo minúsculo e '-' ao invés de '.'.
    - A imagem deve seguir o padrão `korp/<repoName_lowerCase>:<tag>`.
        - `<tag>` = Caso seu serviço seja versionado a tag será algo como `<v>.<v>.<v>.x` ou `2021.4.0.x`. Caso fique
          uma versão fixada (caso não padrão), o ultimo 'x' deve ser substituído pela versão da tag.
        - `<repoName_lowerCase>`= Nome do repositório, tudo minúsculo.

- fora do container:
    - Declarar volume `secret-manager` no compose(isso deve ser feito já no inicio do compose):
      ```
      volumes:
        secret-manager:
          external: true
          name: secret-manager
      ```

---
---

- Compose antes da alteração:

```
version: "3.8"

networks:
  servicos:
    external:
      name: servicos

services:
  teamwork-integration:
    image: "korpcicd/viasoft.teamwork.integration:1.0.x"
    container_name: "viasoft.teamwork.integration"
    restart: always
    environment:
      - URL_CONSUL=http://consul-agent:8500
    networks:
      - servicos
    ports:
      - "9912:9912"
```

- Compose após a alteração:

```
version: "3.8"

networks:
  servicos:
    external:
      name: servicos

volumes:
  secret-manager:
    external: true
    name: secret-manager

services:
  viasoft-teamwork-integration:
    image: "korp/viasoft.teamwork.integration:2021.4.0.x"
    container_name: "Viasoft.Teamwork.Integration"
    restart: always
    environment:
      - URL_CONSUL=http://consul-agent:8500
    networks:
      - servicos
    ports:
      - "9912:9912"
    volumes:
      - secret-manager:/app/SecretManager/
```

---

### Solicitações para  equipe DevOps

1. Solicite para a equipe de DevOps remover a build de branchs do Job no seu serviço no Jenkins.

2. Solicite para a equipe de DevOps atualizar o arquivo do seu compose na nuvem.
